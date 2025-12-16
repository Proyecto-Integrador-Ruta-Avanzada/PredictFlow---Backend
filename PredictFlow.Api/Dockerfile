FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PredictFlow.Api/PredictFlow.Api.csproj", "api/"]
COPY ["PredictFlow.Application/PredictFlow.Application.csproj", "application/"]
COPY ["PredictFlow.Domain/PredictFlow.Domain.csproj", "domain/"]
COPY ["PredictFlow.Infrastructure/PredictFlow.Infrastructure.csproj", "infrastructure/"]


RUN dotnet restore "api/PredictFlow.Api.csproj"

COPY . .

WORKDIR "/src/api"
RUN dotnet build "PredictFlow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "PredictFlow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PredictFlow.Api.dll"]
