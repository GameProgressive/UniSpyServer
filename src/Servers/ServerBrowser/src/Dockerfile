FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 28910
EXPOSE 28900

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/ServerBrowser/src/UniSpyServer.Servers.ServerBrowser.csproj", "src/Servers/ServerBrowser/src/"]
COPY ["src/Servers/NatNegotiation/src/UniSpyServer.Servers.NatNegotiation.csproj", "src/Servers/NatNegotiation/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
COPY ["src/Servers/QueryReport/src/UniSpyServer.Servers.QueryReport.csproj", "src/Servers/QueryReport/src/"]
RUN dotnet restore "src/Servers/ServerBrowser/src/UniSpyServer.Servers.ServerBrowser.csproj"
COPY . .
WORKDIR "/src/src/Servers/ServerBrowser/src"
RUN dotnet build "UniSpyServer.Servers.ServerBrowser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.ServerBrowser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.ServerBrowser.dll"]