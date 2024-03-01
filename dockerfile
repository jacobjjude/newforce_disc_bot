# Use the .NET Core SDK image to build the project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and any of its dependencies
COPY *.csproj ./
RUN dotnet restore

#Copy the project files and build the release
COPY . ./
RUN dotnet publish -c Release -o out

#Generate the runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

#Command to run the application
ENTRYPOINT ["dotnet", "main_bot.dll"]