FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 27901

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/NATNegotiation/NATNegotiation.csproj", "/Servers/NATNegotiation/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/NATNegotiation/NATNegotiation.csproj"
COPY . .
WORKDIR "/src/Servers/NATNegotiation"
RUN dotnet build "NATNegotiation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NATNegotiation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/ConfigFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "NATNegotiation.dll"]