using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FoxPro_Med
{

	[Guid("364C5E66-4412-48E3-8BD8-7B2BF09E8925")]
	[ComVisible(true)]
	public interface IDocument
	{
		string request_id { get; set; }
		string document_id { get; set; }
		string date { get; set; }
		string processed_date { get; set; }
		string sender { get; set; }
		string receiver { get; set; }
		string sys_id { get; set; }
		string doc_type { get; set; }
		string doc_status { get; set; }
		int? file_uploadtype { get; set; }
		string version { get; set; }
		// income
		string sender_sys_id { get; set; }
		// outcome
		string device_id { get; set; }
		string skzkm_origin_msg_id { get; set; }
	}

	/*
	[Guid("364C5E66-4412-48E3-8BD8-7B2BF09E8924")]
	[ComVisible(true)]
	public interface IIncomeDocument
	{
		string sender_sys_id { get; set; }
	}

	[Guid("364C5E66-4412-48E3-8BD8-7B2BF09E8923")]
	[ComVisible(true)]
	public interface IOutcomeDocument
	{
		string device_id { get; set; }
		string skzkm_origin_msg_id { get; set; }
	}
	*/

	[Guid("8C034F6A-1D3F-4DB8-BC99-B73873D8C210")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class Document: IDocument
	{
		public string request_id { get; set; }
		public string document_id { get; set; }
		public string date { get; set; }
		public string processed_date { get; set; }
		public string sender { get; set; }
		public string receiver { get; set; }
		public string sys_id { get; set; }
		public string doc_type { get; set; }
		public string doc_status { get; set; }
		public int? file_uploadtype { get; set; }
		public string version { get; set; }
		// income
		public string sender_sys_id { get; set; }
		// outcome
		public string device_id { get; set; }
		public string skzkm_origin_msg_id { get; set; }
	}
	/*

	[Guid("8C034F6A-1D3F-4DB8-BC99-B73873D8C299")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class IncomeDocument : Document, IDocument, IIncomeDocument
	{
		public string sender_sys_id { get; set; }
	}

	[Guid("8C034F6A-1D3F-4DB8-BC99-B73873D8C298")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class OutcomeDocument : Document, IOutcomeDocument, IDocument
	{
		public string device_id { get; set; }
		public string skzkm_origin_msg_id { get; set; }
	}
	*/
}
