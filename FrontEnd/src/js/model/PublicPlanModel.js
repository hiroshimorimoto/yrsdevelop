export default class PublicPlanModel {
    constructor() {
        this.PlanInfo = null;//PublicPlanInfo
        this.AcceptDates = [];//string Array
        this.CategoryIds = [];//String Array
        this.AreaIds = []; //String Array
        this.TopImage = null;//PlanImage
        this.TempImages = [];//PlanImage Array
    }
}

exports.PublicPlanModel = PublicPlanModel;