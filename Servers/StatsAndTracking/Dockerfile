FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 29920

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/StatsAndTracking/StatsAndTracking.csproj", "/Servers/StatsAndTracking/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/StatsAndTracking/StatsAndTracking.csproj"
COPY . .
WORKDIR "/src/Servers/StatsAndTracking"
RUN dotnet build "StatsAndTracking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatsAndTracking.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/ConfigFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "StatsAndTracking.dll"]