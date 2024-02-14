FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

ARG AGENT_DIRECTORY="/src/adapterhost"
ARG HOST_STARTUP_CONFIG ="$AGENT_DIRECTORY/adapterhoststartup.config"
ENV loadDataDirectory = "$AGENT_DIRECTORY/LoadData"

#logFolder
#archiveFolder
#maxArchiveFiles
#archiveAboveSize

RUN mkdir $AGENT_DIRECTORY
RUN mkdir $loadDataDirectory
ADD bin/Debug/net6.0/* $AGENT_DIRECTORY

WORKDIR $AGENT_DIRECTORY
ENTRYPOINT ["dotnet", "Comindware.Adapter.Agent.dll", "$HOST_STARTUP_CONFIG"]