from fastapi import APIRouter

from backends.library.abstractions.contracts import RESPONSES_DEF, OKResponse
from backends.protocols.gamespy.web_services.handlers import CreateRecordHandler, CreateUserAccountHandler, DeleteRecordHandler, GetMyRecordsHandler, LoginProfileHandler, LoginRemoteAuthHandler, LoginUniqueNickHandler, SearchForRecordsHandler, UpdateRecordHandler
from backends.protocols.gamespy.web_services.responses import CreateRecordResponse, DeleteRecordResponse, GetMyRecordsResponse, LoginProfileResponse, LoginRemoteAuthRepsonse, LoginUniqueNickResponse, SearchForRecordsResponse, UpdateRecordResponse
from backends.urls import WEB_SERVICES
from backends.protocols.gamespy.web_services.requests import CreateRecordRequest, CreateUserAccountRequest, DeleteRecordRequest, GetMyRecordsRequest, LoginProfileRequest,  LoginRemoteAuthRequest,  LoginUniqueNickRequest,  SearchForRecordsRequest, UpdateRecordRequest

router = APIRouter()

# Altas services


# @router.post(f"{WEB_SERVICES}/CreateRecordHandler", responses=RESPONSES_DEF)
# def create_matchless_session(request):
#     raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/CreateSessionHandler", responses=RESPONSES_DEF)
def create_session(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/SetReportIntentionHandler", responses=RESPONSES_DEF)
def set_report_intention(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/SubmitReportHandler", responses=RESPONSES_DEF)
def submit_report(request):
    raise NotImplementedError()


# Auth services
@router.post(f"{WEB_SERVICES}/LoginProfileHandler", responses=RESPONSES_DEF)
def login_profile(request: LoginProfileRequest) -> LoginProfileResponse:
    handler = LoginProfileHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/LoginProfileWithGameIdHandler", responses=RESPONSES_DEF)
def login_profile_with_game_id(request: LoginProfileRequest) -> LoginProfileResponse:
    return login_profile(request)


@router.post(f"{WEB_SERVICES}/LoginRemoteAuthHandler", responses=RESPONSES_DEF)
def login_remote_auth(request: LoginRemoteAuthRequest) -> LoginRemoteAuthRepsonse:
    handler = LoginRemoteAuthHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/LoginRemoteAuthWithGameIdHandler", responses=RESPONSES_DEF)
def login_remote_auth_with_game_id(request: LoginRemoteAuthRequest) -> LoginRemoteAuthRepsonse:
    return login_remote_auth(request)


@router.post(f"{WEB_SERVICES}/LoginUniqueNickHandler", responses=RESPONSES_DEF)
def login_uniquenick(request: LoginUniqueNickRequest) -> LoginUniqueNickResponse:
    handler = LoginUniqueNickHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/LoginUniqueNickWithGameIdHandler", responses=RESPONSES_DEF)
def login_uniquenick_with_game_id(request: LoginUniqueNickRequest) -> LoginUniqueNickResponse:
    return login_uniquenick(request)


@router.post(f"{WEB_SERVICES}/CreateUserAccountHandler", responses=RESPONSES_DEF)
def create_user_account(request: CreateUserAccountRequest):
    handler = CreateUserAccountHandler(request)
    handler.handle()
    return handler.response

# SAKE services


@router.post(f"{WEB_SERVICES}/CreateRecordHandler", responses=RESPONSES_DEF)
def create_record(request: CreateRecordRequest) -> CreateRecordResponse:
    handler = CreateRecordHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/UpdateRecordHandler", responses=RESPONSES_DEF)
def update_record(request: UpdateRecordRequest) -> UpdateRecordResponse:
    handler = UpdateRecordHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/DeleteRecordHandler", responses=RESPONSES_DEF)
def delete_record(request: DeleteRecordRequest) -> DeleteRecordResponse:
    handler = DeleteRecordHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/GetMyRecordsHandler", responses=RESPONSES_DEF)
def get_my_records(request: GetMyRecordsRequest) -> GetMyRecordsResponse:
    handler = GetMyRecordsHandler(request)
    handler.handle()
    return handler.response


@router.post(f"{WEB_SERVICES}/GetRandomRecordsHandler", responses=RESPONSES_DEF)
def get_random_records(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/GetRecordLimitHandler", responses=RESPONSES_DEF)
def get_record_limit(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/RateRecordHandler", responses=RESPONSES_DEF)
def rate_record(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/SearchForRecordsHandler", responses=RESPONSES_DEF)
def search_for_records(request: SearchForRecordsRequest) -> SearchForRecordsResponse:
    handler = SearchForRecordsHandler(request)
    handler.handle()
    return handler.response


if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI
    app = FastAPI()
    app.include_router(router)
    uvicorn.run(app, host="0.0.0.0", port=8080)
