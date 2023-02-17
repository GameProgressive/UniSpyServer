FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 28910
EXPOSE 28900

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/ServerBrowser/src/UniSpy.Server.ServerBrowser.csproj", "src/Servers/ServerBrowser/src/"]
COPY ["src/Servers/NatNegotiation/src/UniSpy.Server.NatNegotiation.csproj", "src/Servers/NatNegotiation/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
COPY ["src/Servers/QueryReport/src/UniSpy.Server.QueryReport.csproj", "src/Servers/QueryReport/src/"]
RUN dotnet restore "src/Servers/ServerBrowser/src/UniSpy.Server.ServerBrowser.csproj"
COPY . .
WORKDIR "/src/src/Servers/ServerBrowser/src"
RUN dotnet build "UniSpy.Server.ServerBrowser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.ServerBrowser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.ServerBrowser.dll"]