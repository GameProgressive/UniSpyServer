FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 29900

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["/Servers/PresenceConnectionManager/PresenceConnectionManager.csproj", "/Servers/PresenceConnectionManager/"]
COPY ["/Library/GameSpyLib/GameSpyLib.csproj", "/Library/GameSpyLib/"]
COPY ["/Servers/PresenceSearchPlayer/PresenceSearchPlayer.csproj", "/Servers/PresenceSearchPlayer/"]
RUN dotnet restore "/Servers/PresenceConnectionManager/PresenceConnectionManager.csproj"
COPY . .
WORKDIR "/src/Servers/PresenceConnectionManager"
RUN dotnet build "PresenceConnectionManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PresenceConnectionManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "PresenceConnectionManager.dll"]