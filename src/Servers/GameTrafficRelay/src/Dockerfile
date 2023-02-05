FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10086/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/GameTrafficRelay/src/UniSpyServer.Servers.GameTrafficRelay.csproj", "src/Servers/GameTrafficRelay/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
RUN dotnet restore "src/Servers/GameTrafficRelay/src/UniSpyServer.Servers.GameTrafficRelay.csproj"
COPY . .
WORKDIR "/src/src/Servers/GameTrafficRelay/src"
RUN dotnet build "UniSpyServer.Servers.GameTrafficRelay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.GameTrafficRelay.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.GameTrafficRelay.dll"]