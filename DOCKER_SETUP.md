# Docker Kurulum ve Kullanım

## Docker Kurulumu

### macOS
1. [Docker Desktop for Mac](https://docs.docker.com/desktop/install/mac-install/) indirin ve kurun
2. Docker Desktop'ı başlatın
3. Terminal'de Docker'ın çalıştığını kontrol edin:
   ```bash
   docker --version
   docker-compose --version
   ```

### Windows
1. [Docker Desktop for Windows](https://docs.docker.com/desktop/install/windows-install/) indirin ve kurun
2. Docker Desktop'ı başlatın
3. WSL2 kurulumu gerekebilir

### Linux
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install docker.io docker-compose
sudo systemctl start docker
sudo systemctl enable docker
sudo usermod -aG docker $USER
```

## Projeyi Docker ile Çalıştırma

### 1. Docker Desktop'ı Başlatın
Docker Desktop uygulamasını açın ve çalıştığından emin olun.

### 2. Projeyi Başlatın
```bash
# Proje dizinine gidin
cd Designer

# Docker imajlarını oluşturun ve başlatın
docker-compose up --build
```

### 3. Uygulamaya Erişin
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:7001
- **Swagger UI**: http://localhost:7001/swagger

## Docker Komutları

### Yardımcı Script Kullanımı
```bash
# Script'i çalıştırılabilir yapın
chmod +x docker-commands.sh

# Komutları kullanın
./docker-commands.sh build    # İmajları oluştur
./docker-commands.sh up       # Uygulamayı başlat
./docker-commands.sh down     # Uygulamayı durdur
./docker-commands.sh logs     # Logları görüntüle
./docker-commands.sh status   # Durumu kontrol et
./docker-commands.sh clean    # Temizlik yap
```

### Manuel Komutlar
```bash
# İmajları oluştur
docker-compose build

# Uygulamayı başlat (arka planda)
docker-compose up -d

# Uygulamayı durdur
docker-compose down

# Logları görüntüle
docker-compose logs -f

# Container durumlarını kontrol et
docker-compose ps

# Tüm container'ları ve volume'ları sil
docker-compose down -v
docker system prune -f
```

## Sorun Giderme

### Docker Daemon Çalışmıyor
```bash
# macOS/Linux
sudo systemctl start docker

# macOS (Docker Desktop)
# Docker Desktop uygulamasını açın
```

### Port Çakışması
Eğer port 3000 veya 7001 kullanımdaysa, docker-compose.yml dosyasını düzenleyin:
```yaml
ports:
  - "3001:80"  # 3000 yerine 3001
```

### İmaj Yeniden Oluşturma
```bash
# Cache'i temizle ve yeniden oluştur
docker-compose build --no-cache
```

### Container Logları
```bash
# Belirli bir servisin loglarını görüntüle
docker-compose logs backend
docker-compose logs frontend

# Gerçek zamanlı loglar
docker-compose logs -f backend
```

## Geliştirme Ortamı

### Hot Reload (Geliştirme için)
```bash
# Development modunda çalıştır
docker-compose -f docker-compose.dev.yml up
```

### Veritabanı Verilerini Koruma
```bash
# Volume'ları koruyarak durdur
docker-compose down

# Volume'ları da sil
docker-compose down -v
```

## Production Deployment

### Environment Variables
```bash
# .env dosyası oluşturun
cp .env.example .env

# Production değerlerini ayarlayın
ASPNETCORE_ENVIRONMENT=Production
REACT_APP_API_URL=https://your-api-domain.com/api
```

### Production Build
```bash
# Production imajları oluştur
docker-compose -f docker-compose.prod.yml up --build
```

## Performans Optimizasyonu

### Multi-stage Build
Dockerfile'lar multi-stage build kullanarak imaj boyutunu küçültür.

### Volume Mounting
Geliştirme sırasında kod değişikliklerini anında görmek için:
```yaml
volumes:
  - ./src:/app/src
```

### Health Checks
```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost/health"]
  interval: 30s
  timeout: 10s
  retries: 3
``` 