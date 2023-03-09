FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 5001
EXPOSE 5002

COPY ["../../Core/SnowaTec.Test.API/SnowaTec.Test.API.csproj", "SnowaTec.Test.API/"]
COPY ["../../Core/SnowaTec.Test.Domain/SnowaTec.Test.Domain.csproj", "SnowaTec.Test.Domain/"]
COPY ["../../Core/SnowaTec.Test.Infrastructure/SnowaTec.Test.Infrastructure.csproj", "SnowaTec.Test.Infrastructure/"]
COPY ["../../Core/SnowaTec.Test.Service/SnowaTec.Test.Service.csproj", "SnowaTec.Test.Service/"]
COPY ["../../Core/SnowaTec.Test.Persistence/SnowaTec.Test.Persistence.csproj", "SnowaTec.Test.Persistence/"]

RUN dotnet restore "SnowaTec.Test.API/SnowaTec.Test.API.csproj"

COPY . .
RUN dotnet build "SnowaTec.Test.API/SnowaTec.Test.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SnowaTec.Test.API/SnowaTec.Test.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SnowaTec.Test.API.dll"]