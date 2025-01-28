# 使用 .NET 8 SDK 作為 build 映像
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 精確複製專案檔案
COPY WebApplication1/WebApplication1.csproj WebApplication1/
RUN dotnet restore "WebApplication1/WebApplication1.csproj"

# 複製其餘檔案並建置
COPY WebApplication1/ WebApplication1/
RUN dotnet build "WebApplication1/WebApplication1.csproj" -c Release -o /app/build

# 發佈
RUN dotnet publish "WebApplication1/WebApplication1.csproj" -c Release -o /app/publish

# 使用 .NET 8 ASP.NET 執行環境
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# ─────────────────────────────────────────────────────
# 最終ステージで Git/SSH をインストール
RUN apt-get update && apt-get install -y git openssh-client
# ─────────────────────────────────────────────────────

# 複製發佈檔案
COPY --from=build /app/publish .

# 移除 appsettings.json 的硬性複製，使用 docker-compose 的 volumes 掛載即可

# 開放 80 端口
EXPOSE 80

# 設定執行條目
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
#docker-compose down
# docker-compose up --build

