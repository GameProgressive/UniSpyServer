FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 27900/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/QueryReport/src/UniSpyServer.Servers.QueryReport.csproj", "src/Servers/QueryReport/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
RUN dotnet restore "src/Servers/QueryReport/src/UniSpyServer.Servers.QueryReport.csproj"
COPY . .
WORKDIR "/src/src/Servers/QueryReport/src"
RUN dotnet build "UniSpyServer.Servers.QueryReport.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.QueryReport.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.QueryReport.dll"]