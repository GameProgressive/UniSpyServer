from backends.library.abstractions.websocket_manager import WebSocketManager


MANAGER = WebSocketManager()

# create redis pubsub to share message cross all backends
# currently we simply implement without redis pubsub
