using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CryptoPro.Sharpei;
using CryptoPro.Sharpei.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Net;
using System.Runtime.InteropServices;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography.Pkcs;
using System.Reflection;
using Newtonsoft.Json;
using WebDav;

namespace FoxPro_Med
{

	[Guid("87a36153-82cb-4dc4-9a8f-33b542edf2f5")]
	[ComVisible(true)]
	public interface IFoxPro_MedClass
	{
		// system
		int getErrorCount();
		Error getError(int index = 0);
		void resetErrors();
		string getErrorMessage(Error er);
		string getErrorType(Error er);
		string getErrorFrom(Error er);
		string getLastErrorString();


		string version();
		void setLogEnable(bool enabled = true);
		void showSettings();
		bool updateUrls(string urlHttp, string urlHttps);
		bool updateClientData(string client_id, string client_secret);
		string getHttp();
		string getHttps();
		string getHttpHost();
		string getHttpsHost();
		string getUTF8FromBytes(byte[] b);
		byte[] getBytesFromUTF8(string s);
		string getGUID();
		string getHashSHA256(byte[] b);
		object getObjectFromJson(string json);
		object getObjectFromXml(string xml);

		// util
		string doRequest(string requestMethod, string url, string headers, string body = null);
		string doRequest(string requestMethod, string url, string[] headers, string body = null);
		string sign(string str);
		string signBytes(byte[] arr);
		bool checkSign(byte[] arr, string sign);

		bool tokenIsValid(bool autoUpdate = true);
		string getToken(bool autoUpdate = true);
		string getTokenExpireDateTime();
		string getCurrentUserId();
		void selectSertificates();
		string selectUserCertificate();
		string selectCertificateForeign();
		string getUserCertificateName();
		string getCertificateForeignName();

		// requests
		byte[] getDocumentTicket(string document_id);
		string getDocumentTicketLink(string document_id);
		Document getDocumentData(string document_id);
		ResponseDocumentList getDocumentsByRequest(string request_id);

		Document getIncomeDocument(ResponseDocumentList resp, int index);
		ResponseDocumentList getIncomeDocuments(int start_from = 0, int count = 10, DocFilter filter = null);
		byte[] downloadFile(string document_id);
		string getDocumentLinkById(string document_id);
		byte[] webdavDownload(string link);

		ResponseDocumentList getOutcomeDocuments(int start_from = 0, int count = 10, DocFilter filter = null);
		string uploadDocument(byte[] fileIn);
		string uploadDocumentFromUTF8String(string fileUTF8String);
		string getUploadedDocumentSignatureById(string document_id);
		//string uploadSmallFile(string fileBase64);
		string uploadSmallFile(byte[] fileIn);
		ResponseUploadLargeDocumentLink getLinkForDocumentUpload(byte[] fileIn);
		bool webdavUpload(byte[] file, string link);
		bool finishUploadLargeFile(string document_id);
		bool cancelUploadLargeFile(string document_id, string request_id);

		string getNewToken(string clientId = null, string clientSecret = null);
		ResponseAuth getUserAuthCode(string clientId = null, string clientSecret = null);
		ResponseSession getSessionToken(string sessionRequestCode);
		User getCurrentUserInfo();
		Group getGroupInfo(string group_id, string token = null);
		string registerNewUser(string first_name, string last_name, string middle_name, string email,
			string sessionToken, string sys_id = null, X509Certificate2 cert = null);
		string createNewGroup(string group_name, string sessionToken, string[] rights);
		bool addUserToGroup(string group_id, string user_id, string sessionToken);

		// Для самого первого тестового контура
		string getTestUserToken();
	}

