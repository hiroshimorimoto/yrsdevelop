export default class ApplicationInfo {
    constructor() {
        this.ApplicationId = -1;//申込ID
        this.PlanId = 0;//プランID
        this.ProviderId = "";//事業者ID
        this.ApplicationStatus = 0;//申込ステータス
        this.UserName = "";//ユーザ名
        this.UserMailAddress = "";//ユーザメールアドレス
        this.UserPhone = "";//ユーザ電話番号
        this.AcceptDate = "";//来場日
        this.Num_Stays = 0;//泊数
        this.Num_Adult = 0;//大人人数
        this.Num_Child = 0;//児童人数
        this.Num_Infant = 0;//幼児人数
        this.Fee_Adult = 0;//大人料金(申込時)
        this.Fee_Child = 0;//児童料金(申込時)
        this.Fee_Infant = 0;//幼児料金(申込時)
        this.PointCardNo = "";//吉野ポイントカード№
        this.PointCardFlg = 0;//ポイントカード申込フラグ
        this.ZipCode = "";//郵便番号
        this.ProviderAddress = "";//住所
        this.BirthDate = 0;//生年月日
        this.Sex = 0;//性別
        this.InsertDate = ""; //登録日
        this.UpdateDate = ""; //更新日
        this.DeleteFlg = 0;//削除フラグ
    }
}
exports.ApplicationInfo = ApplicationInfo;