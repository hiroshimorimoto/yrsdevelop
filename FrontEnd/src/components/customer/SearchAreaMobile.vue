<style lang="scss" scoped>
div[role="alert"] {
  margin-left: -30px;
  margin-right: -10px;
  padding-top: 5px;
  padding-bottom: 5px;
  cursor: pointer;
}

div[role="alert"] div.title-right {
  display: inline-block;
  // float: right;
}
</style>
<template>
  <div>
    <b-row>
      <b-col cols="12" sm="12" md="12" lg="6" xl="6">
        <div style="padding : 10px;">
          <div class="alert alert-warning" role="alert" v-b-toggle.category>
            <div class="title-right">
              <b-icon v-if="!searchAreaShow['category']" icon="chevron-double-down" scale="1"></b-icon>
              <b-icon v-if="searchAreaShow['category']" icon="chevron-double-up" scale="1"></b-icon>
            </div>カテゴリーで絞り込み:
          </div>
          <b-collapse
            ref="category"
            id="category"
            @shown="SeachAreaCollapseChanged('category')"
            @hidden="SeachAreaCollapseChanged('category')"
          >
            <CategoryList
              :SelectedCategoryIds="SelectedCategoryIds"
              @CategorySelectedChanged="CategorySelectedChanged"
            ></CategoryList>

            <div class="d-flex justify-content-end">
              <b-button variant="success" @click="SearchClick">
                <b-icon icon="search" scale="1"></b-icon>
                <span>絞り込む</span>
              </b-button>
            </div>
          </b-collapse>
        </div>
      </b-col>
      <b-col cols="12" sm="12" md="12" lg="6" xl="6">
        <div style="padding : 10px;">
          <div class="alert alert-warning" role="alert" v-b-toggle.dates>
            <div class="title-right">
              <b-icon v-if="!searchAreaShow['dates']" icon="chevron-double-down" scale="1"></b-icon>
              <b-icon v-if="searchAreaShow['dates']" icon="chevron-double-up" scale="1"></b-icon>
            </div>受付可能日で絞り込み:
          </div>
          <b-collapse
            ref="dates"
            id="dates"
            @shown="SeachAreaCollapseChanged('dates')"
            @hidden="SeachAreaCollapseChanged('dates')"
          >
            <DatesChooser :StartDate="StartDate" v-model="AcceptDates"></DatesChooser>
            <div class="d-flex justify-content-end">
              <b-button variant="success" @click="SearchClick">
                <b-icon icon="search" scale="1"></b-icon>
                <span>絞り込む</span>
              </b-button>
            </div>
          </b-collapse>
        </div>
      </b-col>
      <b-col cols="12" sm="12" md="12" lg="6" xl="6">
        <div style="padding : 10px;">
          <div class="alert alert-warning" role="alert" v-b-toggle.area>
            <div class="title-right">
              <b-icon v-if="!searchAreaShow['area']" icon="chevron-double-down" scale="1"></b-icon>
              <b-icon v-if="searchAreaShow['area']" icon="chevron-double-up" scale="1"></b-icon>
            </div>地域で絞り込み:
          </div>
          <b-collapse
            ref="area"
            id="area"
            @shown="SeachAreaCollapseChanged('area')"
            @hidden="SeachAreaCollapseChanged('area')"
          >
            <AreaMap ref="AreaMap" :SelectedAreaIds="SelectedAreaIds" :TitleVisible="false"></AreaMap>
            <div class="d-flex justify-content-end">
              <b-button variant="success" @click="SearchClick">
                <b-icon icon="search" scale="1"></b-icon>
                <span>絞り込む</span>
              </b-button>
            </div>
          </b-collapse>
        </div>
      </b-col>
    </b-row>
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
      searchAreaShow: {
        category: false,
        dates: false,
        area: false,
      },
      AreaMapInited: false,
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
    SeachAreaCollapseChanged: function (type) {
      this.searchAreaShow[type] = this.$refs[type].show;
      if (type == "area" && !this.AreaMapInited) {
        this.$nextTick(() => {
          this.$refs.AreaMap.InitAreaMap();
          this.AreaMapInited = true;
        });
      }
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
    console.log("!!MOBILE MOUNTED!!");
  },
};
</script>