FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 6667

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/Chat/src/UniSpy.Server.Chat.csproj", "src/Servers/Chat/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
COPY ["src/Servers/QueryReport/src/UniSpy.Server.QueryReport.csproj", "src/Servers/QueryReport/src/"]
RUN dotnet restore "src/Servers/Chat/src/UniSpy.Server.Chat.csproj"
COPY . .
WORKDIR "/src/src/Servers/Chat/src"
RUN dotnet build "UniSpy.Server.Chat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.Chat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.Chat.dll"]