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

namespace HotelWebsiteBuilder.Services
{
    public class SiteCloneService
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;

        public SiteCloneService(HttpClient httpClient, IWebHostEnvironment environment)
        {
            _httpClient = httpClient;
            _environment = environment;
        }

        public async Task<string> CloneSiteAsync(string url, WebsiteKeys hotelData, string outputDir)
        {
            try
            {
                Console.WriteLine($"Site klonlanıyor: {url}");
                
                // 1. URL'den HTML'i al
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                // 2. HTML'i parse et
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // 3. Otel bilgilerini değiştir
                var updatedHtml = ReplaceHotelInfo(doc, hotelData);

                // 4. Website URL'si varsa domain'leri değiştir
                if (!string.IsNullOrEmpty(hotelData.website))
                {
                    try
                    {
                        var uri = new Uri(hotelData.website);
                        var domain = uri.Host;
                        ReplaceDomainUrls(doc, domain);
                        
                        // Özellikle royaltyezel.com domain'ini değiştir
                        ReplaceSpecificDomain(doc, "royaltyezel.com", domain);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Domain değiştirilemedi: {ex.Message}");
                    }
                }

                // 5. Asset'leri indir ve yolları güncelle
                var finalHtml = await DownloadAndUpdateAssets(doc, outputDir);

                // 6. Klasörü oluştur ve dosyaları kaydet
                Directory.CreateDirectory(outputDir);
                await File.WriteAllTextAsync(Path.Combine(outputDir, "index.html"), finalHtml);

                var relativePath = $"/sites/{Path.GetFileName(outputDir)}/index.html";
                Console.WriteLine($"Site başarıyla klonlandı: {relativePath}");
                
                return relativePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Site klonlanırken hata oluştu: {ex.Message}");
                throw new Exception($"Site klonlanırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<string> CloneSiteWithComprehensiveDataAsync(string url, ComprehensiveHotel hotelData, string outputDir)
        {
            try
            {
                Console.WriteLine($"Kapsamlı verilerle site klonlanıyor: {url}");
                
                // 1. URL'den HTML'i al
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                // 2. HTML'i parse et
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // 3. Kapsamlı otel bilgilerini değiştir
                var updatedHtml = ReplaceComprehensiveHotelInfo(doc, hotelData);

                // 4. Website URL'si varsa domain'leri değiştir
                if (!string.IsNullOrEmpty(hotelData.Website))
                {
                    try
                    {
                        var uri = new Uri(hotelData.Website);
                        var domain = uri.Host;
                        ReplaceDomainUrls(doc, domain);
                        
                        // Özellikle royaltyezel.com domain'ini değiştir
                        ReplaceSpecificDomain(doc, "royaltyezel.com", domain);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Domain değiştirilemedi: {ex.Message}");
                    }
                }

                // 5. Asset'leri indir ve yolları güncelle
                var finalHtml = await DownloadAndUpdateAssets(doc, outputDir);

                // 6. Klasörü oluştur ve dosyaları kaydet
                Directory.CreateDirectory(outputDir);
                await File.WriteAllTextAsync(Path.Combine(outputDir, "index.html"), finalHtml);

                var relativePath = $"/sites/{Path.GetFileName(outputDir)}/index.html";
                Console.WriteLine($"Site başarıyla klonlandı: {relativePath}");
                
                return relativePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Site klonlanırken hata oluştu: {ex.Message}");
                throw new Exception($"Site klonlanırken hata oluştu: {ex.Message}");
            }
        }

        private string ReplaceHotelInfo(HtmlDocument doc, WebsiteKeys hotelData)
        {
            // Hotel name değiştir
            if (!string.IsNullOrEmpty(hotelData.hotelname))
            {
                // Title tag'ini değiştir
                var titleNode = doc.DocumentNode.SelectSingleNode("//title");
                if (titleNode != null)
                {
                    titleNode.InnerHtml = hotelData.hotelname;
                }

                // H1 tag'lerini değiştir
                var h1Nodes = doc.DocumentNode.SelectNodes("//h1");
                if (h1Nodes != null)
                {
                    foreach (var h1 in h1Nodes)
                    {
                        h1.InnerHtml = hotelData.hotelname;
                    }
                }

                // H2 tag'lerini değiştir (otel ile ilgili olanları)
                var h2Nodes = doc.DocumentNode.SelectNodes("//h2");
                if (h2Nodes != null)
                {
                    foreach (var h2 in h2Nodes)
                    {
                        var currentText = h2.InnerText.Trim().ToLower();
                        if (currentText.Contains("hotel") || currentText.Contains("otel") || 
                            currentText.Contains("welcome") || currentText.Contains("about") ||
                            currentText.Contains("royalty") || currentText.Contains("ezel"))
                        {
                            h2.InnerHtml = hotelData.hotelname;
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
                        if (currentText.Contains("hotel") || currentText.Contains("otel") || 
                            currentText.Contains("welcome") || currentText.Contains("about"))
                        {
                            h3.InnerHtml = hotelData.hotelname;
                        }
                    }
                }

                // Diğer text içeriklerini değiştir
                ReplaceTextInDocument(doc, hotelData.hotelname, "Example Domain", "Example", "Domain", "Royalty Ezel", "Royalty", "Ezel");
            }

            // Phone değiştir
            if (!string.IsNullOrEmpty(hotelData.phone))
            {
                // Tel linklerini değiştir
                var telLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'tel:')]");
                if (telLinks != null)
                {
                    foreach (var link in telLinks)
                    {
                        link.SetAttributeValue("href", $"tel:{hotelData.phone}");
                        link.InnerHtml = hotelData.phone;
                    }
                }

                // Phone text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.phone, "phone", "telefon", "Phone", "Telefon");

                // Span ve div içindeki telefon numaralarını değiştir
                var phoneElements = doc.DocumentNode.SelectNodes("//*[contains(text(), '+90') or contains(text(), 'phone') or contains(text(), 'telefon')]");
                if (phoneElements != null)
                {
                    foreach (var element in phoneElements)
                    {
                        var currentText = element.InnerText.Trim();
                        if (currentText.Contains("+90") || currentText.Contains("phone") || currentText.Contains("telefon"))
                        {
                            element.InnerHtml = hotelData.phone;
                        }
                    }
                }
            }

            // Email değiştir
            if (!string.IsNullOrEmpty(hotelData.email))
            {
                // Mailto linklerini değiştir
                var mailtoLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'mailto:')]");
                if (mailtoLinks != null)
                {
                    foreach (var link in mailtoLinks)
                    {
                        link.SetAttributeValue("href", $"mailto:{hotelData.email}");
                        link.InnerHtml = hotelData.email;
                    }
                }

                // Email text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.email, "email", "e-posta", "Email", "E-posta");

                // Span ve div içindeki email adreslerini değiştir
                var emailElements = doc.DocumentNode.SelectNodes("//*[contains(text(), '@') or contains(text(), 'email') or contains(text(), 'e-posta')]");
                if (emailElements != null)
                {
                    foreach (var element in emailElements)
                    {
                        var currentText = element.InnerText.Trim();
                        if (currentText.Contains("@") || currentText.Contains("email") || currentText.Contains("e-posta"))
                        {
                            element.InnerHtml = hotelData.email;
                        }
                    }
                }
            }

            // Address değiştir
            if (!string.IsNullOrEmpty(hotelData.address))
            {
                // Address tag'lerini değiştir
                var addressNodes = doc.DocumentNode.SelectNodes("//address");
                if (addressNodes != null)
                {
                    foreach (var address in addressNodes)
                    {
                        address.InnerHtml = hotelData.address;
                    }
                }

                // Address text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.address, "address", "adres", "Address", "Adres");

                // Span ve div içindeki adres bilgilerini değiştir
                var addressElements = doc.DocumentNode.SelectNodes("//*[contains(text(), 'address') or contains(text(), 'adres') or contains(text(), 'antalya') or contains(text(), 'location')]");
                if (addressElements != null)
                {
                    foreach (var element in addressElements)
                    {
                        var currentText = element.InnerText.Trim().ToLower();
                        if (currentText.Contains("address") || currentText.Contains("adres") || 
                            currentText.Contains("antalya") || currentText.Contains("location") ||
                            currentText.Contains("konum") || currentText.Contains("yer"))
                        {
                            element.InnerHtml = hotelData.address;
                        }
                    }
                }
            }

            // Description değiştir
            if (!string.IsNullOrEmpty(hotelData.description))
            {
                // P tag'lerini değiştir
                var pNodes = doc.DocumentNode.SelectNodes("//p");
                if (pNodes != null)
                {
                    foreach (var p in pNodes)
                    {
                        var currentText = p.InnerText.Trim().ToLower();
                        // Otel ile ilgili açıklama metinlerini değiştir
                        if (currentText.Contains("this domain is for use in illustrative examples") ||
                            currentText.Contains("royalty ezel") || currentText.Contains("antalyada") ||
                            currentText.Contains("lüx butik otel") || currentText.Contains("eşsiz konfor") ||
                            currentText.Contains("welcome") || currentText.Contains("about us") ||
                            currentText.Contains("description") || currentText.Contains("açıklama"))
                        {
                            p.InnerHtml = hotelData.description;
                        }
                    }
                }

                // Span tag'lerini değiştir (otel ile ilgili olanları)
                var spanNodes = doc.DocumentNode.SelectNodes("//span");
                if (spanNodes != null)
                {
                    foreach (var span in spanNodes)
                    {
                        var currentText = span.InnerText.Trim().ToLower();
                        if (currentText.Contains("royalty ezel") || currentText.Contains("antalyada") ||
                            currentText.Contains("lüx butik") || currentText.Contains("eşsiz konfor"))
                        {
                            span.InnerHtml = hotelData.description;
                        }
                    }
                }

                // Div tag'lerini değiştir (otel ile ilgili olanları)
                var divNodes = doc.DocumentNode.SelectNodes("//div");
                if (divNodes != null)
                {
                    foreach (var div in divNodes)
                    {
                        var currentText = div.InnerText.Trim().ToLower();
                        if (currentText.Contains("royalty ezel") || currentText.Contains("antalyada") ||
                            currentText.Contains("lüx butik") || currentText.Contains("eşsiz konfor"))
                        {
                            div.InnerHtml = hotelData.description;
                        }
                    }
                }

                // Description text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.description, "description", "açıklama", "Description", "Açıklama");
            }

            // Website URL değiştir
            if (!string.IsNullOrEmpty(hotelData.website))
            {
                // Website linklerini değiştir
                var websiteLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'http')]");
                if (websiteLinks != null)
                {
                    foreach (var link in websiteLinks)
                    {
                        var href = link.GetAttributeValue("href", "");
                        // Sadece genel website linklerini değiştir (sosyal medya hariç)
                        if (!href.Contains("facebook.com") && !href.Contains("instagram.com") && 
                            !href.Contains("twitter.com") && !href.Contains("linkedin.com") &&
                            href.StartsWith("http") && !href.Contains("example.com"))
                        {
                            link.SetAttributeValue("href", hotelData.website);
                        }
                    }
                }

