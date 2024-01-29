// Xử lý server response
const res = {
    successCode: {
        SubmissionSaved: 201,
        ContestProblemAccountSaved: 301,
    },
    errorCode: {},
    /**
     * Thành công hay ko
     * @param {*} res
     * @returns
     */
    isSuccess(res) {
        return res && res.Success;
    },
    /**
     * Có successCode k
     * @param {*} successCode
     * @param {*} res
     * @returns
     */
    hasSuccessCode(res, successCode) {
        if (res) {
            if (res.SuccessCode != null && res.SuccessCode == successCode) {
                return true;
            }

            if (
                res.MoreData &&
                res.MoreData.some(
                    (md) =>
                        md.SuccessCode != null && md.SuccessCode == successCode
                )
            ) {
                return true;
            }

            return false;
        }
        return false;
    },
    /**
     * lấy data theo successCode
     * @param {*} successCode
     * @param {*} res
     * @returns
     */
    getDataSuccessCode(res, successCode) {
        if (res) {
            if (res.SuccessCode != null && res.SuccessCode == successCode) {
                return res.Data;
            }

            if (res.MoreData) {
                let item = res.MoreData.find(item => item.SuccessCode == successCode);
                return item != null ? item.Data : item;
            }
        }
        return null;
    },
};
export default res;