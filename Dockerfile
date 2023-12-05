# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy only necessary project file for restore (excluding tests)
COPY CompanyPMO.NET.csproj ./
RUN dotnet restore

# Copy the rest of the application (excluding tests)
COPY . ./

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App

# Copy published output from the build stage
COPY --from=build-env /App/out .

ENTRYPOINT ["dotnet", "CompanyPMO.dll"]
