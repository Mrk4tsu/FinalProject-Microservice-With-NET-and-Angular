# Base image dùng để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Stage build code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ source code vào container
COPY . .

# Restore dependencies
RUN dotnet restore "FN.UserService/FN.UserService.csproj"

# Build project
RUN dotnet build "FN.UserService/FN.UserService.csproj" -c Release -o /app/build

# Publish project
FROM build AS publish
RUN dotnet publish "FN.UserService/FN.UserService.csproj" -c Release -o /app/publish

# Final stage: chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FN.UserService.dll"]
