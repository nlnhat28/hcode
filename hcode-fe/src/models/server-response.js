class serverResponse {
    constructor(success, errorCode, data, devMsg, userMsg, moreInfo) {
        this.success = success;
        this.errorCode = errorCode;
        this.data = data;
        this.devMsg = devMsg;
        this.userMsg = userMsg;
        this.moreInfo = moreInfo;
    }
}
