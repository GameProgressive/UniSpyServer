FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 27900

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/QueryReport/QueryReport.csproj", "/Servers/QueryReport/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/QueryReport/QueryReport.csproj"
COPY . .
WORKDIR "/src/Servers/QueryReport"
RUN dotnet build "QueryReport.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QueryReport.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.xml .
ENTRYPOINT ["dotnet", "QueryReport.dll"]