import logging
from logging.handlers import TimedRotatingFileHandler
import os


class LogWriter:
    original_logger: logging.Logger

    def __init__(self, logger) -> None:
        self.original_logger = logger

    def debug(self, message: str):
        self.original_logger.debug(message)

    def info(self, message: str):
        self.original_logger.info(message)

    def error(self, message: str):
        self.original_logger.error(message)

    def warn(self, message: str):
        self.original_logger.warn(message)


def create_dir(path):
    """
    创建对应目录,如果该目录不存在
    """
    log_path = os.path.dirname(path)
    if not os.path.exists(log_path):
        os.makedirs(log_path)


class LogManager:

    @staticmethod
    def create(log_file_path: str, logger_name: str) -> "LogWriter":
        create_dir(log_file_path)

        logging.basicConfig(
            filename=log_file_path,
            level=logging.INFO,
            format=f"%(asctime)s [{logger_name}] [%(levelname)s]: %(message)s",
            datefmt="%Y-%m-%d %H:%M:%S",
        )
        # 滚动日志文件
        file_handler = TimedRotatingFileHandler(
            log_file_path,
            when="midnight",
            interval=1,
            backupCount=7,
            encoding="utf-8",
        )
        formatter = logging.Formatter(
            f"%(asctime)s [{logger_name}] [%(levelname)s]: %(message)s",
            datefmt="%Y-%m-%d %H:%M:%S",
        )
        file_handler.setLevel(
            logging.DEBUG
        )  # Set the desired log level for the console
        file_handler.setFormatter(formatter)

        # 控制台日志输出
        console_handler = logging.StreamHandler()
        console_handler.setLevel(logging.DEBUG)
        console_handler.setFormatter(formatter)

        logger = logging.getLogger(log_file_path)
        logger.addHandler(file_handler)
        logger.addHandler(console_handler)
        return LogWriter(logger)
