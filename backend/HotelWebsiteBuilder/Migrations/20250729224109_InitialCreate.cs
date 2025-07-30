using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelWebsiteBuilder.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HotelName = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Website = table.Column<string>(type: "text", nullable: true),
                    CheckInTime = table.Column<string>(type: "text", nullable: false),
                    CheckOutTime = table.Column<string>(type: "text", nullable: false),
                    GalleryImagesJson = table.Column<string>(type: "text", nullable: true),
                    SliderImagesJson = table.Column<string>(type: "text", nullable: true),
                    Facebook = table.Column<string>(type: "text", nullable: true),
                    Instagram = table.Column<string>(type: "text", nullable: true),
                    Twitter = table.Column<string>(type: "text", nullable: true),
                    LinkedIn = table.Column<string>(type: "text", nullable: true),
                    YouTube = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AmenitiesJson = table.Column<string>(type: "text", nullable: true),
                    RoomsJson = table.Column<string>(type: "text", nullable: true),
                    FacilitiesJson = table.Column<string>(type: "text", nullable: true),
                    MetaTitle = table.Column<string>(type: "text", nullable: true),
                    MetaDescription = table.Column<string>(type: "text", nullable: true),
                    MetaKeywords = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    StarRating = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    GalleryImage1 = table.Column<string>(type: "text", nullable: true),
                    GalleryImage2 = table.Column<string>(type: "text", nullable: true),
                    GalleryImage3 = table.Column<string>(type: "text", nullable: true),
                    GalleryImage4 = table.Column<string>(type: "text", nullable: true),
                    GalleryImage5 = table.Column<string>(type: "text", nullable: true),
                    RoomTypes = table.Column<string>(type: "text", nullable: true),
                    Pricing = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "AmenitiesJson", "CheckInTime", "CheckOutTime", "CreatedAt", "Currency", "Description", "Email", "Facebook", "FacilitiesJson", "GalleryImage1", "GalleryImage2", "GalleryImage3", "GalleryImage4", "GalleryImage5", "GalleryImagesJson", "HotelName", "Instagram", "Language", "Latitude", "LinkedIn", "LogoUrl", "Longitude", "MetaDescription", "MetaKeywords", "MetaTitle", "Phone", "Pricing", "RoomTypes", "RoomsJson", "SliderImagesJson", "StarRating", "Twitter", "UpdatedAt", "Website", "YouTube" },
                values: new object[,]
                {
                    { 1, "Sultanahmet Meydanı No:1, İstanbul, Türkiye", "[\"\\u00DCcretsiz Wi-Fi\",\"Spa \\u0026 Wellness\",\"Restoran\",\"Bar\",\"Havuz\",\"Fitness Merkezi\",\"Oda Servisi\",\"Valet Parking\",\"Konferans Salonu\"]", "14:00", "12:00", new DateTime(2025, 7, 29, 22, 41, 9, 444, DateTimeKind.Utc).AddTicks(7770), "TRY", "İstanbul'un kalbinde, tarihi yarımadada konumlanan lüks otelimizde unutulmaz bir konaklama deneyimi yaşayın.", "info@grandistanbul.com", "https://facebook.com/grandistanbulhotel", "[{\"Name\":\"Spa \\u0026 Wellness\",\"Description\":\"Rahatlat\\u0131c\\u0131 spa hizmetleri ve wellness merkezi\",\"Icon\":\"spa-icon.png\",\"IsAvailable\":true},{\"Name\":\"Restoran\",\"Description\":\"Geleneksel ve modern lezzetlerin bulu\\u015Ftu\\u011Fu restoran\",\"Icon\":\"restaurant-icon.png\",\"IsAvailable\":true},{\"Name\":\"Havuz\",\"Description\":\"Kapal\\u0131 ve a\\u00E7\\u0131k havuz se\\u00E7enekleri\",\"Icon\":\"pool-icon.png\",\"IsAvailable\":true},{\"Name\":\"Fitness Merkezi\",\"Description\":\"Modern ekipmanlarla donat\\u0131lm\\u0131\\u015F fitness merkezi\",\"Icon\":\"fitness-icon.png\",\"IsAvailable\":true}]", "https://via.placeholder.com/800x600?text=Istanbul+View", "https://via.placeholder.com/800x600?text=Hotel+Lobby", "https://via.placeholder.com/800x600?text=Deluxe+Room", "https://via.placeholder.com/800x600?text=Restaurant", "https://via.placeholder.com/800x600?text=Spa+Center", "[\"https://via.placeholder.com/800x600?text=Istanbul\\u002BView\",\"https://via.placeholder.com/800x600?text=Hotel\\u002BLobby\",\"https://via.placeholder.com/800x600?text=Deluxe\\u002BRoom\",\"https://via.placeholder.com/800x600?text=Restaurant\",\"https://via.placeholder.com/800x600?text=Spa\\u002BCenter\"]", "Grand Istanbul Hotel", "https://instagram.com/grandistanbulhotel", "tr", 41.008200000000002, "https://linkedin.com/company/grandistanbulhotel", "https://via.placeholder.com/200x80?text=Grand+Istanbul", 28.978400000000001, "İstanbul'un kalbinde, tarihi yarımadada konumlanan lüks otelimizde unutulmaz bir konaklama deneyimi yaşayın.", "istanbul, otel, lüks, sultanahmet, konaklama, spa, restoran", "Grand Istanbul Hotel - İstanbul'daki Lüks Otel", "+90 212 555 0123", "Standart: 1200 TL/gece, Deluxe: 1800 TL/gece, Süit: 2800 TL/gece, Başkanlık: 5000 TL/gece", "Standart Oda, Deluxe Oda, Süit Oda, Başkanlık Süiti", "[{\"Type\":\"Standart Oda\",\"Description\":\"Konforlu ve \\u015F\\u0131k standart odalar\\u0131m\\u0131zda rahat bir konaklama deneyimi.\",\"Price\":1200,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"Wi-Fi\"],\"Images\":[\"standart1.jpg\",\"standart2.jpg\"],\"Capacity\":2,\"IsAvailable\":true},{\"Type\":\"Deluxe Oda\",\"Description\":\"Geni\\u015F ve l\\u00FCks deluxe odalar\\u0131m\\u0131zda \\u015Fehir manzaras\\u0131 e\\u015Fli\\u011Finde konaklama.\",\"Price\":1800,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"Wi-Fi\",\"\\u015Eehir Manzaras\\u0131\"],\"Images\":[\"deluxe1.jpg\",\"deluxe2.jpg\"],\"Capacity\":2,\"IsAvailable\":true},{\"Type\":\"S\\u00FCit Oda\",\"Description\":\"L\\u00FCks s\\u00FCit odalar\\u0131m\\u0131zda ayr\\u0131 oturma alan\\u0131 ve jakuzi ile konforlu konaklama.\",\"Price\":2800,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"Wi-Fi\",\"Jakuzi\",\"Oturma Alan\\u0131\"],\"Images\":[\"suit1.jpg\",\"suit2.jpg\"],\"Capacity\":3,\"IsAvailable\":true},{\"Type\":\"Ba\\u015Fkanl\\u0131k S\\u00FCiti\",\"Description\":\"En l\\u00FCks ba\\u015Fkanl\\u0131k s\\u00FCitimizde \\u00F6zel hizmetler ve geni\\u015F alan.\",\"Price\":5000,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"Wi-Fi\",\"Jakuzi\",\"Oturma Alan\\u0131\",\"\\u00D6zel Hizmetler\"],\"Images\":[\"presidential1.jpg\",\"presidential2.jpg\"],\"Capacity\":4,\"IsAvailable\":true}]", "[\"https://via.placeholder.com/1200x600?text=Grand\\u002BIstanbul\\u002BSlider\\u002B1\",\"https://via.placeholder.com/1200x600?text=Grand\\u002BIstanbul\\u002BSlider\\u002B2\",\"https://via.placeholder.com/1200x600?text=Grand\\u002BIstanbul\\u002BSlider\\u002B3\"]", 5, "https://twitter.com/grandistanbul", new DateTime(2025, 7, 29, 22, 41, 9, 444, DateTimeKind.Utc).AddTicks(7770), "https://grandistanbul.com", "https://youtube.com/grandistanbulhotel" },
                    { 2, "Konyaaltı Sahili No:15, Antalya, Türkiye", "[\"Deniz Manzaras\\u0131\",\"Havuz\",\"Plaj Eri\\u015Fimi\",\"Su Sporlar\\u0131\",\"Spa\",\"Restoran\",\"Bar\",\"\\u00C7ocuk Kul\\u00FCb\\u00FC\",\"Animasyon\"]", "15:00", "11:00", new DateTime(2025, 7, 29, 22, 41, 9, 444, DateTimeKind.Utc).AddTicks(8400), "TRY", "Antalya'nın en güzel sahillerinde, mavi denizin kucakladığı resort otelimizde tatil keyfini yaşayın.", "info@bluesea.com", "https://facebook.com/bluesearesort", "[{\"Name\":\"Su Sporlar\\u0131\",\"Description\":\"Dal\\u0131\\u015F, jet ski, parasailing gibi su sporlar\\u0131 aktiviteleri\",\"Icon\":\"water-sports-icon.png\",\"IsAvailable\":true},{\"Name\":\"Spa \\u0026 Wellness\",\"Description\":\"Deniz manzaras\\u0131 e\\u015Fli\\u011Finde spa ve wellness hizmetleri\",\"Icon\":\"spa-icon.png\",\"IsAvailable\":true},{\"Name\":\"\\u00C7ocuk Kul\\u00FCb\\u00FC\",\"Description\":\"\\u00C7ocuklar i\\u00E7in \\u00F6zel aktivite alan\\u0131 ve animasyon\",\"Icon\":\"kids-club-icon.png\",\"IsAvailable\":true},{\"Name\":\"Plaj\",\"Description\":\"\\u00D6zel plaj alan\\u0131 ve \\u015Fezlong hizmeti\",\"Icon\":\"beach-icon.png\",\"IsAvailable\":true}]", "https://via.placeholder.com/800x600?text=Sea+View", "https://via.placeholder.com/800x600?text=Pool+Area", "https://via.placeholder.com/800x600?text=Beach+Access", "https://via.placeholder.com/800x600?text=Water+Sports", "https://via.placeholder.com/800x600?text=Sunset+Bar", "[\"https://via.placeholder.com/800x600?text=Sea\\u002BView\",\"https://via.placeholder.com/800x600?text=Pool\\u002BArea\",\"https://via.placeholder.com/800x600?text=Beach\\u002BAccess\",\"https://via.placeholder.com/800x600?text=Water\\u002BSports\",\"https://via.placeholder.com/800x600?text=Sunset\\u002BBar\"]", "Blue Sea Resort", "https://instagram.com/bluesearesort", "tr", 36.896900000000002, "https://linkedin.com/company/bluesearesort", "https://via.placeholder.com/200x80?text=Blue+Sea", 30.7133, "Antalya'nın en güzel sahillerinde, mavi denizin kucakladığı resort otelimizde tatil keyfini yaşayın.", "antalya, resort, deniz, plaj, tatil, spa, su sporları", "Blue Sea Resort - Antalya'daki Resort Otel", "+90 242 555 0456", "Deniz Manzaralı: 1500 TL/gece, Aile: 2200 TL/gece, Villa: 3500 TL/gece, Başkanlık: 6000 TL/gece", "Deniz Manzaralı Oda, Aile Odası, Villa, Başkanlık Villa", "[{\"Type\":\"Deniz Manzaral\\u0131 Oda\",\"Description\":\"Masmavi deniz manzaras\\u0131 e\\u015Fli\\u011Finde konforlu konaklama.\",\"Price\":1500,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"Deniz Manzaras\\u0131\",\"Balkon\"],\"Images\":[\"sea-view1.jpg\",\"sea-view2.jpg\"],\"Capacity\":2,\"IsAvailable\":true},{\"Type\":\"Aile Odas\\u0131\",\"Description\":\"Geni\\u015F aile odalar\\u0131m\\u0131zda \\u00E7ocuklar\\u0131n\\u0131zla birlikte rahat konaklama.\",\"Price\":2200,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"\\u0130ki Yatak\",\"\\u00C7ocuk K\\u00F6\\u015Fesi\"],\"Images\":[\"family1.jpg\",\"family2.jpg\"],\"Capacity\":4,\"IsAvailable\":true},{\"Type\":\"Villa\",\"Description\":\"\\u00D6zel villalar\\u0131m\\u0131zda l\\u00FCks ve konfor bir arada.\",\"Price\":3500,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"\\u00D6zel Havuz\",\"Bah\\u00E7e\"],\"Images\":[\"villa1.jpg\",\"villa2.jpg\"],\"Capacity\":6,\"IsAvailable\":true},{\"Type\":\"Ba\\u015Fkanl\\u0131k Villa\",\"Description\":\"En l\\u00FCks ba\\u015Fkanl\\u0131k villam\\u0131zda \\u00F6zel hizmetler.\",\"Price\":6000,\"Features\":[\"Klima\",\"TV\",\"Mini bar\",\"\\u00D6zel Havuz\",\"Bah\\u00E7e\",\"\\u00D6zel Hizmetler\"],\"Images\":[\"presidential-villa1.jpg\",\"presidential-villa2.jpg\"],\"Capacity\":8,\"IsAvailable\":true}]", "[\"https://via.placeholder.com/1200x600?text=Blue\\u002BSea\\u002BSlider\\u002B1\",\"https://via.placeholder.com/1200x600?text=Blue\\u002BSea\\u002BSlider\\u002B2\",\"https://via.placeholder.com/1200x600?text=Blue\\u002BSea\\u002BSlider\\u002B3\"]", 4, "https://twitter.com/bluesea", new DateTime(2025, 7, 29, 22, 41, 9, 444, DateTimeKind.Utc).AddTicks(8400), "https://bluesea.com", "https://youtube.com/bluesearesort" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hotels");
        }
    }
}
