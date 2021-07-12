FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine3.13 AS base
WORKDIR /app
EXPOSE 28910

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/ServerBrowser/ServerBrowser.csproj", "src/Servers/ServerBrowser/"]
COPY ["src/Servers/QueryReport/QueryReport.csproj", "src/Servers/QueryReport/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
COPY ["src/Servers/NatNegotiation/NatNegotiation.csproj", "src/Servers/NatNegotiation/"]
RUN dotnet restore "src/Servers/ServerBrowser/ServerBrowser.csproj"
COPY . .
WORKDIR "/src/src/Servers/ServerBrowser"
RUN dotnet build "ServerBrowser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServerBrowser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "ServerBrowser.dll"]