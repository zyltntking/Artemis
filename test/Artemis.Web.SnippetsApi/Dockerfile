#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["test/Artemis.Web.SnippetsApi/Artemis.Web.SnippetsApi.csproj", "test/Artemis.Web.SnippetsApi/"]
RUN dotnet restore "test/Artemis.Web.SnippetsApi/Artemis.Web.SnippetsApi.csproj"
COPY . .
WORKDIR "/src/test/Artemis.Web.SnippetsApi"
RUN dotnet build "Artemis.Web.SnippetsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Artemis.Web.SnippetsApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artemis.Web.SnippetsApi.dll"]