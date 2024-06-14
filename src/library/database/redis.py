import asyncio
# import redis
import aioredis

from library.unispy_server_config import CONFIG


# SESSION = redis.Redis.from_url(CONFIG.redis.url)

pool = aioredis.from_url(CONFIG.redis.url)
loop=asyncio.get_event_loop()
loop.run_until_complete(pool.set("hello","hi"))
data = loop.run_until_complete(pool.get("hello"))
pass


