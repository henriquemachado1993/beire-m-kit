using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeireMKit.Domain.Extensions
{
    public static class HttpRequestExtension
    {
        public static string GetBaseUrl(this HttpRequest request)
        {
            if (request == null)
                return string.Empty;

            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? -1
            };

            if (uriBuilder.Uri.IsDefaultPort)
                uriBuilder.Port = -1;

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
