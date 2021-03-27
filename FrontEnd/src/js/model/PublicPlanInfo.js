export default class PublicPlanInfo {
    constructor() {
        this.ProviderName = "";
        this.ProviderIndustry = "";
        this.ProviderAddress = "";
        this.TantoName = "";
        this.TantoMailAddress = "";
        this.TantoPhone = "";
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

        this.MinAcceptDate = null;//最小 受付可能日
        this.MaxAcceptDate = null;//最大 受付可能日
        this.MaxFee_Adult = 0;//最大 大人料金
        this.MinFee_Adult = 0;//最小 大人料金
        this.MaxFee_Child = 0;//最大 児童料金
        this.MinFee_Child = 0;//最小 児童料金
        this.MaxFee_Infant = 0;//最大 幼児料金
        this.MinFee_Infant = 0;//最小 幼児料金

        this.PublicTopImageUrl = "";
    }
}

exports.PublicPlanInfo = PublicPlanInfo;