import ApiUtil from '@js/ApiUtil';
import BizMethodInfo from "@js/dto/BizMethodInfo.js";

export default class ManagerAuthBiz {
    constructor() {
    }

    static get BIZ_NAME() { return "ManagerAuth"; }

    static async CheckLogin() {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("CheckLogin");

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ManagerAuthBiz.BIZ_NAME, bmi);
    }

    static async Login(loginpassword) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("Login");
        bmi.AddStringParam("loginpassword", loginpassword);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ManagerAuthBiz.BIZ_NAME, bmi);
    }

    static async Logout() {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("Logout");

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ManagerAuthBiz.BIZ_NAME, bmi);
    }

    static async ChangePassword(oldPassword, newPassword, newPasswordConf) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("ChangePassword");
        bmi.AddStringParam("oldPassword", oldPassword);
        bmi.AddStringParam("newPassword", newPassword);
        bmi.AddStringParam("newPasswordConf", newPasswordConf);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ManagerAuthBiz.BIZ_NAME, bmi);
    }

    static async ResetPasswordRequest(authCode) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("ResetPasswordRequest");
        bmi.AddStringParam("authCode", authCode);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ManagerAuthBiz.BIZ_NAME, bmi);
    }

    static async ResetPasswordConfirm(resetPasswordConfirmEt) {
        //Bizメソッド APIコール
        return ApiUtil.callApi_MethodName(ManagerAuthBiz.BIZ_NAME, "ResetPasswordConfirm", resetPasswordConfirmEt);
    }
}