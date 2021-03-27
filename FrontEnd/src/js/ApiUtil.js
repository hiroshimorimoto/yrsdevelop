import axios from 'axios'

var ApiUtil = {
};


ApiUtil.callApi_BizMethodInfo = async function (bizName, bizMethodInfo) {
    return new Promise((resolve, reject) => {
        const url = process.env.VUE_APP_API_BASE_URL + "/api/" + bizName;
        axios
            .post(url, bizMethodInfo
                // , {
                //   headers: headers
                // }
            )
            .then(res => {
                if (res.status == "200") {
                    const apiResult = res.data;
                    if (apiResult.hasError) {
                        reject(apiResult.errorMessage);
                    } else {
                        const resultData = apiResult.resultData;
                        var obj = JSON.parse(resultData);
                        resolve(obj);
                    }
                } else {
                    reject(res.errorMessage);
                }
            })
            .catch(err => {
                const res = err.response;
                if (res && res.data && res.data.hasError) {
                    reject(res.data.errorMessage);
                } else {
                    //システムエラー
                    reject(err);
                }
            });
    });
}

ApiUtil.callApi_MethodName = async function (bizName, methodName, paramObj) {
    return new Promise((resolve, reject) => {
        const url = process.env.VUE_APP_API_BASE_URL + "/api/" + bizName + "/" + methodName;
        axios
            .post(url, paramObj
                // , {
                //   headers: headers
                // }
            )
            .then(res => {
                if (res.status == "200") {
                    const apiResult = res.data;
                    if (apiResult.hasError) {
                        reject(apiResult.errorMessage);
                    } else {
                        const resultData = apiResult.resultData;
                        var obj = JSON.parse(resultData);
                        resolve(obj);
                    }
                } else {
                    reject(res.errorMessage);
                }
            })
            .catch(err => {
                const res = err.response;
                if (res && res.data && res.data.hasError) {
                    reject(res.data.errorMessage);
                } else {
                    //システムエラー
                    reject(err);
                }
            });
    });
}

ApiUtil.callApi_BizMethodInfo_Files = async function (bizName, bizMethodInfo, files) {
    return new Promise((resolve, reject) => {
        const url = process.env.VUE_APP_API_BASE_URL + "/api/form/" + bizName;

        const params = new FormData();
        params.append('jsonData', JSON.stringify(bizMethodInfo));
        params.append('files', files);

        axios
            .post(url, params,
                {
                    headers: {
                        'content-type': 'multipart/form-data',
                    },
                })
            .then(res => {
                if (res.status == "200") {
                    const apiResult = res.data;
                    if (apiResult.hasError) {
                        reject(apiResult.errorMessage);
                    } else {
                        const resultData = apiResult.resultData;
                        var obj = JSON.parse(resultData);
                        resolve(obj);
                    }
                } else {
                    reject(res.errorMessage);
                }
            })
            .catch(err => {
                const res = err.response;
                if (res && res.data && res.data.hasError) {
                    reject(res.data.errorMessage);
                } else {
                    //システムエラー
                    reject(err);
                }
            });
    });
}

ApiUtil.callApi_MethodName_Files = async function (bizName, methodName, paramObj, files) {
    return new Promise((resolve, reject) => {
        const url = process.env.VUE_APP_API_BASE_URL + "/api/form/" + bizName + "/" + methodName;

        const params = new FormData();
        params.append('jsonData', JSON.stringify(paramObj));

        if (Array.isArray(files)) {
            for (var file of files) {
                params.append('files', file);
            }
        } else {
            params.append('files', files);
        }

        axios
            .post(url, params,
                {
                    headers: {
                        'content-type': 'multipart/form-data',
                    },
                })
            .then(res => {
                if (res.status == "200") {
                    const apiResult = res.data;
                    if (apiResult.hasError) {
                        reject(apiResult.errorMessage);
                    } else {
                        const resultData = apiResult.resultData;
                        var obj = JSON.parse(resultData);
                        resolve(obj);
                    }
                } else {
                    reject(res.errorMessage);
                }
            })
            .catch(err => {
                const res = err.response;
                if (res && res.data && res.data.hasError) {
                    reject(res.data.errorMessage);
                } else {
                    //システムエラー
                    reject(err);
                }
            });
    });
}

export default ApiUtil;