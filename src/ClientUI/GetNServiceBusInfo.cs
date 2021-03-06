﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NServiceBus;

public static class GetNServiceBusInfo
{
	public static HtmlString OutputNServiceBusInfo(this IHtmlHelper<dynamic> _)
	{
		var nsbAssembly = typeof(IEndpointInstance).Assembly;
		var att = nsbAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute)).OfType<AssemblyFileVersionAttribute>().First();
		var v = new Version(att.Version);
		var script = $"<script>window.NSB_VERSION = '{v.Major}.{v.Minor}.{v.Build}';</script>";
		return new HtmlString(script);
	}
}