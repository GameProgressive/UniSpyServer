FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 29901

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/PresenceSearchPlayer/src/UniSpy.Server.PresenceSearchPlayer.csproj", "src/Servers/PresenceSearchPlayer/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
RUN dotnet restore "src/Servers/PresenceSearchPlayer/src/UniSpy.Server.PresenceSearchPlayer.csproj"
COPY . .
WORKDIR "/src/src/Servers/PresenceSearchPlayer/src"
RUN dotnet build "UniSpy.Server.PresenceSearchPlayer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.PresenceSearchPlayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.PresenceSearchPlayer.dll"]