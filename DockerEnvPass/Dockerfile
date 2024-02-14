FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

ARG AGENT_DIRECTORY="/src/adapterhost"
ARG HOST_STARTUP_CONFIG="$AGENT_DIRECTORY/adapterhoststartup.config"
ENV loadDataDirectory="$AGENT_DIRECTORY/LoadData"
ENV TEST="xyu"
#logFolder
#archiveFolder
#maxArchiveFiles
#archiveAboveSize

RUN mkdir -p $AGENT_DIRECTORY
RUN mkdir -p $loadDataDirectory

WORKDIR $AGENT_DIRECTORY

ADD bin/Debug/net6.0/* $AGENT_DIRECTORY
COPY template.yml "$AGENT_DIRECTORY/template.yml"
COPY replace_env_vars.sh "$AGENT_DIRECTORY/replace_env_vars.sh"

RUN chmod +x "$AGENT_DIRECTORY/replace_env_vars.sh"

RUN /src/adapterhost/replace_env_vars.sh /src/adapterhost/template.yml /src/adapterhost/template.yml

WORKDIR $AGENT_DIRECTORY
ENTRYPOINT ["dotnet", "DockerEnvPass.dll", "/src/adapterhost/template2.yml"]