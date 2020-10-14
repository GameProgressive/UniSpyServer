FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/WebServices/WebServices.csproj", "src/Servers/WebServices/"]
COPY ["src/Libraries/GameSpyLib/GameSpyLib.csproj", "src/Libraries/GameSpyLib/"]
COPY ["src/Libraries/SOAPMiddleware/SOAPMiddleware.csproj", "src/Libraries/SOAPMiddleware/"]
RUN dotnet restore "src/Servers/WebServices/WebServices.csproj"
COPY . .
WORKDIR "/src/src/Servers/WebServices"
RUN dotnet build "WebServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./common/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "WebServices.dll"]