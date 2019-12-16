using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxPro_Med
{
	// POST http://api.stage.mdlp.crpt.ru/api/v1/rights/a5d134fe-43cb-42a1-85eb-61a5cfcffef9/user_add // REPLACE
	// Content-Type: application/json;charset=UTF-8
	// Authorization: token 13b5b046-0cd7-4e1c-8409-da9541986d1c // REPLACE
	class RequestUserToGroup
	{
		public string user_id { get; set; }
	}
	// Response is http "200 OK"
}
