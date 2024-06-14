from flask import Flask, request
from backends.urls import *

app = Flask(__name__)


@app.route(f"/GetNatNegotiationInfo", methods=["POST"])
async def get_natneg_info():
    data = request.json
    