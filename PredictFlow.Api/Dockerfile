FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PredictFlow.Api/PredictFlow.Api.csproj", "PredictFlow.Api/"]
COPY ["PredictFlow.Application/PredictFlow.Application.csproj", "PredictFlow.Application/"]
COPY ["PredictFlow.Domain/PredictFlow.Domain.csproj", "PredictFlow.Domain/"]
COPY ["PredictFlow.Infrastructure/PredictFlow.Infrastructure.csproj", "PredictFlow.Infrastructure/"]

RUN dotnet restore "PredictFlow.Api/PredictFlow.Api.csproj"

COPY . .

WORKDIR "/src/PredictFlow.Api"
RUN dotnet build "PredictFlow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "PredictFlow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PredictFlow.Api.dll"]
