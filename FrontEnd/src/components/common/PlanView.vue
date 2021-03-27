<style lang="scss" scoped>
img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  margin: auto;
}

.plan-title {
  margin-left: -10px;
  text-align: left;
}

.plan-title.no-button {
  cursor: default;
  padding-top: 12px;
  padding-bottom: 12px;
}
.plan-title span {
  color: white;
  margin-left: 10px;
}
.plan-title svg {
  color: white;
}
div[role="alert"] {
  margin-left: -20px;
  margin-right: -10px;
  padding-top: 5px;
  padding-bottom: 5px;
}
</style>
<template>
  <div>
    <div v-if="this.PublicPlanModel == null">
      <div class="alert alert-danger" role="alert">プランが見つかりません</div>
    </div>
    <div v-if="this.PublicPlanModel != null">
      <b-row>
        <b-col cols="0" lg="1" xl="2"></b-col>
        <b-col cols="12" lg="10" xl="8">
          <!-- 申込ボタン１ -->
          <b-row style="padding-bottom:10px;">
            <b-col cols="12">
              <div class="d-flex justify-content-end">
                <b-button variant="info" @click="ReserveClick">
                  <b-icon icon="search" scale="1"></b-icon>
                  <span>このプランに申し込む</span>
                </b-button>
              </div>
            </b-col>
          </b-row>
          <!-- プランタイトル -->
          <b-row>
            <b-col cols="12">
              <b-button block variant="warning" class="plan-title no-button">
                <b-icon icon="emoji-smile" scale="2"></b-icon>
                <span>{{PlanTitle}}</span>
              </b-button>
            </b-col>
          </b-row>
          <!-- イメージ表示エリア -->
          <b-row>
            <b-col cols="6">
              <img :src="TopImageSrc" />
            </b-col>
            <b-col cols="6">
              <b-row>
                <b-col cols="12"></b-col>
              </b-row>
              <b-row>
                <b-col cols="6"></b-col>
                <b-col cols="6"></b-col>
              </b-row>
            </b-col>
          </b-row>
          <!-- 概要 -->
          <b-row>
            <b-col cols="12">
              <div>{{Overview}}</div>
            </b-col>
          </b-row>
          <!-- プラン内容 -->
          <b-row>
            <b-col cols="12">
              <div style="padding : 10px;">
                <div class="alert alert-warning" role="alert">プラン内容:</div>
                <div>{{Contents}}</div>
              </div>
            </b-col>
          </b-row>
          <!-- 期間 -->
          <b-row>
            <b-col cols="12">
              <div style="padding : 10px;">
                <div class="alert alert-warning" role="alert">期間:</div>
                <div>{{PlanTerm}}</div>
              </div>
            </b-col>
          </b-row>
          <!-- アクセス -->
          <b-row>
            <b-col cols="12">
              <div style="padding : 10px;">
                <div class="alert alert-warning" role="alert">アクセス:</div>
                <div>{{PlanAddress}}</div>
                <div>{{Access}}</div>
              </div>
            </b-col>
          </b-row>
          <!-- カテゴリー -->
          <b-row>
            <b-col cols="12">
              <div style="padding : 10px;">
                <div class="alert alert-warning" role="alert">カテゴリー:</div>
                <CategoryList :ReadOnly="true" :SelectedCategoryIds="CategoryIds"></CategoryList>
              </div>
            </b-col>
          </b-row>
          <!-- 受付可能日 -->
          <b-row>
            <b-col cols="12">
              <div style="padding : 10px;">
                <div class="alert alert-warning" role="alert">受付可能日:</div>
                <DatesChooser
                  :ReadOnly="true"
                  :StartDate="StartDate"
                  :EndDate="EndDate"
                  :ListVisible="false"
                  v-model="AcceptDates"
                ></DatesChooser>
              </div>
            </b-col>
          </b-row>
          <!-- 地域 -->
          <b-row>
            <b-col cols="11">
              <div class="alert alert-warning" role="alert">地域:</div>
              <div style="padding : 10px;">
                <AreaMap
                  ref="AreaMap"
                  :ReadOnly="true"
                  :SelectedAreaIds="AreaIds"
                  :TitleVisible="false"
                  :TreeVisible="false"
                ></AreaMap>
              </div>
            </b-col>
          </b-row>
          <!-- 申込ボタン１ -->
          <b-row style="padding-top:10px;">
            <b-col cols="12">
              <div class="d-flex justify-content-end">
                <b-button variant="info" @click="ReserveClick">
                  <b-icon icon="search" scale="1"></b-icon>
                  <span>このプランに申し込む</span>
                </b-button>
              </div>
            </b-col>
          </b-row>
        </b-col>
        <b-col cols="0" lb="1" xl="2"></b-col>
      </b-row>
    </div>
  </div>