                // Website text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.website, "website", "site", "Website", "Site");
            }

            // Social media links değiştir
            if (!string.IsNullOrEmpty(hotelData.facebook))
            {
                var facebookLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'facebook.com')]");
                if (facebookLinks != null)
                {
                    foreach (var link in facebookLinks)
                    {
                        link.SetAttributeValue("href", hotelData.facebook);
                    }
                }
            }

            if (!string.IsNullOrEmpty(hotelData.instagram))
            {
                var instagramLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'instagram.com')]");
                if (instagramLinks != null)
                {
                    foreach (var link in instagramLinks)
                    {
                        link.SetAttributeValue("href", hotelData.instagram);
                    }
                }
            }

            if (!string.IsNullOrEmpty(hotelData.twitter))
            {
                var twitterLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'twitter.com')]");
                if (twitterLinks != null)
                {
                    foreach (var link in twitterLinks)
                    {
                        link.SetAttributeValue("href", hotelData.twitter);
                    }
                }
            }

            // Sosyal medya ikonlarını ve metinlerini değiştir
            var socialElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'social') or contains(@class, 'facebook') or contains(@class, 'instagram') or contains(@class, 'twitter')]");
            if (socialElements != null)
            {
                foreach (var element in socialElements)
                {
                    var className = element.GetAttributeValue("class", "").ToLower();
                    if (className.Contains("facebook") && !string.IsNullOrEmpty(hotelData.facebook))
                    {
                        var link = element.SelectSingleNode(".//a");
                        if (link != null)
                        {
                            link.SetAttributeValue("href", hotelData.facebook);
                        }
                    }
                    else if (className.Contains("instagram") && !string.IsNullOrEmpty(hotelData.instagram))
                    {
                        var link = element.SelectSingleNode(".//a");
                        if (link != null)
                        {
                            link.SetAttributeValue("href", hotelData.instagram);
                        }
                    }
                    else if (className.Contains("twitter") && !string.IsNullOrEmpty(hotelData.twitter))
                    {
                        var link = element.SelectSingleNode(".//a");
                        if (link != null)
                        {
                            link.SetAttributeValue("href", hotelData.twitter);
                        }
                    }
                }
            }

            // Logo URL değiştir
            if (!string.IsNullOrEmpty(hotelData.logourl))
            {
                var logoImages = doc.DocumentNode.SelectNodes("//img[contains(@src, 'logo') or contains(@alt, 'logo') or contains(@class, 'logo')]");
                if (logoImages != null)
                {
                    foreach (var img in logoImages)
                    {
                        img.SetAttributeValue("src", hotelData.logourl);
                    }
                }
            }

            // Gallery images değiştir
            var galleryImages = doc.DocumentNode.SelectNodes("//img[contains(@class, 'gallery') or contains(@alt, 'gallery') or contains(@src, 'gallery') or contains(@class, 'slide') or contains(@class, 'carousel')]");
            if (galleryImages != null && galleryImages.Count > 0)
            {
                var galleryUrls = new[] { hotelData.galleryimage1, hotelData.galleryimage2, hotelData.galleryimage3, hotelData.galleryimage4, hotelData.galleryimage5 };
                var validUrls = galleryUrls.Where(url => !string.IsNullOrEmpty(url)).ToList();
                
                for (int i = 0; i < galleryImages.Count && i < validUrls.Count; i++)
                {
                    galleryImages[i].SetAttributeValue("src", validUrls[i]);
                    Console.WriteLine($"Galeri resmi değiştirildi: {galleryImages[i].GetAttributeValue("src", "")} -> {validUrls[i]}");
                }
            }

            // Slider images değiştir
            var sliderImages = doc.DocumentNode.SelectNodes("//img[contains(@class, 'slider') or contains(@class, 'hero') or contains(@class, 'banner') or contains(@alt, 'slider') or contains(@alt, 'hero')]");
            if (sliderImages != null && sliderImages.Count > 0)
            {
                var sliderUrls = new[] { hotelData.galleryimage1, hotelData.galleryimage2, hotelData.galleryimage3, hotelData.galleryimage4, hotelData.galleryimage5 };
                var validUrls = sliderUrls.Where(url => !string.IsNullOrEmpty(url)).ToList();
                
                for (int i = 0; i < sliderImages.Count && i < validUrls.Count; i++)
                {
                    sliderImages[i].SetAttributeValue("src", validUrls[i]);
                    Console.WriteLine($"Slider resmi değiştirildi: {sliderImages[i].GetAttributeValue("src", "")} -> {validUrls[i]}");
                }
            }

            // Amenities değiştir
            if (!string.IsNullOrEmpty(hotelData.amenities))
            {
                var amenityElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'amenity') or contains(@class, 'facility') or contains(text(), 'amenity') or contains(text(), 'facility')]");
                if (amenityElements != null)
                {
                    foreach (var element in amenityElements)
                    {
                        element.InnerHtml = hotelData.amenities;
                    }
                }
            }

            // Room types değiştir
            if (!string.IsNullOrEmpty(hotelData.roomtypes))
            {
                var roomElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'room') or contains(@class, 'suite') or contains(text(), 'room') or contains(text(), 'suite')]");
                if (roomElements != null)
                {
                    foreach (var element in roomElements)
                    {
                        element.InnerHtml = hotelData.roomtypes;
                    }
                }
            }

            // Pricing değiştir
            if (!string.IsNullOrEmpty(hotelData.pricing))
            {
                var pricingElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'price') or contains(@class, 'cost') or contains(text(), 'price') or contains(text(), 'cost')]");
                if (pricingElements != null)
                {
                    foreach (var element in pricingElements)
                    {
                        element.InnerHtml = hotelData.pricing;
                    }
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

        private string ReplaceComprehensiveHotelInfo(HtmlDocument doc, ComprehensiveHotel hotelData)
        {
            // Hotel name değiştir
            if (!string.IsNullOrEmpty(hotelData.Name))
            {
                // Title tag'ini değiştir
                var titleNode = doc.DocumentNode.SelectSingleNode("//title");
                if (titleNode != null)
                {
                    titleNode.InnerHtml = hotelData.Name;
                }

                // H1 tag'lerini değiştir
                var h1Nodes = doc.DocumentNode.SelectNodes("//h1");
                if (h1Nodes != null)
                {
                    foreach (var h1 in h1Nodes)
                    {
                        h1.InnerHtml = hotelData.Name;
                    }
                }

                // H2 tag'lerini değiştir (otel ile ilgili olanları)
                var h2Nodes = doc.DocumentNode.SelectNodes("//h2");
                if (h2Nodes != null)
                {
                    foreach (var h2 in h2Nodes)
                    {
                        var currentText = h2.InnerText.Trim().ToLower();
                        if (currentText.Contains("hotel") || currentText.Contains("otel") || 
                            currentText.Contains("welcome") || currentText.Contains("about") ||
                            currentText.Contains("royalty") || currentText.Contains("ezel"))
                        {
                            h2.InnerHtml = hotelData.Name;
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
                        if (currentText.Contains("hotel") || currentText.Contains("otel") || 
                            currentText.Contains("welcome") || currentText.Contains("about"))
                        {
                            h3.InnerHtml = hotelData.Name;
                        }
                    }
                }

                // Diğer text içeriklerini değiştir
                ReplaceTextInDocument(doc, hotelData.Name, "Example Domain", "Example", "Domain", "Royalty Ezel", "Royalty", "Ezel");
            }

            // Phone değiştir
            if (!string.IsNullOrEmpty(hotelData.Phone))
            {
                // Tel linklerini değiştir
                var telLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'tel:')]");
                if (telLinks != null)
                {
                    foreach (var link in telLinks)
                    {
                        link.SetAttributeValue("href", $"tel:{hotelData.Phone}");
                        link.InnerHtml = hotelData.Phone;
                    }
                }

                // Phone text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.Phone, "phone", "telefon", "Phone", "Telefon");

                // Span ve div içindeki telefon numaralarını değiştir
                var phoneElements = doc.DocumentNode.SelectNodes("//*[contains(text(), '+90') or contains(text(), 'phone') or contains(text(), 'telefon')]");
                if (phoneElements != null)
                {
                    foreach (var element in phoneElements)
                    {
                        var currentText = element.InnerText.Trim();
                        if (currentText.Contains("+90") || currentText.Contains("phone") || currentText.Contains("telefon"))
                        {
                            element.InnerHtml = hotelData.Phone;
                        }
                    }
                }
            }

            // Email değiştir
            if (!string.IsNullOrEmpty(hotelData.Email))
            {
                // Mailto linklerini değiştir
                var mailtoLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'mailto:')]");
                if (mailtoLinks != null)
                {
                    foreach (var link in mailtoLinks)
                    {
                        link.SetAttributeValue("href", $"mailto:{hotelData.Email}");
                        link.InnerHtml = hotelData.Email;
                    }
                }

                // Email text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.Email, "email", "e-posta", "Email", "E-posta");

                // Span ve div içindeki email adreslerini değiştir
                var emailElements = doc.DocumentNode.SelectNodes("//*[contains(text(), '@') or contains(text(), 'email') or contains(text(), 'e-posta')]");
                if (emailElements != null)
                {
                    foreach (var element in emailElements)
                    {
                        var currentText = element.InnerText.Trim();
                        if (currentText.Contains("@") || currentText.Contains("email") || currentText.Contains("e-posta"))
                        {
                            element.InnerHtml = hotelData.Email;
                        }
                    }
                }
            }

            // Address değiştir
            if (!string.IsNullOrEmpty(hotelData.Address))
            {
                // Address tag'lerini değiştir
                var addressNodes = doc.DocumentNode.SelectNodes("//address");
                if (addressNodes != null)
                {
                    foreach (var address in addressNodes)
                    {
                        address.InnerHtml = hotelData.Address;
                    }
                }

                // Address text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.Address, "address", "adres", "Address", "Adres");

                // Span ve div içindeki adres bilgilerini değiştir
                var addressElements = doc.DocumentNode.SelectNodes("//*[contains(text(), 'address') or contains(text(), 'adres') or contains(text(), 'antalya') or contains(text(), 'location')]");
                if (addressElements != null)
                {
                    foreach (var element in addressElements)
                    {
                        var currentText = element.InnerText.Trim().ToLower();
                        if (currentText.Contains("address") || currentText.Contains("adres") || 
                            currentText.Contains("antalya") || currentText.Contains("location") ||
                            currentText.Contains("konum") || currentText.Contains("yer"))
                        {
                            element.InnerHtml = hotelData.Address;
                        }
                    }
                }
            }

            // Description değiştir
            if (!string.IsNullOrEmpty(hotelData.Description))
            {
                // P tag'lerini değiştir
                var pNodes = doc.DocumentNode.SelectNodes("//p");
                if (pNodes != null)
                {
                    foreach (var p in pNodes)
                    {
                        var currentText = p.InnerText.Trim().ToLower();
                        // Otel ile ilgili açıklama metinlerini değiştir
                        if (currentText.Contains("this domain is for use in illustrative examples") ||
                            currentText.Contains("royalty ezel") || currentText.Contains("antalyada") ||
                            currentText.Contains("lüx butik otel") || currentText.Contains("eşsiz konfor") ||
                            currentText.Contains("welcome") || currentText.Contains("about us") ||
                            currentText.Contains("description") || currentText.Contains("açıklama"))
                        {
                            p.InnerHtml = hotelData.Description;
                        }
                    }
                }

                // Span tag'lerini değiştir (otel ile ilgili olanları)
                var spanNodes = doc.DocumentNode.SelectNodes("//span");
                if (spanNodes != null)
                {
                    foreach (var span in spanNodes)
                    {
                        var currentText = span.InnerText.Trim().ToLower();
                        if (currentText.Contains("royalty ezel") || currentText.Contains("antalyada") ||
                            currentText.Contains("lüx butik") || currentText.Contains("eşsiz konfor"))
                        {
                            span.InnerHtml = hotelData.Description;
                        }
                    }
                }

                // Div tag'lerini değiştir (otel ile ilgili olanları)
                var divNodes = doc.DocumentNode.SelectNodes("//div");
                if (divNodes != null)
                {
                    foreach (var div in divNodes)
                    {
                        var currentText = div.InnerText.Trim().ToLower();
                        if (currentText.Contains("royalty ezel") || currentText.Contains("antalyada") ||
                            currentText.Contains("lüx butik") || currentText.Contains("eşsiz konfor"))
                        {
                            div.InnerHtml = hotelData.Description;
                        }
                    }
                }

                // Description text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.Description, "description", "açıklama", "Description", "Açıklama");
            }

            // Website URL değiştir
            if (!string.IsNullOrEmpty(hotelData.Website))
            {
                // Website linklerini değiştir
                var websiteLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'http')]");
                if (websiteLinks != null)
                {
                    foreach (var link in websiteLinks)
                    {
                        var href = link.GetAttributeValue("href", "");
                        // Sadece genel website linklerini değiştir (sosyal medya hariç)
                        if (!href.Contains("facebook.com") && !href.Contains("instagram.com") && 
                            !href.Contains("twitter.com") && !href.Contains("linkedin.com") &&
                            href.StartsWith("http") && !href.Contains("example.com"))
                        {
                            link.SetAttributeValue("href", hotelData.Website);
                        }
                    }
                }

                // Website text'lerini değiştir
                ReplaceTextInDocument(doc, hotelData.Website, "website", "site", "Website", "Site");
            }

            // Social media links değiştir
            if (!string.IsNullOrEmpty(hotelData.Social?.Facebook))
            {
                var facebookLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'facebook.com')]");
                if (facebookLinks != null)
                {
                    foreach (var link in facebookLinks)
                    {
                        link.SetAttributeValue("href", hotelData.Social.Facebook);
                    }
                }
            }

            if (!string.IsNullOrEmpty(hotelData.Social?.Instagram))
            {
                var instagramLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'instagram.com')]");
                if (instagramLinks != null)
                {
                    foreach (var link in instagramLinks)
                    {
                        link.SetAttributeValue("href", hotelData.Social.Instagram);
                    }
                }
            }

            if (!string.IsNullOrEmpty(hotelData.Social?.Twitter))
            {
                var twitterLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'twitter.com')]");
                if (twitterLinks != null)
                {
                    foreach (var link in twitterLinks)
                    {
                        link.SetAttributeValue("href", hotelData.Social.Twitter);
                    }
                }
            }

            // Sosyal medya ikonlarını ve metinlerini değiştir
            var socialElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'social') or contains(@class, 'facebook') or contains(@class, 'instagram') or contains(@class, 'twitter')]");
            if (socialElements != null)
            {
                foreach (var element in socialElements)
                {
                    var className = element.GetAttributeValue("class", "").ToLower();
                    if (className.Contains("facebook") && !string.IsNullOrEmpty(hotelData.Social?.Facebook))
                    {
                        var link = element.SelectSingleNode(".//a");
                        if (link != null)
                        {
                            link.SetAttributeValue("href", hotelData.Social.Facebook);
                        }
                    }
                    else if (className.Contains("instagram") && !string.IsNullOrEmpty(hotelData.Social?.Instagram))
                    {
                        var link = element.SelectSingleNode(".//a");
                        if (link != null)
                        {
                            link.SetAttributeValue("href", hotelData.Social.Instagram);
                        }
                    }
                    else if (className.Contains("twitter") && !string.IsNullOrEmpty(hotelData.Social?.Twitter))
                    {
                        var link = element.SelectSingleNode(".//a");
                        if (link != null)
                        {
                            link.SetAttributeValue("href", hotelData.Social.Twitter);
                        }
                    }
                }
            }

            // Logo URL değiştir
            if (!string.IsNullOrEmpty(hotelData.LogoUrl))
            {
                var logoImages = doc.DocumentNode.SelectNodes("//img[contains(@src, 'logo') or contains(@alt, 'logo') or contains(@class, 'logo')]");
                if (logoImages != null)
                {
                    foreach (var img in logoImages)
                    {
                        img.SetAttributeValue("src", hotelData.LogoUrl);
                    }
                }
            }

                        // Gallery images değiştir
            var galleryImages = doc.DocumentNode.SelectNodes("//img[contains(@class, 'gallery') or contains(@alt, 'gallery')]");
            if (galleryImages != null && galleryImages.Count > 0 && hotelData.GalleryImages != null)
            {
                for (int i = 0; i < galleryImages.Count && i < hotelData.GalleryImages.Count; i++)
                {
                    galleryImages[i].SetAttributeValue("src", hotelData.GalleryImages[i]);
                }
            }

            // Amenities değiştir
            if (hotelData.Amenities != null && hotelData.Amenities.Count > 0)
            {
                var amenityElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'amenity') or contains(@class, 'facility') or contains(text(), 'amenity') or contains(text(), 'facility')]");
                if (amenityElements != null)
                {
                    foreach (var element in amenityElements)
                    {
                        element.InnerHtml = string.Join(", ", hotelData.Amenities);
                    }
                }
            }

            // Room types değiştir - Rooms listesinden oda tiplerini al
            if (hotelData.Rooms != null && hotelData.Rooms.Count > 0)
            {
                var roomElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'room') or contains(@class, 'suite') or contains(text(), 'room') or contains(text(), 'suite')]");
                if (roomElements != null)
                {
                    var roomTypes = string.Join(", ", hotelData.Rooms.Select(r => r.Type));
                    foreach (var element in roomElements)
                    {
                        element.InnerHtml = roomTypes;
                    }
                }
            }

            // Pricing değiştir - Rooms listesinden fiyatları al
            if (hotelData.Rooms != null && hotelData.Rooms.Count > 0)
            {
                var pricingElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'price') or contains(@class, 'cost') or contains(text(), 'price') or contains(text(), 'cost')]");
                if (pricingElements != null)
                {
                    var pricing = string.Join(", ", hotelData.Rooms.Select(r => $"{r.Type}: {r.Price:C}"));
                    foreach (var element in pricingElements)
                    {
                        element.InnerHtml = pricing;
                    }
                }
            }

             // Kapsamlı hotel verilerini işle
             ReplaceComprehensiveHotelData(doc, hotelData);

             return doc.DocumentNode.OuterHtml;
         }

         private void ReplaceComprehensiveHotelData(HtmlDocument doc, ComprehensiveHotel hotelData)
         {
             // Oda bilgilerini değiştir
             if (hotelData.Rooms != null && hotelData.Rooms.Count > 0)
             {
                 ReplaceRoomData(doc, hotelData.Rooms);
             }

             // Tesis bilgilerini değiştir
             if (hotelData.Facilities != null && hotelData.Facilities.Count > 0)
             {
                 ReplaceFacilityData(doc, hotelData.Facilities);
             }

             // Amenities bilgilerini değiştir
             if (hotelData.Amenities != null && hotelData.Amenities.Count > 0)
             {
                 ReplaceAmenitiesData(doc, hotelData.Amenities);
             }

             // Slider resimlerini değiştir
             if (hotelData.SliderImages != null && hotelData.SliderImages.Count > 0)
             {
                 ReplaceSliderImages(doc, hotelData.SliderImages);
             }

             // Gallery resimlerini değiştir
             if (hotelData.GalleryImages != null && hotelData.GalleryImages.Count > 0)
             {
                 ReplaceGalleryImages(doc, hotelData.GalleryImages);
             }

             // Check-in/out zamanlarını değiştir
             ReplaceCheckInOutTimes(doc, hotelData.CheckInTime, hotelData.CheckOutTime);

             // Meta bilgilerini değiştir
             if (hotelData.Meta != null)
             {
                 ReplaceMetaData(doc, hotelData.Meta);
             }

             // Sosyal medya bilgilerini değiştir
             if (hotelData.Social != null)
             {
                 ReplaceSocialMediaData(doc, hotelData.Social);
             }

             // Yıldız rating'ini değiştir
             ReplaceStarRating(doc, hotelData.StarRating);
         }

         private void ReplaceRoomData(HtmlDocument doc, List<HotelRoom> rooms)
         {
             // Oda listesi elementlerini bul
             var roomElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'room') or contains(@class, 'suite') or contains(@class, 'accommodation')]");
             if (roomElements != null)
             {
                 for (int i = 0; i < roomElements.Count && i < rooms.Count; i++)
                 {
                     var room = rooms[i];
                     var element = roomElements[i];

                     // Oda tipini değiştir
                     var typeElements = element.SelectNodes(".//*[contains(@class, 'room-type') or contains(@class, 'suite-type')]");
                     if (typeElements != null)
                     {
                         foreach (var typeElement in typeElements)
                         {
                             typeElement.InnerHtml = room.Type;
                         }
                     }

                     // Oda açıklamasını değiştir
                     var descElements = element.SelectNodes(".//*[contains(@class, 'room-description') or contains(@class, 'suite-description')]");
                     if (descElements != null)
                     {
                         foreach (var descElement in descElements)
                         {
                             descElement.InnerHtml = room.Description;
                         }
                     }

                     // Oda fiyatını değiştir
                     var priceElements = element.SelectNodes(".//*[contains(@class, 'room-price') or contains(@class, 'suite-price')]");
                     if (priceElements != null)
                     {
                         foreach (var priceElement in priceElements)
                         {
                             priceElement.InnerHtml = $"{room.Price:C}";
                         }
                     }

                     // Oda özelliklerini değiştir
                     if (room.Features != null && room.Features.Count > 0)
                     {
                         var featureElements = element.SelectNodes(".//*[contains(@class, 'room-features') or contains(@class, 'suite-features')]");
                         if (featureElements != null)
                         {
                             foreach (var featureElement in featureElements)
                             {
                                 featureElement.InnerHtml = string.Join(", ", room.Features);
                             }
                         }
                     }
                 }
             }
         }

         private void ReplaceFacilityData(HtmlDocument doc, List<HotelFacility> facilities)
         {
             // Tesis listesi elementlerini bul
             var facilityElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'facility') or contains(@class, 'amenity') or contains(@class, 'service')]");
             if (facilityElements != null)
             {
                 for (int i = 0; i < facilityElements.Count && i < facilities.Count; i++)
                 {
                     var facility = facilities[i];
                     var element = facilityElements[i];

                     // Tesis adını değiştir
                     var nameElements = element.SelectNodes(".//*[contains(@class, 'facility-name') or contains(@class, 'amenity-name')]");
                     if (nameElements != null)
                     {
                         foreach (var nameElement in nameElements)
                         {
                             nameElement.InnerHtml = facility.Name;
                         }
                     }

                     // Tesis açıklamasını değiştir
                     var descElements = element.SelectNodes(".//*[contains(@class, 'facility-description') or contains(@class, 'amenity-description')]");
                     if (descElements != null)
                     {
                         foreach (var descElement in descElements)
                         {
                             descElement.InnerHtml = facility.Description;
                         }
                     }

                     // Tesis ikonunu değiştir
                     var iconElements = element.SelectNodes(".//*[contains(@class, 'facility-icon') or contains(@class, 'amenity-icon')]");
                     if (iconElements != null && !string.IsNullOrEmpty(facility.Icon))
                     {
                         foreach (var iconElement in iconElements)
                         {
                             iconElement.SetAttributeValue("src", facility.Icon);
                         }
                     }
                 }
             }
         }

         private void ReplaceAmenitiesData(HtmlDocument doc, List<string> amenities)
         {
             // Amenities listesi elementlerini bul
             var amenityElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'amenities') or contains(@class, 'facilities-list')]");
             if (amenityElements != null)
             {
                 foreach (var element in amenityElements)
                 {
                     element.InnerHtml = string.Join(", ", amenities);
                 }
             }
         }

         private void ReplaceSliderImages(HtmlDocument doc, List<string> sliderImages)
         {
             // Slider resimlerini bul
             var sliderElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'slider') or contains(@class, 'carousel')]//img");
             if (sliderElements != null)
             {
                 for (int i = 0; i < sliderElements.Count && i < sliderImages.Count; i++)
                 {
                     sliderElements[i].SetAttributeValue("src", sliderImages[i]);
                 }
             }
         }

         private void ReplaceGalleryImages(HtmlDocument doc, List<string> galleryImages)
         {
             // Gallery resimlerini bul
             var galleryElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'gallery')]//img");
             if (galleryElements != null)
             {
                 for (int i = 0; i < galleryElements.Count && i < galleryImages.Count; i++)
                 {
                     galleryElements[i].SetAttributeValue("src", galleryImages[i]);
                 }
             }
         }

         private void ReplaceCheckInOutTimes(HtmlDocument doc, string checkInTime, string checkOutTime)
         {
             // Check-in zamanını değiştir
             var checkInElements = doc.DocumentNode.SelectNodes("//*[contains(text(), 'check-in') or contains(text(), 'giriş')]");
             if (checkInElements != null)
             {
                 foreach (var element in checkInElements)
                 {
                     element.InnerHtml = element.InnerHtml.Replace("14:00", checkInTime);
                 }
             }

             // Check-out zamanını değiştir
             var checkOutElements = doc.DocumentNode.SelectNodes("//*[contains(text(), 'check-out') or contains(text(), 'çıkış')]");
             if (checkOutElements != null)
             {
                 foreach (var element in checkOutElements)
                 {
                     element.InnerHtml = element.InnerHtml.Replace("12:00", checkOutTime);
                 }
             }
         }

         private void ReplaceMetaData(HtmlDocument doc, HotelMeta meta)
         {
             // Title tag'ini değiştir
             var titleNode = doc.DocumentNode.SelectSingleNode("//title");
             if (titleNode != null && !string.IsNullOrEmpty(meta.Title))
             {
                 titleNode.InnerHtml = meta.Title;
             }

             // Meta description'ı değiştir
             var metaDescElements = doc.DocumentNode.SelectNodes("//meta[@name='description']");
             if (metaDescElements != null && !string.IsNullOrEmpty(meta.Description))
             {
                 foreach (var element in metaDescElements)
                 {
                     element.SetAttributeValue("content", meta.Description);
                 }
             }

             // Meta keywords'ü değiştir
             var metaKeywordsElements = doc.DocumentNode.SelectNodes("//meta[@name='keywords']");
             if (metaKeywordsElements != null && !string.IsNullOrEmpty(meta.Keywords))
             {
                 foreach (var element in metaKeywordsElements)
                 {
                     element.SetAttributeValue("content", meta.Keywords);
                 }
             }
         }

         private void ReplaceSocialMediaData(HtmlDocument doc, HotelSocial social)
         {
             // Instagram linklerini değiştir
             if (!string.IsNullOrEmpty(social.Instagram))
             {
                 var instagramLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'instagram.com')]");
                 if (instagramLinks != null)
                 {
                     foreach (var link in instagramLinks)
                     {
                         link.SetAttributeValue("href", social.Instagram);
                     }
                 }
             }

             // Facebook linklerini değiştir
             if (!string.IsNullOrEmpty(social.Facebook))
             {
                 var facebookLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'facebook.com')]");
                 if (facebookLinks != null)
                 {
                     foreach (var link in facebookLinks)
                     {
                         link.SetAttributeValue("href", social.Facebook);
                     }
                 }
             }

             // Twitter linklerini değiştir
             if (!string.IsNullOrEmpty(social.Twitter))
             {
                 var twitterLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'twitter.com')]");
                 if (twitterLinks != null)
                 {
                     foreach (var link in twitterLinks)
                     {
                         link.SetAttributeValue("href", social.Twitter);
                     }
                 }
             }

             // LinkedIn linklerini değiştir
             if (!string.IsNullOrEmpty(social.LinkedIn))
             {
                 var linkedinLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'linkedin.com')]");
                 if (linkedinLinks != null)
                 {
                     foreach (var link in linkedinLinks)
                     {
                         link.SetAttributeValue("href", social.LinkedIn);
                     }
                 }
             }

             // YouTube linklerini değiştir
             if (!string.IsNullOrEmpty(social.YouTube))
             {
                 var youtubeLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'youtube.com')]");
                 if (youtubeLinks != null)
                 {
                     foreach (var link in youtubeLinks)
                     {
                         link.SetAttributeValue("href", social.YouTube);
                     }
                 }
             }
         }

         private void ReplaceStarRating(HtmlDocument doc, int starRating)
         {
             // Yıldız rating elementlerini bul
             var starElements = doc.DocumentNode.SelectNodes("//*[contains(@class, 'star') or contains(@class, 'rating')]");
             if (starElements != null)
             {
                 foreach (var element in starElements)
                 {
                     // Yıldız sayısını güncelle
                     var stars = new string('★', starRating) + new string('☆', 5 - starRating);
                     element.InnerHtml = stars;
                 }
             }
         }

        private void ReplaceTextInDocument(HtmlDocument doc, string newValue, params string[] searchTerms)
        {
            foreach (var term in searchTerms)
            {
                // Text node'ları bul ve değiştir
                var textNodes = doc.DocumentNode.SelectNodes($"//text()[contains(., '{term}')]");
                if (textNodes != null)
                {
                    foreach (var node in textNodes)
                    {
                        node.InnerHtml = node.InnerHtml.Replace(term, newValue);
                    }
                }

                // Attribute'larda değiştir
                var elementsWithHref = doc.DocumentNode.SelectNodes($"//*[@href]");
                if (elementsWithHref != null)
                {
                    foreach (var element in elementsWithHref)
                    {
                        var href = element.GetAttributeValue("href", "");
                        if (href.Contains(term))
                        {
                            element.SetAttributeValue("href", href.Replace(term, newValue));
                        }
                    }
                }

                // Alt attribute'larda değiştir
                var elementsWithAlt = doc.DocumentNode.SelectNodes($"//*[@alt]");
                if (elementsWithAlt != null)
                {
                    foreach (var element in elementsWithAlt)
                    {
                        var alt = element.GetAttributeValue("alt", "");
                        if (alt.Contains(term))
                        {
                            element.SetAttributeValue("alt", alt.Replace(term, newValue));
                        }
                    }
                }

                // Title attribute'larda değiştir
                var elementsWithTitle = doc.DocumentNode.SelectNodes($"//*[@title]");
                if (elementsWithTitle != null)
                {
                    foreach (var element in elementsWithTitle)
                    {
                        var title = element.GetAttributeValue("title", "");
                        if (title.Contains(term))
                        {
                            element.SetAttributeValue("title", title.Replace(term, newValue));
                        }
                    }
                }

                // Placeholder attribute'larda değiştir
                var elementsWithPlaceholder = doc.DocumentNode.SelectNodes($"//*[@placeholder]");
                if (elementsWithPlaceholder != null)
                {
                    foreach (var element in elementsWithPlaceholder)
                    {
                        var placeholder = element.GetAttributeValue("placeholder", "");
                        if (placeholder.Contains(term))
                        {
                            element.SetAttributeValue("placeholder", placeholder.Replace(term, newValue));
                        }
                    }
                }
            }
        }

        private async Task<string> DownloadAndUpdateAssets(HtmlDocument doc, string outputDir)
        {
            var assetsDir = Path.Combine(outputDir, "assets");
            Directory.CreateDirectory(assetsDir);

            // CSS dosyalarını indir
            var cssLinks = doc.DocumentNode.SelectNodes("//link[@rel='stylesheet']");
            if (cssLinks != null)
            {
                foreach (var link in cssLinks)
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href))
                    {
                        var localPath = await DownloadAsset(href, assetsDir, "css");
                        link.SetAttributeValue("href", localPath);
                    }
                }
            }

            // JS dosyalarını indir
            var scriptTags = doc.DocumentNode.SelectNodes("//script[@src]");
            if (scriptTags != null)
            {
                foreach (var script in scriptTags)
                {
                    var src = script.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src))
                    {
                        var localPath = await DownloadAsset(src, assetsDir, "js");
                        script.SetAttributeValue("src", localPath);
                    }
                }
            }

            // Resimleri indir
            var images = doc.DocumentNode.SelectNodes("//img[@src]");
            if (images != null)
            {
                foreach (var img in images)
                {
                    var src = img.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src))
                    {
                        var localPath = await DownloadAsset(src, assetsDir, "images");
                        img.SetAttributeValue("src", localPath);
                    }
                }
            }

            // Font dosyalarını indir
            var fontLinks = doc.DocumentNode.SelectNodes("//link[@rel='preload' and @as='font']");
            if (fontLinks != null)
            {
                foreach (var link in fontLinks)
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href))
                    {
                        var localPath = await DownloadAsset(href, assetsDir, "fonts");
                        link.SetAttributeValue("href", localPath);
                    }
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

        private bool ShouldDownloadAsset(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            var lowerUrl = url.ToLower();
            
            // CSS, JS, resim dosyalarını her zaman indir
            if (lowerUrl.EndsWith(".css") || lowerUrl.EndsWith(".js") || 
                lowerUrl.EndsWith(".png") || lowerUrl.EndsWith(".jpg") || 
                lowerUrl.EndsWith(".jpeg") || lowerUrl.EndsWith(".gif") || 
                lowerUrl.EndsWith(".svg") || lowerUrl.EndsWith(".ico") ||
                lowerUrl.EndsWith(".woff") || lowerUrl.EndsWith(".woff2") ||
                lowerUrl.EndsWith(".ttf") || lowerUrl.EndsWith(".eot"))
            {
                return true;
            }

            // CDN dosyalarını da indir
            if (lowerUrl.Contains("cdn") || lowerUrl.Contains("static") || 
                lowerUrl.Contains("assets") || lowerUrl.Contains("files"))
            {
                return true;
            }

            // Otel ile ilgili olabilecek domain'ler
            var hotelDomains = new[] { 
                "royaltyezel.com", "hotel", "otel", "resort", "boutique", 
                "accommodation", "lodging", "hospitality"
            };

            // Eğer URL bu domain'lerden birini içeriyorsa indir
            if (hotelDomains.Any(domain => lowerUrl.Contains(domain)))
            {
                return true;
            }

            // Eğer URL'de otel ile ilgili dosya isimleri varsa indir
            var hotelFiles = new[] {
                "logo", "hotel", "otel", "room", "suite", "amenity", 
                "facility", "gallery", "contact", "about", "booking",
                "reservation", "services", "menu", "header", "footer"
            };

            if (hotelFiles.Any(file => lowerUrl.Contains(file)))
            {
                return true;
            }

            return false;
        }

        private async Task<string> DownloadAsset(string url, string assetsDir, string subDir)
        {
            try
            {
                // URL'yi normalize et
                if (!url.StartsWith("http"))
                {
                    // Relative URL'yi absolute yap
                    url = new Uri(new Uri("https://example.com"), url).ToString();
                }

                // URL'den dosya adını al
                var uri = new Uri(url);
                var fileName = Path.GetFileName(uri.LocalPath);
                
                // Eğer dosya adı yoksa veya geçersizse, URL'den türet
                if (string.IsNullOrEmpty(fileName) || fileName.Contains("?"))
                {
                    var urlHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(url))
                        .Replace("/", "_").Replace("+", "-").Replace("=", "");
                    fileName = $"{urlHash}.{GetFileExtension(url)}";
                }

                var localDir = Path.Combine(assetsDir, subDir);
                Directory.CreateDirectory(localDir);
                var localPath = Path.Combine(localDir, fileName);

                // Dosyayı indir
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(localPath, content);
                    Console.WriteLine($"Asset indirildi: {url} -> {localPath}");
                }
                else
                {
                    Console.WriteLine($"Asset indirilemedi: {url}, Status: {response.StatusCode}");
                    return url; // Orijinal URL'yi koru
                }

                return $"assets/{subDir}/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Asset indirilemedi: {url}, Hata: {ex.Message}");
                return url; // Orijinal URL'yi koru
            }
        }

        private string GetFileExtension(string url)
        {
            var lowerUrl = url.ToLower();
            if (lowerUrl.Contains(".css")) return "css";
            if (lowerUrl.Contains(".js")) return "js";
            if (lowerUrl.Contains(".png")) return "png";
            if (lowerUrl.Contains(".jpg") || lowerUrl.Contains(".jpeg")) return "jpg";
            if (lowerUrl.Contains(".gif")) return "gif";
            if (lowerUrl.Contains(".svg")) return "svg";
            if (lowerUrl.Contains(".ico")) return "ico";
            if (lowerUrl.Contains(".woff")) return "woff";
            if (lowerUrl.Contains(".woff2")) return "woff2";
            if (lowerUrl.Contains(".ttf")) return "ttf";
            if (lowerUrl.Contains(".eot")) return "eot";
            return "file";
        }

        private void ReplaceDomainUrls(HtmlDocument doc, string newDomain)
        {
            if (string.IsNullOrEmpty(newDomain))
                return;

            // Sadece belirli türdeki linkleri değiştir (otel ile ilgili olanlar)
            var hotelRelatedLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'http')]");
            if (hotelRelatedLinks != null)
            {
                foreach (var link in hotelRelatedLinks)
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href) && href.StartsWith("http"))
                    {
                        // Sadece otel ile ilgili linkleri değiştir
                        if (ShouldReplaceUrl(href))
                        {
                            try
                            {
                                var uri = new Uri(href);
                                var newUri = new Uri(uri.Scheme + "://" + newDomain + uri.PathAndQuery);
                                link.SetAttributeValue("href", newUri.ToString());
                                Console.WriteLine($"URL değiştirildi: {href} -> {newUri}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"URL değiştirilemedi: {href}, Hata: {ex.Message}");
                            }
                        }
                    }
                }
            }

            // Sadece otel ile ilgili resimleri değiştir
            var hotelImages = doc.DocumentNode.SelectNodes("//img[contains(@src, 'http')]");
            if (hotelImages != null)
            {
                foreach (var img in hotelImages)
                {
                    var src = img.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src) && src.StartsWith("http"))
                    {
                        // Sadece otel ile ilgili resimleri değiştir
                        if (ShouldReplaceUrl(src))
                        {
                            try
                            {
                                var uri = new Uri(src);
                                var newUri = new Uri(uri.Scheme + "://" + newDomain + uri.PathAndQuery);
                                img.SetAttributeValue("src", newUri.ToString());
                                Console.WriteLine($"Image URL değiştirildi: {src} -> {newUri}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Image URL değiştirilemedi: {src}, Hata: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

        private bool ShouldReplaceUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            // Sadece belirli domain'lerden gelen URL'leri değiştir
            var lowerUrl = url.ToLower();
            
            // Otel ile ilgili olabilecek domain'ler
            var hotelDomains = new[] { 
                "royaltyezel.com", "hotel", "otel", "resort", "boutique", 
                "accommodation", "lodging", "hospitality"
            };

            // Eğer URL bu domain'lerden birini içeriyorsa değiştir
            if (hotelDomains.Any(domain => lowerUrl.Contains(domain)))
            {
                return true;
            }

            // Eğer URL'de otel ile ilgili path'ler varsa değiştir
            var hotelPaths = new[] {
                "/about", "/rooms", "/amenities", "/contact", "/booking", 
                "/reservation", "/gallery", "/services", "/facilities"
            };

            if (hotelPaths.Any(path => lowerUrl.Contains(path)))
            {
                return true;
            }

            // Eğer URL'de otel ile ilgili dosya isimleri varsa değiştir
            var hotelFiles = new[] {
                "logo", "hotel", "otel", "room", "suite", "amenity", 
                "facility", "gallery", "contact", "about"
            };

            if (hotelFiles.Any(file => lowerUrl.Contains(file)))
            {
                return true;
            }

            return false;
        }

        private void ReplaceSpecificDomain(HtmlDocument doc, string oldDomain, string newDomain)
        {
            // Tüm href attribute'larını bul ve royaltyezel.com'u değiştir
            var allLinks = doc.DocumentNode.SelectNodes("//*[@href]");
            if (allLinks != null)
            {
                foreach (var link in allLinks)
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href) && href.Contains(oldDomain))
                    {
                        var newHref = href.Replace(oldDomain, newDomain);
                        link.SetAttributeValue("href", newHref);
                        Console.WriteLine($"Specific domain değiştirildi: {href} -> {newHref}");
                    }
                }
            }

            // Tüm src attribute'larını bul ve royaltyezel.com'u değiştir
            var allImages = doc.DocumentNode.SelectNodes("//*[@src]");
            if (allImages != null)
            {
                foreach (var img in allImages)
                {
                    var src = img.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src) && src.Contains(oldDomain))
                    {
                        var newSrc = src.Replace(oldDomain, newDomain);
                        img.SetAttributeValue("src", newSrc);
                        Console.WriteLine($"Specific image domain değiştirildi: {src} -> {newSrc}");
                    }
                }
            }

            // Tüm data-src attribute'larını bul ve royaltyezel.com'u değiştir
            var allDataSrc = doc.DocumentNode.SelectNodes("//*[@data-src]");
            if (allDataSrc != null)
            {
                foreach (var element in allDataSrc)
                {
                    var dataSrc = element.GetAttributeValue("data-src", "");
                    if (!string.IsNullOrEmpty(dataSrc) && dataSrc.Contains(oldDomain))
                    {
                        var newDataSrc = dataSrc.Replace(oldDomain, newDomain);
                        element.SetAttributeValue("data-src", newDataSrc);
                        Console.WriteLine($"Specific data-src domain değiştirildi: {dataSrc} -> {newDataSrc}");
                    }
                }
            }
        }
    }
}