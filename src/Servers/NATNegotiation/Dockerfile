FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 27901
EXPOSE 27901/udp

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/NATNegotiation/NATNegotiation.csproj", "src/Servers/NATNegotiation/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
RUN dotnet restore "src/Servers/NATNegotiation/NATNegotiation.csproj"
COPY . .
WORKDIR "/src/src/Servers/NATNegotiation"
RUN dotnet build "NATNegotiation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NATNegotiation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "NATNegotiation.dll"]