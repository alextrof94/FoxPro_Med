namespace FoxPro_Med
{
	public class ResponseUser
	{
		public User user;
	}

	public class User
	{
		public string user_id { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public bool is_admin { get; set; }
		public string position { get; set; }
		public string sys_id { get; set; }
		public string login { get; set; }
		public string[] groups { get; set; }
	}
}