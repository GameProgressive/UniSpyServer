FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 27901/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/NatNegotiation/src/UniSpyServer.Servers.NatNegotiation.csproj", "src/Servers/NatNegotiation/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
RUN dotnet restore "src/Servers/NatNegotiation/src/UniSpyServer.Servers.NatNegotiation.csproj"
COPY . .
WORKDIR "/src/src/Servers/NatNegotiation/src"
RUN dotnet build "UniSpyServer.Servers.NatNegotiation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.NatNegotiation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.NatNegotiation.dll"]