FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.7-alpine3.12 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.401-alpine3.12 AS build
WORKDIR /src
COPY ["/Servers/WebServices/WebServices.csproj", "/Servers/WebServices/"]
COPY ["/Library/GameSpyLib/GameSpyLib.csproj", "/Library/GameSpyLib/"]
COPY ["/Library/SOAPMiddleware/SOAPMiddleware.csproj", "/Library/SOAPMiddleware/"]
RUN dotnet restore "/Servers/WebServices/WebServices.csproj"
COPY . .
WORKDIR "/src/Servers/WebServices"
RUN dotnet build "WebServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "WebServices.dll"]