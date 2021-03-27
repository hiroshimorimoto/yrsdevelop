export default class ApplicationUser {
    constructor() {
        var now = new Date();

        this.ApplicationId = 0;//申込ID
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
        this.InsertDate = now;//登録日
        this.UpdateDate = now;//更新日
        this.DeleteFlg = 0;//削除フラグ
    }
}
exports.ApplicationUser = ApplicationUser;