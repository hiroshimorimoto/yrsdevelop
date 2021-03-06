export default class PlanAppInfo {
    constructor() {
        this.PlanId = 0;//プランID
        this.ProviderId = "";//事業者ID
        this.PublicStatus = 0;//公開ステータス
        this.PlanTitle = "";//プランタイトル
        this.PlanStartDate = "";//プラン開始日
        this.PlanEndDate = "";//プラン終了日
        this.PlanAddress = "";//会場住所
        this.Access = "";//アクセス
        this.Overview = "";//概要
        this.Contents = "";//プラン内容
        this.Fee_Adult = 0;//大人料金
        this.Fee_Child = 0;//児童料金
        this.Fee_Infant = 0;//幼児料金
        this.MeetingPlace = "";//集合場所
        this.TimeRequired = "";//所要時間
        this.MaxApplicantsNum = 0;//最大申込人数
        this.PlanCourse = "";//コース
        this.HiddenFlg = 0;//非表示フラグ
        this.CancelPolicy = "";//キャンセルポリシー
        this.AllPersonFlg = 0;//全個人情報フラグ
        this.ApplicationId = 0;//申込ID
        this.AcceptDate = "";//来場日
        this.Amount = 0;//決済金額
        this.Fee_Adult = 0;//大人料金(申込時)
        this.Fee_Child = 0;//児童料金(申込時)
        this.Fee_Infant = 0;//幼児料金(申込時)
        this.ApplicationStatus = 0;//申込ステータス
        this.PaymentMethods = 0;//決済方法
        this.Num_Adult = 0;//大人人数
        this.Num_Child = 0;//子供人数
        this.Num_Infant = 0;//幼児人数
        this.Transportation = 0;//交通手段
        this.CheckInTime = "";//チェックイン時刻
        this.PointCardNo = "";//吉野ポイントカード№
        this.PointCardFlg = 0;//ポイントカード申込フラグ
        this.UserNo = 0;//申込者No
        this.UserName1 = "";//名前（姓）
        this.UserName2 = "";//名前（名）
        this.UserRuby1 = "";//フリガナ（姓）
        this.UserRuby2 = "";//フリガナ（名）
        this.ZipCode = "";//郵便番号
        this.UserAddress1 = "";//都道府県
        this.UserAddress2 = "";//市区町村
        this.UserAddress3 = "";//番地
        this.BirthDate = "";//生年月日
        this.Age = 0;//年齢
        this.UserPhone1 = "";//電話番号
        this.UserPhone2 = "";//緊急連絡先
        this.UserMailAddress = "";//メールアドレス
        this.Sex = 0;//性別
    }
}
exports.PlanAppInfo = PlanAppInfo;