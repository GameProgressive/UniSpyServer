import argparse
from multiprocessing import Process
import os
import time
from typing import TYPE_CHECKING, Callable
from watchdog.events import FileSystemEventHandler
from watchdog.observers import Observer
if TYPE_CHECKING:
    from frontends.gamespy.library.abstractions.server_launcher import ServerLauncherBase


class FileChangeHandler(FileSystemEventHandler):
    process: Process | None
    monitor_path: str
    launch_func: Callable

    def __init__(self, folder_path: str, launch_func: Callable):
        assert isinstance(folder_path, str)
        super().__init__()
        self.process = None
        self.monitor_path = os.path.abspath(folder_path)
        self.launch_func = launch_func
        self.__start_process()

    def __start_process(self):
        if self.process is not None:
            self.process.terminate()
        self.process = Process(target=self.launch_func)
        self.process.start()

    def on_modified(self, event):
        assert isinstance(event.src_path, str)
        abs_event_path = os.path.abspath(event.src_path)
        if abs_event_path.endswith('.py'):
            common_path = os.path.commonpath(
                [abs_event_path, self.monitor_path])
            if common_path == self.monitor_path:
                print(
                    f"File {event.src_path} has been modified. Restarting process...")
                self.__start_process()


class DebugHelper:
    _observer: object
    _folder_path: str
    _launch_cls: type["ServerLauncherBase"]

    def __init__(self, folder_path: str, launch_cls: type["ServerLauncherBase"]) -> None:
        self._folder_path = folder_path
        self._launch_cls = launch_cls

    def start(self):
        # Initialize the ArgumentParser
        parser = argparse.ArgumentParser(
            description='Example script to demonstrate argparse with debug mode.')

        # Add a debug argument
        parser.add_argument(
            '--debug',
            action='store_true',
            help='Enable debugging mode.'
        )

        # Parse the arguments
        args = parser.parse_args()

        # Check if debug mode is enabled
        if args.debug:
            print(
                "\033[93mUniSpy is starting with watchdog, any changes in code will restart the server\033[0m")
            self.__start_with_watch()
        else:
            self.__start_normal()

    def __start_with_watch(self):
        event_handler = FileChangeHandler(
            self._folder_path, self.__start_normal)
        self._observer = Observer()
        self._observer.schedule(
            event_handler, self._folder_path, recursive=True)
        try:
            self._observer.start()
            while True:
                time.sleep(1)
        except:
            self._observer.stop()

    def __start_normal(self):
        target = self._launch_cls()  # type: ignore
        target.start()  # type: ignore
