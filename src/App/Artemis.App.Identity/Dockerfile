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
COPY ["src/Extensions/Artemis.Extensions.Rpc/Artemis.Extensions.Rpc.csproj", "src/Extensions/Artemis.Extensions.Rpc/"]
COPY ["src/Protos/Artemis.Protos.Common/Artemis.Protos.Common.csproj", "src/Protos/Artemis.Protos.Common/"]
COPY ["src/Extensions/Artemis.Extensions.Web.Identity/Artemis.Extensions.Web.Identity.csproj", "src/Extensions/Artemis.Extensions.Web.Identity/"]
COPY ["src/Data/Artemis.Data.Core/Artemis.Data.Core.csproj", "src/Data/Artemis.Data.Core/"]
COPY ["src/Shared/Artemis.Shared.Identity/Artemis.Shared.Identity.csproj", "src/Shared/Artemis.Shared.Identity/"]
COPY ["src/Extensions/Artemis.Extensions.Web.Serilog/Artemis.Extensions.Web.Serilog.csproj", "src/Extensions/Artemis.Extensions.Web.Serilog/"]
COPY ["src/Extensions/Artemis.Extensions.Web.Validators/Artemis.Extensions.Web.Validators.csproj", "src/Extensions/Artemis.Extensions.Web.Validators/"]
COPY ["src/Protos/Artemis.Protos.Identity/Artemis.Protos.Identity.csproj", "src/Protos/Artemis.Protos.Identity/"]
COPY ["src/Services/Artemis.Services.Identity/Artemis.Services.Identity.csproj", "src/Services/Artemis.Services.Identity/"]
COPY ["src/Data/Artemis.Data.Store/Artemis.Data.Store.csproj", "src/Data/Artemis.Data.Store/"]
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