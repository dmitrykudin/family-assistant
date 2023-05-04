FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FamilyAssistant.Sandbox/FamilyAssistant.Sandbox.csproj", "FamilyAssistant.Sandbox/"]
RUN dotnet restore "FamilyAssistant.Sandbox/FamilyAssistant.Sandbox.csproj"
COPY . .
WORKDIR "/src/FamilyAssistant.Sandbox"
RUN dotnet build "FamilyAssistant.Sandbox.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamilyAssistant.Sandbox.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilyAssistant.Sandbox.dll"]
