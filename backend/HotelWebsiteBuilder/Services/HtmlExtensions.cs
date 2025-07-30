using System.Text.RegularExpressions;
using HotelWebsiteBuilder.Models;

namespace HotelWebsiteBuilder.Services
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// HTML içeriğinde belirtilen ID'ye sahip elementi günceller
        /// </summary>
        public static string UpdateElementById(this string html, string elementId, string newContent)
        {
            // Önce mevcut element'i bul ve içeriğini güncelle
            var pattern = $@"<([^>]*)\s+id\s*=\s*[""']{Regex.Escape(elementId)}[""'][^>]*>(.*?)</[^>]*>";
            var match = Regex.Match(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            
            if (match.Success)
            {
                var tagName = match.Groups[1].Value.Split(' ')[0]; // İlk tag adını al
                var replacement = $"<{tagName} id=\"{elementId}\">{newContent}</{tagName}>";
                return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            
            // ID bulunamazsa, yeni element ekle
            return html.Replace("</body>", $"<div id=\"{elementId}\">{newContent}</div>\n</body>");
        }

        /// <summary>
        /// HTML içeriğinde belirtilen class'a sahip elementleri günceller
        /// </summary>
        public static string UpdateElementByClass(this string html, string className, string newContent)
        {
            var pattern = $@"<[^>]*class\s*=\s*[""'][^""']*{Regex.Escape(className)}[^""']*[""'][^>]*>(.*?)</[^>]*>";
            var replacement = $"<div class=\"{className}\">{newContent}</div>";
            
            return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// HTML içeriğinde belirtilen tag'leri günceller
        /// </summary>
        public static string UpdateElementByTag(this string html, string tagName, string newContent)
        {
            var pattern = $@"<{Regex.Escape(tagName)}[^>]*>(.*?)</{Regex.Escape(tagName)}>";
            var replacement = $"<{tagName}>{newContent}</{tagName}>";
            
            return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// HTML içeriğinde src attribute'unu günceller
        /// </summary>
        public static string UpdateImageSrc(this string html, string imageId, string newSrc)
        {
            var pattern = $@"<img[^>]*id\s*=\s*[""']{Regex.Escape(imageId)}[""'][^>]*>";
            var replacement = $"<img id=\"{imageId}\" src=\"{newSrc}\" alt=\"{imageId}\">";
            
            return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// HTML içeriğinde href attribute'unu günceller
        /// </summary>
        public static string UpdateLinkHref(this string html, string linkId, string newHref)
        {
            var pattern = $@"<a[^>]*id\s*=\s*[""']{Regex.Escape(linkId)}[""'][^>]*>";
            var replacement = $"<a id=\"{linkId}\" href=\"{newHref}\">";
            
            return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// HTML içeriğinde meta description'ı günceller
        /// </summary>
        public static string UpdateMetaDescription(this string html, string description)
        {
            var pattern = @"<meta[^>]*name\s*=\s*[""']description[""'][^>]*>";
            var replacement = $"<meta name=\"description\" content=\"{description}\">";
            
            if (Regex.IsMatch(html, pattern, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
            }
            
            // Meta description yoksa ekle
            return html.Replace("<head>", $"<head>\n<meta name=\"description\" content=\"{description}\">");
        }

        /// <summary>
        /// HTML içeriğinde title'ı günceller
        /// </summary>
        public static string UpdateTitle(this string html, string title)
        {
            var pattern = @"<title[^>]*>(.*?)</title>";
            var replacement = $"<title>{title}</title>";
            
            if (Regex.IsMatch(html, pattern, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
            }
            
            // Title yoksa ekle
            return html.Replace("<head>", $"<head>\n<title>{title}</title>");
        }

        /// <summary>
        /// HTML içeriğinde placeholder text'leri günceller
        /// </summary>
        public static string UpdatePlaceholderText(this string html, string placeholder, string newText)
        {
            return html.Replace(placeholder, newText);
        }

        /// <summary>
        /// HTML içeriğinde belirtilen pattern'e uyan tüm text'leri günceller
        /// </summary>
        public static string UpdateTextByPattern(this string html, string pattern, string replacement)
        {
            return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// HTML içeriğinde belirtilen data attribute'unu günceller
        /// </summary>
        public static string UpdateDataAttribute(this string html, string elementId, string dataName, string dataValue)
        {
            var pattern = $@"<[^>]*id\s*=\s*[""']{Regex.Escape(elementId)}[""'][^>]*>";
            var replacement = $"<div id=\"{elementId}\" data-{dataName}=\"{dataValue}\">";
            
            return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// HTML içeriğinde belirtilen style attribute'unu günceller
        /// </summary>
        public static string UpdateStyle(this string html, string elementId, string style)
        {
            var pattern = $@"<[^>]*id\s*=\s*[""']{Regex.Escape(elementId)}[""'][^>]*>";
            var replacement = $"<div id=\"{elementId}\" style=\"{style}\">";
            
            return Regex.Replace(html, pattern, replacement, RegexOptions.IgnoreCase);
        }
    }
} 