namespace FoxPro_Med
{
	internal class ResponseGroup
	{
		public Group group { get; set; }
	}

	public class Group
	{
		public string group_id { get; set; }
		public string group_name { get; set; }
		public string[] rights { get; set; }
	}
}