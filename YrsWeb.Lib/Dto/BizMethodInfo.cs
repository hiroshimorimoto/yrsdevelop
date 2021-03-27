using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;

namespace YrsWeb.Lib.Dto
{
    public class BizMethodInfo
    {

        public BizMethodInfo() { }

        //public BizMethodInfo(MethodBase methodBase)
        //{
        //    this.Name = methodBase.Name;

        //    this.MethodParamList.AddRange(
        //        methodBase.GetParameters().Select(pi => new BizMethodParam(pi)).ToList()
        //    );
        //}

        public string Name { get; set; }
        public List<BizMethodParam> MethodParamList { get; set; } = new List<BizMethodParam>();

        //public BizMethodParam GetMethodParam(string name)
        //{
        //    return this.MethodParamList.FirstOrDefault(pi => pi.Name == name);
        //}

        //public BizMethodParam SetStringParam(
        //    string Name,
        //    string Value
        //)
        //{
        //    BizMethodParam param = this.GetMethodParam(Name);
        //    if (param == null) return null;
        //    param.Value = Value;
        //    param.IsJsonValue = false;
        //    return param;
        //}

        //public BizMethodParam SetJsonParam(
        //    string Name,
        //    object ValueObject
        //)
        //{
        //    BizMethodParam param = this.GetMethodParam(Name);
        //    if (param == null) return null;

        //    param.Value = JsonConvert.SerializeObject(ValueObject);
        //    param.IsJsonValue = true;
        //    return param;
        //}

        //public BizMethodParam AddStringParam(
        //    string Name,
        //    Type ParameterType,
        //    string Value
        //)
        //{
        //    BizMethodParam param = this.GetMethodParam(Name);
        //    if (param != null)
        //        this.MethodParamList.Remove(param);

        //    param = new BizMethodParam(Name, ParameterType, Value, false);

        //    this.MethodParamList.Add(param);
        //    return param;
        //}

        //public BizMethodParam AddJsonParam(
        //    string Name,
        //    Type ParameterType,
        //    object ValueObject
        //)
        //{
        //    string jsonValue = JsonConvert.SerializeObject(ValueObject);
        //    BizMethodParam param = this.GetMethodParam(Name);
        //    if (param != null)
        //        this.MethodParamList.Remove(param);

        //    param = new BizMethodParam(Name, ParameterType, jsonValue, true);
        //    this.MethodParamList.Add(param);
        //    return param;
        //}


        public object[] GetParameterValues()
        {
            List<object> paramValues = new List<object>();
            foreach (BizMethodParam bmp in this.MethodParamList)
            {
                object paramValue = null;
                if (bmp.IsJsonValue)
                {
                    paramValue = JsonConvert.DeserializeObject(bmp.Value, bmp.ParameterType);
                }
                else
                {
                    //paramValue = bmp.Value;
                    paramValue = Convert.ChangeType(bmp.Value, bmp.ParameterType);
                }

                paramValues.Add(paramValue);
            }
            return paramValues.ToArray();
        }

        public Type[] GetParameterTypes()
        {
            return this.MethodParamList.Select(pi => pi.ParameterType).ToArray();
        }

    }

    public class BizMethodParam
    {
        public BizMethodParam() { }

        //public BizMethodParam(
        //    string Name,
        //    Type ParameterType,
        //    string Value,
        //    bool IsJsonValue
        //    )
        //{
        //    this.Name = Name;
        //    this.ParameterType = ParameterType;
        //    this.Value = Value;
        //    this.IsJsonValue = IsJsonValue;
        //}

        //public BizMethodParam(ParameterInfo paramInfo)
        //{
        //    this.Name = paramInfo.Name;
        //    this.ParameterType = paramInfo.ParameterType;
        //}

        public string Name { get; set; }
        public Type ParameterType
        {
            get
            {
                Type ret = typeof(string);
                switch (this.ParamDataType)
                {
                    case "number":
                        ret = typeof(Int32);
                        break;
                    case "boolean":
                        ret = typeof(Boolean);
                        break;
                }
                return ret;
            }
        }

        /// <summary>
        ///  "string" or "number" or "boolean"
        /// </summary>
        public string ParamDataType { get; set; }

        public string Value { get; set; }
        public bool IsJsonValue { get; set; } = false;
    }
}
