import ApplicationHeader from "@js/model/ApplicationHeader";
import ApplicationPlan from "@js/model/ApplicationPlan";
import ApplicationUser from "@js/model/ApplicationUser";
import ApplicationPayment from "@js/model/ApplicationPayment";

export default class AppEditModel {
    constructor() {
        this.ApplicationHeader = null;
        this.ApplicationPlans = [];
        this.ApplicationUsers = [];
        this.ApplicationPayments = [];


        //#region 以下 Post時は必要無し
        this.PlanModelList = [];
        this.EnableBankPay = false;//銀行振込 可・不可
        this.IsAllPerson = false;//全 個人情報フラグ
        this.MaxApplicantsNum = 0;//最大 申し込み人数
        //#endregion
    }
}
