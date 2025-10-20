import logging
from logging.handlers import TimedRotatingFileHandler
import os

from frontends.gamespy.library.configs import CONFIG


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


class ColoredFormatter(logging.Formatter):
    COLORS = {
        "DEBUG": "\033[94m",  # Blue
        "INFO": "\033[92m",  # Green
        "WARNING": "\033[93m",  # Yellow
        "ERROR": "\033[91m",  # Red
        "CRITICAL": "\033[41m",  # White text on Red background
    }
    RESET = "\033[0m"  # Reset to default color

    def format(self, record):
        # Get the color for the levelname
        color = self.COLORS.get(record.levelname, self.RESET)
        # Format the levelname with color
        record.levelname = f"{color}{record.levelname}{self.RESET}"
        return super().format(record)


class LogManager:
    @staticmethod
    def create(logger_name: str) -> "LogWriter":
        log_file_path = CONFIG.logging.path
        create_dir(log_file_path)
        file_name = f"{log_file_path}/{logger_name}.log"
        if CONFIG.logging.min_log_level == "debug":
            log_level = logging.DEBUG
        else:
            log_level = logging.INFO

        logging.basicConfig(
            filename=file_name,
            level=log_level,
            format=f"%(asctime)s [{logger_name}] [%(levelname)s]: %(message)s",
            datefmt="%Y-%m-%d %H:%M:%S",
        )
        # 滚动日志文件
        file_handler = TimedRotatingFileHandler(
            file_name,
            when="midnight",
            interval=1,
            backupCount=7,
            encoding="utf-8",
        )
        formatter = ColoredFormatter(
            f"%(asctime)s [{logger_name}] [%(levelname)s]: %(message)s",
            datefmt="%Y-%m-%d %H:%M:%S",
        )
        file_handler.setFormatter(formatter)

        # create console log handler
        console_handler = logging.StreamHandler()
        console_handler.setFormatter(formatter)
        # create logger
        logger = logging.getLogger(logger_name)
        logger.addHandler(file_handler)
        logger.addHandler(console_handler)
        return LogWriter(logger)

    @staticmethod
    def logger_exists(name) -> bool:
        logger = logging.getLogger(name)
        is_exist = len(logger.handlers) > 0
        return is_exist


GLOBAL_LOGGER = LogManager.create("unispy")
"""
the global logger of unispy
"""
