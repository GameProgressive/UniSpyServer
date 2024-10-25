from fastapi import FastAPI
import uvicorn

from library.src.log.log_manager import LogManager
from library.src.configs import ServerConfig
from backends.routers.gamespy import chat, gstats, natneg, presence_connection_manager, presence_search_player, query_report, server_browser, webservices
app = FastAPI()

# app.include_router(chat.router)
app.include_router(gstats.router)
app.include_router(natneg.router)
app.include_router(presence_connection_manager.router)
app.include_router(presence_search_player.router)
app.include_router(query_report.router)
app.include_router(server_browser.router)
app.include_router(webservices.router)

LogManager.create("backend")


@app.post("/")
def home(request: ServerConfig):
    # todo add the server config to our database
    return {"status": "online"}


if __name__ == "__main__":
    uvicorn.run("backends.routers.home:app",
                host="127.0.0.1", port=8080, reload=True)
