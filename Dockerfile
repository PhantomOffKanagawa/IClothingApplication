FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

RUN apt-get update && \
    apt-get install -y curl apt-transport-https gnupg software-properties-common && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    # Remove conflicting packages
    apt-get remove -y libodbc2 libodbcinst2 unixodbc-common && \
    # Force install of required Microsoft packages
    ACCEPT_EULA=Y apt-get install -y --allow-downgrades --allow-remove-essential --allow-change-held-packages \
      msodbcsql17 mssql-tools unixodbc-dev && \
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

ENV PATH="/opt/mssql-tools/bin:$PATH"

COPY ./sql/start.sh /start.sh
RUN chmod +x /start.sh

ENTRYPOINT ["/start.sh"]
