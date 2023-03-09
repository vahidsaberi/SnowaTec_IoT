FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 5001
EXPOSE 5002
COPY ["/SnowaTec.Test.RealTime/SnowaTec.Test.RealTime.csproj", "SnowaTec.Test.RealTime"]

RUN dotnet restore "/SnowaTec.Test.RealTime/SnowaTec.Test.RealTime.csproj"

COPY . .
RUN dotnet build "/SnowaTec.Test.RealTime/SnowaTec.Test.RealTime.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/SnowaTec.Test.RealTime/SnowaTec.Test.RealTime.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SnowaTec.Test.API.dll"]