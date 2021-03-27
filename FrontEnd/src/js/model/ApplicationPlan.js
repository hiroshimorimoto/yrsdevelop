export default class ApplicationPlan {
    constructor() {
        this.ApplicationId = 0;//申込ID
        this.PlanId = 0;//プランID
        this.AcceptDate = null;//来場日
        this.Fee_Adult = 0;//大人料金(申込時)
        this.Fee_Child = 0;//児童料金(申込時)
        this.Fee_Infant = 0;//幼児料金(申込時)
        this.InsertDate = null;//登録日
        this.UpdateDate = null;//更新日
        this.DeleteFlg = 0;//削除フラグ

    }
}
exports.ApplicationPlan = ApplicationPlan;