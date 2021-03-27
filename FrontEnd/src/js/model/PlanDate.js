export default class PlanDate {
    constructor() {
        this.PlanId = 0;//プランID
        this.AcceptDate = "";//プラン受付日
        this.Fee_Adult = 0;//大人料金
        this.Fee_Child = 0;//児童料金
        this.Fee_Infant = 0;//幼児料金

        this.HasFee = false;

        

    }
}
exports.PlanDate = PlanDate;