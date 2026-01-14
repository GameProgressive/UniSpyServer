from pydantic import BaseModel

from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.enums import SakeRecordType


class RecordContext(BaseModel):
    name: str
    value: object
    type: SakeRecordType


class RecordConverter:

    @staticmethod
    def to_searchable_format(record: list[dict]) -> dict:
        searchable_kv = {}
        for r in record:
            name = r["name"]
            type = list(r['value'].keys())[0]
            value = r['value'][type]['value']
            s_type = SakeRecordType(type)
            if s_type in [SakeRecordType.INT, SakeRecordType.INT64, SakeRecordType.SHORT]:
                value = int(value)
            elif s_type in [SakeRecordType.FLOAT]:
                value = float(value)
            elif s_type in [SakeRecordType.BOOL]:
                value = bool(value)
            elif s_type in [SakeRecordType.UNICODE, SakeRecordType.ASCII, SakeRecordType.BINARY]:
                value = str(value)
            elif s_type in [SakeRecordType.DATE]:
                # todo not implemented
                value = str(value)
            else:
                GLOBAL_LOGGER.warn(f"sake value type unkown {type}")

            searchable_kv[name] = {"value": value, "type": type}
        return searchable_kv

    @staticmethod
    def to_gamespy_format(searchable_kv: dict) -> list[dict]:
        records = []
        for key in searchable_kv:
            name = key
            type = searchable_kv[key]["type"]
            value = searchable_kv[key]["value"]
            record = {"name": name, "value": {type: {"value": str(value)}}}
            records.append(record)
        return records


if __name__ == "__main__":
    records = [{"name": "MyAsciiString", "value": {
        "asciiStringValue": {"value": "this is a record"}}}]
    values = RecordConverter.to_searchable_format(records)
    records2 = RecordConverter.to_gamespy_format(values)
