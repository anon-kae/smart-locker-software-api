# FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
# WORKDIR smartlocker.software.api
# COPY SmartLocker.Software.Backend.csproj smartlocker.software.api/
# RUN dotnet restore smartlocker.software.api/SmartLocker.Software.Backend.csproj
# COPY . .
# # WORKDIR smartlocker.software.api
# RUN dotnet publish --output /app --configuration Release

# FROM mcr.microsoft.com/dotnet/sdk:3.1
# WORKDIR /app
# COPY --from=build-env /app .
# ENTRYPOINT ["dotnet", "SmartLocker.Software.Backend.dll"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Change timezone to local time
ENV TZ=Asia/Bangkok
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
# ENV ASPNETCORE_ENVIRONMENT=development
# ENV ASPNETCORE_URLS=http://*:5000

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "SmartLocker.Software.Backend.dll"]



