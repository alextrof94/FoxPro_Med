namespace FoxPro_Med
{

	public class RequestUploadDocument
	{
	}

	/* FOR SMALL */
	public class RequestUploadSmallDocument
	{
		public string document { get; set; }
		public string sign { get; set; }
		public string request_id { get; set; }
	}

	public class ResponseUploadSmallDocument
	{
		public string document_id { get; set; }
	}

	/* FOR LARGE */
	public class RequestUploadLargeDocumentLink
	{
		public string sign { get; set; }
		public string hash_sum { get; set; }
		public string request_id { get; set; }
	}

	public class ResponseUploadLargeDocumentLink
	{
		public string document_id { get; set; }
		public string link { get; set; }
	}

	public class RequestUploadLargeDocumentFinish
	{
		public string document_id { get; set; }
	}

	public class ResponseUploadLargeDocumentFinish
	{
		public string request_id { get; set; }
	}

	public class RequestUploadLargeDocumentCancel
	{
		public string document_id { get; set; }
		public string request_id { get; set; }
	}
}