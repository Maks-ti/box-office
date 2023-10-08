FROM nginx

# Установка openssl
RUN apt-get update && apt-get install -y openssl && apt-get clean

# Копирование nginx конфигурации и скрипта
COPY nginx.conf /etc/nginx/nginx.conf
COPY generate-cert.sh /etc/nginx/generate-cert.sh
RUN chmod +x /etc/nginx/generate-cert.sh

CMD ["/etc/nginx/generate-cert.sh"]
