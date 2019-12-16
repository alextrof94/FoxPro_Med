using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

namespace FoxPro_Med
{
	//POST http://api.stage.mdlp.crpt.ru/api/v1/token
	//Content-Type: application/json;charset=UTF-8
	class RequestSession
	{
	}

	class RequestSessionResident : RequestSession
	{
		public string code { get; set; }
		public string signature { get; set; }
	}

	class RequestSessionNonResident :RequestSession
	{
		public string code { get; set; }
		public string password { get; set; }
	}
}
