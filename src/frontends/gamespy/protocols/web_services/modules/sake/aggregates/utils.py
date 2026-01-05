class RecordConverter:

    @staticmethod
    def to_searchable_format(record: list[dict]) -> tuple[dict, dict]:
        key_values = {}
        key_types = {}
        for r in record:
            value_type = list(r['value'].keys())[0]
            value = r['value'][value_type]['value']
            key = r["name"]
            key_values[key]= value
            key_types[key]= value_type
        return key_values, key_types

    @staticmethod
    def to_gamespy_format(key_values: dict, key_types: dict) -> list[dict]:
        assert len(key_values) == len(key_types)
        records = []
        for key in key_values:
            record = {"name": key, "value": {
                key_types[key]: {"value": key_values[key]}}}
            records.append(record)
        return records


if __name__ == "__main__":
    records = [{"name": "MyAsciiString", "value": {
        "asciiStringValue": {"value": "this is a record"}}}]
    values, types = RecordConverter.to_searchable_format(records)
    records2 = RecordConverter.to_gamespy_format(values, types)
