#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/App/Artemis.App.Identity/Artemis.App.Identity.csproj", "src/App/Artemis.App.Identity/"]
COPY ["src/Extensions/Artemis.Extensions.ServiceConnect/Artemis.Extensions.ServiceConnect.csproj", "src/Extensions/Artemis.Extensions.ServiceConnect/"]
COPY ["src/Extensions/Artemis.Extensions.Identity/Artemis.Extensions.Identity.csproj", "src/Extensions/Artemis.Extensions.Identity/"]
COPY ["src/Data/Artemis.Data.Core/Artemis.Data.Core.csproj", "src/Data/Artemis.Data.Core/"]
COPY ["src/Services/Artemis.Service.Identity/Artemis.Service.Identity.csproj", "src/Services/Artemis.Service.Identity/"]
COPY ["src/Data/Artemis.Data.Store/Artemis.Data.Store.csproj", "src/Data/Artemis.Data.Store/"]
COPY ["src/Services/Artemis.Service.Shared/Artemis.Service.Shared.csproj", "src/Services/Artemis.Service.Shared/"]
RUN dotnet restore "./src/App/Artemis.App.Identity/Artemis.App.Identity.csproj"
COPY . .
WORKDIR "/src/src/App/Artemis.App.Identity"
RUN dotnet build "./Artemis.App.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Artemis.App.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artemis.App.Identity.dll"]