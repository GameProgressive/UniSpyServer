FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 27900/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/QueryReport/src/UniSpy.Server.QueryReport.csproj", "src/Servers/QueryReport/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
RUN dotnet restore "src/Servers/QueryReport/src/UniSpy.Server.QueryReport.csproj"
COPY . .
WORKDIR "/src/src/Servers/QueryReport/src"
RUN dotnet build "UniSpy.Server.QueryReport.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.QueryReport.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.QueryReport.dll"]