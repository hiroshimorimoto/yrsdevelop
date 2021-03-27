export default class ImageBase {
    constructor() {
        this.EditMode = "Nothing";

        this.ImageNo;//イメージNo
        this.ImageLevel;//イメージレベル
        this.SortOrder;//ソート順
        this.FileName;//ファイル名
        this.Comment;//コメント


        this.LocalImagePath;//ローカルイメージパス
        this.LocalImageUrl;//ローカルイメージURL
        this.PublicImageUrl;//パブリックイメージパス


        this.DeleteFlg = false;//制御：削除フラグ
        this.FileElement;//選択ファイルElement
        this.DataUri;//Image Base64 ソース
    }
}
exports.ImageBase = ImageBase;

