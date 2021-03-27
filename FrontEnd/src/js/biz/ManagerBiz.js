import ApiUtil from '@js/ApiUtil';
import BizMethodInfo from "@js/dto/BizMethodInfo.js";
import ProviderPostEt from "@js/dto/ProviderPostEt.js";

export default class ManagerBiz {
    constructor() {
    }

    static get BIZ_NAME() { return "Manager"; }

    static async GetProvider(providerId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetProvider");
        bmi.AddStringParam("providerId", providerId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ManagerBiz.BIZ_NAME, bmi);
    }

    static async GetProviderList(searchEt) {
        //Bizメソッド APIコール
        return ApiUtil.callApi_MethodName(ManagerBiz.BIZ_NAME, "GetProviderList", searchEt);
    }

    static async PostProvider(/*string*/ editMode, /*bool*/ isSendRegistMail, /*Provider_M*/ providerEt) {
        var postEt = new ProviderPostEt();

        postEt.EditMode = editMode;
        postEt.IsSendRegistMail = isSendRegistMail;
        postEt.ProviderEt = providerEt

        //Bizメソッド APIコール
        return ApiUtil.callApi_MethodName(ManagerBiz.BIZ_NAME, "PostProvider", postEt);
    }

    static async SendRegistMail(providerId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("SendRegistMail");
        bmi.AddStringParam("providerId", providerId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ManagerBiz.BIZ_NAME, bmi);

    }
}
