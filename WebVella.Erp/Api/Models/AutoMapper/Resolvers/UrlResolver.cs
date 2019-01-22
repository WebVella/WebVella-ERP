using System;

namespace WebVella.Erp.Api.Models.AutoMapper.Resolvers
{
    public class UrlResolver
    {
        public static string Resolve(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            if (url.StartsWith("http://") || url.StartsWith("https://"))
                return url;

            return "http://" + url;
        }
    }
}