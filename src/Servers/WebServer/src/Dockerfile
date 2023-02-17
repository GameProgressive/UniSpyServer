FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/WebServer/src/UniSpy.Server.WebServer.csproj", "src/Servers/WebServer/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
RUN dotnet restore "src/Servers/WebServer/src/UniSpy.Server.WebServer.csproj"
COPY . .
WORKDIR "/src/src/Servers/WebServer/src"
RUN dotnet build "UniSpy.Server.WebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.WebServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.WebServer.dll"]