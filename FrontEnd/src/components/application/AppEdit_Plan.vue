<style lang="scss" scoped>
</style>
<template>
  <div>
    <b-row>
      <b-col cols="2" class="sub-title-font"> (プラン{{ PlanNo }}) </b-col>
      <b-col cols="10" class="sub-title-font">{{
        PlanModel.PlanInfo.PlanTitle
      }}</b-col>
    </b-row>
    <b-row>
      <b-col cols="2" class="sub-title-font">申込日</b-col>
      <b-col cols="7" class="sub-title-font">プラン料金</b-col>
      <b-col cols="3">
        <a href="#" @click="PPLinkClick(PlanModel.PlanInfo.PlanId)">
          個人情報保護方針
        </a>
        <br />
        <a href="#" @click="CPLinkClick(PlanModel.PlanInfo.CancelPolicy)">
          キャンセルポリシー
        </a>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="2">{{ AcceptDateStr }}</b-col>
      <b-col cols="10">
        {{ getNumFormat(PlanModel.PlanInfo.Fee_Adult) }}/大人1名&nbsp;&nbsp;
        {{ getNumFormat(PlanModel.PlanInfo.Fee_Child) }}/子供1名&nbsp;&nbsp;
        {{ getNumFormat(PlanModel.PlanInfo.Fee_Infant) }}/幼児1名&nbsp;&nbsp;
      </b-col>
    </b-row>
    <hr v-if="!IsLast" class="dash" />
  </div>
</template>

<script>
// jquery
import $ from "jquery";

import DateUtil from "@js/DateUtil";
import PublicPlanModel from "@js/model/PublicPlanModel";

//数値フォーマッタ
const numFormatter = new Intl.NumberFormat("ja-JP");

export default {
  name: "AppEdit_Plan",
  props: {
    PlanModel: Object,
    PlanIndex: Number,
    PlanCount: Number,
    AcceptDate: String,
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
    AcceptDateStr() {
      var date = DateUtil.GetDateValue(this.AcceptDate);
      return DateUtil.GetDateStringZeroJA(date);
    },
  },
  methods: {
    PPLinkClick(planId) {
      this.$emit("PPLinkClick", planId);
    },
    CPLinkClick(cancelPolicy) {
      this.$emit("CPLinkClick", cancelPolicy);
    },
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