FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 29920

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/GameStatus/src/UniSpy.Server.GameStatus.csproj", "src/Servers/GameStatus/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
RUN dotnet restore "src/Servers/GameStatus/src/UniSpy.Server.GameStatus.csproj"
COPY . .
WORKDIR "/src/src/Servers/GameStatus/src"
RUN dotnet build "UniSpy.Server.GameStatus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.GameStatus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.GameStatus.dll"]