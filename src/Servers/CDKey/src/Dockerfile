FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 29910/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/CDKey/src/UniSpyServer.Servers.CDKey.csproj", "src/Servers/CDKey/src/"]
COPY ["src/Libraries/UniSpyLib/src/UniSpyServer.Libraries.UniSpyLib.csproj", "src/Libraries/UniSpyLib/src/"]
RUN dotnet restore "src/Servers/CDKey/src/UniSpyServer.Servers.CDKey.csproj"
COPY . .
WORKDIR "/src/src/Servers/CDKey/src"
RUN dotnet build "UniSpyServer.Servers.CDKey.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpyServer.Servers.CDKey.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpyServer.Servers.CDKey.dll"]