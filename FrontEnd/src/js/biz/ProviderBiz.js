import ApiUtil from '@js/ApiUtil';
//import FileUtil from '@js/FileUtil';
import FileBiz from '@js/biz/FileBiz';

import BizMethodInfo from "@js/dto/BizMethodInfo.js";
import PlanModel from "@js/dto/PlanModel.js";

export default class ProviderBiz {
    constructor() {
    }

    static get BIZ_NAME() { return "Provider"; }

    static async GetPlan(planId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetPlan");
        bmi.AddNumberParam("planId", planId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }

    static async GetPlanList(searchEt) {
        //Bizメソッド APIコール
        return ApiUtil.callApi_MethodName(ProviderBiz.BIZ_NAME, "GetPlanList", searchEt);
    }


    static async GetPlanModel(planId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetPlanModel");
        bmi.AddNumberParam("planId", planId);

        //Bizメソッド APIコール
        var planModel = ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);

        return planModel;
    }

    static async PostPlan(/*PlanModel*/ planModel, files) {

        var ret = null;
        //イメージをBKUP
        //var selectedTopImageElement = planModel.SelectedTopImageElement;

        //モデルを登録
        planModel = await ApiUtil.callApi_MethodName_Files(ProviderBiz.BIZ_NAME, "PostPlan", planModel, files);
        //planModel.SelectedTopImageElement = selectedTopImageElement;

        return planModel;
    }


    static async PostPlanPublicStatus(planId, publicStatus) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("PostPlanPublicStatus");
        bmi.AddNumberParam("planId", planId);
        bmi.AddNumberParam("publicStatus", publicStatus);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }

    static async DeletePlan(planId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("DeletePlan");
        bmi.AddNumberParam("planId", planId);
        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }

    static async GetApplicationForProvider(applicationId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetApplicationForProvider");
        bmi.AddNumberParam("applicationId", applicationId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }

    static async GetApplicationListForProvider() {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetApplicationListForProvider");
        //bmi.AddNumberParam("providerId", providerId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }

    static async GetApplicationListForProvider() {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetApplicationListForProvider");
        //bmi.AddNumberParam("providerId", providerId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }

    static async GetApplicationListForPlan(planId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetApplicationListForPlan");
        bmi.AddNumberParam("planId", planId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }


    static async DeleteApplication(applicationId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("DeleteApplication");
        bmi.AddNumberParam("applicationId", applicationId);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }


    static async GetPrivacyPolicy() {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetPrivacyPolicy");
        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }


    static async PostPrivacyPolicy(privacyPolicy) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("PostPrivacyPolicy");
        bmi.AddStringParam("privacyPolicy", privacyPolicy);


        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(ProviderBiz.BIZ_NAME, bmi);
    }

}