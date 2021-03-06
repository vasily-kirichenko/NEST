﻿using System.Collections.Generic;

namespace RawClientGenerator
{
	public class CsharpMethod
	{
		public string ReturnType { get; set; }
		public string FullName { get; set; }
		public string QueryStringParamName { get; set; }
		public string DescriptorType { get; set; }
		public string DescriptorTypeGeneric { get; set; }
		public string HttpMethod { get; set; }
		public string Documentation { get; set; }
		public string Path { get; set; }
		public string Arguments { get; set; }
		public IEnumerable<ApiUrlPart> Parts { get; set; }
		public ApiUrl Url { get; set; }
	}
}