FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine3.13 AS base
WORKDIR /app
EXPOSE 27901
EXPOSE 27901/udp

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/NatNegotiation/src/UniSpyServer.Servers.NatNegotiation.csproj", "src/Servers/NatNegotiation/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
RUN dotnet restore "src/Servers/NatNegotiation/NatNegotiation.csproj"
COPY . .
WORKDIR "/src/src/Servers/NatNegotiation"
RUN dotnet build "NatNegotiation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NatNegotiation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "NatNegotiation.dll"]