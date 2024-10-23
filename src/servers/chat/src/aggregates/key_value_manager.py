class KeyValueManager:
    data: dict
    """
    store the key and values
    """

    def __init__(self):
        self.data = {}

    def update(self, data: dict):
        for key, value in data.items():
            self.data[key] = value

    def build_key_value_string(self, key_values: dict):
        flags = ""
        for key, value in key_values.items():
            flags += f"\\{key}\\{value}"
        return flags

    def get_value_string(self, keys: list[str]) -> str:
        values = ""
        for key in keys:
            if key in self.data:
                values += f"\\{self.data[key]}"
            else:
                values += "\\"
                # Uncomment the line below to raise an exception if key is not found
                # raise Exception(f"Can not find key: {key}")
        return values

    def is_contain_all_key(self, keys: list[str]):
        return all(key in self.data for key in keys)
