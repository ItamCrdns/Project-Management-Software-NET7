# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy only necessary files for the main project (excluding tests)
COPY CompanyPMO.NET/CompanyPMO.NET.csproj ./CompanyPMO.NET/
WORKDIR /App/CompanyPMO.NET
RUN dotnet restore

# Copy the rest of the application (excluding tests)
WORKDIR /App
COPY . ./
RUN ls -a  # This command will help you verify the contents in the current directory

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App

# Copy published output from the build stage
COPY --from=build-env /App/CompanyPMO.NET/out .

ENTRYPOINT ["dotnet", "CompanyPMO.NET.dll"]
