FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 29910
EXPOSE 29910/udp

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["/Servers/CDKey/CDKey.csproj", "/Servers/CDKey/"]
COPY ["/Library/GameSpyLib/GameSpyLib.csproj", "/Library/GameSpyLib/"]
RUN dotnet restore "/Servers/CDKey/CDKey.csproj"
COPY . .
WORKDIR "/src/Servers/CDKey"
RUN dotnet build "CDKey.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CDKey.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "CDKey.dll"]