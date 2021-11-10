FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/GameStatus/src/UniSpyServer.Servers.GameStatus.csproj", "src/Servers/GameStatus/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
RUN dotnet restore "src/Servers/GameStatus/src/UniSpyServer.Servers.GameStatus.csproj"
COPY . .
WORKDIR "/src/src/Servers/GameStatus/src"
RUN dotnet build "UniSpyServer.Servers.GameStatus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.GameStatus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.GameStatus.dll"]