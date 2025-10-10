from backends.library.networks.redis_brocker import RedisBrocker
from frontends.gamespy.library.configs import CONFIG


# we only need publish so du not need subscribe
BROCKER = RedisBrocker("master", CONFIG.redis.url)
pass
