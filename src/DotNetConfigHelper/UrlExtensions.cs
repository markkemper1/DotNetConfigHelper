using System;
using System.Net;

namespace DotNetConfigHelper
{
    public static class UriExtensions
    {
        public static string Username(this Uri uri)
        {
            var passwordInfo = uri.UserInfo.Substring(0, uri.UserInfo.IndexOf(':'));
            return !String.IsNullOrWhiteSpace(uri.UserInfo)
                ? uri.UserEscaped ? passwordInfo : Uri.UnescapeDataString(passwordInfo)
                : null;
        }

        public static string Password(this Uri uri)
        {
            var passwordInfo = uri.UserInfo.Substring(uri.UserInfo.IndexOf(':') + 1);
            return !String.IsNullOrWhiteSpace(uri.UserInfo)
                ?  uri.UserEscaped ? passwordInfo : Uri.UnescapeDataString(passwordInfo)
                : null;
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
