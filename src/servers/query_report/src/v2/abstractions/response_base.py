import abc
import library.src.abstractions.contracts
from servers.query_report.src.v2.contracts.requests import RequestBase
from servers.query_report.src.v2.contracts.results import ResultBase


class ResponseBase(library.src.abstractions.contracts.ResponseBase, abc.ABC):
    _result: ResultBase
    _request: RequestBase
