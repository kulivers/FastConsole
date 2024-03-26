FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

ARG AGENT_DIRECTORY="/src/adapterhost"
RUN mkdir -p "/src/adapterhost"
ADD bin/Debug/net6.0/* "/src/adapterhost"
WORKDIR "/src/adapterhost"
ENTRYPOINT ["dotnet", "DockerEnvPass.dll"]