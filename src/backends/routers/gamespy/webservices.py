from fastapi import APIRouter

from backends.protocols.gamespy.web_services.handlers import CreateRecordHandler, GetMyRecordsHandler, LoginProfileHandler, LoginRemoteAuthHandler, LoginUniqueNickHandler, SearchForRecordsHandler
from backends.urls import WEB_SERVICES
from backends.protocols.gamespy.web_services.requests import CreateRecordRequest, GetMyRecordsRequest, LoginProfileRequest,  LoginRemoteAuthRequest,  LoginUniqueNickRequest,  SearchForRecordsRequest

router = APIRouter()

# Altas services


@router.post(f"{WEB_SERVICES}/CreateRecordHandler")
def create_matchless_session(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/CreateSessionHandler")
def create_session(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/SetReportIntentionHandler")
def set_report_intention(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/SubmitReportHandler")
def submit_report(request):
    raise NotImplementedError()


# Auth services
@router.post(f"{WEB_SERVICES}/LoginProfileHandler")
def login_profile(request: LoginProfileRequest):
    handler = LoginProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/LoginProfileWithGameIdHandler")
def login_profile_with_game_id(request: LoginProfileRequest):
    return login_profile(request)


@router.post(f"{WEB_SERVICES}/LoginRemoteAuthHandler")
def login_remote_auth(request: LoginRemoteAuthRequest):
    handler = LoginRemoteAuthHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/LoginRemoteAuthWithGameIdHandler")
def login_remote_auth_with_game_id(request: LoginRemoteAuthRequest):
    return login_remote_auth(request)


@router.post(f"{WEB_SERVICES}/LoginUniqueNickHandler")
def login_uniquenick(request: LoginUniqueNickRequest):
    handler = LoginUniqueNickHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/LoginUniqueNickWithGameIdHandler")
def login_uniquenick_with_game_id(request: LoginUniqueNickRequest):
    return login_uniquenick(request)


# SAKE services
@router.post(f"{WEB_SERVICES}/CreateRecordHandler")
def create_record(request: CreateRecordRequest):
    handler = CreateRecordHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/DeleteRecordHandler")
def delete_record(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/GetMyRecordsHandler")
def get_my_records(request: GetMyRecordsRequest):
    handler = GetMyRecordsHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/GetRandomRecordsHandler")
def get_random_records(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/GetRecordLimitHandler")
def get_record_limit(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/RateRecordHandler")
def rate_record(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/SearchForRecordsHandler")
def search_for_records(request: SearchForRecordsRequest):
    handler = SearchForRecordsHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/UpdateRecordHandler")
def update_record(request):
    raise NotImplementedError()


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
