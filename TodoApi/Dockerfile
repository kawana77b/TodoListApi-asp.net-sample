#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TodoApi/TodoApi.csproj", "TodoApi/"]
RUN dotnet restore "./TodoApi/TodoApi.csproj"
COPY . .
WORKDIR "/src/TodoApi"
# Migration using dotnet-ef to make SQLite database available
RUN dotnet tool install --global dotnet-ef && export PATH="$PATH:/root/.dotnet/tools" && dotnet ef database update
RUN dotnet build "./TodoApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TodoApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# Move the migrated database so that you can write permissions to it. Dangerous in truth!
RUN cp "./app.db" /app/publish/app.db && chmod 777 /app/publish/app.db

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]