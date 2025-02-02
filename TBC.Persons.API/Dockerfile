FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TBC.Persons.API/TBC.Persons.API.csproj", "TBC.Persons.API/"]
RUN dotnet restore "TBC.Persons.API/TBC.Persons.API.csproj"
COPY . .
WORKDIR "/src/TBC.Persons.API"
RUN dotnet build "TBC.Persons.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TBC.Persons.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TBC.Persons.API.dll"]
