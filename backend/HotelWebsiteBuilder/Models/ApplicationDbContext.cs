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

            // Seed data
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    HotelName = "Grand Hotel Istanbul",
                    LogoUrl = "https://via.placeholder.com/200x80?text=Grand+Hotel",
                    Phone = "+90 212 555 0123",
                    Email = "info@grandhotelistanbul.com",
                    Address = "Taksim Meydanı No:1, İstanbul, Türkiye",
                    GalleryImage1 = "https://via.placeholder.com/800x600?text=Hotel+Exterior",
                    GalleryImage2 = "https://via.placeholder.com/800x600?text=Lobby",
                    GalleryImage3 = "https://via.placeholder.com/800x600?text=Room",
                    GalleryImage4 = "https://via.placeholder.com/800x600?text=Restaurant",
                    GalleryImage5 = "https://via.placeholder.com/800x600?text=Pool",
                    Facebook = "https://facebook.com/grandhotelistanbul",
                    Instagram = "https://instagram.com/grandhotelistanbul",
                    Twitter = "https://twitter.com/grandhotelistanbul",
                    Website = "https://grandhotelistanbul.com",
                    Description = "Lüks ve konforun buluştuğu nokta. İstanbul'un kalbinde, Taksim'de yer alan Grand Hotel Istanbul, misafirlerine unutulmaz bir deneyim sunar.",
                    Amenities = "Ücretsiz Wi-Fi, Spa, Fitness Merkezi, Havuz, Restoran, Bar, Oda Servisi, Çamaşırhane",
                    RoomTypes = "Standart Oda, Deluxe Oda, Süit, Başkanlık Süiti",
                    Pricing = "Standart Oda: 1500 TL/gece, Deluxe Oda: 2000 TL/gece, Süit: 3500 TL/gece"
                },
                new Hotel
                {
                    Id = 2,
                    HotelName = "Blue Sea Resort Antalya",
                    LogoUrl = "https://via.placeholder.com/200x80?text=Blue+Sea+Resort",
                    Phone = "+90 242 555 0456",
                    Email = "info@bluesearesort.com",
                    Address = "Konyaaltı Sahili No:15, Antalya, Türkiye",
                    GalleryImage1 = "https://via.placeholder.com/800x600?text=Resort+View",
                    GalleryImage2 = "https://via.placeholder.com/800x600?text=Beach",
                    GalleryImage3 = "https://via.placeholder.com/800x600?text=Garden+Room",
                    GalleryImage4 = "https://via.placeholder.com/800x600?text=Beach+Restaurant",
                    GalleryImage5 = "https://via.placeholder.com/800x600?text=Water+Sports",
                    Facebook = "https://facebook.com/bluesearesort",
                    Instagram = "https://instagram.com/bluesearesort",
                    Twitter = "https://twitter.com/bluesearesort",
                    Website = "https://bluesearesort.com",
                    Description = "Antalya'nın en güzel sahillerinde yer alan Blue Sea Resort, doğa ile iç içe bir tatil deneyimi sunar.",
                    Amenities = "Özel Plaj, Su Sporları, Spa, Fitness Merkezi, Çocuk Kulübü, Animasyon, All-Inclusive",
                    RoomTypes = "Bahçe Manzaralı Oda, Deniz Manzaralı Oda, Aile Odası, Villa",
                    Pricing = "Bahçe Manzaralı: 1200 TL/gece, Deniz Manzaralı: 1800 TL/gece, Villa: 3000 TL/gece"
                }
            );
        }
    }
} 