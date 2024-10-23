from fastapi import APIRouter

from backends.urls import WEB_SERVICES
from backends.protocols.gamespy.web_services.requests import CreateRecordRequest, GetMyRecordsRequest, LoginProfileRequest, LoginProfileWithGameIdRequest, LoginRemoteAuthRequest, LoginRemoteAuthWithGameIdRequest, LoginUniqueNickRequest, LoginUniqueNickWithGameIdRequest, SearchForRecordsRequest

router = APIRouter()

# Altas services
@router.post(f"{WEB_SERVICES}/Altas/CreateRecordHandler")
async def create_matchless_session(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Altas/CreateSessionHandler")
async def create_session(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Altas/SetReportIntentionHandler")
async def set_report_intention(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Altas/SubmitReportHandler")
async def submit_report(request):
    raise NotImplementedError()


# Auth services
@router.post(f"{WEB_SERVICES}/Auth/LoginProfileHandler")
async def login_profile(request: LoginProfileRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Auth/LoginProfileWithGameIdHandler")
async def login_profile_with_game_id(request: LoginProfileWithGameIdRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Auth/LoginRemoteAuthHandler")
async def login_remote_auth(request: LoginRemoteAuthRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Auth/LoginRemoteAuthWithGameIdHandler")
async def login_remote_auth_with_game_id(request: LoginRemoteAuthWithGameIdRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Auth/LoginUniqueNickHandler")
async def login_uniquenick(request: LoginUniqueNickRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Auth/LoginUniqueNickWithGameIdHandler")
async def login_uniquenick_with_game_id(request: LoginUniqueNickWithGameIdRequest):
    raise NotImplementedError()


# SAKE services
@router.post(f"{WEB_SERVICES}/Sake/CreateRecordHandler")
async def create_record(request: CreateRecordRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Sake/DeleteRecordHandler")
async def delete_record(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Sake/GetMyRecordsHandler")
async def get_my_records(request: GetMyRecordsRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Sake/GetRandomRecordsHandler")
async def get_random_records(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Sake/GetRecordLimitHandler")
async def get_record_limit(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Sake/RateRecordHandler")
async def rate_record(request):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Sake/SearchForRecordsHandler")
async def search_for_records(request: SearchForRecordsRequest):
    raise NotImplementedError()


@router.post(f"{WEB_SERVICES}/Sake/UpdateRecordHandler")
async def update_record(request):
    raise NotImplementedError()

if __name__ == "__main__":
    import uvicorn
    from fastapi import FastAPI

    app = FastAPI()
    app.include_router(router)
    uvicorn.run(router, host="0.0.0.0", port=8000)
