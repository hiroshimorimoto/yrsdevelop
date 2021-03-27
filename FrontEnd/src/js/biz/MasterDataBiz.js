import ApiUtil from '@js/ApiUtil';
import BizMethodInfo from "@js/dto/BizMethodInfo.js";

export default class MasterDataBiz {
    constructor() {
    }

    static get BIZ_NAME() { return "MasterData"; }

    static async GetCategoryList() {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetCategoryList");

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(MasterDataBiz.BIZ_NAME, bmi);
    }
}