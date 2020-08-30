FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 28910

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["/Servers/ServerBrowser/ServerBrowser.csproj", "/Servers/ServerBrowser/"]
COPY ["/Servers/QueryReport/QueryReport.csproj", "/Servers/QueryReport/"]
COPY ["/Library/GameSpyLib/GameSpyLib.csproj", "/Library/GameSpyLib/"]
COPY ["/Servers/NatNegotiation/NatNegotiation.csproj", "/Servers/NatNegotiation/"]
RUN dotnet restore "/Servers/ServerBrowser/ServerBrowser.csproj"
COPY . .
WORKDIR "/src/Servers/ServerBrowser"
RUN dotnet build "ServerBrowser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServerBrowser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "ServerBrowser.dll"]