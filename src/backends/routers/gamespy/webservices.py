from fastapi import FastAPI

from backends.urls import WEB_SERVICES

app = FastAPI()

# Altas services


@app.post(f"{WEB_SERVICES}/Altas/CreateRecordHandler")
def create_matchless_session(request):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Altas/CreateSessionHandler")
def create_session(request):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Altas/SetReportIntentionHandler")
def set_report_intention(request):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Altas/SubmitReportHandler")
def submit_report(request):
    raise NotImplementedError()


# Auth services
@app.post(f"{WEB_SERVICES}/Auth/LoginProfileHandler")
def submit_report(request: LoginProfileRequest):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Auth/LoginProfileWithGameIdHandler")
def submit_report(request:LoginProfileWithGameIdRequest):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Auth/LoginRemoteAuthHandler")
def submit_report(request:LoginRemoteAuthRequest):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Auth/LoginRemoteAuthWithGameIdHandler")
def submit_report(request: LoginRemoteAuthWithGameIdRequest):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Auth/LoginUniqueNickHandler")
def submit_report(request:LoginUniqueNickRequest):
    raise NotImplementedError()


@app.post(f"{WEB_SERVICES}/Auth/LoginUniqueNickWithGameIdHandler")
def submit_report(request:LoginUniqueNickWithGameIdRequest):
    raise NotImplementedError()


# SAKE services
@app.post(f"{WEB_SERVICES}/Sake/CreateRecordHandler")
def create_record(request):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
