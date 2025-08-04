# WebBuilder: Otellere Özel Otomatik Web Sitesi Üretici

WebBuilder, oteller için hızlı ve kişiselleştirilmiş web siteleri oluşturan bir sistemdir. Kullanıcı, şablon ya da bir URL girerek başlar; sistem bu yapıyı analiz eder, veritabanındaki otel bilgileriyle HTML içeriğini güncelleyerek yeni bir web sitesi üretir.

## Özellikler

- URL’den veya şablondan HTML alımı
- Otel bilgileriyle içerik güncelleme (isim, telefon, adres vb.)
- HTML, CSS, JS ve görsellerin bozulmadan korunması
- Üretilen sitenin `wwwroot/sites/` klasörüne kaydedilmesi
- RESTful API (.NET 8)
- React tabanlı modern kullanıcı arayüzü

## Teknolojiler

- Backend: ASP.NET 8, C#, Entity Framework Core
- Frontend: React.js, Vite
- Veritabanı: PostgreSQL (Docker destekli)
- HTML işleme: AngleSharp / HtmlAgilityPack (isteğe bağlı)

## API Endpointleri

### Oteller

- `GET /api/hotels` – Tüm otelleri getir
- `GET /api/hotels/{id}` – Belirli otelin bilgilerini getir
- `POST /api/hotels` – Yeni otel kaydı oluştur

### Web Site Üretimi

- `POST /api/generate/template` – Şablondan site üret
- `POST /api/generate/from-url` – URL’den site klonla ve özelleştir

## Örnek Akış

1. Kullanıcı bir URL veya şablon seçer
2. Otel bilgileri veritabanından alınır
3. HTML içeriği otel bilgilerine göre düzenlenir
4. CSS ve JS dosyaları bozulmadan korunur
5. Çıktı `/productiondir/{otel-adi}` klasörüne yazılır
6. Sistem çıktı linkini kullanıcıya döner

## Kurulum

### Backend (.NET)

```
cd backend
dotnet restore
dotnet run
```

### Frontend (React)

```
cd frontend
npm install
npm run dev
```

### PostgreSQL (Docker ile)

```
docker run --name webbuilder-db -e POSTGRES_PASSWORD=123456 -p 5432:5432 -d postgres
```

## Proje Yapısı

```
/backend
  - Controllers
  - Services
  - Models
  - HtmlExtensions
  - wwwroot/productiondir

/frontend
  - pages/
  - components/
  - api.js

/docs
```
