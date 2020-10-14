FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 29920

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/StatsAndTracking/StatsAndTracking.csproj", "src/Servers/StatsAndTracking/"]
COPY ["src/Libraries/GameSpyLib/GameSpyLib.csproj", "src/Libraries/GameSpyLib/"]
RUN dotnet restore "src/Servers/StatsAndTracking/StatsAndTracking.csproj"
COPY . .
WORKDIR "/src/src/Servers/StatsAndTracking"
RUN dotnet build "StatsAndTracking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatsAndTracking.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "StatsAndTracking.dll"]