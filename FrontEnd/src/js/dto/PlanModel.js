export default class PlanModel {
    constructor() {
        this.EditMode;
        this.PlanEt;

        this.AcceptDates = [];//受付可能日リスト
        this.DateFeeList = [];//日別料金リスト
        this.SelectedCategoryIds = []; //カテゴリ
        this.SelectedAreaIds = []; //選択エリア

        this.TopImage = null;//トップ画像エンティティ
        this.TempImages = [];//添付イメージファイルリスト

        this.SelectedTopImageElement = null;
        this.SelectedTopImageFileName = null;
    }
}
exports.PlanModel = PlanModel;