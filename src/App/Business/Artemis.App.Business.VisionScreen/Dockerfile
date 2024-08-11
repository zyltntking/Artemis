#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/App/Business/Artemis.App.Business.VisionScreen/Artemis.App.Business.VisionScreen.csproj", "src/App/Business/Artemis.App.Business.VisionScreen/"]
COPY ["src/Extensions/Artemis.Extensions.Identity/Artemis.Extensions.Identity.csproj", "src/Extensions/Artemis.Extensions.Identity/"]
COPY ["src/Data/Artemis.Data.Core/Artemis.Data.Core.csproj", "src/Data/Artemis.Data.Core/"]
COPY ["src/Extensions/Artemis.Extensions.ServiceConnect/Artemis.Extensions.ServiceConnect.csproj", "src/Extensions/Artemis.Extensions.ServiceConnect/"]
COPY ["src/Services/Artemis.Service.Protos/Artemis.Service.Protos.csproj", "src/Services/Artemis.Service.Protos/"]
RUN dotnet restore "./src/App/Business/Artemis.App.Business.VisionScreen/Artemis.App.Business.VisionScreen.csproj"
COPY . .
WORKDIR "/src/src/App/Business/Artemis.App.Business.VisionScreen"
RUN dotnet build "./Artemis.App.Business.VisionScreen.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Artemis.App.Business.VisionScreen.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artemis.App.Business.VisionScreen.dll"]