using System.Net.Http;
using System.Threading.Tasks;
using HotelWebsiteBuilder.Models;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HotelWebsiteBuilder.Services
{
    public class HotelSiteCloneService
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public HotelSiteCloneService(HttpClient httpClient, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _environment = environment;
            _context = context;
        }

        public async Task<HotelSiteCloneResponse> CloneHotelSiteAsync(string sourceUrl, int hotelId)
        {
            try
            {
                Console.WriteLine($"Otel sitesi klonlanıyor: {sourceUrl} -> Hotel ID: {hotelId}");

                // 1. URL'yi doğrula
                if (string.IsNullOrEmpty(sourceUrl) || !Uri.IsWellFormedUriString(sourceUrl, UriKind.Absolute))
                {
                    throw new Exception("Geçersiz URL formatı. Lütfen geçerli bir web sitesi adresi girin.");
                }

                // 2. Veritabanından otel bilgilerini al
                var hotel = await _context.Hotels.FindAsync(hotelId);
                if (hotel == null)
                {
                    throw new Exception($"Hotel ID {hotelId} bulunamadı.");
                }

                Console.WriteLine($"Otel bulundu: {hotel.HotelName}");

                // 3. URL'den HTML'i al
                Console.WriteLine("HTML içeriği indiriliyor...");
                var response = await _httpClient.GetAsync(sourceUrl);
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(htmlContent))
                {
                    throw new Exception("Web sitesinden içerik alınamadı.");
                }

                Console.WriteLine($"HTML içeriği alındı. Boyut: {htmlContent.Length} karakter");

                // 4. HTML'i parse et
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // 5. Otel bilgilerini değiştir
                Console.WriteLine("Otel bilgileri güncelleniyor...");
                var updatedHtml = ReplaceHotelInfoInHtml(doc, hotel);

                // 6. Asset'leri indir ve yolları güncelle
                Console.WriteLine("Asset'ler indiriliyor...");
                var finalHtml = await DownloadAndUpdateAssets(doc, hotel.HotelName);

                // 7. Klasörü oluştur ve dosyaları kaydet
                var hotelSlug = CreateSlug(hotel.HotelName);
                Console.WriteLine($"Hotel slug: {hotelSlug}");
                Console.WriteLine($"WebRootPath: {_environment.WebRootPath}");
                
                var outputDir = Path.Combine(_environment.WebRootPath, "sites", hotelSlug);
                Console.WriteLine($"Output Directory: {outputDir}");
                
                Directory.CreateDirectory(outputDir);
                Console.WriteLine($"Directory created: {outputDir}");
                
                var indexPath = Path.Combine(outputDir, "index.html");
                await File.WriteAllTextAsync(indexPath, finalHtml);

                Console.WriteLine($"Dosya kaydedildi: {indexPath}");

                // 8. Sonuç URL'ini oluştur
                var siteUrl = $"/sites/{hotelSlug}/index.html";
                
                Console.WriteLine($"Otel sitesi başarıyla klonlandı: {siteUrl}");
                
                return new HotelSiteCloneResponse
                {
                    Success = true,
                    SiteUrl = siteUrl,
                    HotelName = hotel.HotelName,
                    OutputDirectory = outputDir,
                    Message = "Site başarıyla klonlandı ve otel bilgileri güncellendi."
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP hatası: {ex.Message}");
                return new HotelSiteCloneResponse
                {
                    Success = false,
                    Message = $"Web sitesine erişilemiyor: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Otel sitesi klonlanırken hata oluştu: {ex.Message}");
                return new HotelSiteCloneResponse
                {
                    Success = false,
                    Message = $"Hata: {ex.Message}"
                };
            }
        }

        private string ReplaceHotelInfoInHtml(HtmlDocument doc, Hotel hotel)
        {
            // 1. META TAG'LERİ GÜNCELLE
            UpdateMetaTags(doc, hotel);

            // 2. HOTEL NAME DEĞİŞTİR
            if (!string.IsNullOrEmpty(hotel.HotelName))
            {
                // Title tag'ini değiştir
                var titleNode = doc.DocumentNode.SelectSingleNode("//title");
                if (titleNode != null)
                {
                    titleNode.InnerHtml = hotel.HotelName;
                }

                // H1 tag'lerini değiştir
                var h1Nodes = doc.DocumentNode.SelectNodes("//h1");
                if (h1Nodes != null)
                {
                    foreach (var h1 in h1Nodes)
                    {
                        h1.InnerHtml = hotel.HotelName;
                    }
                }

                // H2 tag'lerini değiştir (otel ile ilgili olanları)
                var h2Nodes = doc.DocumentNode.SelectNodes("//h2");
                if (h2Nodes != null)
                {
                    foreach (var h2 in h2Nodes)
                    {
                        var currentText = h2.InnerText.Trim().ToLower();
                        if (IsHotelRelatedText(currentText))
                        {
                            h2.InnerHtml = hotel.HotelName;
                        }
                    }
                }

                // H3 tag'lerini değiştir (otel ile ilgili olanları)
                var h3Nodes = doc.DocumentNode.SelectNodes("//h3");
                if (h3Nodes != null)
                {
                    foreach (var h3 in h3Nodes)
                    {
                        var currentText = h3.InnerText.Trim().ToLower();
                        if (IsHotelRelatedText(currentText))
                        {
                            h3.InnerHtml = hotel.HotelName;
                        }
                    }
                }

                // Logo ve brand alanlarını değiştir
                var logoElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'logo') or contains(@class, 'brand') or contains(@id, 'logo') or contains(@id, 'brand')]");
                if (logoElements != null)
                {
                    foreach (var logo in logoElements)
                    {
                        if (string.IsNullOrEmpty(logo.InnerText.Trim()) || IsHotelRelatedText(logo.InnerText.Trim().ToLower()))
                        {
                            logo.InnerHtml = hotel.HotelName;
                        }
                    }
                }

                // Diğer text içeriklerini değiştir
                ReplaceTextInDocument(doc, hotel.HotelName, 
                    "Example Domain", "Example", "Domain", "Royalty Ezel", "Royalty", "Ezel", 
                    "Hotel Name", "Otel Adı", "hotel name", "otel adı", "hotel", "otel",
                    "Hotel", "Otel", "HOTEL", "OTEL", "Hotel Name:", "Otel Adı:",
                    "Hotel Name:", "Otel Adı:", "Hotel:", "Otel:", "HOTEL:", "OTEL:");
            }

            // 3. PHONE DEĞİŞTİR
            if (!string.IsNullOrEmpty(hotel.Phone))
            {
                // Tel linklerini değiştir
                var telLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'tel:')]");
                if (telLinks != null)
                {
                    foreach (var link in telLinks)
                    {
                        link.SetAttributeValue("href", $"tel:{hotel.Phone}");
                        link.InnerHtml = hotel.Phone;
                    }
                }

                // Phone text'lerini değiştir
                ReplaceTextInDocument(doc, hotel.Phone, 
                    "phone", "telefon", "Phone", "Telefon", "+90 555 123 4567",
                    "Phone:", "Telefon:", "PHONE:", "TELEFON:", "phone:", "telefon:",
                    "Hotel Phone", "Otel Telefonu", "HOTEL PHONE", "OTEL TELEFONU",
                    "hotel phone", "otel telefonu", "Hotel Phone:", "Otel Telefonu:",
                    "Contact Phone", "İletişim Telefonu", "CONTACT PHONE", "İLETİŞİM TELEFONU",
                    "contact phone", "iletişim telefonu", "Contact Phone:", "İletişim Telefonu:");

                // Span ve div içindeki telefon numaralarını değiştir
                var phoneElements = doc.DocumentNode.SelectNodes("//*[contains(text(), '+90') or contains(text(), 'phone') or contains(text(), 'telefon')]");
                if (phoneElements != null)
                {
                    foreach (var element in phoneElements)
                    {
                        var currentText = element.InnerText.Trim();
                        if (currentText.Contains("+90") || currentText.Contains("phone") || currentText.Contains("telefon"))
                        {
                            element.InnerHtml = hotel.Phone;
                        }
                    }
                }
            }

            // 4. EMAIL DEĞİŞTİR
            if (!string.IsNullOrEmpty(hotel.Email))
            {
                // Mailto linklerini değiştir
                var mailtoLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'mailto:')]");
                if (mailtoLinks != null)
                {
                    foreach (var link in mailtoLinks)
                    {
                        link.SetAttributeValue("href", $"mailto:{hotel.Email}");
                        link.InnerHtml = hotel.Email;
                    }
                }

                // Email text'lerini değiştir
                ReplaceTextInDocument(doc, hotel.Email, 
                    "email", "e-posta", "Email", "E-posta", "info@example.com",
                    "Email:", "E-posta:", "EMAIL:", "E-POSTA:", "email:", "e-posta:",
                    "Hotel Email", "Otel E-postası", "HOTEL EMAIL", "OTEL E-POSTASI",
                    "hotel email", "otel e-postası", "Hotel Email:", "Otel E-postası:",
                    "Contact Email", "İletişim E-postası", "CONTACT EMAIL", "İLETİŞİM E-POSTASI",
                    "contact email", "iletişim e-postası", "Contact Email:", "İletişim E-postası:");

                // Span ve div içindeki email adreslerini değiştir
                var emailElements = doc.DocumentNode.SelectNodes("//*[contains(text(), '@') or contains(text(), 'email') or contains(text(), 'e-posta')]");
                if (emailElements != null)
                {
                    foreach (var element in emailElements)
                    {
                        var currentText = element.InnerText.Trim();
                        if (currentText.Contains("@") || currentText.Contains("email") || currentText.Contains("e-posta"))
                        {
                            element.InnerHtml = hotel.Email;
                        }
                    }
                }
            }

            // 5. ADRES DEĞİŞTİR
            if (!string.IsNullOrEmpty(hotel.Address))
            {
                ReplaceTextInDocument(doc, hotel.Address, 
                    "address", "adres", "Address", "Adres", "123 Example Street", "Sultanahmet Meydanı",
                    "Address:", "Adres:", "ADDRESS:", "ADRES:", "address:", "adres:",
                    "Location:", "Konum:", "LOCATION:", "KONUM:", "location:", "konum:",
                    "Hotel Address", "Otel Adresi", "HOTEL ADDRESS", "OTEL ADRESİ",
                    "hotel address", "otel adresi", "Hotel Address:", "Otel Adresi:");
                
                // Address elementlerini değiştir
                var addressElements = doc.DocumentNode.SelectNodes("//address");
                if (addressElements != null)
                {
                    foreach (var address in addressElements)
                    {
                        address.InnerHtml = hotel.Address;
                    }
                }

                // Span ve div içindeki adres bilgilerini değiştir
                var addressTextElements = doc.DocumentNode.SelectNodes("//*[contains(text(), 'address') or contains(text(), 'adres') or contains(text(), 'location') or contains(text(), 'konum')]");
                if (addressTextElements != null)
                {
                    foreach (var element in addressTextElements)
                    {
                        var currentText = element.InnerText.Trim().ToLower();
                        if (IsAddressRelatedText(currentText))
                        {
                            element.InnerHtml = hotel.Address;
                        }
                    }
                }
            }

            // 6. AÇIKLAMA DEĞİŞTİR
            if (!string.IsNullOrEmpty(hotel.Description))
            {
                ReplaceTextInDocument(doc, hotel.Description, 
                    "description", "açıklama", "Description", "Açıklama", "Welcome to our hotel", "Otelimize hoş geldiniz",
                    "Description:", "Açıklama:", "DESCRIPTION:", "AÇIKLAMA:", "description:", "açıklama:",
                    "Hotel Description", "Otel Açıklaması", "HOTEL DESCRIPTION", "OTEL AÇIKLAMASI",
                    "hotel description", "otel açıklaması", "Hotel Description:", "Otel Açıklaması:",
                    "About Hotel", "Otel Hakkında", "ABOUT HOTEL", "OTEL HAKKINDA",
                    "about hotel", "otel hakkında", "About Hotel:", "Otel Hakkında:",
                    "Welcome to", "Hoş geldiniz", "WELCOME TO", "HOŞ GELDİNİZ",
                    "welcome to", "hoş geldiniz", "Welcome to our hotel", "Otelimize hoş geldiniz");
                
                // Meta description'ı güncelle
                var metaDesc = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
                if (metaDesc != null)
                {
                    metaDesc.SetAttributeValue("content", hotel.Description);
                }

                // P tag'lerindeki açıklama metinlerini değiştir
                var pElements = doc.DocumentNode.SelectNodes("//p");
                if (pElements != null)
                {
                    foreach (var p in pElements)
                    {
                        var currentText = p.InnerText.Trim().ToLower();
                        if (IsDescriptionRelatedText(currentText))
                        {
                            p.InnerHtml = hotel.Description;
                        }
                    }
                }
            }

            // 7. WEBSITE URL DEĞİŞTİR
            if (!string.IsNullOrEmpty(hotel.Website))
            {
                var websiteLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'http') and not(contains(@href, 'mailto:')) and not(contains(@href, 'tel:'))]");
                if (websiteLinks != null)
                {
                    foreach (var link in websiteLinks)
                    {
                        var href = link.GetAttributeValue("href", "");
                        if (ShouldReplaceWebsiteLink(href))
                        {
                            link.SetAttributeValue("href", hotel.Website);
                        }
                    }
                }
            }

            // 8. SOSYAL MEDYA LİNKLERİ GÜNCELLE
            UpdateSocialMediaLinks(doc, hotel);

            // 9. CHECK-IN/CHECK-OUT SAATLERİ
            if (!string.IsNullOrEmpty(hotel.CheckInTime))
            {
                ReplaceTextInDocument(doc, hotel.CheckInTime, "check-in", "giriş", "Check-in", "Giriş", "14:00");
            }

            if (!string.IsNullOrEmpty(hotel.CheckOutTime))
            {
                ReplaceTextInDocument(doc, hotel.CheckOutTime, "check-out", "çıkış", "Check-out", "Çıkış", "11:00");
            }

            // 10. YILDIZ RATING
            if (hotel.StarRating > 0)
            {
                var starElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'star') or contains(@class, 'rating')]");
                if (starElements != null)
                {
                    foreach (var star in starElements)
                    {
                        var currentText = star.InnerText.Trim();
                        if (currentText.Contains("★") || currentText.Contains("star") || currentText.Contains("rating"))
                        {
                            star.InnerHtml = new string('★', hotel.StarRating);
                        }
                    }
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

        private void UpdateMetaTags(HtmlDocument doc, Hotel hotel)
        {
            // Meta title
            var metaTitle = doc.DocumentNode.SelectSingleNode("//meta[@name='title']");
            if (metaTitle != null && !string.IsNullOrEmpty(hotel.MetaTitle))
            {
                metaTitle.SetAttributeValue("content", hotel.MetaTitle);
            }

            // Meta description
            var metaDesc = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (metaDesc != null && !string.IsNullOrEmpty(hotel.MetaDescription))
            {
                metaDesc.SetAttributeValue("content", hotel.MetaDescription);
            }

            // Meta keywords
            var metaKeywords = doc.DocumentNode.SelectSingleNode("//meta[@name='keywords']");
            if (metaKeywords != null && !string.IsNullOrEmpty(hotel.MetaKeywords))
            {
                metaKeywords.SetAttributeValue("content", hotel.MetaKeywords);
            }

            // Open Graph tags
            var ogTitle = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
            if (ogTitle != null)
            {
                ogTitle.SetAttributeValue("content", hotel.HotelName);
            }

            var ogDesc = doc.DocumentNode.SelectSingleNode("//meta[@property='og:description']");
            if (ogDesc != null && !string.IsNullOrEmpty(hotel.Description))
            {
                ogDesc.SetAttributeValue("content", hotel.Description);
            }

            var ogUrl = doc.DocumentNode.SelectSingleNode("//meta[@property='og:url']");
            if (ogUrl != null && !string.IsNullOrEmpty(hotel.Website))
            {
                ogUrl.SetAttributeValue("content", hotel.Website);
            }
        }

        private void UpdateSocialMediaLinks(HtmlDocument doc, Hotel hotel)
        {
            // Facebook
            if (!string.IsNullOrEmpty(hotel.Facebook))
            {
                var fbLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'facebook.com') or contains(@href, 'fb.com')]");
                if (fbLinks != null)
                {
                    foreach (var link in fbLinks)
                    {
                        link.SetAttributeValue("href", hotel.Facebook);
                    }
                }
            }

            // Instagram
            if (!string.IsNullOrEmpty(hotel.Instagram))
            {
                var igLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'instagram.com')]");
                if (igLinks != null)
                {
                    foreach (var link in igLinks)
                    {
                        link.SetAttributeValue("href", hotel.Instagram);
                    }
                }
            }

            // Twitter
            if (!string.IsNullOrEmpty(hotel.Twitter))
            {
                var twLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'twitter.com') or contains(@href, 'x.com')]");
                if (twLinks != null)
                {
                    foreach (var link in twLinks)
                    {
                        link.SetAttributeValue("href", hotel.Twitter);
                    }
                }
            }

            // LinkedIn
            if (!string.IsNullOrEmpty(hotel.LinkedIn))
            {
                var liLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'linkedin.com')]");
                if (liLinks != null)
                {
                    foreach (var link in liLinks)
                    {
                        link.SetAttributeValue("href", hotel.LinkedIn);
                    }
                }
            }

            // YouTube
            if (!string.IsNullOrEmpty(hotel.YouTube))
            {
                var ytLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'youtube.com')]");
                if (ytLinks != null)
                {
                    foreach (var link in ytLinks)
                    {
                        link.SetAttributeValue("href", hotel.YouTube);
                    }
                }
            }
        }

        private bool IsHotelRelatedText(string text)
        {
            var hotelKeywords = new[] { "hotel", "otel", "resort", "palace", "inn", "lodge", "suite", "accommodation", "konaklama" };
            return hotelKeywords.Any(keyword => text.Contains(keyword));
        }

        private bool IsAddressRelatedText(string text)
        {
            var addressKeywords = new[] { "address", "adres", "location", "konum", "street", "sokak", "avenue", "cadde" };
            return addressKeywords.Any(keyword => text.Contains(keyword));
        }

        private bool IsDescriptionRelatedText(string text)
        {
            var descriptionKeywords = new[] { "description", "açıklama", "about", "hakkında", "welcome", "hoş geldiniz", "experience", "deneyim" };
            return descriptionKeywords.Any(keyword => text.Contains(keyword));
        }

        private bool ShouldReplaceWebsiteLink(string href)
        {
            if (string.IsNullOrEmpty(href)) return false;
            
            var excludeDomains = new[] { "google.com", "facebook.com", "twitter.com", "instagram.com", "linkedin.com", "youtube.com" };
            return !excludeDomains.Any(domain => href.Contains(domain));
        }

        private void ReplaceTextInDocument(HtmlDocument doc, string newValue, params string[] searchTerms)
        {
            // Önce en uzun terimlerden başla (daha spesifik olanlar)
            var sortedTerms = searchTerms.OrderByDescending(t => t.Length).ToArray();
            
            foreach (var searchTerm in sortedTerms)
            {
                if (string.IsNullOrEmpty(searchTerm)) continue;
                
                // Text node'ları bul
                var nodes = doc.DocumentNode.SelectNodes($"//text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{searchTerm.ToLower()}')]");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        if (node.InnerText.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        {
                            // Sadece tam kelime eşleşmelerini değiştir
                            var pattern = $@"\b{Regex.Escape(searchTerm)}\b";
                            node.InnerHtml = Regex.Replace(node.InnerHtml, pattern, newValue, RegexOptions.IgnoreCase);
                        }
                    }
                }

                // Attribute'larda da değiştir
                var elementsWithAttributes = doc.DocumentNode.SelectNodes("//*[@*]");
                if (elementsWithAttributes != null)
                {
                    foreach (var element in elementsWithAttributes)
                    {
                        foreach (var attr in element.Attributes)
                        {
                            if (attr.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                            {
                                var pattern = $@"\b{Regex.Escape(searchTerm)}\b";
                                attr.Value = Regex.Replace(attr.Value, pattern, newValue, RegexOptions.IgnoreCase);
                            }
                        }
                    }
                }
            }
        }

        private async Task<string> DownloadAndUpdateAssets(HtmlDocument doc, string hotelName)
        {
            var assetsDir = Path.Combine(_environment.WebRootPath, "sites", CreateSlug(hotelName), "assets");
            Directory.CreateDirectory(assetsDir);

            // CSS dosyalarını indir
            var cssLinks = doc.DocumentNode.SelectNodes("//link[@rel='stylesheet']");
            if (cssLinks != null)
            {
                foreach (var link in cssLinks)
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href) && href.StartsWith("http"))
                    {
                        var newPath = await DownloadAsset(href, assetsDir, "css");
                        link.SetAttributeValue("href", newPath);
                    }
                }
            }

            // JS dosyalarını indir
            var jsScripts = doc.DocumentNode.SelectNodes("//script[@src]");
            if (jsScripts != null)
            {
                foreach (var script in jsScripts)
                {
                    var src = script.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src) && src.StartsWith("http"))
                    {
                        var newPath = await DownloadAsset(src, assetsDir, "js");
                        script.SetAttributeValue("src", newPath);
                    }
                }
            }

            // Resimleri indir
            var imgElements = doc.DocumentNode.SelectNodes("//img");
            if (imgElements != null)
            {
                foreach (var img in imgElements)
                {
                    var src = img.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src) && src.StartsWith("http"))
                    {
                        var newPath = await DownloadAsset(src, assetsDir, "images");
                        img.SetAttributeValue("src", newPath);
                    }
                }
            }

            // Font dosyalarını indir
            var fontLinks = doc.DocumentNode.SelectNodes("//link[@rel='preload' and @as='font'] | //link[@rel='stylesheet' and contains(@href, '.woff')] | //link[@rel='stylesheet' and contains(@href, '.ttf')]");
            if (fontLinks != null)
            {
                foreach (var link in fontLinks)
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href) && href.StartsWith("http"))
                    {
                        var newPath = await DownloadAsset(href, assetsDir, "fonts");
                        link.SetAttributeValue("href", newPath);
                    }
                }
            }

            // Favicon ve diğer icon dosyalarını indir
            var iconLinks = doc.DocumentNode.SelectNodes("//link[@rel='icon'] | //link[@rel='shortcut icon'] | //link[@rel='apple-touch-icon']");
            if (iconLinks != null)
            {
                foreach (var link in iconLinks)
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href) && href.StartsWith("http"))
                    {
                        var newPath = await DownloadAsset(href, assetsDir, "icons");
                        link.SetAttributeValue("href", newPath);
                    }
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

        private async Task<string> DownloadAsset(string url, string assetsDir, string subDir)
        {
            try
            {
                var subDirPath = Path.Combine(assetsDir, subDir);
                Directory.CreateDirectory(subDirPath);

                var fileName = Path.GetFileName(url);
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = $"asset_{Guid.NewGuid().ToString("N")[..8]}.file";
                }

                var filePath = Path.Combine(subDirPath, fileName);

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(filePath, content);
                    return $"./assets/{subDir}/{fileName}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Asset indirilemedi: {url}, Hata: {ex.Message}");
            }

            return url; // İndirilemezse orijinal URL'i döndür
        }

        private string CreateSlug(string hotelName)
        {
            if (string.IsNullOrEmpty(hotelName))
                return "hotel";

            // Türkçe karakterleri değiştir
            var slug = hotelName.ToLower()
                .Replace("ç", "c")
                .Replace("ğ", "g")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ş", "s")
                .Replace("ü", "u")
                .Replace(" ", "-")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace("(", "")
                .Replace(")", "");

            // Birden fazla tire'yi tek tire'ye çevir
            slug = Regex.Replace(slug, @"-+", "-");
            
            // Başındaki ve sonundaki tire'leri kaldır
            slug = slug.Trim('-');

            return string.IsNullOrEmpty(slug) ? "hotel" : slug;
        }
    }

    public class HotelSiteCloneResponse
    {
        public bool Success { get; set; }
        public string SiteUrl { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public string OutputDirectory { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
} 