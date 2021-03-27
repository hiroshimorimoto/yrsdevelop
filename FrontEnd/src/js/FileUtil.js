import axios from 'axios'

var FileUtil = {
};

FileUtil.ShareName = "yrs-files";

FileUtil.Upload = async function (folderPath, file) {
    return new Promise((resolve, reject) => {
        const url = process.env.VUE_APP_API_BASE_URL + "/api/File/Upload";

        const params = new FormData();
        params.append('shareName', FileUtil.ShareName);
        params.append('folderPath', folderPath);
        params.append('files', file);

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

export default FileUtil;