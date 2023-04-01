FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src/FamilyAssistant.Migrator

COPY . .

RUN dotnet restore "FamilyAssistant.Migrator.csproj"
RUN dotnet build "FamilyAssistant.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamilyAssistant.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilyAssistant.Migrator.dll"]
