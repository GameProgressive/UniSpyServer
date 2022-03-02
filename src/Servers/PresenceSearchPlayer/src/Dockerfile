FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 29901

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/PresenceSearchPlayer/src/UniSpyServer.Servers.PresenceSearchPlayer.csproj", "src/Servers/PresenceSearchPlayer/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
RUN dotnet restore "src/Servers/PresenceSearchPlayer/src/UniSpyServer.Servers.PresenceSearchPlayer.csproj"
COPY . .
WORKDIR "/src/src/Servers/PresenceSearchPlayer/src"
RUN dotnet build "UniSpyServer.Servers.PresenceSearchPlayer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.PresenceSearchPlayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.PresenceSearchPlayer.dll"]