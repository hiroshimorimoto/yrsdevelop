import ApiUtil from '@js/ApiUtil';
//import FileUtil from '@js/FileUtil';
import FileBiz from '@js/biz/FileBiz';

import BizMethodInfo from "@js/dto/BizMethodInfo.js";
import PlanModel from "@js/dto/PlanModel.js";


import CustomerApiUtil from "@js/CustomerApiUtil";

export default class CustomerBiz {
    constructor() {
    }

    static get BIZ_NAME() { return "Customer"; }

    static async GetCategoryList() {
        var categoryList = await CustomerApiUtil.GetCategoryList();
        return categoryList;
    }

    static async GetPlanList() {
        var planList = await CustomerApiUtil.GetPlanList();

        return planList;
    }

    static async FindPlanList(searchEt) {
        var planList = await CustomerApiUtil.FindPlanList(searchEt);
        return planList;
    }


    static async GetPlanModel(planId) {
        var planModel = CustomerApiUtil.GetPlanModel(planId);
        return planModel;
    }




    static async GetPublicPlanImageList(planId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetPublicPlanImageList");
        bmi.AddNumberParam("planId", planId);

        //Bizメソッド APIコール
        var planModel = ApiUtil.callApi_BizMethodInfo(CustomerBiz.BIZ_NAME, bmi);

        return planModel;
    }

    static async GetPublicPlanTopImage(planId) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetPublicPlanTopImage");
        bmi.AddNumberParam("planId", planId);

        //Bizメソッド APIコール
        var planModel = ApiUtil.callApi_BizMethodInfo(CustomerBiz.BIZ_NAME, bmi);

        return planModel;
    }


    static async GetNewAppEditModel(appEditModelRequest) {
        var appEditModel = await CustomerApiUtil.GetNewAppEditModel(appEditModelRequest);
        return appEditModel;
    }

    static async GetPayCompAppEditModel(applicationId) {
        var appEditModel = await CustomerApiUtil.GetPayCompAppEditModel(applicationId);
        return appEditModel;
    }

    static async PutAppEditModel(appEditModel) {
        var appEditModel = await CustomerApiUtil.PutAppEditModel(appEditModel);
        return appEditModel;
    }

    static async GetPrivacyPolicyFromPlanId(planId) {
        var pp = await CustomerApiUtil.GetPrivacyPolicyFromPlanId(planId);
        return pp;
    }

}