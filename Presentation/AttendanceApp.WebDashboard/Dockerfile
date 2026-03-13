# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get install -y nodejs
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/AttendanceApp.WebDashboard/AttendanceApp.WebDashboard.csproj", "Presentation/AttendanceApp.WebDashboard/"]
COPY ["Core/AttendanceApp.Application/AttendanceApp.Application.csproj", "Core/AttendanceApp.Application/"]
COPY ["Core/AttendanceApp.Domain/AttendanceApp.Domain.csproj", "Core/AttendanceApp.Domain/"]
COPY ["Infrastructure/AttendanceApp.Infrastructure/AttendanceApp.Infrastructure.csproj", "Infrastructure/AttendanceApp.Infrastructure/"]
RUN dotnet restore "./Presentation/AttendanceApp.WebDashboard/AttendanceApp.WebDashboard.csproj"
COPY . .
WORKDIR "/src/Presentation/AttendanceApp.WebDashboard"
RUN dotnet build "./AttendanceApp.WebDashboard.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AttendanceApp.WebDashboard.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AttendanceApp.WebDashboard.dll"]