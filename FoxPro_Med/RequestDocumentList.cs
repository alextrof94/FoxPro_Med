﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;

namespace FoxPro_Med
{
	[Guid("90c1cef4-3e6c-4922-889c-0188365369a4")]
	[ComVisible(true)]
	public interface IRequestDocumentList
	{
		DocFilter filter { get; set; }
		[DefaultValue(-1)]
		int start_from { get; set; }
		int count { get; set; }
	}

	[Guid("565d68c7-a24d-4fa7-b761-6f2f209b9381")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class RequestDocumentList :IRequestDocumentList
	{
		public DocFilter filter { get; set; }
		[DefaultValue(-1)]
		public int start_from { get; set; }
		public int count { get; set; }
	}


	[Guid("7358463e-f328-4a5a-943a-2776b147aed0")]
	[ComVisible(true)]
	public interface IResponseDocumentList
	{
		List<Document> documents { get; set; }
		int total { get; set; }
	}

	[Guid("8C034F6A-1D3F-4DB8-BC99-B73873D8C297")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class ResponseDocumentList :IResponseDocumentList
	{
		public List<Document> documents { get; set; }
		public int total { get; set; }
	}

	/*
	[Guid("9e3e44ea-5f48-4028-90cf-202779f4b97e")]
	[ComVisible(true)]
	public interface IResponseDocumentListIncome
	{
		List<Document> documents { get; set; }
		int total { get; set; }
	}

	[Guid("55c9efb8-5947-4a5e-b55b-79e5ec9cf821")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class ResponseDocumentListIncome :IResponseDocumentListIncome
	{
		public List<Document> documents { get; set; }
		public int total { get; set; }
	}


	[Guid("3f76ee6f-a88e-4748-a8e6-3d53a9d6e301")]
	[ComVisible(true)]
	public interface IResponseDocumentListOutcome
	{
		List<Document> documents { get; set; }
		int total { get; set; }
	}

	[Guid("ca99e651-3a93-429c-adb1-45bb0299e8b0")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class ResponseDocumentListOutcome :IResponseDocumentListOutcome
	{
		public List<Document> documents { get; set; }
		public int total { get; set; }
	}
	*/
}
