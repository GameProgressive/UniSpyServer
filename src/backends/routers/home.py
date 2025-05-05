from ipaddress import IPv4Address
from uuid import UUID
from fastapi import FastAPI
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse
from pydantic import BaseModel
import uvicorn

from frontends.gamespy.library.log.log_manager import LogManager
from frontends.gamespy.library.configs import ServerConfig
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

logger = LogManager.create("backend")


@app.exception_handler(RequestValidationError)
async def validation_exception_handler(request, exc):
    str_error = str(exc)
    logger.error(str_error)
    return JSONResponse({"error": str_error})


@app.exception_handler(Exception)
async def handle_unispy_exception(request, exc):
    str_error = str(exc)
    logger.error(str_error)
    return JSONResponse({"error": str_error})


@app.post("/")
async def home(request: ServerConfig) -> dict:
    # todo add the server config to our database
    return {"status": "online"}


class RegisterRequest(BaseModel):
    server_id: UUID
    client_ip: IPv4Address


@app.post("/token")
async def get_auth_token(request: RegisterRequest):

    pass
if __name__ == "__main__":
    uvicorn.run("backends.routers.home:app",
                host="127.0.0.1", port=8080, reload=True)