	[Guid("741da67c-9881-4e00-a65f-b1fa33df926e")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class FoxPro_MedClass :IFoxPro_MedClass
	{
		public string VERSION = "2019.11.15 12:40:00";
		public string URL_HTTP, URL_HTTPS;

		public string STRING_FORMAT_DATE_TIME = "yyyy.MM.dd HH:mm:ss";
		public string STRING_FORMAT_DATE = "yyyy.MM.dd";
		public string STRING_FORMAT_TIME = "HH:mm:ss";
		private JavaScriptSerializer jss = new JavaScriptSerializer();
		public X509Certificate2 userCert, certForeign;

		List<Error> errors = new List<Error>();

		public FoxPro_MedClass()
		{
			if (String.IsNullOrEmpty(Properties.Settings.Default.userCertSN) || String.IsNullOrEmpty(Properties.Settings.Default.certForeignSN))
				selectSertificates();
			else
			{
				certForeign = certificateFindBySerialNumber(Properties.Settings.Default.certForeignSN);
				userCert = certificateFindBySerialNumber(Properties.Settings.Default.userCertSN);
			}
			if (String.IsNullOrEmpty(Properties.Settings.Default.urlHttp))
			{
				Properties.Settings.Default.urlHttp = "http://api.stage.mdlp.crpt.ru";
				Properties.Settings.Default.urlHttps = "https://api.stage.mdlp.crpt.ru";
				Properties.Settings.Default.client_id = "01db16f2-9a4e-4d9f-b5e8-c68f12566fd5";
				Properties.Settings.Default.client_secret = "9199fe04-42c3-4e81-83b5-120eb5f129f2";
				Properties.Settings.Default.Save();
			}
			URL_HTTP = Properties.Settings.Default.urlHttp;
			URL_HTTPS = Properties.Settings.Default.urlHttps;
		}

		/// <summary>
		/// Записывает действия в логфайл в "документах"
		/// </summary>
		/// <param name="message">Что записать в лог</param>
		private void Log(string message)
		{
			try
			{
				if (!Properties.Settings.Default.logEnabled)
					return;

				if (String.IsNullOrEmpty(Properties.Settings.Default.logDir))
					showSettings();
				File.AppendAllText(Properties.Settings.Default.logDir + "\\logFoxProMed.txt",
					DateTime.Now.ToString(STRING_FORMAT_DATE_TIME) + " " + message + "\r\n");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Не удается сохранить лог по указанному пути: " + ex.Message);
				showSettings();
			}
		}

		/// <summary>
		/// Поиск сертификатов по серийнику
		/// </summary>
		/// <param name="sn">Серийник</param>
		/// <returns>Сертификат</returns>
		private X509Certificate2 certificateFindBySerialNumber(string sn)
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser); // Создаем объект хранилища
			store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly); // Открываем его
			X509Certificate2Collection certificates = store.Certificates; // Получаем список доступных сертификатов
			foreach (X509Certificate2 cert in certificates) // Бежим по сертификатам
				if (cert.SerialNumber == sn && cert.NotAfter > DateTime.Now && cert.NotBefore < DateTime.Now) // Если серийники совпадают и сертификат действителен
					return cert; // Вернем его
			return null; // Еслиничего не нашли - вернем нулл
		}

		private void setError(string message, string from, string type = "Exception")
		{
			Error e = new Error();
			e.message = message;
			e.from = from;
			e.type = type;
			errors.Insert(0, e);
			for (int i = errors.Count - 1; i > 10; i--)
				errors.RemoveAt(i);
		}

		public string getErrorMessage(Error er)
		{
			return er.message;
		}

		public string getErrorType(Error er)
		{
			return er.type;
		}

		public string getErrorFrom(Error er)
		{
			return er.from;
		}

		public string getLastErrorString()
		{
			Error l = getError();
			return l.message + " " + l.from + " " + l.type;
		}

		public int getErrorCount()
		{
			return errors.Count;
		}

		/// <summary>
		/// Получение последней ошибки
		/// </summary>
		/// <returns>Error</returns>
		public Error getError(int index = 0)
		{
			if (errors.Count < 1)
				return null;
			if (index >= errors.Count)
				return null;
			if (index < 0)
				return null;
			return errors[index];
		}

		/// <summary>
		/// Очистка массива ошибок
		/// </summary>
		public void resetErrors()
		{
			errors.Clear();
		}

		/// <summary>
		/// Возвращает дату сборки
		/// </summary>
		/// <returns>Дата в строке</returns>
		public string version() { return VERSION; }

		/// <summary>
		/// Устанавливает флаг записи логов
		/// </summary>
		/// <param name="enabled"></param>
		public void setLogEnable(bool enabled = true)
		{
			Properties.Settings.Default.logEnabled = enabled;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// Отобразить форму настройки библиотеки
		/// </summary>
		public void showSettings()
		{
			FormSettings f = new FormSettings();
			f.parent = this;
			f.ShowDialog();
		}

		/// <summary>
		/// Записать в настройки библиотеки url
		/// </summary>
		/// <param name="urlHttp"></param>
		/// <param name="urlHttps"></param>
		/// <returns>true при удаче</returns>
		public bool updateUrls(string urlHttp, string urlHttps)
		{
			try
			{
				Properties.Settings.Default.urlHttp = urlHttp;
				Properties.Settings.Default.urlHttps = urlHttps;
				Properties.Settings.Default.Save();
				return true;
			}
			catch (Exception ex)
			{
				setError(ex.Message, "updateUrls");
				Log("updateUrls: " + ex.Message + ex.StackTrace);
				return false;
			}
		}

		/// <summary>
		/// Записать в настройки библиотеки client_id и client_secret
		/// </summary>
		/// <param name="client_id"></param>
		/// <param name="client_secret"></param>
		/// <returns>true при удаче</returns>
		public bool updateClientData(string client_id, string client_secret)
		{
			try
			{
				Properties.Settings.Default.client_id = client_id;
				Properties.Settings.Default.client_secret = client_secret;
				Properties.Settings.Default.Save();
				return true;
			}
			catch (Exception ex)
			{
				setError(ex.Message, "updateClientData");
				Log("updateClientData: " + ex.Message + ex.StackTrace);
				return false;
			}
		}

		/// <summary>
		/// Получение строки URL для запросов
		/// </summary>
		/// <returns>Строку с URL</returns>
		public string getHttp()
		{
			return Properties.Settings.Default.urlHttp;
		}

		/// <summary>
		/// Получение строки URL (https) протокола для запроса
		/// </summary>
		/// <returns>Строку с URL (https)</returns>
		public string getHttps()
		{
			return Properties.Settings.Default.urlHttps;
		}

		/// <summary>
		/// Получение значение HOST-header для http запроса
		/// </summary>
		/// <returns>Знаечение для HOST-header</returns>
		public string getHttpHost()
		{
			return Properties.Settings.Default.urlHttp.Substring(Properties.Settings.Default.urlHttp.LastIndexOf("/") + 1);
		}

		/// <summary>
		/// Получение значение HOST-header для https запроса (на случай, если запросы https делаются на другой домен)
		/// </summary>
		/// <returns>Знаечение для HOST-header https запроса</returns>
		public string getHttpsHost()
		{
			return Properties.Settings.Default.urlHttps.Substring(Properties.Settings.Default.urlHttps.LastIndexOf("/") + 1);
		}

		/// <summary>
		/// Получение строки в UTF8 из массива байт
		/// </summary>
		/// <param name="b">Массив байт</param>
		/// <returns>Строка</returns>
		public string getUTF8FromBytes(byte[] b)
		{
			return Encoding.UTF8.GetString(b);
		}

		/// <summary>
		/// Получение массива байт из UTF8 строки
		/// </summary>
		/// <param name="s">Строка</param>
		/// <returns>Массив байт</returns>
		public byte[] getBytesFromUTF8(string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}

		/// <summary>
		/// Получение GUID
		/// </summary>
		/// <returns>Строка GUID</returns>
		public string getGUID()
		{
			return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Получение хэша по SHA256 от массива байт
		/// </summary>
		/// <param name="b">Массив байт</param>
		/// <returns>Строка с хэшем</returns>
		public string getHashSHA256(byte[] b)
		{
			var sha256 = SHA256.Create();
			return Convert.ToBase64String(sha256.ComputeHash(b));
		}

		/// <summary>
		/// Тестовая: получение графа объекта из JSON
		/// </summary>
		/// <param name="json">Строка с JSON</param>
		/// <returns>Объект</returns>
		public object getObjectFromJson(string json)
		{
			return jss.DeserializeObject(json);
		}

		/// <summary>
		/// Тестовая: получение графа объекта из XML
		/// </summary>
		/// <param name="xml">Строка с XML</param>
		/// <returns>Объект</returns>
		public object getObjectFromXml(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			string json = JsonConvert.SerializeXmlNode(doc);
			return jss.DeserializeObject(json);
		}


		/// <summary>
		/// Сделать запрос
		/// </summary>
		/// <param name="requestMethod">POST, GET или PUT</param>
		/// <param name="url">URL запроса, если запрос GET, парамеры запроса должны быть в этом параметре</param>
		/// <param name="headers">Заголовки в виде одной строки соединенной разделителем "\r\n"</param>
		/// <param name="body">Тело запроса (для POST и PUT)</param>
		/// <returns>Строку, что вернул сервер либо null при неудаче</returns>
		public string doRequest(string requestMethod, string url, string headers, string body = null)
		{
			try
			{
				string[] sep = { "\r\n" };
				string[] headersTo = headers.Split(sep, StringSplitOptions.RemoveEmptyEntries);
				return doRequest(requestMethod, url, headersTo, body);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "doRequest(s)");
				Log("doRequest(s): [" + requestMethod + " " + url + "]{" + headers + "}" + body + ": " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Сделать запрос
		/// </summary>
		/// <param name="requestMethod">POST, GET или PUT</param>
		/// <param name="url">URL запроса, если запрос GET, парамеры запроса должны быть в этом параметре</param>
		/// <param name="headers">Массив строк с заголовками запроса</param>
		/// <param name="body">Тело запроса (для POST и PUT)</param>
		/// <returns>Строку, что вернул сервер либо null при неудаче</returns>
		public string doRequest(string requestMethod, string url, string[] headers, string body = null)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			try
			{
				request.Method = requestMethod;
				string log = "Headers setted: \r\n";
				foreach (string header in headers)
				{
					string param = header.Substring(0, header.IndexOf(":"));
					string value = header.Substring(param.Length + 1).Trim();
					request.SetRawHeader(param, value);
					log += header + "\r\n";
				}
				Log(log);

				if (body != null)
				{
					byte[] data = Encoding.UTF8.GetBytes(body);
					using (var stream = request.GetRequestStream())
					{
						stream.Write(data, 0, data.Length);
					}
				}

				if (Properties.Settings.Default.proxyEnabled)
				{
					WebProxy proxy = new WebProxy(Properties.Settings.Default.proxyAddress, Properties.Settings.Default.proxyPort);
					proxy.BypassProxyOnLocal = false;
					request.Proxy = proxy;
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
				return responseString;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "doRequest", "WebException");
				Log("doRequest: [" + requestMethod + " " + url + "]\r\n" + body + "\r\n" + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "doRequest");
				Log("doRequest: [" + requestMethod + " " + url + "]\r\n" + body + "\r\n" + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Подписывает строку НАШим сертификатом и возвращает Base64 значение
		/// </summary>
		/// <param name="str">Строка, которую надо подписать</param>
		/// <returns>Строка подписи в Base64 либо null при неудаче</returns>
		public string sign(string str)
		{
			try
			{
				// из файла C:\Program Files (x86)\Crypto Pro\.NET SDK\Examples\simple.zip\CMS\cs\DetachedSignature
				ContentInfo contentInfo = new ContentInfo(Encoding.UTF8.GetBytes(str));
				SignedCms signedCms = new SignedCms(contentInfo, true);
				// Определяем подписывающего, объектом CmsSigner.
				CmsSigner cmsSigner = new CmsSigner(userCert);
				// Подписываем CMS/PKCS #7 сообение.
				signedCms.ComputeSignature(cmsSigner);
				// Кодируем CMS/PKCS #7 подпись сообщения и сразу вводим ее в параметр тела запроса
				return Convert.ToBase64String(signedCms.Encode());
			}
			catch (Exception ex)
			{
				// Если вдруг ошибка - выведем ее в лог
				setError(ex.Message, "Sign");
				Log("Sign: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Подписывает массив байт НАШим сертификатом и возвращает Base64 значение
		/// </summary>
		/// <param name="arr">Массив байт, который надо подписать</param>
		/// <returns>Строка подписи в Base64 либо null при неудаче</returns>
		public string signBytes(byte[] arr)
		{
			try
			{
				// из файла C:\Program Files (x86)\Crypto Pro\.NET SDK\Examples\simple.zip\CMS\cs\DetachedSignature
				ContentInfo contentInfo = new ContentInfo(arr);
				SignedCms signedCms = new SignedCms(contentInfo, true);
				// Определяем подписывающего, объектом CmsSigner.
				CmsSigner cmsSigner = new CmsSigner(userCert);
				// Подписываем CMS/PKCS #7 сообение.
				signedCms.ComputeSignature(cmsSigner);
				// Кодируем CMS/PKCS #7 подпись сообщения и сразу вводим ее в параметр тела запроса
				return Convert.ToBase64String(signedCms.Encode());
			}
			catch (Exception ex)
			{
				// Если вдруг ошибка - выведем ее в лог
				setError(ex.Message, "SignBytes");
				Log("SignBytes: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Проверяет подпись файлов
		/// </summary>
		/// <param name="arr">Массив, который надо проверить</param>
		/// <param name="sign">Подпись этого массива</param>
		/// <returns>Подпись верна или не верна</returns>
		public bool checkSign(byte[] arr, string sign)
		{
			try
			{
				// Создаем объект, реализующий алгоритм ГОСТ 3410 через CSP.
				Gost3410CryptoServiceProvider Gost = new Gost3410CryptoServiceProvider();
				// Создаем объект, реализующий алгоритм хэширования ГОСТ 3411
				Gost3411CryptoServiceProvider GostHash = new Gost3411CryptoServiceProvider();
				// Проверяем правильность подписи и выводим результат:
				bool res = Gost.VerifyData(arr, GostHash, Convert.FromBase64String(sign));

				return res;
			}
			catch (Exception ex)
			{
				// Если вдруг ошибка - выведем ее в лог
				setError(ex.Message, "checkSign");
				Log("checkSign: " + ex.Message + ex.StackTrace);
				return false;
			}
		}

		/// <summary>
		/// Проверяет токен на валидность по времени, если токен устарел: если указан параметр true - сначала пытается обновить токен и выдает новый, иначе - выдает "EXPIRED"
		/// </summary>
		/// <param name="autoUpdate">Нужно ли автоматически обновить токен, если он устарел</param>
		/// <returns>true - валидный, false - устарел</returns>
		public bool tokenIsValid(bool autoUpdate = true)
		{
			try
			{
				if (String.IsNullOrEmpty(Properties.Settings.Default.tokenExpireDateTime)
					|| String.IsNullOrEmpty(Properties.Settings.Default.token))
				{
					Properties.Settings.Default.token = "";
					Properties.Settings.Default.tokenExpireDateTime = (new DateTime()).ToString(STRING_FORMAT_DATE_TIME);
					Properties.Settings.Default.Save();
				}
				DateTime tokenExpireDateTime = DateTime.ParseExact(Properties.Settings.Default.tokenExpireDateTime, STRING_FORMAT_DATE_TIME, null);
				if (tokenExpireDateTime > DateTime.Now) // если время НЕ вышло
					return true; // токен нормальный
				else // если время вышло
				{
					if (autoUpdate) // если включено автообновление токена
					{
						getNewToken(); // обнови токен
						return true; // токен нормальный
					} else
						return false; // токен устарел
				}
			}
			catch (Exception ex)
			{
				// ERROR
				setError(ex.Message, "checkToken");
				Log("checkToken: " + ex.Message + ex.StackTrace);
				Properties.Settings.Default.token = ""; // обнули токен
				Properties.Settings.Default.tokenExpireDateTime = null; // обнули токен
				Properties.Settings.Default.Save();
				return false; // токен устарел
			}
		}

		/// <summary>
		/// Выдает хранимый токен, если токен устарел: если указан параметр true - сначала пытается обновить токен и выдает новый, иначе - выдает "EXPIRED"
		/// </summary>
		/// <param name="autoUpdate">Нужно ли автоматически обновить токен, если он устарел</param>
		/// <returns>Строка токена либо null при неудаче</returns>
		public string getToken(bool autoUpdate = true)
		{
			try
			{
				if (tokenIsValid(autoUpdate))
					return Properties.Settings.Default.token;
				else
				{
					return "EXPIRED";
				}

			}
			catch (Exception ex)
			{
				// ERROR
				setError(ex.Message, "getToken");
				Log("getToken: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Выдает хранимые датувремя валидности токена в формате yyyy.MM.dd HH:mm:ss
		/// </summary>
		/// <returns>Строка времени либо null при неудаче</returns>
		public string getTokenExpireDateTime()
		{
			try
			{
				return Properties.Settings.Default.tokenExpireDateTime;
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getTokenExpireDateTime");
				Log("getTokenExpireDateTime: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Получает user_id по сертификату от ЧЗ
		/// </summary>
		/// <returns>user_id</returns>
		public string getCurrentUserId()
		{
			try
			{
				User res = getCurrentUserInfo();
				return res.user_id;
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getCurrentUserId");
				Log("getCurrentUserId: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Выбираем сертификаты для работы
		/// </summary>
		public void selectSertificates()
		{
			selectUserCertificate();
			selectCertificateForeign();
		}

		/// <summary>
		/// Выбор только сертификата пользователя
		/// </summary>
		/// <returns>Название сертификата</returns>
		public string selectUserCertificate()
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser); // Создаем объект хранилища
			X509Certificate2Collection scollection;
			// Получаем сертификат ОТПРАВИТЕЛЯ (НАШ)
			store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly); // Открываем хранилище
			scollection = X509Certificate2UI.SelectFromCollection(store.Certificates, "Выберите сертификат ОТПРАВИТЕЛЯ (НАШ)", "Выберите сертификат ОТПРАВИТЕЛЯ", X509SelectionFlag.SingleSelection); // Выбор

			if (scollection.Count > 0)
			{ 
				userCert = scollection[0]; // Записываем сертификат

				// Дальше пишем серийники и сохраняем настройки
				Properties.Settings.Default.userCertSN = userCert.SerialNumber;
				Properties.Settings.Default.Save();
			} else
			{
				// обнуляем
				Properties.Settings.Default.userCertSN = null;
				Properties.Settings.Default.Save();
			}
			return getUserCertificateName();
		}

		/// <summary>
		/// Выбор только сертификата получателя
		/// </summary>
		/// <returns>Название сертификата</returns>
		public string selectCertificateForeign()
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser); // Создаем объект хранилища
			X509Certificate2Collection scollection;
			// Получаем сертификат ОТПРАВИТЕЛЯ (НАШ)
			store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly); // Открываем хранилище
			scollection = X509Certificate2UI.SelectFromCollection(store.Certificates, "Выберите сертификат ПОЛУЧАТЕЛЯ (ЧЕСТНЫЙ ЗНАК)", "Выберите сертификат ПОЛУЧАТЕЛЯ", X509SelectionFlag.SingleSelection); // Выбор

			if (scollection.Count > 0)
			{
				certForeign = scollection[0]; // Записываем сертификат

				// Дальше пишем серийники и сохраняем настройки
				Properties.Settings.Default.certForeignSN = certForeign.SerialNumber;
				Properties.Settings.Default.Save();
			} else
			{
				// обнуляем
				Properties.Settings.Default.certForeignSN = null;
				Properties.Settings.Default.Save();
			}
			return getCertificateForeignName();
		}

		/// <summary>
		/// Получение имени выбранного сертификата пользователя
		/// </summary>
		/// <returns></returns>
		public string getUserCertificateName()
		{
			if (userCert != null)
				return userCert.SubjectName.Name;
			else
				return "НЕ ВЫБРАН";
		}

		/// <summary>
		/// Получение имени выбранного сертификата получателя
		/// </summary>
		/// <returns></returns>
		public string getCertificateForeignName()
		{
			if (certForeign != null)
				return certForeign.SubjectName.Name;
			else
				return "НЕ ВЫБРАН";
		}



		/// <summary>
		/// Получает квитанцию документа по идентификатору
		/// </summary>
		/// <param name="document_id">Идентификатор документа</param>
		/// <returns>Квитанция в виде массива байт</returns>
		public byte[] getDocumentTicket(string document_id)
		{
			try
			{
				string link = getDocumentTicketLink(document_id);
				byte[] file = webdavDownload(link);
				return file;
			}
			catch (Exception ex)
			{
				// Если вдруг ошибка - выведем ее в лог
				setError(ex.Message, "getDocumentTicket");
				Log("getDocumentTicket: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Получает ссылку на квитанцию документа по идентификатору
		/// </summary>
		/// <param name="document_id">Идентификатор документа</param>
		/// <returns>Ссылка на квитанцию</returns>
		public string getDocumentTicketLink(string document_id)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("GET", URL_HTTPS + "/api/v1/documents/" + document_id + "/ticket", headers.ToArray());
				// Десериализую строку ответа, превращая ее в класс
				ResponseDocumentLink res = jss.Deserialize<ResponseDocumentLink>(str);

				Log("\r\n--- Document Ticket: " + str + "\r\n");

				return res.link;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getDocumentTicketLink", "WebException");
				Log("getDocumentTicketLink: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getDocumentTicketLink");
				Log("getDocumentTicketLink: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Получает данные документа по идентификатору
		/// </summary>
		/// <param name="document_id">Идентификатор документа</param>
		/// <returns>Документ</returns>
		public Document getDocumentData(string document_id)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("GET", URL_HTTPS + "/api/v1/documents/" + document_id, headers.ToArray());
				// Десериализую строку ответа, превращая ее в класс
				Document res = jss.Deserialize<Document>(str);

				Log("\r\n--- Document: " + str + "\r\n");

				return res;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getDocumentData", "WebException");
				Log("getDocumentData: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getDocumentData");
				Log("getDocumentData: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Получение списка документов по request_id
		/// </summary>
		/// <param name="request_id">Идентификатор реквеста</param>
		/// <returns>Список документов</returns>
		public ResponseDocumentList getDocumentsByRequest(string request_id)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("GET", URL_HTTPS + "/api/v1/documents/request/" + request_id, headers.ToArray());
				// Десериализую строку ответа, превращая ее в класс 
				ResponseDocumentList res = jss.Deserialize<ResponseDocumentList>(str);

				Log("\r\n--- Docs: " + str + "\r\n");

				return res;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getDocumentsByRequest", "WebException");
				Log("getDocumentsByRequest: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getDocumentsByRequest");
				Log("getDocumentsByRequest: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Получает один документ из набора документов
		/// </summary>
		/// <param name="resp">Полученный с сервера массив документов в специальной структуре</param>
		/// <param name="index">Индекс получаемого документа</param>
		/// <returns>Один документ</returns>
		public Document getIncomeDocument(ResponseDocumentList resp, int index)
		{
			if (index < resp.total)
				return resp.documents[index];
			return null;
		}

		/// <summary>
		/// Получает от ЧЗ список входящих документов
		/// </summary>
		/// <param name="start_from">Индекс первой записи в списке входящих документов</param>
		/// <param name="count">Сколько документов получить</param>
		/// <param name="filter">Фильтер отбора документов</param>
		/// <returns>Массив документов</returns>
		public ResponseDocumentList getIncomeDocuments(int start_from = 0, int count = 10, DocFilter filter = null)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestDocumentList req = new RequestDocumentList();
				if (filter == null)
					filter = new DocFilter();
				req.filter = filter;
				req.start_from = start_from;
				req.count = count;

				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.NullValueHandling = NullValueHandling.Ignore;
				settings.DefaultValueHandling = DefaultValueHandling.Ignore;
				string body = JsonConvert.SerializeObject(req, settings);// "{\"filter\":{},\"start_from\":0,\"count\": 10}";

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("POST", URL_HTTPS + "/api/v1/documents/income", headers.ToArray(), body);
				// Десериализую строку ответа, превращая ее в класс
				ResponseDocumentList res = jss.Deserialize<ResponseDocumentList>(str);

				Log("\r\n--- Income Docs: " + str + "\r\n");

				return res;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getIncomeDocuments", "WebException");
				Log("getIncomeDocuments: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getIncomeDocuments");
				Log("getIncomeDocuments: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Скачивание файла по идентификату
		/// </summary>
		/// <param name="document_id">Идентификатор документа</param>
		/// <returns>Массив байт файла</returns>
		public byte[] downloadFile(string document_id)
		{
			try
			{
				string link = getDocumentLinkById(document_id);
				byte[] file = webdavDownload(link);
				return file;
			}
			catch (Exception ex)
			{
				// Если вдруг ошибка - выведем ее в лог
				setError(ex.Message, "downloadFile");
				Log("downloadFile: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Получает ссылку для скачивания файла
		/// </summary>
		/// <param name="doc_id">id документа</param>
		/// <returns>Ссылка</returns>
		public string getDocumentLinkById(string document_id)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("GET", URL_HTTPS + "/api/v1/documents/download/" + document_id, headers.ToArray());
				// Десериализую строку ответа, превращая ее в класс
				ResponseDocumentLink res = jss.Deserialize<ResponseDocumentLink>(str);

				Log("\r\n--- Doc Link: " + str + "\r\n");

				return res.link;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getIncomeDocuments", "WebException");
				Log("getIncomeDocuments: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getDocumentLinkById");
				Log("getDocumentLinkById: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Скачивает файл по ссылке по протоколу Webdav. Если включено логгирование, сохраняет файл в %temp%.
		/// </summary>
		/// <param name="link">Ссылка для скачивания</param>
		/// <returns>Файл в виде массива байт</returns>
		public byte[] webdavDownload(string link)
		{
			try
			{
				string token = getToken(true);

				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("Authorization", "token " + token);

				var clientParams = new WebDavClientParams {
					BaseAddress = new Uri(link),
					DefaultRequestHeaders = d
				};
				IWebDavClient _client = new WebDavClient(clientParams);
				long? size = 0;
				var prop = _client.Propfind(link).Result;
				foreach (var res in prop.Resources)
				{
					size = res.ContentLength;
					if (size != null)
						Log("\r\n--- Downloading " + size + " bytes\r\n");
				}
				WebDavStreamResponse result = _client.GetRawFile(link).Result;

				if (String.IsNullOrEmpty(Properties.Settings.Default.tempDir))
					showSettings();

				string dest = Properties.Settings.Default.tempDir + "\\doc" + link.Substring(link.LastIndexOf("/") + 1).Replace("-", "") + ".txt";
				FileStream output = new FileStream(dest, FileMode.Create);
				result.Stream.CopyTo(output);
				output.Close();

				byte[] bytes = File.ReadAllBytes(dest);

				if (!Properties.Settings.Default.logEnabled)
					File.Delete(dest);

				return bytes;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "webdavDownload", "WebException");
				Log("webdavDownload: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "webdavDownload");
				Log("webdavDownload: " + ex.Message + ex.StackTrace);
			}
			return null;
		}


		/// <summary>
		/// Получает от ЧЗ список исходящих документов
		/// </summary>
		/// <param name="start_from">Индекс первой записи в списке входящих документов</param>
		/// <param name="count">Сколько документов получить</param>
		/// <param name="filter">Фильтер отбора документов</param>
		/// <returns>Массив документов</returns>
		public ResponseDocumentList getOutcomeDocuments(int start_from = 0, int count = 10, DocFilter filter = null)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestDocumentList req = new RequestDocumentList();
				if (filter == null)
					filter = new DocFilter();
				req.filter = filter;
				req.start_from = start_from;
				req.count = count;

				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.NullValueHandling = NullValueHandling.Ignore;
				settings.DefaultValueHandling = DefaultValueHandling.Ignore;
				string body = JsonConvert.SerializeObject(req, settings);// "{\"filter\":{},\"start_from\":0,\"count\": 10}";

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("POST", URL_HTTPS + "/api/v1/documents/outcome", headers.ToArray(), body);
				// Десериализую строку ответа, превращая ее в класс
				ResponseDocumentList res = jss.Deserialize<ResponseDocumentList>(str);

				Log("\r\n--- Outcome Docs: " + str + "\r\n");

				return res;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getOutcomeDocuments", "WebException");
				Log("getOutcomeDocuments: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getOutcomeDocuments");
				Log("getOutcomeDocuments: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Загружает документ в ЧЗ
		/// </summary>
		/// <param name="fileIn">Массив байт файла</param>
		/// <returns>document_id</returns>
		public string uploadDocument(byte[] fileIn)
		{
			try
			{
				if (fileIn.Length < 1048576)
				{
					return uploadSmallFile(fileIn);
				} else
				{
					ResponseUploadLargeDocumentLink docInfo = getLinkForDocumentUpload(fileIn);
					if (webdavUpload(fileIn, docInfo.link))
						if (finishUploadLargeFile(docInfo.document_id))
							return docInfo.document_id;

					setError("Что-то пошло не так при загрузке большого документа", "uploadDocument", "WebException");
					Log("uploadDocument: неопознанная ошибка");
				}
				return null;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "uploadDocument", "WebException");
				Log("uploadDocument: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "uploadDocument");
				Log("uploadDocument: " + ex.Message + ex.StackTrace);
			}
			return null;
		}


		/// <summary>
		/// Загружает документ в ЧЗ из строкив UTF8
		/// </summary>
		/// <param name="fileUTF8String">Файл в виде строки UTF8</param>
		/// <returns>document_id</returns>
		public string uploadDocumentFromUTF8String(string fileUTF8String)
		{
			try
			{
				byte[] file = Encoding.UTF8.GetBytes(fileUTF8String);
				return uploadDocument(file);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "uploadDocumentFromUTF8String");
				Log("uploadDocumentFromUTF8String: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Получение подписи исходящего документа по идентификатору
		/// </summary>
		/// <param name="document_id">Идентификатор документа</param>
		/// <returns>Подпись строкой</returns>
		public string getUploadedDocumentSignatureById(string document_id)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("GET", URL_HTTPS + "/api/v1/documents/" + document_id + "/signature", headers.ToArray());

				Log("\r\n--- Doc Sign: " + str + "\r\n");

				return str;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getDocumentSignatureById", "WebException");
				Log("getDocumentSignatureById: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getDocumentSignatureById");
				Log("getDocumentSignatureById: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Загружает в ЧЗ документ менее 1мб
		/// </summary>
		/// <param name="fileBase64">Документ в BASE64</param>
		/// <returns>document_id</returns>
		public string uploadSmallFile(byte[] fileIn)
		{
			try
			{
				string fileBase64 = Convert.ToBase64String(fileIn); ;

				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestUploadSmallDocument req = new RequestUploadSmallDocument();
				req.document = fileBase64;
				req.sign = signBytes(fileIn);
				req.request_id = Guid.NewGuid().ToString();
				string body = jss.Serialize(req);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("POST", URL_HTTPS + "/api/v1/documents/send", headers.ToArray(), body);
				// Десериализую строку ответа, превращая ее в класс
				ResponseUploadSmallDocument res = jss.Deserialize<ResponseUploadSmallDocument>(str);

				Log("\r\n--- Uploaded document_id: " + res.document_id + "\r\n");

				return res.document_id;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "uploadSmallFile", "WebException");
				Log("uploadSmallFile: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "uploadSmallFile");
				Log("uploadSmallFile: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Получает ссылку и document_id для загрузки большого файла
		/// </summary>
		/// <param name="fileIn">Массив байт файла</param>
		/// <returns>Объек, содержащий ссылку и document_id</returns>
		public ResponseUploadLargeDocumentLink getLinkForDocumentUpload(byte[] fileIn)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestUploadLargeDocumentLink req = new RequestUploadLargeDocumentLink();
				var sha256 = SHA256.Create();
				req.hash_sum = Convert.ToBase64String(sha256.ComputeHash(fileIn));
				req.sign = sign(Encoding.UTF8.GetString(fileIn));
				req.request_id = Guid.NewGuid().ToString();
				string body = jss.Serialize(req);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("POST", URL_HTTPS + "/api/v1/documents/send", headers.ToArray(), body);
				// Десериализую строку ответа, превращая ее в класс
				ResponseUploadLargeDocumentLink res = jss.Deserialize<ResponseUploadLargeDocumentLink>(str);

				Log("\r\n--- Uploading documentInfo: " + str + "\r\n");

				return res;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getLinkForDocumentUpload", "WebException");
				Log("getLinkForDocumentUpload: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getLinkForDocumentUpload");
				Log("getLinkForDocumentUpload: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Загружает файл (представленный массивом байт) по ссылке
		/// </summary>
		/// <param name="file">Файл для загрузки</param>
		/// <param name="link">Ссылка для загрузки</param>
		/// <returns>Получилось или нет</returns>
		public bool webdavUpload(byte[] file, string link)
		{
			try
			{
				string token = getToken(true);

				Dictionary<string, string> d = new Dictionary<string, string>();
				d.Add("Authorization", "token " + token);
				d.Add("Host", "api.sb.mdlp.crpt.ru");
				d.Add("Content-Type", "application/xml");
				d.Add("Cache-Control", "no-cache");

				var clientParams = new WebDavClientParams {
					BaseAddress = new Uri(link),
					DefaultRequestHeaders = d
				};
				IWebDavClient _client = new WebDavClient(clientParams);
				Stream stream = new MemoryStream(file);
				var result = _client.PutFile(link, stream);
				return true;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "webdavUpload", "WebException");
				Log("webdavUpload: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "webdavUpload");
				Log("webdavUpload: " + ex.Message + ex.StackTrace);
			}
			return false;
		}

		/// <summary>
		/// Завершает отправку документа
		/// </summary>
		/// <param name="document_id"></param>
		/// <returns>Получилось или нет</returns>
		public bool finishUploadLargeFile(string document_id)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestUploadLargeDocumentFinish req = new RequestUploadLargeDocumentFinish();
				req.document_id = document_id;
				string body = jss.Serialize(req);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("POST", URL_HTTPS + "/api/v1/documents/send_finished", headers.ToArray(), body);
				// Десериализую строку ответа, превращая ее в класс
				ResponseUploadLargeDocumentFinish res = jss.Deserialize<ResponseUploadLargeDocumentFinish>(str);

				Log("\r\n--- Document uploaded, request_id: " + res.request_id + "\r\n");

				return true;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "finishUploadLargeFile", "WebException");
				Log("finishUploadLargeFile: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "finishUploadLargeFile");
				Log("finishUploadLargeFile: " + ex.Message + ex.StackTrace);
			}
			return true;
		}

		/// <summary>
		/// Отменяет загрузку большого документа
		/// </summary>
		/// <param name="document_id"></param>
		/// <param name="request_id"></param>
		/// <returns></returns>
		public bool cancelUploadLargeFile(string document_id, string request_id)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestUploadLargeDocumentCancel req = new RequestUploadLargeDocumentCancel();
				req.document_id = document_id;
				req.request_id = request_id;
				string body = jss.Serialize(req);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Authorization: token " + token);
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string str = doRequest("POST", URL_HTTPS + "/api/v1/documents/cancel", headers.ToArray(), body);

				Log("\r\n--- Document upload canceled \r\n");

				return true;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "cancelUploadLargeFile", "WebException");
				Log("cancelUploadLargeFile: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "cancelUploadLargeFile");
				Log("cancelUploadLargeFile: " + ex.Message + ex.StackTrace);
			}
			return true;
		}


		/// <summary>
		/// Получает новый хранимый токен от ЧЗ по НАШему сертификату
		/// </summary>
		/// <param name="clientId">Неизвестно</param>
		/// <param name="clientSecret">Неизвестно</param>
		/// <returns>Строка токена либо null при неудаче</returns>
		public string getNewToken(string clientId = null, string clientSecret = null)
		{
			try
			{
				ResponseAuth a = getUserAuthCode(clientId, clientSecret);
				ResponseSession s = getSessionToken(a.code);
				Properties.Settings.Default.token = s.token;
				Properties.Settings.Default.tokenExpireDateTime = DateTime.Now.AddMinutes(int.Parse(s.life_time)).ToString(STRING_FORMAT_DATE_TIME);
				Properties.Settings.Default.Save();
				return s.token;
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getNewToken");
				Log("getNewToken: " + ex.Message + ex.StackTrace);
				return null;
			}
		}

		/// <summary>
		/// Получает code авторизации по НАШему сертификату
		/// </summary>
		/// <param name="clientId">Неизвестно</param>
		/// <param name="clientSecret">Неизвестно</param>
		/// <returns>Объект класса, содержащий code авторизации, либо null при неудаче</returns>
		public ResponseAuth getUserAuthCode(string clientId = null, string clientSecret = null)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestAuth authreq = new RequestAuth();

				if (String.IsNullOrEmpty(clientId))
					authreq.client_id = Properties.Settings.Default.client_id;
				else
					authreq.client_id = clientId;

				if (String.IsNullOrEmpty(clientSecret))
					authreq.client_secret = Properties.Settings.Default.client_secret;
				else
					authreq.client_secret = clientSecret;

				authreq.auth_type = "SIGNED_CODE";
				authreq.user_id = userCert.Thumbprint.ToLower();
				string body = jss.Serialize(authreq);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json;charset=UTF-8");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string res = doRequest("POST", URL_HTTP + "/api/v1/auth", headers.ToArray(), body);

				// Десериализую строку ответа, превращая ее в класс
				ResponseAuth authResponse = jss.Deserialize<ResponseAuth>(res);

				Log("\r\n--- authcode " + authResponse.code + "\r\n");
				return authResponse;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getUserAuthCode", "WebException");
				Log("getUserAuthCode: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getUserAuthCode");
				Log("getUserAuthCode: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Получает от ЧЗ новый токен по code, путем подписывания code НАШим сертификатом
		/// </summary>
		/// <param name="sessionRequestCode">code полученный в запросе на авторизацию</param>
		/// <returns>Объект класса, содержащий токен и время жизни, либо null при неудаче</returns>
		public ResponseSession getSessionToken(string sessionRequestCode)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;
				// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
				RequestSessionResident sessReq = new RequestSessionResident();
				sessReq.code = sessionRequestCode;
				sessReq.signature = sign(sessionRequestCode);
				string body = jss.Serialize(sessReq);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json;charset=UTF-8");

				// Делаю запрос передавая в него тип запроса, URL, шапку, тело
				string res = doRequest("POST", URL_HTTP + "/api/v1/token", headers.ToArray(), body);
				// Десериализую строку ответа, превращая ее в класс
				ResponseSession sessionResponse = jss.Deserialize<ResponseSession>(res);

				Log("\r\n--- token: " + sessionResponse.token + "  lifetime: " + sessionResponse.life_time + "\r\n");
				return sessionResponse;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getSessionToken", "WebException");
				Log("getSessionToken: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getSessionToken");
				Log("getSessionToken: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Получает пользователя по сертификату от ЧЗ
		/// </summary>
		/// <returns>User</returns>
		public User getCurrentUserInfo()
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;

				// Получаю токен
				string token = getToken(true);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");
				headers.Add("Authorization: token " + token);

				// Делаю запрос передавая в него тип запроса, URL, шапку
				string res = doRequest("GET", URL_HTTPS + "/api/v1/users/current", headers.ToArray());
				// Десериализую строку ответа, превращая ее в класс
				ResponseUser resUser = jss.Deserialize<ResponseUser>(res);

				Log("\r\n--- user: " + res + "\r\n");
				return resUser.user;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getCurrentUserInfo", "WebException");
				Log("getCurrentUserInfo: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getCurrentUserInfo");
				Log("getCurrentUserInfo: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Получает информацию о группе (включая права) по group_id от ЧЗ
		/// </summary>
		/// <param name="group_id">id группы</param>
		/// <param name="token">Необязательный, для выполнения запроса от другого пользователя (например от тестового юзера)</param>
		/// <returns>Group</returns>
		public Group getGroupInfo(string group_id, string token = null)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;

				// Получаю токен, если не указан в параметрах
				if (token == null)
					token = getToken(true);

				// Создаю и заполняю массив хидеров
				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json");
				headers.Add("Host: " + URL_HTTPS.Substring(URL_HTTPS.LastIndexOf("/") + 1)); // для тестового контура получится "api.stage.mdlp.crpt.ru", для песочницы "api.sb.mdlp.crpt.ru"
				headers.Add("Cache-Control: no-cache");
				headers.Add("Authorization: token " + token);

				// Делаю запрос передавая в него тип запроса, URL, шапку
				string res = doRequest("GET", URL_HTTPS + "/api/v1/rights/" + group_id, headers.ToArray());
				// Десериализую строку ответа, превращая ее в класс
				ResponseGroup resGroup = jss.Deserialize<ResponseGroup>(res);

				Log("\r\n--- group: " + res + "\r\n");
				return resGroup.group;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getGroupInfo", "WebException");
				Log("getGroupInfo: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getGroupInfo");
				Log("getGroupInfo: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Регистрация нового пользователя в ЧЗ
		/// </summary>
		/// <param name="first_name">имя</param>
		/// <param name="last_name">Фамилия</param>
		/// <param name="middle_name">Отчество</param>
		/// <param name="email">Мыло</param>
		/// <param name="sessionToken">Токен сессии</param>
		/// <param name="sys_id">Учетная система, в которой надо его зарегистрировать</param>
		/// <param name="cert">X509Certificate2 сертификат пользователя</param>
		/// <returns>user_id зарегистрированного пользователя</returns>
		public string registerNewUser(string first_name, string last_name, string middle_name, string email,
			string sessionToken, string sys_id = null, X509Certificate2 cert = null)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;

				RequestRegisterUser regreq = new RequestRegisterUser();
				if (String.IsNullOrEmpty(sys_id))
					regreq.sys_id = "9dedee17-e43a-47f1-910e-3a88ff6bc81b";
				else
					regreq.sys_id = sys_id;
				regreq.first_name = first_name;
				regreq.last_name = last_name;
				regreq.middle_name = middle_name;
				regreq.email = email;
				if (cert == null)
					regreq.public_cert = Convert.ToBase64String(userCert.Export(X509ContentType.Cert)).Replace("\r\n", "");
				else
					regreq.public_cert = Convert.ToBase64String(cert.Export(X509ContentType.Cert)).Replace("\r\n", "");

				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json;charset=UTF-8");
				headers.Add("Authorization: token " + sessionToken);

				string res = doRequest("POST", URL_HTTP + "/api/v1/registration/user_resident", headers.ToArray(), jss.Serialize(regreq));
				ResponseRegisterUser regUserResponse = jss.Deserialize<ResponseRegisterUser>(res);
				Log("\r\n--- user_id = " + regUserResponse.user_id + "\r\n");
				return regUserResponse.user_id;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "registerNewTestUser", "WebException");
				Log("registerNewTestUser: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "registerNewTestUser");
				Log("registerNewTestUser: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Создание новой группы
		/// </summary>
		/// <param name="group_name">Название</param>
		/// <param name="sessionToken">Токен сессии</param>
		/// <param name="rights">Права</param>
		/// <returns>group_id</returns>
		public string createNewGroup(string group_name, string sessionToken, string[] rights = null)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;

				RequestRegisterGroup regreq = new RequestRegisterGroup();
				regreq.group_name = group_name;
				if (rights != null)
					regreq.rights = rights;
				string body = jss.Serialize(regreq);

				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json;charset=UTF-8");
				headers.Add("Authorization: token " + sessionToken);

				string res = doRequest("POST", URL_HTTP + "/api/v1/rights/create_group", headers.ToArray(), body);
				ResponseRegisterGroup regUserResponse = jss.Deserialize<ResponseRegisterGroup>(res);
				Log("\r\n--- group_id = " + regUserResponse.group_id + "\r\n");
				return regUserResponse.group_id;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "createNewGroup", "WebException");
				Log("createNewGroup: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "createNewGroup");
				Log("createNewGroup: " + ex.Message + ex.StackTrace);
			}
			return null;
		}

		/// <summary>
		/// Регистрирует пользователя в группе в ЧЗ
		/// </summary>
		/// <param name="group_id">Где</param>
		/// <param name="user_id">Кого</param>
		/// <param name="sessionToken">токен сессии</param>
		/// <returns>Получилось или нет</returns>
		public bool addUserToGroup(string group_id, string user_id, string sessionToken)
		{
			try
			{
				URL_HTTP = Properties.Settings.Default.urlHttp;
				URL_HTTPS = Properties.Settings.Default.urlHttps;

				RequestUserToGroup regreq = new RequestUserToGroup();
				regreq.user_id = user_id;

				List<string> headers = new List<string>();
				headers.Add("Content-Type: application/json;charset=UTF-8");
				headers.Add("Authorization: token " + sessionToken);

				doRequest("POST", URL_HTTP + "/api/v1/rights/" + group_id + "/user_add", headers.ToArray(), jss.Serialize(regreq));
				return true;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "addUserToGroup", "WebException");
				Log("addUserToGroup: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "addUserToGroup");
				Log("addUserToGroup: " + ex.Message + ex.StackTrace);
			}
			return false;
		}

		/// <summary>
		/// Получение токена тестового юзера первого тестового контура от ЧЗ
		/// </summary>
		/// <returns>token</returns>
		public string getTestUserToken()
		{
			try
			{
				ResponseAuth authres;
				{
					// Создаю и заполняю тело запроса (у меня с помощью описанного класса для сериализации, у Вас может быть хоть простая генерация нужной строки конкатенацией)
					RequestAuth req = new RequestAuth();
					req.auth_type = "PASSWORD";
					req.client_id = "01db16f2-9a4e-4d9f-b5e8-c68f12566fd5";
					req.client_secret = "9199fe04-42c3-4e81-83b5-120eb5f129f2";
					req.user_id = "starter_resident_1";
					string body = jss.Serialize(req);

					// Создаю и заполняю массив хидеров
					List<string> headers = new List<string>();
					headers.Add("Content-Type: application/json;charset=UTF-8");

					// Делаю запрос передавая в него тип запроса, URL, шапку, тело
					string str = doRequest("POST", "http://api.stage.mdlp.crpt.ru/api/v1/auth", headers.ToArray(), body);
					// Десериализую строку ответа, превращая ее в класс
					authres = jss.Deserialize<ResponseAuth>(str);
				}

				ResponseSession sessres;
				{
					RequestSessionNonResident sesreq = new RequestSessionNonResident();
					sesreq.code = authres.code;
					sesreq.password = "password";
					string body = jss.Serialize(sesreq);

					// Создаю и заполняю массив хидеров
					List<string> headers = new List<string>();
					headers.Add("Content-Type: application/json;charset=UTF-8");

					string str = doRequest("POST", "http://api.stage.mdlp.crpt.ru/api/v1/token", headers.ToArray(), body);
					// Десериализую строку ответа, превращая ее в класс
					sessres = jss.Deserialize<ResponseSession>(str);
					Log("\r\n--- TEST_USER token = " + sessres.token + "\r\n");
				}
				return sessres.token;
			}
			catch (WebException ex)
			{
				setError((ex.Response as HttpWebResponse).StatusCode + " " + (ex.Response as HttpWebResponse).StatusDescription + " " + ex.Message, "getTestUserToken", "WebException");
				Log("getTestUserToken: " + ex.Message + ex.StackTrace);
			}
			catch (Exception ex)
			{
				setError(ex.Message, "getTestUserToken");
				Log("getTestUserToken: " + ex.Message + ex.StackTrace);
			}
			return null;
		}


		/*
	/// <summary>
	/// Регистрирует нового пользователя
	/// </summary>
	/// <param name="first_name"></param>
	/// <param name="last_name"></param>
	/// <param name="middle_name"></param>
	/// <param name="email"></param>
	/// <returns>Строка user_id либо null</returns>
	public string registerNewUser(string first_name, string last_name, string middle_name, string email, string token = null)
	{
		try
		{
			RegisterUserResponse u;
			if (token == null)
			{
				// если токена нет во входных данных - сначала его получим
				AuthResponse a = getTestUserAuthCode();
				SessionResponse s = getSessionToken(a.code); // от имени тестового юзера (нерезидента)
				u = getRegisterUser(first_name, last_name, middle_name, email, s.token);
			} else
				u = getRegisterUser(first_name, last_name, middle_name, email, token);
			return u.user_id;
		}
		catch (Exception ex)
		{
			Log("registerNewUser: " + ex.Message);
			return null;
		}
	}


	public AuthResponse getTestUserAuthCode()
	{
		try
		{
			AuthTestUserRequest authreq = new AuthTestUserRequest();
			string res = httpPostRequest((new JavaScriptSerializer()).Serialize(authreq), 
				"http://api.stage.mdlp.crpt.ru/api/v1/auth");
			AuthResponse authResponse = (new JavaScriptSerializer()).Deserialize<AuthResponse>(res);
			Log("--- authcode " + authResponse.code);
			return authResponse;
		}
		catch (Exception ex)
		{
			Log("getTestUserAuthCode: " + ex.Message);
			return null;
		}
	}

	public RegisterUserResponse regUserResponse;
	public RegisterUserResponse getRegisterUser(string first_name, string last_name, string middle_name, string email, 
		string sessionToken = null)
	{
		try
		{
			RegisterUserRequest regReq = new RegisterUserRequest();
			regReq.first_name = first_name;
			regReq.last_name = last_name;
			regReq.middle_name = middle_name;
			regReq.email = email;
			regReq.public_cert = Convert.ToBase64String(userCert.Export(X509ContentType.Cert)).Replace("\r\n", "");

			List<string> headers = new List<string>();
			headers.Add("Authorization: token " + sessionToken);

			string res = doRequest("POST",
				"http://api.stage.mdlp.crpt.ru/api/v1/registration/user_resident", headers.ToArray(), jss.Serialize(regReq));
			MessageBox.Show(res);
			regUserResponse = (new JavaScriptSerializer()).Deserialize<RegisterUserResponse>(res);
			Log("--- user_id = " + regUserResponse.user_id);
			Properties.Settings.Default.main_user_id = regUserResponse.user_id;
			Properties.Settings.Default.Save();
			return regUserResponse;
		}
		catch (Exception ex)
		{
			Log("getRegisterUser: " + ex.Message);
			return null;
		}
	}
	/**/
	}
}