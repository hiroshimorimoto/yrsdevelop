<style lang="scss">
@import "../../style/custom.scss";
</style>

<style lang="scss" scoped>
DIV.bank-info {
  border: 1px solid gray;
  margin: 5px 20px;
  padding: 10px;
  border-radius: 0.25rem;
}
.bank-info .name {
  font-weight: bold;
}
.bank-info .description {
  padding-left: 20px;
}
</style>
<template>
  <div v-if="value != null">
    <b-row><span class="sub-title-font"> プラン申込詳細情報</span></b-row>
    <!-- 参加人数 -->
    <b-row>
      <b-col>
        <span class="field-title">参加人数</span>
        <span class="required-badge">必須</span>
      </b-col>
    </b-row>
    <b-row>
      <!-- 参加人数 大人 -->
      <b-col cols="4">
        <b-input-group>
          <b-input-group-prepend>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >大人</b-input-group-text
            >
          </b-input-group-prepend>
          <b-form-input
            cols="2"
            id="input-Num_Adult"
            type="number"
            size="sm"
            min="0"
            class="num-input"
            v-model="$v.value.Num_Adult.$model"
            :state="ValidateState('Num_Adult')"
            aria-describedby="Num_Adult-fb"
          ></b-form-input>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >人</b-input-group-text
            >
          </b-input-group-append>
          <b-form-invalid-feedback id="Num_Adult-fb">
            <small v-if="!$v.value.Num_Adult.required">入力してください </small>
            <small
              v-if="$v.value.Num_Adult.required && !$v.value.Num_Adult.numeric"
              >数値で入力してください
            </small>
            <small
              v-if="$v.value.Num_Adult.required && !$v.value.Num_Adult.minValue"
              >1名以上で入力してください
            </small>
            <small
              v-if="
                AppEditModel.MaxApplicantsNum > 0 &&
                $v.value.Num_Adult.required &&
                !$v.value.Num_Adult.maxValue
              "
            >
              {{ AppEditModel.MaxApplicantsNum }}名以下で入力してください
            </small>
          </b-form-invalid-feedback>
        </b-input-group>
      </b-col>

      <!-- 参加人数 子供 -->
      <b-col cols="4">
        <b-input-group>
          <b-input-group-prepend>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >子供</b-input-group-text
            >
          </b-input-group-prepend>
          <b-form-input
            cols="2"
            id="input-Num_Child"
            type="number"
            size="sm"
            min="0"
            class="num-input"
            v-model="$v.value.Num_Child.$model"
            :state="ValidateState('Num_Child')"
            aria-describedby="Num_Child-fb"
          ></b-form-input>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >人</b-input-group-text
            >
          </b-input-group-append>
          <b-form-invalid-feedback id="Num_Child-fb">
            <small v-if="!$v.value.Num_Child.numeric"
              >数値で入力してください
            </small>
          </b-form-invalid-feedback>
        </b-input-group>
      </b-col>

      <!-- 参加人数 幼児 -->
      <b-col cols="4">
        <b-input-group>
          <b-input-group-prepend>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >幼児</b-input-group-text
            >
          </b-input-group-prepend>
          <b-form-input
            cols="2"
            id="input-Num_Infant"
            type="number"
            size="sm"
            min="0"
            class="num-input"
            v-model="$v.value.Num_Infant.$model"
            :state="ValidateState('Num_Infant')"
            aria-describedby="Num_Infant-fb"
          ></b-form-input>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >人</b-input-group-text
            >
          </b-input-group-append>
          <b-form-invalid-feedback id="Num_Infant-fb">
            <small v-if="!$v.value.Num_Infant.numeric"
              >数値で入力してください
            </small>
          </b-form-invalid-feedback>
        </b-input-group>
      </b-col>
    </b-row>

    <!-- 交通手段 -->
    <b-row>
      <b-col>
        <span class="field-title">集合場所（または宿）までの交通手段</span>
        <span class="required-badge">必須</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <b-form-radio-group
          id="radio-group-Transportation"
          v-model="value.Transportation"
        >
          <b-form-radio value="0">公共交通機関</b-form-radio>
          <br />
          <b-form-radio value="1">車・バイク</b-form-radio>
        </b-form-radio-group>
      </b-col>
    </b-row>

    <!-- チェックイン時刻 -->
    <b-row>
      <b-col>
        <span class="field-title">チェックイン時刻</span><br />
        <span class="field-annotation">
          ※ご宿泊の方のみご入力ください（宿泊ツアーは除く）
        </span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="2">
        <b-form-select
          variant="success"
          v-model="value.CheckInTime"
          :options="this.CheckInTime_HHMMList"
        ></b-form-select>
      </b-col>
    </b-row>

    <!-- 支払方法 -->
    <b-row>
      <b-col>
        <hr class="sub" />
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <span class="sub-title-font">各プラン毎の料金</span>
      </b-col>
    </b-row>
    <b-row>
      <!-- プラン事の料金表示 -->
      <b-col>
        <PlanFeeItem
          v-for="(planModel, index) in this.AppEditModel.PlanModelList"
          :key="planModel.PlanInfo.PlanId"
          :PlanModel="planModel"
          :PlanIndex="index"
          :PlanCount="AppEditModel.PlanModelList.length"
          :AppHeader="AppEditModel.ApplicationHeader"
        >
        </PlanFeeItem>
        <b-row>
          <b-col cols="9"></b-col>
          <b-col cols="3">
            <span class="sub-title-font">
              合計：{{ getNumFormat(SumFee) }} 円
            </span>
          </b-col>
        </b-row>
      </b-col>
    </b-row>

    <!-- お支払方法選択 -->
    <b-row>
      <b-col>
        <span class="field-title">お支払方法選択</span>
        <span class="required-badge">必須</span><br />
        <span class="field-annotation">
          ※プランによって利用できる支払方法が異なります。詳しくはプランページをご確認ください。
        </span>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <b-form-radio-group
          id="radio-group-PaymentMethods"
          v-model="value.PaymentMethods"
        >
          <b-form-radio value="0">クレジットカード払い</b-form-radio>
          <br />
          <!-- <b-form-radio value="2">現地支払</b-form-radio><br /> -->
          <b-form-radio value="1" v-if="AppEditModel.EnableBankPay">
            銀行振込（振込手数料はお客様のご負担となります）
          </b-form-radio>
          <br v-if="AppEditModel.EnableBankPay" />
        </b-form-radio-group>
      </b-col>
    </b-row>
    <!-- 振込先情報 -->
    <b-row v-if="AppEditModel.EnableBankPay">
      <b-col>
        <div class="bank-info">
          <div v-for="bankInfo in this.banks" :key="bankInfo.Name">
            <div class="name">○{{ bankInfo.Name }}</div>
            <div class="description">{{ bankInfo.Description }}</div>
          </div>
        </div>
      </b-col>
    </b-row>
  </div>
