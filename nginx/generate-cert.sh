#!/bin/sh

# Создание самоподписанного сертификата
openssl req -x509 -newkey rsa:4096 -keyout /etc/nginx/key.pem -out /etc/nginx/cert.pem -days 365 -nodes -subj "/CN=localhost"

# Запуск nginx
exec nginx -g "daemon off;"
