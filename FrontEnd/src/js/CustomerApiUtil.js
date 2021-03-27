import $ from 'jquery'

var CustomerApiUtil = {
    _ajax: async function (subUrl, type, data, contentType) {
        return new Promise((resolve, reject) => {
            const url = process.env.VUE_APP_API_BASE_URL + subUrl;
            $.ajax({
                url: url,
                type: type,
                data: data ? data : null,
                contentType: contentType ? contentType : null
            })
                .done((data, status, xhr) => {
                    if (xhr.status == "200") {
                        const apiResult = data;
                        if (apiResult.hasError) {
                            reject(apiResult.errorMessage);
                        } else {
                            const resultData = apiResult.resultData;
                            var obj = JSON.parse(resultData);
                            resolve(obj);
                        }
                    } else {
                        reject(data.errorMessage);
                    }
                })
                .fail((jqXHR, textStatus, errorThrown) => {
                    const res = jqXHR.responseJSON;
                    if (res && res.hasError) {
                        reject(res.errorMessage);
                    } else {
                        //システムエラー
                        reject("エラーが発生しました。ステータス：" + jqXHR.status);
                    }
                });
        });
    }
};

CustomerApiUtil.GetCategoryList = async function () {
    return await this._ajax("/jobj/categories", "GET", null);
}

CustomerApiUtil.GetPlanList = async function () {
    return await this._ajax("/jobj/plans", "GET", null);
}

CustomerApiUtil.GetPlanModel = async function (planId) {
    return await this._ajax("/jobj/plans/" + planId, "GET", null);
}

CustomerApiUtil.GetPrivacyPolicyFromPlanId = async function (planId) {
    return await this._ajax("/jobj/plans/" + planId + "/pp", "GET", null);
}


CustomerApiUtil.FindPlanList = async function (searchEt) {
    var data = JSON.stringify(searchEt);
    return await this._ajax("/jobj/plans", "POST", data, 'application/json');
}

CustomerApiUtil.GetNewAppEditModel = async function (appEditModelRequest) {
    var data = JSON.stringify(appEditModelRequest);
    return await this._ajax("/jobj/apps", "POST", data, 'application/json');
}

CustomerApiUtil.GetPayCompAppEditModel = async function (applicationId) {
    var data = JSON.stringify({ "applicationId": applicationId });
    return await this._ajax("/jobj/apps/" + applicationId, "GET", data, 'application/json');
}


CustomerApiUtil.PutAppEditModel = async function (appEditModel) {
    var data = JSON.stringify(appEditModel);
    return await this._ajax("/jobj/apps", "PUT", data, 'application/json');
    //return await this._ajax("/jobj/app", "POST", data, 'application/json');
}

export default CustomerApiUtil;