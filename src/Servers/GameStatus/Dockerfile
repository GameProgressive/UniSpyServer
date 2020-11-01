FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 29920

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/GameStatus/GameStatus.csproj", "src/Servers/GameStatus/"]
COPY ["src/Libraries/GameSpyLib/GameSpyLib.csproj", "src/Libraries/GameSpyLib/"]
RUN dotnet restore "src/Servers/GameStatus/GameStatus.csproj"
COPY . .
WORKDIR "/src/src/Servers/GameStatus"
RUN dotnet build "GameStatus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameStatus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "GameStatus.dll"]