FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine3.13 AS base
WORKDIR /app
EXPOSE 29910
EXPOSE 29910/udp

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/CDKey/CDKey.csproj", "src/Servers/CDKey/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
RUN dotnet restore "src/Servers/CDKey/CDKey.csproj"
COPY . .
WORKDIR "/src/src/Servers/CDKey"
RUN dotnet build "CDKey.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CDKey.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "CDKey.dll"]