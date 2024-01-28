// Xử lý server response
const res = {
    successCode: {
        SubmissionSaved: 201,
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
                res.MoreData.some((md) => md.SuccessCode != null && md.SuccessCode == successCode)
            ) {
                return true;
            }

            return false;
        }
        return false;
    },
};
export default res;