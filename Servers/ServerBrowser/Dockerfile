FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 28910

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/ServerBrowser/ServerBrowser.csproj", "/Servers/ServerBrowser/"]
COPY ["/Servers/QueryReport/QueryReport.csproj", "/Servers/QueryReport/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/ServerBrowser/ServerBrowser.csproj"
COPY . .
WORKDIR "/src/Servers/ServerBrowser"
RUN dotnet build "ServerBrowser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServerBrowser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/ConfigFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "ServerBrowser.dll"]