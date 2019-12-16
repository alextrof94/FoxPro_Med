using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxPro_Med
{
	public class RequestAuth
	{
		public string client_id { get; set; }
		public string client_secret { get; set; }
		public string user_id { get; set; } // НЕ user_id. Это thumbprint зарегистрированного сертификата УКЭП
		public string auth_type { get; set; }
	}

	public class ResponseAuth
	{
		public string code { get; set; }
	}
}
