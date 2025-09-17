FROM node:20 AS client-build
WORKDIR /src/client

COPY ./client/package*.json ./
RUN npm ci

COPY ./client/ ./
# The build output should be in /src/API/wwwroot
RUN npm run build -- -c production

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src/API

COPY ./API/ ./
RUN dotnet restore API.csproj

COPY --from=client-build /src/API/wwwroot ./wwwroot
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
