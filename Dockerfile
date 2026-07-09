# تغییر به Alpine و استفاده از نسخه پایدار
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY . .

RUN dotnet restore "AI-Assistans-CRM-Service/AI-Assistans-CRM-Service.csproj" --source https://api.nuget.org/v3/index.json
RUN dotnet publish "AI-Assistans-CRM-Service/AI-Assistans-CRM-Service.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

COPY --from=build /app/publish .

RUN addgroup -g 1000 appuser && \
    adduser -u 1000 -G appuser -s /bin/sh -D appuser && \
    mkdir -p /app/logs && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "AI-Assistans-CRM-Service.dll"]