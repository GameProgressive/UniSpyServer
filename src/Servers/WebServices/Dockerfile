FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.13 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build
WORKDIR /src
COPY ["src/Servers/WebServices/WebServices.csproj", "src/Servers/WebServices/"]
COPY ["src/Libraries/UniSpyLib/UniSpyLib.csproj", "src/Libraries/UniSpyLib/"]
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
COPY ./common/UniSpyServer.cfg .
ENTRYPOINT ["dotnet", "WebServices.dll"]