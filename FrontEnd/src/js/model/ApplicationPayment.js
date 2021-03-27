export default class ApplicationPayment {
    constructor() {
        this.ApplicationId = 0;//申込ID
        this.PaymentSeq = 0;//決済連番
        this.PaymentStatus = 0;//決済ステータス
        this.InsertDate = null;//登録日
    }
}
exports.ApplicationPayment = ApplicationPayment;