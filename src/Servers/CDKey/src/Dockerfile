FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 29910/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/CDKey/src/UniSpy.Server.CDKey.csproj", "src/Servers/CDKey/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
RUN dotnet restore "src/Servers/CDKey/src/UniSpy.Server.CDKey.csproj"
COPY . .
WORKDIR "/src/src/Servers/CDKey/src"
RUN dotnet build "UniSpy.Server.CDKey.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.CDKey.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.CDKey.dll"]