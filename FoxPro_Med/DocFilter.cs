using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace FoxPro_Med
{
	[Guid("6eb04c30-a671-4148-ae34-28ab4b87850b")]
	[ComVisible(true)]
	public interface IDocFilter
	{
		string start_date { get; set; }
		string end_date { get; set; }
		string document_id { get; set; }
		string request_id { get; set; }
		int doc_type { get; set; }
		string doc_status { get; set; }
		int file_uploadtype { get; set; }
		string processed_date_from { get; set; }
		string processed_date_to { get; set; }
		string sender_id { get; set; }
		string receiver_id { get; set; }
	}

	[DataContract]
	[Guid("c51a07c0-fa4e-417d-8126-6e015fae43d1")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class DocFilter : IDocFilter
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string start_date { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string end_date { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string document_id { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string request_id { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DefaultValue(-1)]
		public int doc_type { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string doc_status { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DefaultValue(-1)]
		public int file_uploadtype { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string processed_date_from { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string processed_date_to { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string sender_id { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string receiver_id { get; set; }
		// Тип данных Date принимается в формате: yyyy-MM-dd HH:mm:ss
		
		public DocFilter()
		{
			doc_type = -1;
			file_uploadtype = -1;
		}
	}
}
