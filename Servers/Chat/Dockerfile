FROM mcr.microsoft.com/dotnet/core/runtime:3.1.2-alpine3.11 AS base
WORKDIR /app
EXPOSE 6667

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102-alpine3.11 AS build
WORKDIR /src
COPY ["/Servers/Chat/Chat.csproj", "/Servers/Chat/"]
COPY ["/GameSpyLib/GameSpyLib.csproj", "/GameSpyLib/"]
RUN dotnet restore "/Servers/Chat/Chat.csproj"
COPY . .
WORKDIR "/src/Servers/Chat"
RUN dotnet build "Chat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./CommonFiles/RetroSpyServer.json .
ENTRYPOINT ["dotnet", "Chat.dll"]