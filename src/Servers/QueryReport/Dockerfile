FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.12 AS base
WORKDIR /app
EXPOSE 27900
EXPOSE 27900/udp

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/QueryReport/QueryReport.csproj", "src/Servers/QueryReport/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
RUN dotnet restore "src/Servers/QueryReport/QueryReport.csproj"
COPY . .
WORKDIR "/src/src/Servers/QueryReport"
RUN dotnet build "QueryReport.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QueryReport.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "QueryReport.dll"]