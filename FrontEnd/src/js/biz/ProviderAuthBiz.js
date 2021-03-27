import ApiUtil from '@js/ApiUtil';
import BizMethodInfo from "@js/dto/BizMethodInfo.js";

export default class ProviderAuthBiz {
    constructor() {
    }

    static get BIZ_NAME() { return "ProviderAuth"; }

    static async CheckLogin() {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("CheckLogin");

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderAuthBiz.BIZ_NAME, bmi);
    }

    static async Login(providerId, loginpassword) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("Login");
        bmi.AddStringParam("providerId", providerId);
        bmi.AddStringParam("loginpassword", loginpassword);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderAuthBiz.BIZ_NAME, bmi);
    }

    static async Logout(loginpassword) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("Logout");

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderAuthBiz.BIZ_NAME, bmi);
    }

    static async ChangePassword(providerId, oldPassword, newPassword, newPasswordConf) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("ChangePassword");
        bmi.AddStringParam("providerId", providerId);
        bmi.AddStringParam("oldPassword", oldPassword);
        bmi.AddStringParam("newPassword", newPassword);
        bmi.AddStringParam("newPasswordConf", newPasswordConf);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderAuthBiz.BIZ_NAME, bmi);
    }

    static async ResetPasswordRequest(providerId, authCode) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("ResetPasswordRequest");
        bmi.AddStringParam("providerId", providerId);
        bmi.AddStringParam("authCode", authCode);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderAuthBiz.BIZ_NAME, bmi);
    }

    static async ResetPasswordConfirm(resetPasswordConfirmEt) {
        // //Bizメソッドパラメータ
        // var bmi = new BizMethodInfo("ResetPasswordConfirm");
        // bmi.AddObjectParam("ResetPasswordConfirmEt", resetPasswordConfirmEt);

        // //Bizメソッド APIコール
        // return ApiUtil.callApi_BizMethodInfo(ProviderAuthBiz.BIZ_NAME, bmi);

        //Bizメソッド APIコール
        return ApiUtil.callApi_MethodName(ProviderAuthBiz.BIZ_NAME, "ResetPasswordConfirm", resetPasswordConfirmEt);
    }
}