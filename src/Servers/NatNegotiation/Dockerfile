FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 27901
EXPOSE 27901/udp

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/NatNegotiation/NatNegotiation.csproj", "src/Servers/NatNegotiation/"]
COPY ["src/Libraries/GameSpyLib/GameSpyLib.csproj", "src/Libraries/GameSpyLib/"]
RUN dotnet restore "src/Servers/NatNegotiation/NatNegotiation.csproj"
COPY . .
WORKDIR "/src/src/Servers/NatNegotiation"
RUN dotnet build "NatNegotiation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NatNegotiation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "NatNegotiation.dll"]