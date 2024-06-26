from servers.webservices.modules.sake.abstractions.contracts import (
    RequestBase,
    NAMESPACE,
)
import xmltodict

from servers.webservices.modules.sake.exceptions.general import SakeException


class CreateRecordRequest(RequestBase):
    values: dict

    def parse(self) -> None:
        super().parse()
        value_node = self._content_element.find(f".//{{{NAMESPACE}}}values")
        if value_node is None:
            raise SakeException("values is missing from request")

        self.values = xmltodict.parse(value_node)


class DeleteRecordRequest(RequestBase):
    record_id: int

    def parse(self) -> None:
        super().parse()
        record_id = self._content_element.find(f".//{{{NAMESPACE}}}recordid")

        if record_id is None:
            raise SakeException("recordid is missing from request")

        self.record_id = int(record_id)


class GetMyRecordRequest(RequestBase):
    fields: dict

    def parse(self) -> None:
        super().parse()
        fields_node = self._content_element.find(f".//{{{NAMESPACE}}}fields")
        if fields_node is None:
            raise SakeException("fields is missing from request")
        self.fields = xmltodict.parse(fields_node)


class GetRandomRecordsRequest(RequestBase):
    max: str
    fields: dict

    def parse(self) -> None:
        super().parse()
        max = self._content_element.find(f".//{{{NAMESPACE}}}max")
        if max is None:
            raise SakeException("max is missing from request")
        self.max = int(max)

        fields_node = self._content_element.find(f".//{{{NAMESPACE}}}fields")
        if fields_node is None:
            raise SakeException("fields is missing from request")
        self.fields = xmltodict.parse(fields_node)


class GetRecordLimitRequest(RequestBase):
    pass