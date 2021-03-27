export default class Provider_M {
    constructor() {
        this.ProviderId = "";//事業者ID
        this.RegistStatus = 0;//登録ステータス
        this.ProviderPassword = "";//パスワード
        this.ProviderName = "";//事業者名
        this.ProviderIndustry = "";//業種
        this.ProviderAddress = "";//住所
        this.TantoName = "";//担当者名
        this.TantoMailAddress = "";//担当者メールアドレス
        this.TantoPhone = "";//電話番号
        this.PreRegistDate = "";//仮登録日
        this.ProRegistDate = "";//本登録日
        this.RegistMailCount = 0;//登録メール送信回数
        this.DeleteFlg = 0;//削除フラグ
    }
}
exports.Provider_M = Provider_M;