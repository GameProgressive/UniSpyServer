import time
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

class FileWatcher:
    pass

class MyHandler(FileSystemEventHandler):
    def on_any_event(self, event):
        if event.is_directory:
            return None
        elif event.event_type == 'created':
            print(f"Created: {event.src_path}")
        elif event.event_type =='modified':
            print(f"Modified: {event.src_path}")


if __name__ == "__main__":
    event_handler = MyHandler()
    observer = Observer()
    path = '.'  # Monitor the current directory
    observer.schedule(event_handler, path, recursive=True)
    observer.start()
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
    observer.join()