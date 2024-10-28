import redis

from library.src.configs import CONFIG


# SESSION = redis.Redis.from_url(CONFIG.redis.url)

client = redis.from_url(CONFIG.redis.url)
client.set("hello", "hi")
data = client.get("hello")
pass
