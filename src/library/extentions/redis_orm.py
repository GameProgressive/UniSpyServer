import redis


class RedisORM:
    def __init__(self, host="localhost", port=6379, db=0):
        # self.redis_conn = redis.StrictRedis(host=host, port=port, db=db)
        pass

    def query(self, table_class):
        return QueryBuilder(self.redis_conn, table_class)


class QueryBuilder:
    def __init__(self, redis_conn, table_class):
        self.redis_conn = redis_conn
        self.table_class = table_class

    def filter_by(self, **kwargs):
        self.filter_criteria = kwargs
        return self

    def first(self):
        key = f"{self.table_class.__name__}:{self.filter_criteria['url']}"
        data = self.redis_conn.hgetall(key)
        return data


# Example usage
class User:
    pass


redis_orm = RedisORM()
query = QueryBuilder(None, None)
result = query.filter_by(url="example.com", name="hello").first()
print(result)
