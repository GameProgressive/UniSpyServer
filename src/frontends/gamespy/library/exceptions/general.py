import traceback
from typing import TYPE_CHECKING, Optional, cast

from pydantic import BaseModel

from frontends.gamespy.library.abstractions.contracts import ResponseBase
from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.log.log_manager import GLOBAL_LOGGER

if TYPE_CHECKING:
    from frontends.gamespy.library.abstractions.client import ClientBase


class UniSpyExceptionValidator(BaseModel):
    """
    The unispy exception validator
    convert http exception data to correct format
    """
    message: str


class UniSpyException(Exception):
    message: str
    """the error message"""

    _validator: UniSpyExceptionValidator

    def __init__(self, message: str) -> None:
        self.message = message

    @staticmethod
    def handle_exception(e: Exception, client: Optional["ClientBase"] = None):
        # first log the exception
        if client is None:
            GLOBAL_LOGGER.info(str(e))
        else:
            ex_type = type(e)
            # first we check if it is a ResponseBase
            if issubclass(ex_type, ResponseBase):
                ex: UniSpyException = e  # type:ignore
                client.log_error(ex.message)
                exp_resp = e
                if TYPE_CHECKING:
                    exp_resp = cast(ResponseBase, e)
                client.send(exp_resp)
            elif issubclass(ex_type, UniSpyException):
                ex: UniSpyException = e  # type:ignore
                client.log_error(ex.message)
            elif issubclass(ex_type, BrokenPipeError):
                client.log_warn(f"client disconnect before message send")

            else:
                client.log_error(traceback.format_exc())
        # if we are unittesting we raise the exception out
        if CONFIG.unittest.is_raise_except:
            raise e

    def __repr__(self) -> str:
        # return super().__repr__()
        return f'Error message: "{self.message}"'

    @staticmethod
    def get_validator(exp_cls: type["UniSpyException"]) -> UniSpyExceptionValidator:
        annotations = {}
        for exp_cls in exp_cls.mro():
            # Update annotations if they exist in the current class
            if issubclass(exp_cls, UniSpyException):
                for key, value in exp_cls.__annotations__.items():
                    if key not in annotations:
                        if "key" in annotations:
                            continue
                        annotations[key] = value
        if "_validator" not in annotations:
            raise UniSpyException(
                f"validator not found in exception class: {exp_cls.__name__}")

        return annotations['_validator']

    @staticmethod
    def get_init_params(exp_cls: type["UniSpyException"], data: dict) -> dict:
        validator_cls = exp_cls.get_validator(exp_cls)
        v_instance = validator_cls.model_validate(data)
        init_params = v_instance.model_dump()
        import inspect
        init_signature = inspect.signature(exp_cls.__init__)
        init_params_filterd = {key: value for key, value in init_params.items()
                               if key in init_signature.parameters}
        return init_params_filterd


class DatabaseConnectionException(UniSpyException):
    def __init__(self, message: str = "Can not connect to database.") -> None:
        super().__init__(message)


class RedisConnectionException(UniSpyException):
    def __init__(self, message: str = "Can not connect to redis") -> None:
        super().__init__(message)


def get_exceptions_dict(module_name: str) -> dict[str, type[UniSpyException]]:
    import inspect
    import sys
    modules = inspect.getmembers(sys.modules[module_name], inspect.isclass)
    exceptions_dict = {}
    for exc_class in modules:
        class_name, class_type = exc_class
        if "Exception" in class_name:
            exceptions_dict[class_name] = class_type
    return exceptions_dict


if __name__ == "__main__":
    err = UniSpyException("test")
    pass
