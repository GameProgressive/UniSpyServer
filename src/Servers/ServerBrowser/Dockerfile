FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 28910

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/ServerBrowser/ServerBrowser.csproj", "src/Servers/ServerBrowser/"]
COPY ["src/Servers/QueryReport/QueryReport.csproj", "src/Servers/QueryReport/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
COPY ["src/Servers/NATNegotiation/NATNegotiation.csproj", "src/Servers/NATNegotiation/"]
RUN dotnet restore "src/Servers/ServerBrowser/ServerBrowser.csproj"
COPY . .
WORKDIR "/src/src/Servers/ServerBrowser"
RUN dotnet build "ServerBrowser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServerBrowser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "ServerBrowser.dll"]