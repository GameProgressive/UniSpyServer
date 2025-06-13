import threading
import time
from typing import Callable

import schedule


class Schedular:
    _job_func: Callable
    _is_started: bool
    _interval: int

    def __init__(self, job_func: Callable, interval: int) -> None:
        self._job_func = job_func
        self._interval = interval
        schedule.every(interval).seconds.do(job_func)
        self._is_started = False

    def start(self):
        scheduler_thread = threading.Thread(target=self._run_schedule)
        scheduler_thread.daemon = True
        scheduler_thread.start()

    def _run_schedule(self):
        self._is_started = True
        while True:
            if not self._is_started:
                break
            schedule.run_pending()
            time.sleep(self._interval)

    def stop(self):
        self._is_started = False
