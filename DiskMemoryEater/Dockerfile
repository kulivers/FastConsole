FROM mono:latest

#WORKDIR /app

ENV AGENT_DIRECTORY = "/src/adapterhost"
RUN mkdir -p $AGENT_DIRECTORY

ADD ./bin/Debug/* $AGENT_DIRECTORY

WORKDIR $AGENT_DIRECTORY
COPY template.config "$AGENT_DIRECTORY/template.config"

COPY replace_env_vars.sh "$AGENT_DIRECTORY/replace_env_vars.sh"

RUN chmod +x "$AGENT_DIRECTORY/replace_env_vars.sh"

RUN /app/replace_env_vars.sh "$AGENT_DIRECTORY/template.config" "$AGENT_DIRECTORY/config.txt"

#WORKDIR $AGENT_DIRECTORY
#ENTRYPOINT ["mono", "DiskMemoryEater.exe"]