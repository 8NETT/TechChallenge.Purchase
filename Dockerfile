FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["TechChallenge.Purchases.sln", "./"]
COPY ["Src/TechChallenge.Purchases.Web/TechChallenge.Purchases.Web.csproj", "Src/TechChallenge.Purchases.Web/"]
COPY ["Src/TechChallenge.Purchases.Core/TechChallenge.Purchases.Core.csproj", "Src/TechChallenge.Purchases.Core/"]
COPY ["Src/TechChallenge.Purchases.Infrastructure/TechChallenge.Purchases.Infrastructure.csproj", "Src/TechChallenge.Purchases.Infrastructure/"]
COPY ["Src/TechChallenge.Purchases.Application/TechChallenge.Purchases.Application.csproj", "Src/TechChallenge.Purchases.Application/"]
COPY ["TechChallenge.Purchases.Tests/TechChallenge.Purchases.Tests.csproj", "TechChallenge.Purchases.Tests/"]

RUN dotnet restore "TechChallenge.Purchases.sln"

# Copy everything else
COPY . ./ 

# Run tests
RUN dotnet test "TechChallenge.Purchases.sln" --no-restore

# Build the application
RUN dotnet publish "Src/TechChallenge.Purchases.Web/TechChallenge.Purchases.Web.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so

WORKDIR /app
COPY --from=build-env /app/publish .

# Force ASP.NET Core to listen on port 8081
ENV ASPNETCORE_URLS=http://+:8081

# Expose port 8081 so Azure Container Apps ingress can connect
EXPOSE 8081

ENTRYPOINT ["dotnet", "TechChallenge.Purchases.Web.dll"]
