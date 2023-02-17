FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 27901/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Servers/NatNegotiation/src/UniSpy.Server.NatNegotiation.csproj", "src/Servers/NatNegotiation/src/"]
COPY ["src/Libraries/Core/src/UniSpy.Server.Core.csproj", "src/Libraries/Core/src/"]
RUN dotnet restore "src/Servers/NatNegotiation/src/UniSpy.Server.NatNegotiation.csproj"
COPY . .
WORKDIR "/src/src/Servers/NatNegotiation/src"
RUN dotnet build "UniSpy.Server.NatNegotiation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniSpy.Server.NatNegotiation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniSpy.Server.NatNegotiation.dll"]