</template>

<script>
// jquery
import $ from "jquery";
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

//リソースUtil
import ResourceUtil from "@js/ResourceUtil";

import PlanFeeItem from "./PlanFeeItem";

//数値フォーマッタ
const numFormatter = new Intl.NumberFormat("ja-JP");

//申込最大人数チェック
const MaxApplicantsValidator = function (value) {
  if (this.AppEditModel.MaxApplicantsNum <= 0) return true;
  return value <= this.AppEditModel.MaxApplicantsNum;
};

export default {
  mixins: [validationMixin],
  name: "AppEdit_Main",
  props: {
    AppEditModel: Object,
    value: Object, //ApplicationHeader
  },
  validations: {
    value: {
      Num_Adult: {
        required,
        numeric,
        minValue: minValue(1),
        maxValue: MaxApplicantsValidator,
      },
      Num_Child: { numeric, minValue: minValue(0) },
      Num_Infant: { numeric, minValue: minValue(0) },
    },
  },
  components: {
    PlanFeeItem,
  },
  data() {
    return {
      banks: [],
    };
  },
  computed: {
    CheckInTime_HHMMList() {
      const startHH = 8;
      const endHH = 19;
      const strMMs = ["00", "30"];
      var ret = [];
      for (var hh = startHH; hh <= endHH; hh++) {
        let strHH = ("00" + hh).slice(-2);
        for (var i in strMMs) {
          ret.push(strHH + ":" + strMMs[i]);
        }
      }

      return ret;
    },

    SumFee() {
      var ret = 0;

      this.AppEditModel.PlanModelList.forEach((planModel) => {
        ret += planModel.PlanInfo.Fee_Adult * this.value.Num_Adult;
        ret += planModel.PlanInfo.Fee_Child * this.value.Num_Child;
        ret += planModel.PlanInfo.Fee_Infant * this.value.Num_Infant;
      });

      return ret;
    },
  },
  methods: {
    ValidateState(propName) {
      const { $dirty, $error } = this.$v.value[propName];
      return $dirty ? !$error : null;
    },
    // updateModel(newValue) {
    //   // inputイベントによってv-modelが更新され、するとバインドされているthis.valueも更新される。
    //   this.$emit("input", newValue);
    //         console.log("Num_Adult Changed!!");
    // },
    getNumFormat(num) {
      if (!$.isNumeric(num)) {
        num = new Number(num);
      }
      return numFormatter.format(num);
    },
  },
  mounted: async function () {
    var bankData = await ResourceUtil.GetBankData();
    this.banks = bankData.Banks;
  },
};
</script>