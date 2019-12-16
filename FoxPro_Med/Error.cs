using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FoxPro_Med
{
	[Guid("66610bb1-17d2-4d5b-859b-3e54dd88824c")]
	[ComVisible(true)]
	public interface IError
	{
		string message { get; set; }
		string type { get; set; }
		string from { get; set; }
	}

	public class Error
	{
		public string message { get; set; }
		public string type { get; set; }
		public string from { get; set; }
	}
}
