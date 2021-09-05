FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine3.13 AS base
WORKDIR /app
EXPOSE 29901

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/PresenceSearchPlayer/PresenceSearchPlayer.csproj", "src/Servers/PresenceSearchPlayer/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
RUN dotnet restore "src/Servers/PresenceSearchPlayer/PresenceSearchPlayer.csproj"
COPY . .
WORKDIR "/src/src/Servers/PresenceSearchPlayer"
RUN dotnet build "PresenceSearchPlayer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PresenceSearchPlayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "PresenceSearchPlayer.dll"]