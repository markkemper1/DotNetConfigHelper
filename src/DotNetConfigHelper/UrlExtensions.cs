using System;
using System.Net;

namespace DotNetConfigHelper
{
	public static class UriExtensions
	{
		public static string Username(this Uri uri)
		{
			if (String.IsNullOrWhiteSpace(uri.UserInfo)) return null;
			var passwordInfo = uri.UserInfo.Substring(0, uri.UserInfo.IndexOf(':'));
			return uri.UserEscaped ? passwordInfo : Uri.UnescapeDataString(passwordInfo);
		}

		public static string Password(this Uri uri)
		{
			if (String.IsNullOrWhiteSpace(uri.UserInfo)) return null;
			var passwordInfo = uri.UserInfo.Substring(uri.UserInfo.IndexOf(':') + 1);
			return uri.UserEscaped ? passwordInfo : Uri.UnescapeDataString(passwordInfo);
		}

		public static ICredentials Credentials(this Uri uri)
		{
			var u = uri.Username();
			var p = uri.Password();
			return u != null && p != null ? new NetworkCredential(uri.Username(), uri.Password()) : null;
		}

		public static NetworkCredential NetworkCredentials(this Uri uri)
		{
			var u = uri.Username();
			var p = uri.Password();
			return u != null && p != null ? new NetworkCredential(uri.Username(), uri.Password()) : null;
		}

	}
}
