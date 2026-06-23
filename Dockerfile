# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj files and restore — layer cache optimization
COPY Firmeza.Domain/Firmeza.Domain.csproj Firmeza.Domain/
COPY Firmeza.Admin/Firmeza.Admin.csproj Firmeza.Admin/
RUN dotnet restore Firmeza.Admin/Firmeza.Admin.csproj

# Copy source and publish
COPY . .
RUN dotnet publish Firmeza.Admin/Firmeza.Admin.csproj -c Release -o /out

# Stage 2: Runtime only
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Firmeza.Admin.dll"]
