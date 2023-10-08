# Используем базовый образ с .NET 6 SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Копируем файлы проекта и воспроизводим зависимости
COPY ./box-office/*.csproj ./
RUN dotnet restore

# Копируем остальные файлы приложения и выполняем сборку
COPY ./box-office/ ./
RUN dotnet publish -c Release -o out

# Используем образ ASP.NET 6
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

RUN apt-get update && apt-get install -y openssl && \
    mkdir /https/ && \
    openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -days 365 -nodes -subj "/CN=localhost" && \
    openssl pkcs12 -export -out /https/localhost.pfx -inkey key.pem -in cert.pem -passout pass:mypassword && \
    mv key.pem /https/ && \
    mv cert.pem /https/ && \
    chmod 600 /https/cert.pem && chmod 600 /https/key.pem && chmod 600 /https/localhost.pfx

# Запускаем приложение
ENTRYPOINT ["dotnet", "box-office.dll"]
