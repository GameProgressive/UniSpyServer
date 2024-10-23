from mongoengine import connect

from library.src.configs import CONFIG


def connect_to_db():
    connect(host=CONFIG.mongodb.url)


def get_ttl_param(seconds: int):
    assert isinstance(seconds, int)
    return {"indexes": [{"fields": ["created"], "expireAfterSeconds": seconds}]}


if __name__ == "__main__":
    from pymongo import MongoClient

    # Connect to the database
    client = MongoClient('mongodb://unispy:123456@172.17.0.1:27017/')
    # Select the database
    db = client['mydatabase']   

    # Select the collection
    collection = db['mycollection']

    # Insert a document
    document = {'name': 'John Doe', 'age': 30}
    collection.insert_one(document)

    # Find all documents
    documents = collection.find()
    for document in documents:
        print(document)

    # Update a document
    filter = {'name': 'John Doe'}
    update = {'$set': {'age': 31}}
    collection.update_one(filter, update)

    # Delete a document
    filter = {'name': 'John Doe'}
    collection.delete_one(filter)

    # Close the client
    client.close()
