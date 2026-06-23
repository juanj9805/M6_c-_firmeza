# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy source and publish
COPY . .
RUN dotnet publish Firmeza.Admin -c Release -o /out

# Stage 2: Runtime only
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Firmeza.Admin.dll"]
