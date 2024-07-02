#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/App/Artemis.App.School/Artemis.App.School.csproj", "src/App/Artemis.App.School/"]
COPY ["src/Extensions/Artemis.Extensions.ServiceConnect/Artemis.Extensions.ServiceConnect.csproj", "src/Extensions/Artemis.Extensions.ServiceConnect/"]
COPY ["src/Data/Artemis.Data.Core/Artemis.Data.Core.csproj", "src/Data/Artemis.Data.Core/"]
COPY ["src/Data/Artemis.Data.Shared/Artemis.Data.Shared.csproj", "src/Data/Artemis.Data.Shared/"]
COPY ["src/Services/Artemis.Service.School/Artemis.Service.School.csproj", "src/Services/Artemis.Service.School/"]
COPY ["src/Data/Artemis.Data.Store/Artemis.Data.Store.csproj", "src/Data/Artemis.Data.Store/"]
COPY ["src/Services/Artemis.Service.Protos/Artemis.Service.Protos.csproj", "src/Services/Artemis.Service.Protos/"]
RUN dotnet restore "./src/App/Artemis.App.School/Artemis.App.School.csproj"
COPY . .
WORKDIR "/src/src/App/Artemis.App.School"
RUN dotnet build "./Artemis.App.School.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Artemis.App.School.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artemis.App.School.dll"]