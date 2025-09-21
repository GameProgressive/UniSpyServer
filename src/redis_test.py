
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
import socket

SERVER_IP = "127.0.0.1"   # change to target IP if needed
SERVER_PORT = 27901
MESSAGE = b"hello"

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

try:
    for _ in range(10):
        sock.sendto(MESSAGE, (SERVER_IP, SERVER_PORT))
        print(f"Sent {MESSAGE!r} to {SERVER_IP}:{SERVER_PORT}")
finally:
    sock.close()