</template>
<script>
import CustomerBiz from "@js/biz/CustomerBiz";

import DateUtil from "@js/DateUtil"; //DateUtil

import DatesChooser from "@components/common/DatesChooser"; //日付選択部品
import CategoryList from "@components/common/CategoryList"; //カテゴリ
import AreaMap from "@components/common/AreaMap"; //エリアマップ
import numeric from "vuelidate/lib/validators/numeric";
import PlanModel from "@js/dto/PlanModel";

export default {
  name: "PlanView",
  components: { AreaMap, DatesChooser, CategoryList },
  props: {
    PlanId: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      PublicPlanModel: null,
    };
  },
  computed: {
    StartDate() {
      return DateUtil.GetDateString(new Date());
    },
    EndDate() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.PlanInfo.PlanEndDate;
      } else {
        return "";
      }
    },
    PlanTitle() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.PlanInfo.PlanTitle;
      } else {
        return "";
      }
    },
    Overview() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.PlanInfo.Overview;
      } else {
        return "";
      }
    },
    Contents() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.PlanInfo.Contents;
      } else {
        return "";
      }
    },
    PlanAddress() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.PlanInfo.PlanAddress;
      } else {
        return "";
      }
    },
    Access() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.PlanInfo.Access;
      } else {
        return "";
      }
    },
    PlanTerm() {
      var ret = "";
      if (this.PublicPlanModel) {
        ret += DateUtil.GetDateString(
          this.PublicPlanModel.PlanInfo.PlanStartDate
        );
        ret += " ～ ";
        ret += DateUtil.GetDateString(
          this.PublicPlanModel.PlanInfo.PlanEndDate
        );
      }
      return ret;
    },
    AcceptDates: {
      get() {
        if (this.PublicPlanModel) {
          return this.PublicPlanModel.AcceptDates;
        } else {
          return [];
        }
      },
      set(newValue) {},
    },
    CategoryIds() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.CategoryIds;
      } else {
        return [];
      }
    },
    AreaIds() {
      if (this.PublicPlanModel) {
        return this.PublicPlanModel.AreaIds;
      } else {
        return [];
      }
    },
    TopImageSrc() {
      if (this.PublicPlanModel == null) {
        return null;
      } else {
        return (
          process.env.VUE_APP_API_BASE_URL +
          this.PublicPlanModel.PlanInfo.PublicTopImageUrl
        );
      }
    },
  },
  methods: {
    SetPlanId: async function (planId) {
      try {
        //プラン情報のGET
        var planModel = await CustomerBiz.GetPlanModel(planId);
        planModel.AcceptDates = DateUtil.ToDateArray(planModel.AcceptDates);
        this.PublicPlanModel = planModel;

        this.InitMap();
      } catch (e) {
        this.$bvToast.toast(e, {
          variant: "danger ",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 3000,
          noCloseButton: true,
          textCenter: true,
        });
      }
    },
    InitMap: function () {
      setTimeout(() => {
        this.$refs.AreaMap.InitAreaMap();
      }, 500);
    },
    ReserveClick: function () {
      this.$emit("ReserveClick", planListSearchEt);
    },
  },
  async mounted() {
    await this.SetPlanId(this.PlanId);
  },
};
</script>