FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 29900

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/PresenceConnectionManager/PresenceConnectionManager.csproj", "src/Servers/PresenceConnectionManager/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
COPY ["src/Servers/PresenceSearchPlayer/PresenceSearchPlayer.csproj", "src/Servers/PresenceSearchPlayer/"]
RUN dotnet restore "src/Servers/PresenceConnectionManager/PresenceConnectionManager.csproj"
COPY . .
WORKDIR "/src/src/Servers/PresenceConnectionManager"
RUN dotnet build "PresenceConnectionManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PresenceConnectionManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "PresenceConnectionManager.dll"]