from dataclasses import dataclass
import os
from typing import Literal, Optional
from uuid import UUID

from library.src.exceptions.general import UniSpyException


@dataclass
class PostgreSql:
    server: str
    port: int
    database: str
    username: str
    password: str
    ssl_mode: str  # You might want to restrict this to specific values
    trust_server_cert: bool
    ssl_key: Optional[str] = None  # Optional field for SSL key
    ssl_password: Optional[str] = None  # Optional field for SSL password
    root_cert: Optional[str] = None  # Optional field for root certificate

    def __post_init__(self):
        pass

    @property
    def url(self) -> str:
        return f"postgresql://{self.username}:{self.password}@{self.server}:{self.port}/{self.database}?sslmode={self.ssl_mode}"


@dataclass
class RedisConfig:
    server: str
    port: int
    user: str
    password: str
    ssl: bool  # Use bool for SSL flag
    ssl_host: Optional[str] = None  # Optional field for SSL host

    @property
    def url(self) -> str:
        if self.ssl:
            return (
                f"rediss://{self.user}:{self.password}@{self.server}:{self.port}/0"
            )
        else:
            return (
                f"redis://{self.user}:{self.password}@{self.server}:{self.port}/0"
            )


@dataclass
class ServerConfig:
    server_id: UUID
    server_name: str
    public_address: str
    listening_port: int  # Ensures listening_port is between 1 and 65535


@dataclass
class LoggingConfig:
    path: str
    min_log_level: Literal["debug", "info", "warning", "error"]

    def __post_init__(self):
        if "~" in self.path:
            self.path = os.path.expanduser(self.path)


@dataclass
class BackendConfig:
    url: str


@dataclass
class MongoDbConfig:
    server: str
    username: str
    password: str
    port: int
    database: str

    @property
    def url(self):
        url = f"mongodb+srv://{self.username}:{self.password}@{self.server}"
        if self.port is not None:
            url += f":{self.port}"
        if self.database is not None:
            url += f"/{self.database}"
        return url


@dataclass
class UniSpyServerConfig:
    postgresql: PostgreSql
    redis: RedisConfig
    backend: BackendConfig
    servers: dict[str, ServerConfig]
    mongodb: MongoDbConfig
    logging: LoggingConfig

    def __post_init__(self):

        self.postgresql = PostgreSql(**self.postgresql)  # type: ignore
        self.redis = RedisConfig(**self.redis)  # type: ignore
        self.backend = BackendConfig(**self.backend)  # type: ignore
        for key, value in self.servers.items():
            self.servers[key] = ServerConfig(**value)  # type: ignore
        self.mongodb = MongoDbConfig(**self.mongodb)  # type: ignore
        self.logging = LoggingConfig(**self.logging)  # type: ignore


unispy_config = os.environ.get("UNISPY_CONFIG")
if unispy_config is None:

    raise UniSpyException(
        "Unispy server config not found, you should set the UNISPY_CONFIG in the system enviroment."
    )
with open(unispy_config, "r") as f:
    import json

    config = json.load(f)
    CONFIG = UniSpyServerConfig(**config)
    pass

if __name__ == "__main__":
    pass
