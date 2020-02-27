FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 29900

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/PresenceConnectionManager/PresenceConnectionManager.csproj", "/Servers/PresenceConnectionManager/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/PresenceConnectionManager/PresenceConnectionManager.csproj"
COPY . .
WORKDIR "/src/Servers/PresenceConnectionManager"
RUN dotnet build "PresenceConnectionManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PresenceConnectionManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.xml .
ENTRYPOINT ["dotnet", "PresenceConnectionManager.dll"]