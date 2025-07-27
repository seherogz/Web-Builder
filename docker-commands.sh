#!/bin/bash

# Docker komutları için yardımcı script

case "$1" in
    "build")
        echo "🔨 Docker imajları oluşturuluyor..."
        docker-compose build
        ;;
    "up")
        echo "🚀 Uygulama başlatılıyor..."
        docker-compose up -d
        ;;
    "down")
        echo "🛑 Uygulama durduruluyor..."
        docker-compose down
        ;;
    "restart")
        echo "🔄 Uygulama yeniden başlatılıyor..."
        docker-compose restart
        ;;
    "logs")
        echo "📋 Loglar görüntüleniyor..."
        docker-compose logs -f
        ;;
    "clean")
        echo "🧹 Docker temizliği yapılıyor..."
        docker-compose down -v
        docker system prune -f
        ;;
    "status")
        echo "📊 Container durumları:"
        docker-compose ps
        ;;
    *)
        echo "Docker komutları için yardımcı script"
        echo ""
        echo "Kullanım:"
        echo "  ./docker-commands.sh build    - Docker imajları oluştur"
        echo "  ./docker-commands.sh up       - Uygulamayı başlat"
        echo "  ./docker-commands.sh down     - Uygulamayı durdur"
        echo "  ./docker-commands.sh restart  - Uygulamayı yeniden başlat"
        echo "  ./docker-commands.sh logs     - Logları görüntüle"
        echo "  ./docker-commands.sh clean    - Docker temizliği yap"
        echo "  ./docker-commands.sh status   - Container durumlarını göster"
        echo ""
        echo "Örnek:"
        echo "  ./docker-commands.sh build && ./docker-commands.sh up"
        ;;
esac 