<style lang="scss" scoped>
div[role="alert"] {
  margin-left: -30px;
  margin-right: -10px;
  padding-top: 5px;
  padding-bottom: 5px;
}
</style>
<template>
  <div>
    <b-row>
      <b-col cols="12" sm="12" md="12" lg="6" xl="6">
        <div style="padding : 10px;">
          <div class="alert alert-warning" role="alert">カテゴリーで絞り込み:</div>
          <CategoryList
            :SelectedCategoryIds="SelectedCategoryIds"
            @CategorySelectedChanged="CategorySelectedChanged"
          ></CategoryList>
        </div>
      </b-col>
      <b-col cols="12" sm="12" md="12" lg="6" xl="6">
        <div style="padding : 10px;">
          <div class="alert alert-warning" role="alert">受付可能日で絞り込み:</div>
          <DatesChooser :StartDate="StartDate" v-model="AcceptDates"></DatesChooser>
        </div>
      </b-col>
      <b-col cols="12" sm="12" md="12" lg="6" xl="6">
        <div style="padding : 10px;">
          <div class="alert alert-warning" role="alert">地域で絞り込み:</div>
          <AreaMap ref="AreaMap" :SelectedAreaIds="SelectedAreaIds" :TitleVisible="false"></AreaMap>
        </div>
      </b-col>
    </b-row>
    <div class="d-flex justify-content-end">
      <b-button variant="success" @click="SearchClick">
        <b-icon icon="search" scale="1"></b-icon>
        <span>絞り込む</span>
      </b-button>
    </div>
  </div>
</template>
<script>
import DateUtil from "@js/DateUtil"; //DateUtil

//検索用Entity
import PlanListSearchEt from "@js/dto/PlanListSearchEt";

import DatesChooser from "@components/common/DatesChooser"; //日付選択部品
import CategoryList from "@components/common/CategoryList"; //カテゴリ
import AreaMap from "@components/common/AreaMap"; //エリアマップ

var searchEt = new PlanListSearchEt();
export default {
  name: "SeachArea",
  components: { AreaMap, DatesChooser, CategoryList },
  data() {
    return {
      AcceptDates: [], //受付可能日付
      SelectedCategoryIds: [], //カテゴリ
      SelectedAreaIds: [], //選択エリア
    };
  },
  computed: {
    StartDate: function () {
      return DateUtil.GetDateString(new Date());
    },
  },
  methods: {
    CategorySelectedChanged: function (selectedCategoryIds) {
      this.SelectedCategoryIds = selectedCategoryIds;
    },
    InitMap: function () {
      setTimeout(() => {
        this.$refs.AreaMap.InitAreaMap();
      }, 0);
    },
    SearchClick: function () {
      var planListSearchEt = new PlanListSearchEt();
      planListSearchEt.AcceptDates = this.AcceptDates;
      planListSearchEt.CategoryIds = this.SelectedCategoryIds;
      planListSearchEt.AreaIds = this.SelectedAreaIds;
      this.$emit("SearchClick", planListSearchEt);
    },
  },
  mounted() {
    console.log("!!NORMAL MOUNTED!!");
    this.$nextTick(() => {
      this.$refs.AreaMap.InitAreaMap();
    });
  },
};
</script>