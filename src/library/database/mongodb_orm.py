from mongoengine import connect

from library.unispy_server_config import CONFIG


def connect_to_db():
    connect(host=CONFIG.mongodb.url)


def get_ttl_param(seconds: int):
    assert isinstance(seconds, int)
    return {"indexes": [{"fields": ["created"], "expireAfterSeconds": seconds}]}
