from fastapi import FastAPI

from library.src.unispy_server_config import ServerConfig
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


@app.post("/")
def home(request: ServerConfig):
    # todo add the server config to our database
    return {"status": "online"}
