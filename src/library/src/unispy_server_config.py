import json
from typing import Dict, Optional
from uuid import UUID
import os

from library.src.exceptions.error import UniSpyException


class PostgreSql:
    url: str

    def __init__(
        self,
        server,
        port,
        database,
        username,
        password,
        ssl_mode,
        trust_server_cert,
        ssl_key,
        ssl_password,
        root_cert,
    ) -> None:
        self.server = server
        self.port = int(port)
        self.database = database
        self.username = username
        self.password = password
        self.ssl_mode = ssl_mode
        self.trust_server_cert = trust_server_cert
        self.ssl_key = ssl_key
        self.ssl_password = ssl_password
        self.root_cert = root_cert
        self.url = f"postgresql://{self.username}:{self.password}@{self.server}:{self.port}/{self.database}?sslmode={self.ssl_mode}"


class RedisConfig:
    url: str

    def __init__(self, server, port, user, password, ssl, ssl_host) -> None:
        self.server = server
        self.port = int(port)
        self.user = user
        self.password = password
        self.ssl = ssl
        self.ssl_host = ssl_host
        if self.ssl == "true":
            self.url = (
                f"rediss://{self.user}:{self.password}@{self.server}:{self.port}/0"
            )


class ServerConfig:
    def __init__(self, server_id, server_name, public_address, listening_port) -> None:
        self.server_id = UUID(server_id)
        self.server_name = server_name
        self.public_address = public_address
        self.listening_port = int(listening_port)


class LoggingConfig:
    def __init__(self, path: str, min_log_level: str) -> None:
        self.path = path
        self.min_log_level = min_log_level


class BackendConfig:
    def __init__(self, url: str) -> None:
        self.url = url


class MongoDbConfig:
    server: str
    port: int
    username: str
    password: str
    database: str
    url: str

    def __init__(self, server, port, username, password, database) -> None:
        self.server = server
        self.port = port
        self.username = username
        self.password = password
        self.database = database
        self.url = f"mongodb+srv://{self.username}:{self.password}@{server}"
        if port is not None:
            self.url += f":{port}"
        if database is not None:
            self.url += f"/{database}"


class UniSpyServerConfig:
    postgresql: PostgreSql
    redis: RedisConfig
    backend: BackendConfig
    servers: Dict[str, ServerConfig] = {}
    mongodb: MongoDbConfig

    def __init__(self, config: Dict[str, str]) -> None:
        self.mongodb = MongoDbConfig(**config["mongodb"])
        self.postgresql = PostgreSql(**config["postgresql"])
        self.redis = RedisConfig(**config["redis"])
        self.backend = BackendConfig(**config["backend"])
        self.logging = LoggingConfig(**config["logging"])
        for info in config["servers"]:
            self.servers[info["server_name"]] = ServerConfig(**info)


unispy_config = os.environ.get("UNISPY_CONFIG")
if unispy_config is None:
    raise UniSpyException(
        "Unispy server config not found, you should set the UNISPY_CONFIG in the system enviroment."
    )
with open(unispy_config, "r") as f:
    config = json.load(f)
    CONFIG = UniSpyServerConfig(config)
    pass

if __name__ == "__main__":
    pass
