using Microsoft.EntityFrameworkCore;

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
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Istanbul+View",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Hotel+Lobby",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Deluxe+Room",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Restaurant",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Spa+Center",
                                Facebook = "https://facebook.com/grandistanbulhotel",
                                Instagram = "https://instagram.com/grandistanbulhotel",
                                Twitter = "https://twitter.com/grandistanbul",
                                Website = "https://grandistanbul.com",
                                Description = "İstanbul'un kalbinde, tarihi yarımadada konumlanan lüks otelimizde unutulmaz bir konaklama deneyimi yaşayın.",
                                Amenities = "Ücretsiz Wi-Fi, Spa & Wellness, Restoran, Bar, Havuz, Fitness Merkezi, Oda Servisi, Valet Parking",
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
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Sea+View",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Pool+Area",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Beach+Access",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Water+Sports",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Sunset+Bar",
                                Facebook = "https://facebook.com/bluesearesort",
                                Instagram = "https://instagram.com/bluesearesort",
                                Twitter = "https://twitter.com/bluesea",
                                Website = "https://bluesea.com",
                                Description = "Antalya'nın en güzel sahillerinde, mavi denizin kucakladığı resort otelimizde tatil keyfini yaşayın.",
                                Amenities = "Deniz Manzarası, Havuz, Plaj Erişimi, Su Sporları, Spa, Restoran, Bar, Çocuk Kulübü",
                                RoomTypes = "Deniz Manzaralı Oda, Aile Odası, Villa, Başkanlık Villa",
                                Pricing = "Deniz Manzaralı: 1500 TL/gece, Aile: 2200 TL/gece, Villa: 3500 TL/gece, Başkanlık: 6000 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 3,
                                HotelName = "Mountain Lodge",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Mountain+Lodge",
                                Phone = "+90 352 555 0789",
                                Email = "info@mountainlodge.com",
                                Address = "Erciyes Dağı No:25, Kayseri, Türkiye",
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Mountain+View",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Ski+Slopes",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Cozy+Room",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Fireplace",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Spa+Center",
                                Facebook = "https://facebook.com/mountainlodge",
                                Instagram = "https://instagram.com/mountainlodge",
                                Twitter = "https://twitter.com/mountainlodge",
                                Website = "https://mountainlodge.com",
                                Description = "Erciyes Dağı'nın eteklerinde, doğanın kalbinde konforlu bir konaklama deneyimi sunuyoruz.",
                                Amenities = "Kayak Merkezi, Spa & Wellness, Restoran, Bar, Şömine, Wi-Fi, Otopark",
                                RoomTypes = "Standart Oda, Dağ Manzaralı Oda, Süit, Aile Odası",
                                Pricing = "Standart: 800 TL/gece, Dağ Manzaralı: 1200 TL/gece, Süit: 2000 TL/gece, Aile: 1800 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 4,
                                HotelName = "Cappadocia Cave Hotel",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Cave+Hotel",
                                Phone = "+90 384 555 0321",
                                Email = "info@cavehotel.com",
                                Address = "Göreme No:8, Nevşehir, Türkiye",
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Cave+Room",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Hot+Air+Balloon",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Valley+View",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Terrace",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Sunrise",
                                Facebook = "https://facebook.com/cavehotel",
                                Instagram = "https://instagram.com/cavehotel",
                                Twitter = "https://twitter.com/cavehotel",
                                Website = "https://cavehotel.com",
                                Description = "Kapadokya'nın eşsiz doğal güzelliklerinde, tarihi mağara odalarında benzersiz bir deneyim yaşayın.",
                                Amenities = "Balon Turu, Mağara Odalar, Teras, Restoran, Bar, Wi-Fi, Otopark",
                                RoomTypes = "Standart Mağara Oda, Deluxe Mağara Oda, Süit Mağara, Aile Mağara",
                                Pricing = "Standart: 1000 TL/gece, Deluxe: 1500 TL/gece, Süit: 2500 TL/gece, Aile: 2000 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 5,
                                HotelName = "Bosphorus Palace",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Bosphorus+Palace",
                                Phone = "+90 212 555 0654",
                                Email = "info@bosphoruspalace.com",
                                Address = "Beşiktaş Sahil No:12, İstanbul, Türkiye",
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Bosphorus+View",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Palace+Lobby",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Luxury+Suite",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Fine+Dining",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Marina",
                                Facebook = "https://facebook.com/bosphoruspalace",
                                Instagram = "https://instagram.com/bosphoruspalace",
                                Twitter = "https://twitter.com/bosphoruspalace",
                                Website = "https://bosphoruspalace.com",
                                Description = "Boğaz'ın incisi, lüks ve konforun buluştuğu saray otelimizde unutulmaz anlar yaşayın.",
                                Amenities = "Boğaz Manzarası, Lüks Spa, Fine Dining, Marina, Helikopter Pist, Valet Parking",
                                RoomTypes = "Deluxe Oda, Süit, Başkanlık Süiti, Royal Villa",
                                Pricing = "Deluxe: 2500 TL/gece, Süit: 4000 TL/gece, Başkanlık: 8000 TL/gece, Royal: 15000 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 6,
                                HotelName = "Aegean Pearl",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Aegean+Pearl",
                                Phone = "+90 232 555 0987",
                                Email = "info@aegeanpearl.com",
                                Address = "Çeşme Sahil No:30, İzmir, Türkiye",
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Aegean+Sea",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Beach+Club",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Water+Sports",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Sunset+Terrace",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Wine+Cellar",
                                Facebook = "https://facebook.com/aegeanpearl",
                                Instagram = "https://instagram.com/aegeanpearl",
                                Twitter = "https://twitter.com/aegeanpearl",
                                Website = "https://aegeanpearl.com",
                                Description = "Ege Denizi'nin masmavi sularında, Çeşme'nin en güzel koyunda lüks tatil deneyimi.",
                                Amenities = "Özel Plaj, Su Sporları, Beach Club, Şarap Mahzeni, Spa, Restoran, Bar",
                                RoomTypes = "Deniz Manzaralı Oda, Süit, Villa, Başkanlık Villa",
                                Pricing = "Deniz Manzaralı: 1800 TL/gece, Süit: 3000 TL/gece, Villa: 4500 TL/gece, Başkanlık: 7500 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 7,
                                HotelName = "Black Sea Villa",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Black+Sea+Villa",
                                Phone = "+90 462 555 0123",
                                Email = "info@blackseavilla.com",
                                Address = "Trabzon Sahil No:45, Trabzon, Türkiye",
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Black+Sea+View",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Green+Mountains",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Villa+Interior",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Local+Cuisine",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Tea+Garden",
                                Facebook = "https://facebook.com/blackseavilla",
                                Instagram = "https://instagram.com/blackseavilla",
                                Twitter = "https://twitter.com/blackseavilla",
                                Website = "https://blackseavilla.com",
                                Description = "Karadeniz'in yeşil dağları ve masmavi denizinin buluştuğu noktada, doğayla iç içe villa deneyimi.",
                                Amenities = "Deniz Manzarası, Yeşil Alan, Çay Bahçesi, Yerel Mutfak, Spa, Wi-Fi",
                                RoomTypes = "Villa, Aile Villa, Başkanlık Villa, Garden Villa",
                                Pricing = "Villa: 1200 TL/gece, Aile Villa: 1800 TL/gece, Başkanlık: 3000 TL/gece, Garden: 1500 TL/gece"
                            },
                            new Hotel
                            {
                                Id = 8,
                                HotelName = "Mediterranean Dream",
                                LogoUrl = "https://via.placeholder.com/200x80?text=Mediterranean+Dream",
                                Phone = "+90 242 555 0567",
                                Email = "info@mediterraneandream.com",
                                Address = "Kaş Sahil No:22, Antalya, Türkiye",
                                GalleryImage1 = "https://via.placeholder.com/800x600?text=Mediterranean+Sea",
                                GalleryImage2 = "https://via.placeholder.com/800x600?text=Diving+Center",
                                GalleryImage3 = "https://via.placeholder.com/800x600?text=Island+View",
                                GalleryImage4 = "https://via.placeholder.com/800x600?text=Yacht+Club",
                                GalleryImage5 = "https://via.placeholder.com/800x600?text=Sunset+Cruise",
                                Facebook = "https://facebook.com/mediterraneandream",
                                Instagram = "https://instagram.com/mediterraneandream",
                                Twitter = "https://twitter.com/mediterraneandream",
                                Website = "https://mediterraneandream.com",
                                Description = "Akdeniz'in en temiz sularında, Kaş'ın berrak koylarında dalış ve yat turizminin merkezi.",
                                Amenities = "Dalış Merkezi, Yat Kulübü, Sunset Cruise, Spa, Restoran, Bar, Plaj",
                                RoomTypes = "Deniz Manzaralı Oda, Süit, Villa, Yat Kulübü Odası",
                                Pricing = "Deniz Manzaralı: 1600 TL/gece, Süit: 2800 TL/gece, Villa: 4200 TL/gece, Yat Kulübü: 3500 TL/gece"
                            }
                        );
                    }
    }
} 