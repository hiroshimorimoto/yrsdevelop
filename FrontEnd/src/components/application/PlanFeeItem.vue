<style lang="scss" scoped>
hr.main {
  height: 1px;
  border-color: lightgray;
  margin-top: 10px;
  border-width: 2px;
}
hr.sub {
  height: 1px;
  border-color: lightgray;
  margin-top: 10px;
  border-width: 1px;
}

hr.dash {
  border: none;
  border-top: dashed 1px lightgray;
  height: 1px;
  margin-left: 20px;
  margin-right: 20px;
}
span.fee-info {
  margin-right: 30px;
}
</style>
<template>
  <div>
    <b-row>
      <b-col cols="1"> (プラン{{ PlanNo }}) </b-col>
      <b-col class="sub-title-font">{{ PlanModel.PlanInfo.PlanTitle }}</b-col>
    </b-row>
    <b-row>
      <b-col cols="3"></b-col>
      <b-col cols="6">
        <span class="fee-info">
          大人：{{ getNumFormat(PlanModel.PlanInfo.Fee_Adult) }}
          ×
          {{ AppHeader.Num_Adult }}名
        </span>
        <span class="fee-info">
          子供：{{ getNumFormat(PlanModel.PlanInfo.Fee_Child) }}
          ×
          {{ AppHeader.Num_Child }}名
        </span>

        <span class="fee-info">
          幼児：{{ getNumFormat(PlanModel.PlanInfo.Fee_Infant) }}
          ×
          {{ AppHeader.Num_Infant }}名
        </span>
      </b-col>
      <b-col cols="3"> 小計：{{ getNumFormat(SumFee) }} 円 </b-col>
    </b-row>
    <hr v-if="!IsLast" class="dash" />
    <hr v-if="IsLast" class="sub" />
  </div>
</template>

<script>
// jquery
import $ from "jquery";

import DateUtil from "@js/DateUtil";

//数値フォーマッタ
const numFormatter = new Intl.NumberFormat("ja-JP");

export default {
  name: "PlanFeeItem",
  props: {
    PlanModel: Object,
    PlanIndex: Number,
    PlanCount: Number,
    AppHeader: Object,
    // Num_Adult: 0, //大人人数
    // Num_Child: 0, //子供人数
    // Num_Infant: 0, //幼児人数
  },
  components: {},
  data() {
    return {};
  },
  computed: {
    PlanNo() {
      return this.PlanIndex + 1;
    },
    IsLast() {
      return this.PlanIndex + 1 >= this.PlanCount;
    },
    SumFee() {
      var ret = 0;
      ret += this.PlanModel.PlanInfo.Fee_Adult * this.AppHeader.Num_Adult;
      ret += this.PlanModel.PlanInfo.Fee_Child * this.AppHeader.Num_Child;
      ret += this.PlanModel.PlanInfo.Fee_Infant * this.AppHeader.Num_Infant;
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
  mounted: async function () {},
};
</script>