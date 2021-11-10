FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/WebServer/src/UniSpyServer.Servers.WebServer.csproj", "src/Servers/WebServer/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
RUN dotnet restore "src/Servers/WebServer/src/UniSpyServer.Servers.WebServer.csproj"
COPY . .
WORKDIR "/src/src/Servers/WebServer/src"
RUN dotnet build "UniSpyServer.Servers.WebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.WebServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.WebServer.dll"]