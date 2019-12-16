using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxPro_Med
{
	//POST http://api.stage.mdlp.crpt.ru/api/v1/rights/create_group
	//Content-Type: application/json;charset=UTF-8
	//Authorization: token 13b5b046-0cd7-4e1c-8409-da9541986d1c // REPLACE
	public class RequestRegisterGroup
	{
		public string group_name { get; set; }
		public string[] rights = { "UPLOAD_DOCUMENT", "OUTCOME_LIST", "INCOME_LIST", "DOWNLOAD_DOCUMENT",
			"MANAGE_ACCOUNTS", "VIEW_ACCOUNTS", "REESTR_ALL",
			"MANAGE_TRUSTED_PARTNERS", "VIEW_TRUSTED_PARTNERS", "MANAGE_BRANCH", "MANAGE_SAFE_WAREHOUSE",
			"VIEW_REGISTRATION_FOREIGN_COUNTERPARTY_LOG", "MANAGE_FOREIGN_COUNTERPARTY", "MANAGE_MEMBER"};
		/*
			"REESTR_FEDERAL_SUBJECT", "REESTR_EGRUL",
			"REESTR_EGRIP", "REESTR_REFP", "REESTR_DUES", "REESTR_PROD_LICENSES", "REESTR_PHARM_LICENSES",
			"REESTR_ESKLP", "REESTR_GS1", "REESTR_FIAS", "REESTR_SGTIN", "REESTR_OWNED_SSCC_SGTIN", "REESTR_MED_PRODUCTS",
			"REESTR_COUNTERPARTY", "REESTR_REGISTRATION_DEVICES", "REESTR_VIRTUAL_STORAGE"
		/**/
	}

	class ResponseRegisterGroup
	{
		public string group_id { get; set; }
	}
}
