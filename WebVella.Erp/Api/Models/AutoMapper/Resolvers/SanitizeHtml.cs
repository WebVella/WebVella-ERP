using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WebVella.Erp.Api.Models.AutoMapper.Resolvers
{
    public static class SanitizeHtml
    {
        private static readonly Regex Tags = new Regex("<[^>]*(>|$)",
                                                        RegexOptions.Singleline | RegexOptions.ExplicitCapture |
                                                        RegexOptions.Compiled);

        private static readonly Regex Whitelist =
            new Regex(
                @"
	^</?(b(lockquote)?|code|d(d|t|l|el)|em|h(1|2|3)|i|kbd|li|ol|p(re)?|s(ub|up|trong|trike)?|ul)>$|
	^<(b|h)r\s?/?>$",
                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled |
                RegexOptions.IgnorePatternWhitespace);

        private static readonly Regex WhitelistA =
            new Regex(
                @"
	^<a\s
	href=""(\#\d+|(https?|ftp)://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+)""
	(\stitle=""[^""<>]+"")?\s?>$|
	^</a>$",
                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled |
                RegexOptions.IgnorePatternWhitespace);

        private static readonly Regex WhitelistImg =
            new Regex(
                @"
	^<img\s
	src=""https?://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+""
	(\swidth=""\d{1,3}"")?
	(\sheight=""\d{1,3}"")?
	(\salt=""[^""<>]*"")?
	(\stitle=""[^""<>]*"")?
	\s?/?>$",
                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled |
                RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// 	sanitize any potentially dangerous tags from the provided raw HTML input using
        /// 	a whitelist based approach, leaving the "safe" HTML tags
        /// </summary>
        public static string Sanitize(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;
            // match every HTML tag in the input
            MatchCollection tags = Tags.Matches(html);
            for (int i = tags.Count - 1; i > -1; i--)
            {
                Match tag = tags[i];
                string tagname = tag.Value.ToLowerInvariant();
                if (!(Whitelist.IsMatch(tagname) || WhitelistA.IsMatch(tagname) || WhitelistImg.IsMatch(tagname)))
                {
                    html = html.Remove(tag.Index, tag.Length);
                }
            }
            return html;
        }
    }
}