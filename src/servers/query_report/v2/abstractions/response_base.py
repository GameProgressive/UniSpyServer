import abc
import library.abstractions.contracts
from servers.query_report.v2.contracts.requests import RequestBase
from servers.query_report.v2.contracts.results import ResultBase


class ResponseBase(library.abstractions.contracts.ResponseBase, abc.ABC):
    _result: ResultBase
    _request: RequestBase
