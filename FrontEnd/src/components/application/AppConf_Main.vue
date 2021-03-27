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
      </b-col>
    </b-row>
    <b-row>
      <!-- 参加人数 大人 -->
      <b-col cols="2">
        <b-input-group>
          <b-input-group-prepend>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >大人</b-input-group-text
            >
          </b-input-group-prepend>
          <div class="conf-value num">{{ value.Num_Adult }}</div>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >人</b-input-group-text
            >
          </b-input-group-append>
        </b-input-group>
      </b-col>

      <!-- 参加人数 子供 -->
      <b-col cols="2">
        <b-input-group>
          <b-input-group-prepend>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >子供</b-input-group-text
            >
          </b-input-group-prepend>
          <div class="conf-value num">{{ value.Num_Child }}</div>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >人</b-input-group-text
            >
          </b-input-group-append>
        </b-input-group>
      </b-col>

      <!-- 参加人数 幼児 -->
      <b-col cols="2">
        <b-input-group>
          <b-input-group-prepend>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >幼児</b-input-group-text
            >
          </b-input-group-prepend>
          <div class="conf-value num">{{ value.Num_Infant }}</div>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >人</b-input-group-text
            >
          </b-input-group-append>
        </b-input-group>
      </b-col>
    </b-row>

    <!-- 交通手段 -->
    <b-row>
      <b-col>
        <span class="field-title">集合場所（または宿）までの交通手段</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <div v-if="value.Transportation == 0" class="conf-value">
          公共交通機関
        </div>
        <br />
        <div v-if="value.Transportation == 1" class="conf-value">
          車・バイク
        </div>
      </b-col>
    </b-row>

    <!-- チェックイン時刻 -->
    <b-row>
      <b-col> <span class="field-title">チェックイン時刻</span><br /> </b-col>
    </b-row>
    <b-row>
      <b-col cols="2">
        <div class="conf-value">{{ value.CheckInTime }}</div>
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
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <div class="conf-value" v-if="value.PaymentMethods == 0">
          クレジットカード払い
        </div>
        <br />
        <div class="conf-value" v-if="value.PaymentMethods == 2">>現地支払</div>
        <br />
        <div class="conf-value" v-if="value.PaymentMethods == 1">銀行振込</div>
      </b-col>
    </b-row>
    <!-- 振込先情報 -->
    <b-row v-if="value.PaymentMethods == 1">
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

//リソースUtil
import ResourceUtil from "@js/ResourceUtil";

import PlanFeeItem from "./PlanFeeItem";

//数値フォーマッタ
const numFormatter = new Intl.NumberFormat("ja-JP");

export default {
  name: "AppConf_Main",
  props: {
    AppEditModel: Object,
    value: Object, //ApplicationHeader
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
    SumFee() {
      var ret = 0;

      this.AppEditModel.PlanModelList.forEach((planModel) => {
        ret += planModel.PlanInfo.Fee_Adult * this.value.Num_Adult;
        ret += planModel.PlanInfo.Fee_Child * this.value.Num_Child;
        ret += planModel.PlanInfo.Fee_Infant * this.value.Num_Infant;
      });

      this.value.Amount = ret;
      return ret;
    },
  },
  methods: {
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