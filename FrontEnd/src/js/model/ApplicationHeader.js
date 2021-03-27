export default class ApplicationHeader {
    constructor() {
        var now = new Date();

        this.ApplicationId = 0;//申込ID
        this.ApplicationStatus = 0;//申込ステータス
        this.OrderNo = "";//注文番号
        this.PaymentMethods = 0;//決済方法/*  0：クレジットカード払い 1：銀行振込 2：現地支払 */
        this.Amount = 0;//決済金額
        this.Num_Adult = 0;//大人人数
        this.Num_Child = 0;//子供人数
        this.Num_Infant = 0;//幼児人数
        this.Transportation = 0;//交通手段 /* 0：公共交通機関 1：車・バイク */
        this.CheckInTime = "";//チェックイン時刻
        this.PointCardNo = "";//吉野ポイントカード№
        this.PointCardFlg = 0;//ポイントカード申込フラグ
        this.InsertDate = now;//登録日
        this.UpdateDate = now;//更新日
        this.DeleteFlg = 0;//削除フラグ
    }
}
exports.ApplicationHeader = ApplicationHeader;