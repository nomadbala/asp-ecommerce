# Use .NET 8.0 SDK as the base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the entire solution
COPY . .

# Build the projects
RUN dotnet publish -c Release -o out

# Use .NET 8.0 runtime as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Install Docker and Docker Compose
RUN apt-get update && apt-get install -y \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg \
    lsb-release && \
    curl -fsSL https://download.docker.com/linux/debian/gpg | gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg && \
    echo "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian $(lsb_release -cs) stable" | tee /etc/apt/sources.list.d/docker.list > /dev/null && \
    apt-get update && apt-get install -y docker-ce docker-ce-cli containerd.io && \
    curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose && \
    chmod +x /usr/local/bin/docker-compose

WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Copy the docker-compose.yml file from the build stage (if it exists)
COPY --from=build /app/docker-compose.yml* ./

# Expose the port used by the API Gateway
EXPOSE 8080

# Command to run when the container starts
CMD ["sh", "-c", "if [ -f docker-compose.yml ]; then docker-compose up --build; else dotnet ApiGateway.dll; fi"]