export default class BizMethodInfo {
    //class BizMethodInfo {
    constructor(methodName) {
        this.Name = methodName;
        this.MethodParamList = new Array();

        this.RemoveMethodParam = function (name) {
            var param = this.GetMethodParam(name);
            if (param) {
                var index = this.MethodParamList.findIndex(e => e.Name == name);
                this.MethodParamList.splice(index, 1);
            }
        }

        this.GetMethodParam = function (name) {
            return this.MethodParamList.find(e => e.Name == name);
        }

        this.AddStringParam = function (name, strValue) {
            var param = this.GetMethodParam(name);
            if (param) this.RemoveMethodParam(name);

            param = new BizMethodParam();
            param.Name = name;
            param.Value = strValue;

            param.ParamDataType = "string";
            param.IsJsonValue = false;

            this.MethodParamList.push(param);
        }

        this.AddNumberParam = function (name, num) {
            var param = this.GetMethodParam(name);
            if (param) this.RemoveMethodParam(name);

            param = new BizMethodParam();
            param.Name = name;
            param.Value = num;

            param.ParamDataType = "number";
            param.IsJsonValue = false;

            this.MethodParamList.push(param);
        }

        this.AddBooleanParam = function (name, bool) {
            var param = this.GetMethodParam(name);
            if (param) this.RemoveMethodParam(name);

            param = new BizMethodParam();
            param.Name = name;
            param.Value = bool;

            param.ParamDataType = "boolean";
            param.IsJsonValue = false;

            this.MethodParamList.push(param);
        }

        this.AddJsonParam = function (name, jsonString) {
            var param = this.GetMethodParam(name);
            if (param) this.RemoveMethodParam(name);

            param = new BizMethodParam();
            param.Name = name;
            param.ParamDataType = "string";

            param.Value = jsonString;
            param.IsJsonValue = true;

            this.MethodParamList.push(param);
        }

        this.AddObjectParam = function (name, object) {
            var param = this.GetMethodParam(name);
            if (param) this.RemoveMethodParam(name);

            param = new BizMethodParam();
            param.Name = name;
            param.ParamDataType = "string";

            param.Value = JSON.stringify(object);
            param.IsJsonValue = true;

            this.MethodParamList.push(param);
        }
    }
};

class BizMethodParam {
    constructor() {
        this.Name = "";
        this.ParamDataType = "string";
        this.Value = null;
        this.IsJsonValue = false;
    }
}
exports.BizMethodParam = BizMethodParam;
