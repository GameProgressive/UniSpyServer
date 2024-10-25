import os
from typing import Literal, Optional
from uuid import UUID

from pydantic import BaseModel,field_validator



class PostgreSql(BaseModel):
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

    @property
    def url(self) -> str:
        return f"postgresql://{self.username}:{self.password}@{self.server}:{self.port}/{self.database}?sslmode={self.ssl_mode}"


class RedisConfig(BaseModel):
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


class ServerConfig(BaseModel):
    server_id: UUID
    server_name: str
    public_address: str
    listening_port: int  # Ensures listening_port is between 1 and 65535


class LoggingConfig(BaseModel):
    path: str
    min_log_level: Literal["debug", "info", "warning", "error"]

    @field_validator('path', mode='before')
    def expand_user_path(cls, value):
        if "~" in value:
            return os.path.expanduser(value)
        return value


class BackendConfig(BaseModel):
    url: str



class UniSpyServerConfig(BaseModel):
    postgresql: PostgreSql
    redis: RedisConfig
    backend: BackendConfig
    servers: dict[str, ServerConfig]
    logging: LoggingConfig

unispy_config = os.environ.get("UNISPY_CONFIG")
default_config = "../common/config.json"
if unispy_config is None:
    unispy_config = default_config
    # raise Exception(
    #     "Unispy server config not found, you should set the UNISPY_CONFIG in the system enviroment."
    # )
if not os.path.exists(unispy_config):
    raise Exception(
        "Unispy server config file not exist, check UNISPY_CONFIG path."
    )
with open(unispy_config, "r") as f:
    import json

    config = json.load(f)
    CONFIG = UniSpyServerConfig(**config)
    pass

if __name__ == "__main__":
    pass
