# =============================================================
# Stage 1: Build (SDK completo para compilar o projeto)
# =============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar apenas os arquivos de projeto primeiro para cache de camadas
COPY ["Portfolio.API/Portfolio.API.csproj", "Portfolio.API/"]
RUN dotnet restore "Portfolio.API/Portfolio.API.csproj"

# Copiar o restante do código e fazer o publish
COPY . .
WORKDIR "/src/Portfolio.API"
RUN dotnet publish "Portfolio.API.csproj" -c Release -o /app/publish --no-restore

# =============================================================
# Stage 2: Runtime (imagem final, muito menor)
# Suporte ARM64 (Oracle Ampere A1)
# =============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Criar diretório onde a Wallet da Oracle será montada
RUN mkdir -p /app/oracle-wallet

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.API.dll"]
