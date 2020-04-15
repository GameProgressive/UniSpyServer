FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 27901

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/NatNegotiation/NatNegotiation.csproj", "/Servers/NatNegotiation/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/NatNegotiation/NatNegotiation.csproj"
COPY . .
WORKDIR "/src/Servers/NatNegotiation"
RUN dotnet build "NatNegotiation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NatNegotiation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/ConfigFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "NatNegotiation.dll"]