FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/WebServices/WebServices.csproj", "/Servers/WebServices/"]
RUN dotnet restore "/Servers/WebServices/WebServices.csproj"
COPY . .
WORKDIR "/src/Servers/WebServices"
RUN dotnet build "WebServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebServices.dll"]