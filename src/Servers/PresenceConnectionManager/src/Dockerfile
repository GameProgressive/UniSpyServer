FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/PresenceConnectionManager/src/UniSpyServer.Servers.PresenceConnectionManager.csproj", "src/Servers/PresenceConnectionManager/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
COPY ["src/Servers/PresenceSearchPlayer/src/UniSpyServer.Servers.PresenceSearchPlayer.csproj", "src/Servers/PresenceSearchPlayer/src/"]
RUN dotnet restore "src/Servers/PresenceConnectionManager/src/UniSpyServer.Servers.PresenceConnectionManager.csproj"
COPY . .
WORKDIR "/src/src/Servers/PresenceConnectionManager/src"
RUN dotnet build "UniSpyServer.Servers.PresenceConnectionManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.PresenceConnectionManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.PresenceConnectionManager.dll"]