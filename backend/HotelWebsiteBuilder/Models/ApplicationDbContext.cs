using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HotelWebsiteBuilder.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }

                            protected override void OnModelCreating(ModelBuilder modelBuilder)
                    {
                        base.OnModelCreating(modelBuilder);

                        // Configure table name for PostgreSQL
                        modelBuilder.Entity<Hotel>().ToTable("Hotels");

                        // Seed data for in-memory database
                        modelBuilder.Entity<Hotel>().HasData(
                            new Hotel
                            {
                                Id = 1,
                                HotelName = "Grand Istanbul Hotel",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Grand+Istanbul",
                                Phone = "+90 212 555 0123",
                                Email = "info@grandistanbul.com",
                                Address = "Sultanahmet Meydanı No:1, İstanbul, Türkiye",
                    Website = "https://grandistanbul.com",
                    CheckInTime = "14:00",
                    CheckOutTime = "12:00",
                    StarRating = 5,
                    Currency = "TRY",
                    Language = "tr",
                    
                    // Gallery images as JSON
                    GalleryImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/800x600?text=Istanbul+View",
                        "https://via.placeholder.com/800x600?text=Hotel+Lobby",
                        "https://via.placeholder.com/800x600?text=Deluxe+Room",
                        "https://via.placeholder.com/800x600?text=Restaurant",
                        "https://via.placeholder.com/800x600?text=Spa+Center"
                    }),
                    
                    // Slider images as JSON
                    SliderImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/1200x600?text=Grand+Istanbul+Slider+1",
                        "https://via.placeholder.com/1200x600?text=Grand+Istanbul+Slider+2",
                        "https://via.placeholder.com/1200x600?text=Grand+Istanbul+Slider+3"
                    }),
                    
                    // Social media
                    Facebook = "https://facebook.com/grandistanbulhotel",
                    Instagram = "https://instagram.com/grandistanbulhotel",
                    Twitter = "https://twitter.com/grandistanbul",
                    LinkedIn = "https://linkedin.com/company/grandistanbulhotel",
                    YouTube = "https://youtube.com/grandistanbulhotel",
                    
                    // Basic info
                    Description = "İstanbul'un kalbinde, tarihi yarımadada konumlanan lüks otelimizde unutulmaz bir konaklama deneyimi yaşayın.",
                    
                    // Amenities as JSON
                    AmenitiesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "Ücretsiz Wi-Fi", "Spa & Wellness", "Restoran", "Bar", "Havuz", 
                        "Fitness Merkezi", "Oda Servisi", "Valet Parking", "Konferans Salonu"
                    }),
                    
                    // Rooms as JSON
                    RoomsJson = JsonSerializer.Serialize(new List<HotelRoom>
                    {
                        new HotelRoom
                        {
                            Type = "Standart Oda",
                            Description = "Konforlu ve şık standart odalarımızda rahat bir konaklama deneyimi.",
                            Price = 1200,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Wi-Fi" },
                            Images = new List<string> { "standart1.jpg", "standart2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Deluxe Oda",
                            Description = "Geniş ve lüks deluxe odalarımızda şehir manzarası eşliğinde konaklama.",
                            Price = 1800,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Wi-Fi", "Şehir Manzarası" },
                            Images = new List<string> { "deluxe1.jpg", "deluxe2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Süit Oda",
                            Description = "Lüks süit odalarımızda ayrı oturma alanı ve jakuzi ile konforlu konaklama.",
                            Price = 2800,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Wi-Fi", "Jakuzi", "Oturma Alanı" },
                            Images = new List<string> { "suit1.jpg", "suit2.jpg" },
                            Capacity = 3,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Başkanlık Süiti",
                            Description = "En lüks başkanlık süitimizde özel hizmetler ve geniş alan.",
                            Price = 5000,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Wi-Fi", "Jakuzi", "Oturma Alanı", "Özel Hizmetler" },
                            Images = new List<string> { "presidential1.jpg", "presidential2.jpg" },
                            Capacity = 4,
                            IsAvailable = true
                        }
                    }),
                    
                    // Facilities as JSON
                    FacilitiesJson = JsonSerializer.Serialize(new List<HotelFacility>
                    {
                        new HotelFacility
                        {
                            Name = "Spa & Wellness",
                            Description = "Rahatlatıcı spa hizmetleri ve wellness merkezi",
                            Icon = "spa-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Restoran",
                            Description = "Geleneksel ve modern lezzetlerin buluştuğu restoran",
                            Icon = "restaurant-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Havuz",
                            Description = "Kapalı ve açık havuz seçenekleri",
                            Icon = "pool-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Fitness Merkezi",
                            Description = "Modern ekipmanlarla donatılmış fitness merkezi",
                            Icon = "fitness-icon.png",
                            IsAvailable = true
                        }
                    }),
                    
                    // Meta information
                    MetaTitle = "Grand Istanbul Hotel - İstanbul'daki Lüks Otel",
                    MetaDescription = "İstanbul'un kalbinde, tarihi yarımadada konumlanan lüks otelimizde unutulmaz bir konaklama deneyimi yaşayın.",
                    MetaKeywords = "istanbul, otel, lüks, sultanahmet, konaklama, spa, restoran",
                    
                    // Location
                    Latitude = 41.0082,
                    Longitude = 28.9784,
                    
                    // Legacy fields for backward compatibility
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Istanbul+View",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Hotel+Lobby",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Deluxe+Room",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Restaurant",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Spa+Center",
                                RoomTypes = "Standart Oda, Deluxe Oda, Süit Oda, Başkanlık Süiti",
                                Pricing = "Standart: 1200 TL/gece, Deluxe: 1800 TL/gece, Süit: 2800 TL/gece, Başkanlık: 5000 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 2,
                                HotelName = "Blue Sea Resort",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Blue+Sea",
                                Phone = "+90 242 555 0456",
                                Email = "info@bluesea.com",
                                Address = "Konyaaltı Sahili No:15, Antalya, Türkiye",
                    Website = "https://bluesea.com",
                    CheckInTime = "15:00",
                    CheckOutTime = "11:00",
                    StarRating = 4,
                    Currency = "TRY",
                    Language = "tr",
                    
                    // Gallery images as JSON
                    GalleryImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/800x600?text=Sea+View",
                        "https://via.placeholder.com/800x600?text=Pool+Area",
                        "https://via.placeholder.com/800x600?text=Beach+Access",
                        "https://via.placeholder.com/800x600?text=Water+Sports",
                        "https://via.placeholder.com/800x600?text=Sunset+Bar"
                    }),
                    
                    // Slider images as JSON
                    SliderImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/1200x600?text=Blue+Sea+Slider+1",
                        "https://via.placeholder.com/1200x600?text=Blue+Sea+Slider+2",
                        "https://via.placeholder.com/1200x600?text=Blue+Sea+Slider+3"
                    }),
                    
                    // Social media
                                Facebook = "https://facebook.com/bluesearesort",
                                Instagram = "https://instagram.com/bluesearesort",
                                Twitter = "https://twitter.com/bluesea",
                    LinkedIn = "https://linkedin.com/company/bluesearesort",
                    YouTube = "https://youtube.com/bluesearesort",
                    
                    // Basic info
                                Description = "Antalya'nın en güzel sahillerinde, mavi denizin kucakladığı resort otelimizde tatil keyfini yaşayın.",
                    
                    // Amenities as JSON
                    AmenitiesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "Deniz Manzarası", "Havuz", "Plaj Erişimi", "Su Sporları", "Spa", 
                        "Restoran", "Bar", "Çocuk Kulübü", "Animasyon"
                    }),
                    
                    // Rooms as JSON
                    RoomsJson = JsonSerializer.Serialize(new List<HotelRoom>
                    {
                        new HotelRoom
                        {
                            Type = "Deniz Manzaralı Oda",
                            Description = "Masmavi deniz manzarası eşliğinde konforlu konaklama.",
                            Price = 1500,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Deniz Manzarası", "Balkon" },
                            Images = new List<string> { "sea-view1.jpg", "sea-view2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Aile Odası",
                            Description = "Geniş aile odalarımızda çocuklarınızla birlikte rahat konaklama.",
                            Price = 2200,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "İki Yatak", "Çocuk Köşesi" },
                            Images = new List<string> { "family1.jpg", "family2.jpg" },
                            Capacity = 4,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Villa",
                            Description = "Özel villalarımızda lüks ve konfor bir arada.",
                            Price = 3500,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Özel Havuz", "Bahçe" },
                            Images = new List<string> { "villa1.jpg", "villa2.jpg" },
                            Capacity = 6,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Başkanlık Villa",
                            Description = "En lüks başkanlık villamızda özel hizmetler.",
                            Price = 6000,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Özel Havuz", "Bahçe", "Özel Hizmetler" },
                            Images = new List<string> { "presidential-villa1.jpg", "presidential-villa2.jpg" },
                            Capacity = 8,
                            IsAvailable = true
                        }
                    }),
                    
                    // Facilities as JSON
                    FacilitiesJson = JsonSerializer.Serialize(new List<HotelFacility>
                    {
                        new HotelFacility
                        {
                            Name = "Su Sporları",
                            Description = "Dalış, jet ski, parasailing gibi su sporları aktiviteleri",
                            Icon = "water-sports-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Spa & Wellness",
                            Description = "Deniz manzarası eşliğinde spa ve wellness hizmetleri",
                            Icon = "spa-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Çocuk Kulübü",
                            Description = "Çocuklar için özel aktivite alanı ve animasyon",
                            Icon = "kids-club-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Plaj",
                            Description = "Özel plaj alanı ve şezlong hizmeti",
                            Icon = "beach-icon.png",
                            IsAvailable = true
                        }
                    }),
                    
                    // Meta information
                    MetaTitle = "Blue Sea Resort - Antalya'daki Resort Otel",
                    MetaDescription = "Antalya'nın en güzel sahillerinde, mavi denizin kucakladığı resort otelimizde tatil keyfini yaşayın.",
                    MetaKeywords = "antalya, resort, deniz, plaj, tatil, spa, su sporları",
                    
                    // Location
                    Latitude = 36.8969,
                    Longitude = 30.7133,
                    
                    // Legacy fields for backward compatibility
                    GalleryImage1 = "https://via.placeholder.com/800x600?text=Sea+View",
                    GalleryImage2 = "https://via.placeholder.com/800x600?text=Pool+Area",
                    GalleryImage3 = "https://via.placeholder.com/800x600?text=Beach+Access",
                    GalleryImage4 = "https://via.placeholder.com/800x600?text=Water+Sports",
                    GalleryImage5 = "https://via.placeholder.com/800x600?text=Sunset+Bar",
                    RoomTypes = "Deniz Manzaralı Oda, Aile Odası, Villa, Başkanlık Villa",
                    Pricing = "Deniz Manzaralı: 1500 TL/gece, Aile: 2200 TL/gece, Villa: 3500 TL/gece, Başkanlık: 6000 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 3,
                                HotelName = "Cappadocia Cave Hotel",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Cappadocia+Cave",
                                Phone = "+90 384 555 0789",
                                Email = "info@cappadociacave.com",
                                Address = "Göreme Vadisi No:25, Nevşehir, Türkiye",
                    Website = "https://cappadociacave.com",
                    CheckInTime = "13:00",
                    CheckOutTime = "11:00",
                    StarRating = 5,
                    Currency = "TRY",
                    Language = "tr",
                    
                    // Gallery images as JSON
                    GalleryImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/800x600?text=Cave+Room",
                        "https://via.placeholder.com/800x600?text=Hot+Air+Balloon",
                        "https://via.placeholder.com/800x600?text=Valley+View",
                        "https://via.placeholder.com/800x600?text=Traditional+Breakfast",
                        "https://via.placeholder.com/800x600?text=Sunset+Terrace"
                    }),
                    
                    // Slider images as JSON
                    SliderImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/1200x600?text=Cappadocia+Slider+1",
                        "https://via.placeholder.com/1200x600?text=Cappadocia+Slider+2",
                        "https://via.placeholder.com/1200x600?text=Cappadocia+Slider+3"
                    }),
                    
                    // Social media
                    Facebook = "https://facebook.com/cappadociacavehotel",
                    Instagram = "https://instagram.com/cappadociacavehotel",
                    Twitter = "https://twitter.com/cappadociacave",
                    LinkedIn = "https://linkedin.com/company/cappadociacavehotel",
                    YouTube = "https://youtube.com/cappadociacavehotel",
                    
                    // Basic info
                    Description = "Kapadokya'nın eşsiz doğal güzelliklerinde, mağara odalarında benzersiz bir konaklama deneyimi.",
                    
                    // Amenities as JSON
                    AmenitiesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "Mağara Odaları", "Balon Turu", "Vadi Manzarası", "Geleneksel Kahvaltı", 
                        "Spa", "Restoran", "Bar", "Ücretsiz Wi-Fi", "Otopark"
                    }),
                    
                    // Rooms as JSON
                    RoomsJson = JsonSerializer.Serialize(new List<HotelRoom>
                    {
                        new HotelRoom
                        {
                            Type = "Standart Mağara Odası",
                            Description = "Doğal mağara yapısında konforlu konaklama.",
                            Price = 1800,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Mağara Yapısı", "Vadi Manzarası" },
                            Images = new List<string> { "cave-standard1.jpg", "cave-standard2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Deluxe Mağara Süiti",
                            Description = "Geniş mağara süitinde lüks konaklama.",
                            Price = 2800,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Jakuzi", "Oturma Alanı", "Özel Teras" },
                            Images = new List<string> { "cave-deluxe1.jpg", "cave-deluxe2.jpg" },
                            Capacity = 3,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Başkanlık Mağara Süiti",
                            Description = "En lüks mağara süitimizde özel hizmetler.",
                            Price = 4500,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Jakuzi", "Oturma Alanı", "Özel Teras", "Özel Hizmetler" },
                            Images = new List<string> { "cave-presidential1.jpg", "cave-presidential2.jpg" },
                            Capacity = 4,
                            IsAvailable = true
                        }
                    }),
                    
                    // Facilities as JSON
                    FacilitiesJson = JsonSerializer.Serialize(new List<HotelFacility>
                    {
                        new HotelFacility
                        {
                            Name = "Balon Turu",
                            Description = "Kapadokya'nın eşsiz manzarasını balonla keşfedin",
                            Icon = "balloon-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Geleneksel Kahvaltı",
                            Description = "Yöresel lezzetlerle zengin kahvaltı büfesi",
                            Icon = "breakfast-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Spa & Wellness",
                            Description = "Mağara yapısında spa ve wellness hizmetleri",
                            Icon = "spa-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Vadi Turu",
                            Description = "Rehber eşliğinde vadi keşif turları",
                            Icon = "valley-tour-icon.png",
                            IsAvailable = true
                        }
                    }),
                    
                    // Meta information
                    MetaTitle = "Cappadocia Cave Hotel - Kapadokya'daki Mağara Oteli",
                    MetaDescription = "Kapadokya'nın eşsiz doğal güzelliklerinde, mağara odalarında benzersiz bir konaklama deneyimi.",
                    MetaKeywords = "kapadokya, mağara oteli, balon turu, nevşehir, göreme, vadi",
                    
                    // Location
                    Latitude = 38.6431,
                    Longitude = 34.8283,
                    
                    // Legacy fields for backward compatibility
                    GalleryImage1 = "https://via.placeholder.com/800x600?text=Cave+Room",
                    GalleryImage2 = "https://via.placeholder.com/800x600?text=Hot+Air+Balloon",
                    GalleryImage3 = "https://via.placeholder.com/800x600?text=Valley+View",
                    GalleryImage4 = "https://via.placeholder.com/800x600?text=Traditional+Breakfast",
                    GalleryImage5 = "https://via.placeholder.com/800x600?text=Sunset+Terrace",
                    RoomTypes = "Standart Mağara Odası, Deluxe Mağara Süiti, Başkanlık Mağara Süiti",
                    Pricing = "Standart: 1800 TL/gece, Deluxe: 2800 TL/gece, Başkanlık: 4500 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 4,
                                HotelName = "Bosphorus Palace Hotel",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Bosphorus+Palace",
                                Phone = "+90 212 555 0321",
                                Email = "info@bosphoruspalace.com",
                                Address = "Ortaköy Mahallesi No:8, İstanbul, Türkiye",
                    Website = "https://bosphoruspalace.com",
                    CheckInTime = "14:00",
                    CheckOutTime = "12:00",
                    StarRating = 5,
                    Currency = "TRY",
                    Language = "tr",
                    
                    // Gallery images as JSON
                    GalleryImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/800x600?text=Bosphorus+View",
                        "https://via.placeholder.com/800x600?text=Luxury+Lobby",
                        "https://via.placeholder.com/800x600?text=Palace+Suite",
                        "https://via.placeholder.com/800x600?text=Fine+Dining",
                        "https://via.placeholder.com/800x600?text=Rooftop+Bar"
                    }),
                    
                    // Slider images as JSON
                    SliderImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/1200x600?text=Bosphorus+Slider+1",
                        "https://via.placeholder.com/1200x600?text=Bosphorus+Slider+2",
                        "https://via.placeholder.com/1200x600?text=Bosphorus+Slider+3"
                    }),
                    
                    // Social media
                    Facebook = "https://facebook.com/bosphoruspalacehotel",
                    Instagram = "https://instagram.com/bosphoruspalacehotel",
                    Twitter = "https://twitter.com/bosphoruspalace",
                    LinkedIn = "https://linkedin.com/company/bosphoruspalacehotel",
                    YouTube = "https://youtube.com/bosphoruspalacehotel",
                    
                    // Basic info
                    Description = "Boğaz'ın eşsiz manzarasında, lüks ve konforun buluştuğu palace otelimizde unutulmaz anlar yaşayın.",
                    
                    // Amenities as JSON
                    AmenitiesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "Boğaz Manzarası", "Lüks Spa", "Fine Dining", "Rooftop Bar", 
                        "Helikopter Transferi", "Valet Parking", "Butler Hizmeti", "Özel Havuz"
                    }),
                    
                    // Rooms as JSON
                    RoomsJson = JsonSerializer.Serialize(new List<HotelRoom>
                    {
                        new HotelRoom
                        {
                            Type = "Boğaz Manzaralı Oda",
                            Description = "Boğaz'ın eşsiz manzarası eşliğinde lüks konaklama.",
                            Price = 3500,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Boğaz Manzarası", "Balkon" },
                            Images = new List<string> { "bosphorus-view1.jpg", "bosphorus-view2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Palace Süiti",
                            Description = "Lüks palace süitimizde butler hizmeti ile konaklama.",
                            Price = 5500,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Jakuzi", "Oturma Alanı", "Butler Hizmeti" },
                            Images = new List<string> { "palace-suite1.jpg", "palace-suite2.jpg" },
                            Capacity = 3,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Başkanlık Palace Süiti",
                            Description = "En lüks başkanlık palace süitimizde özel hizmetler.",
                            Price = 8500,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Jakuzi", "Oturma Alanı", "Butler Hizmeti", "Özel Havuz" },
                            Images = new List<string> { "presidential-palace1.jpg", "presidential-palace2.jpg" },
                            Capacity = 4,
                            IsAvailable = true
                        }
                    }),
                    
                    // Facilities as JSON
                    FacilitiesJson = JsonSerializer.Serialize(new List<HotelFacility>
                    {
                        new HotelFacility
                        {
                            Name = "Fine Dining",
                            Description = "Michelin yıldızlı şeflerin hazırladığı özel menüler",
                            Icon = "fine-dining-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Lüks Spa",
                            Description = "Boğaz manzarası eşliğinde lüks spa hizmetleri",
                            Icon = "luxury-spa-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Rooftop Bar",
                            Description = "Boğaz manzarası eşliğinde kokteyl ve canlı müzik",
                            Icon = "rooftop-bar-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Helikopter Transferi",
                            Description = "Havalimanından lüks helikopter transferi",
                            Icon = "helicopter-icon.png",
                            IsAvailable = true
                        }
                    }),
                    
                    // Meta information
                    MetaTitle = "Bosphorus Palace Hotel - İstanbul'daki Lüks Palace Otel",
                    MetaDescription = "Boğaz'ın eşsiz manzarasında, lüks ve konforun buluştuğu palace otelimizde unutulmaz anlar yaşayın.",
                    MetaKeywords = "istanbul, boğaz, palace otel, lüks, fine dining, spa",
                    
                    // Location
                    Latitude = 41.0553,
                    Longitude = 29.0273,
                    
                    // Legacy fields for backward compatibility
                    GalleryImage1 = "https://via.placeholder.com/800x600?text=Bosphorus+View",
                    GalleryImage2 = "https://via.placeholder.com/800x600?text=Luxury+Lobby",
                    GalleryImage3 = "https://via.placeholder.com/800x600?text=Palace+Suite",
                    GalleryImage4 = "https://via.placeholder.com/800x600?text=Fine+Dining",
                    GalleryImage5 = "https://via.placeholder.com/800x600?text=Rooftop+Bar",
                    RoomTypes = "Boğaz Manzaralı Oda, Palace Süiti, Başkanlık Palace Süiti",
                    Pricing = "Boğaz Manzaralı: 3500 TL/gece, Palace: 5500 TL/gece, Başkanlık: 8500 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 5,
                                HotelName = "Pamukkale Thermal Hotel",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Pamukkale+Thermal",
                                Phone = "+90 258 555 0654",
                                Email = "info@pamukkale.com",
                                Address = "Pamukkale Travertenleri No:12, Denizli, Türkiye",
                    Website = "https://pamukkale.com",
                    CheckInTime = "14:00",
                    CheckOutTime = "11:00",
                    StarRating = 4,
                    Currency = "TRY",
                    Language = "tr",
                    
                    // Gallery images as JSON
                    GalleryImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/800x600?text=Thermal+Pool",
                        "https://via.placeholder.com/800x600?text=Travertine+View",
                        "https://via.placeholder.com/800x600?text=Ancient+Pool",
                        "https://via.placeholder.com/800x600?text=Wellness+Center",
                        "https://via.placeholder.com/800x600?text=Natural+Springs"
                    }),
                    
                    // Slider images as JSON
                    SliderImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/1200x600?text=Pamukkale+Slider+1",
                        "https://via.placeholder.com/1200x600?text=Pamukkale+Slider+2",
                        "https://via.placeholder.com/1200x600?text=Pamukkale+Slider+3"
                    }),
                    
                    // Social media
                    Facebook = "https://facebook.com/pamukkalethermalhotel",
                    Instagram = "https://instagram.com/pamukkalethermalhotel",
                    Twitter = "https://twitter.com/pamukkalethermal",
                    LinkedIn = "https://linkedin.com/company/pamukkalethermalhotel",
                    YouTube = "https://youtube.com/pamukkalethermalhotel",
                    
                    // Basic info
                    Description = "Pamukkale'nin doğal termal kaynaklarında, sağlık ve dinlenmenin buluştuğu termal otelimizde şifa bulun.",
                    
                    // Amenities as JSON
                    AmenitiesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "Termal Havuzlar", "Doğal Kaynaklar", "Spa & Wellness", "Sağlık Merkezi", 
                        "Restoran", "Bar", "Ücretsiz Wi-Fi", "Otopark", "Tarihi Tur"
                    }),
                    
                    // Rooms as JSON
                    RoomsJson = JsonSerializer.Serialize(new List<HotelRoom>
                    {
                        new HotelRoom
                        {
                            Type = "Termal Oda",
                            Description = "Termal su ile donatılmış özel odalar.",
                            Price = 1200,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Termal Su", "Balkon" },
                            Images = new List<string> { "thermal-room1.jpg", "thermal-room2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Spa Süiti",
                            Description = "Özel spa hizmetleri ile donatılmış süit.",
                            Price = 2000,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Özel Spa", "Jakuzi" },
                            Images = new List<string> { "spa-suite1.jpg", "spa-suite2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Aile Termal Odası",
                            Description = "Aileler için geniş termal odalar.",
                            Price = 2800,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Termal Su", "İki Yatak" },
                            Images = new List<string> { "family-thermal1.jpg", "family-thermal2.jpg" },
                            Capacity = 4,
                            IsAvailable = true
                        }
                    }),
                    
                    // Facilities as JSON
                    FacilitiesJson = JsonSerializer.Serialize(new List<HotelFacility>
                    {
                        new HotelFacility
                        {
                            Name = "Termal Havuzlar",
                            Description = "Doğal termal kaynaklardan gelen şifalı sular",
                            Icon = "thermal-pool-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Spa & Wellness",
                            Description = "Termal su ile spa ve wellness hizmetleri",
                            Icon = "spa-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Sağlık Merkezi",
                            Description = "Uzman doktorlar eşliğinde sağlık hizmetleri",
                            Icon = "health-center-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Tarihi Tur",
                            Description = "Hierapolis antik kenti ve travertenler turu",
                            Icon = "historical-tour-icon.png",
                            IsAvailable = true
                        }
                    }),
                    
                    // Meta information
                    MetaTitle = "Pamukkale Thermal Hotel - Denizli'deki Termal Otel",
                    MetaDescription = "Pamukkale'nin doğal termal kaynaklarında, sağlık ve dinlenmenin buluştuğu termal otelimizde şifa bulun.",
                    MetaKeywords = "pamukkale, termal, denizli, spa, wellness, sağlık",
                    
                    // Location
                    Latitude = 37.9244,
                    Longitude = 29.1214,
                    
                    // Legacy fields for backward compatibility
                    GalleryImage1 = "https://via.placeholder.com/800x600?text=Thermal+Pool",
                    GalleryImage2 = "https://via.placeholder.com/800x600?text=Travertine+View",
                    GalleryImage3 = "https://via.placeholder.com/800x600?text=Ancient+Pool",
                    GalleryImage4 = "https://via.placeholder.com/800x600?text=Wellness+Center",
                    GalleryImage5 = "https://via.placeholder.com/800x600?text=Natural+Springs",
                    RoomTypes = "Termal Oda, Spa Süiti, Aile Termal Odası",
                    Pricing = "Termal: 1200 TL/gece, Spa: 2000 TL/gece, Aile: 2800 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 6,
                                HotelName = "Mount Ararat Boutique Hotel",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Mount+Ararat",
                                Phone = "+90 472 555 0987",
                                Email = "info@mountararat.com",
                                Address = "Doğubayazıt Yolu No:45, Ağrı, Türkiye",
                    Website = "https://mountararat.com",
                    CheckInTime = "15:00",
                    CheckOutTime = "11:00",
                    StarRating = 3,
                    Currency = "TRY",
                    Language = "tr",
                    
                    // Gallery images as JSON
                    GalleryImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/800x600?text=Mountain+View",
                        "https://via.placeholder.com/800x600?text=Cozy+Lobby",
                        "https://via.placeholder.com/800x600?text=Local+Cuisine",
                        "https://via.placeholder.com/800x600?text=Adventure+Tour",
                        "https://via.placeholder.com/800x600?text=Sunset+Terrace"
                    }),
                    
                    // Slider images as JSON
                    SliderImagesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "https://via.placeholder.com/1200x600?text=Mount+Ararat+Slider+1",
                        "https://via.placeholder.com/1200x600?text=Mount+Ararat+Slider+2",
                        "https://via.placeholder.com/1200x600?text=Mount+Ararat+Slider+3"
                    }),
                    
                    // Social media
                    Facebook = "https://facebook.com/mountararatboutiquehotel",
                    Instagram = "https://instagram.com/mountararatboutiquehotel",
                    Twitter = "https://twitter.com/mountararat",
                    LinkedIn = "https://linkedin.com/company/mountararatboutiquehotel",
                    YouTube = "https://youtube.com/mountararatboutiquehotel",
                    
                    // Basic info
                    Description = "Ağrı Dağı'nın eteklerinde, doğal güzelliklerin ortasında konforlu bir konaklama deneyimi.",
                    
                    // Amenities as JSON
                    AmenitiesJson = JsonSerializer.Serialize(new List<string>
                    {
                        "Dağ Manzarası", "Yerel Mutfak", "Macera Turu", "Ücretsiz Wi-Fi", 
                        "Restoran", "Bar", "Otopark", "Rehberlik Hizmeti"
                    }),
                    
                    // Rooms as JSON
                    RoomsJson = JsonSerializer.Serialize(new List<HotelRoom>
                    {
                        new HotelRoom
                        {
                            Type = "Standart Oda",
                            Description = "Dağ manzarası eşliğinde konforlu konaklama.",
                            Price = 800,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Dağ Manzarası" },
                            Images = new List<string> { "standard1.jpg", "standard2.jpg" },
                            Capacity = 2,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Aile Odası",
                            Description = "Aileler için geniş ve konforlu odalar.",
                            Price = 1200,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "İki Yatak", "Dağ Manzarası" },
                            Images = new List<string> { "family1.jpg", "family2.jpg" },
                            Capacity = 4,
                            IsAvailable = true
                        },
                        new HotelRoom
                        {
                            Type = "Macera Süiti",
                            Description = "Macera tutkunları için özel tasarlanmış süit.",
                            Price = 1800,
                            Features = new List<string> { "Klima", "TV", "Mini bar", "Özel Ekipman", "Rehberlik" },
                            Images = new List<string> { "adventure-suite1.jpg", "adventure-suite2.jpg" },
                            Capacity = 3,
                            IsAvailable = true
                        }
                    }),
                    
                    // Facilities as JSON
                    FacilitiesJson = JsonSerializer.Serialize(new List<HotelFacility>
                    {
                        new HotelFacility
                        {
                            Name = "Macera Turu",
                            Description = "Ağrı Dağı ve çevresinde rehber eşliğinde turlar",
                            Icon = "adventure-tour-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Yerel Mutfak",
                            Description = "Doğu Anadolu'nun geleneksel lezzetleri",
                            Icon = "local-cuisine-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Rehberlik Hizmeti",
                            Description = "Deneyimli rehberler eşliğinde turlar",
                            Icon = "guide-service-icon.png",
                            IsAvailable = true
                        },
                        new HotelFacility
                        {
                            Name = "Fotoğraf Turu",
                            Description = "Profesyonel fotoğrafçı eşliğinde turlar",
                            Icon = "photo-tour-icon.png",
                            IsAvailable = true
                        }
                    }),
                    
                    // Meta information
                    MetaTitle = "Mount Ararat Boutique Hotel - Ağrı'daki Boutique Otel",
                    MetaDescription = "Ağrı Dağı'nın eteklerinde, doğal güzelliklerin ortasında konforlu bir konaklama deneyimi.",
                    MetaKeywords = "ağrı, ağrı dağı, boutique otel, macera, doğu anadolu",
                    
                    // Location
                    Latitude = 39.7191,
                    Longitude = 44.0433,
                    
                    // Legacy fields for backward compatibility
                    GalleryImage1 = "https://via.placeholder.com/800x600?text=Mountain+View",
                    GalleryImage2 = "https://via.placeholder.com/800x600?text=Cozy+Lobby",
                    GalleryImage3 = "https://via.placeholder.com/800x600?text=Local+Cuisine",
                    GalleryImage4 = "https://via.placeholder.com/800x600?text=Adventure+Tour",
                    GalleryImage5 = "https://via.placeholder.com/800x600?text=Sunset+Terrace",
                    RoomTypes = "Standart Oda, Aile Odası, Macera Süiti",
                    Pricing = "Standart: 800 TL/gece, Aile: 1200 TL/gece, Macera: 1800 TL/gece"
                            }
                        );
                    }
    }
} 