FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 29901

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/PresenceSearchPlayer/PresenceSearchPlayer.csproj", "/Servers/PresenceSearchPlayer/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/PresenceSearchPlayer/PresenceSearchPlayer.csproj"
COPY . .
WORKDIR "/src/Servers/PresenceSearchPlayer"
RUN dotnet build "PresenceSearchPlayer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PresenceSearchPlayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.xml .
ENTRYPOINT ["dotnet", "PresenceSearchPlayer.dll"]