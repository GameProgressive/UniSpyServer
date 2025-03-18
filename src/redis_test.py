
# import redis
# from frontends.gamespy.library.configs import CONFIG


# # SESSION = redis.Redis.from_url(CONFIG.redis.url)

# client = redis.from_url(CONFIG.redis.url)
# pubsub = client.pubsub()
# pubsub.subscribe("test")

# for message in pubsub.listen():
#     if message['type'] == 'message':
#         print(f"Received: {message['data'].decode('utf-8')}")

# client.set("hello", "hi")
# data = client.get("hello")
# pass
