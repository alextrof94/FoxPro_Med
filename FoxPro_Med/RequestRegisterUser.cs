using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxPro_Med
{
	//POST http://api.stage.mdlp.crpt.ru/api/v1/registration/user_resident
	//Content-Type: application/json;charset=UTF-8
	//Authorization: token 13b5b046-0cd7-4e1c-8409-da9541986d1c // REPLACE
	class RequestRegisterUser
	{
		public string sys_id = "9dedee17-e43a-47f1-910e-3a88ff6bc81b";
		public string public_cert { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string middle_name { get; set; }
		public string email { get; set; }
		//public string position = "Программист";
	}

	public class ResponseRegisterUser
	{
		public string user_id { get; set; }
	}
}
