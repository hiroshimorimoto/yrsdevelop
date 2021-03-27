//検証部品
import { validationMixin } from "vuelidate";
import {
    required,
    minLength,
    maxLength,
    numeric,
    email,
    minValue,
    maxValue,
} from "vuelidate/lib/validators";

export default class PlanInfo {
    constructor() {
        this.PlanId = -1;//プランID
        this.ProviderId = "";//事業者ID
        this.PublicStatus = 0;//公開ステータス
        this.PlanTitle = "";//プランタイトル
        this.PlanStartDate = "";//プラン開始日
        this.PlanEndDate = "";//プラン終了日
        this.PublicStartDate = "";//公開開始日
        this.PubcliEndDate = "";//公開終了日
        this.PlanAddress = "";//会場住所
        this.Access = "";//アクセス
        this.Overview = "";//概要
        this.Contents;//プラン内容
        this.Fee_Adult = 0;//大人料金
        this.Fee_Child = 0;//児童料金
        this.Fee_Infant = 0;//幼児料金
        this.DeleteFlg = 0;//削除フラグ
        this.MeetingPlace = "";//集合場所
        this.TimeRequired = "";//所要時間
        this.MaxApplicantsNum = 0;//最大申込人数
        this.PlanCourse = "";//コース 
    }

    static get Validations() {
        return {
            PlanTitle: { required, maxLength: maxLength(100) }, //プランタイトル
            PlanStartDate: {
                required: required,
                //ltEndDate: (v, c) => c.IsltEndDate(v, c),
                ltEndDate: function (v, c) {
                    if (!v) return true
                }
            }, //プラン開始日
            PlanEndDate: {
                required: required,
                gtStartDate: function (v, c) {
                    if (!v) return true
                }
            }, //プラン終了日
            PlanAddress: { maxLength: maxLength(100) }, //会場住所
            Access: { maxLength: maxLength(200) }, //アクセス
            Overview: { maxLength: maxLength(200) }, //概要
            Contents: { maxLength: maxLength(1024) }, //プラン内容
            Fee_Adult: { numeric }, //大人料金
            Fee_Child: { numeric }, //児童料金
            Fee_Infant: { numeric }, //幼児料金
            MeetingPlace: { maxLength: maxLength(256) }, //集合場所
            TimeRequired: { maxLength: maxLength(256) }, //所要時間
            MaxApplicantsNum: { numeric }, //最大申込人数
        }
    }
}
exports.PlanInfo = PlanInfo;