<style lang="scss">
fieldset small {
  margin-top: 0px;
  padding-bottom: 7px;
}
</style>

<style lang="scss" scoped>
.bv-no-focus-ring > div {
  padding-top: 7px;
  padding-bottom: 7px;
}
</style>
<template>
  <div>
    <EditModdalBase
      ref="editModalBase"
      edit-modal-size="md"
      HeaderTitle="申し込み情報"
      HeaderBgVariant="success"
      HeaderTextVariant="light"
      @CancelClicked="CancelClicked"
    >
      <template v-slot:EditModalBody>
        <b-row v-if="PlanAppInfoItem.DeleteFlg == 1">
          <b-col>
            <b-badge variant="danger">削除済み</b-badge>
          </b-col>
        </b-row>
        <div v-bind:disabled="Loading">
          <b-row>
            <b-col>
              <!-- プランタイトル -->
              <b-form-group
                label="プラン名:"
                label-size="md"
                label-cols="5"
                label-align="left"
                label-for="input-PlanTitle"
              >
                <a>{{PlanAppInfoItem.PlanTitle}}</a>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- ユーザ名 -->
              <b-form-group
                label="申込者 氏名:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-Access"
              >
                <div>{{PlanAppInfoItem.UserName}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- ユーザメールアドレス -->
              <b-form-group
                label="申込者 メールアドレス:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-UserMailAddress"
              >
                <div>{{PlanAppInfoItem.UserMailAddress}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- ユーザ電話番号 -->
              <b-form-group
                label="申込者 電話番号:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-UserPhone"
              >
                <div>{{PlanAppInfoItem.UserPhone}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 来場日 -->
              <b-form-group
                label="来場日:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-AcceptDate"
              >
                <div>{{AcceptDateStr}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 泊数 -->
              <b-form-group
                label="泊数:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-Num_Stays"
              >
                <div>{{PlanAppInfoItem.Num_Stays}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 大人人数 -->
              <b-form-group
                label="大人人数:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-Num_Adult"
              >
                <div>{{PlanAppInfoItem.Num_Adult}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 児童人数 -->
              <b-form-group
                label="児童人数:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-Num_Child"
              >
                <div>{{PlanAppInfoItem.Num_Child}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 幼児人数 -->
              <b-form-group
                label="幼児人数:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-Num_Infant"
              >
                <div>{{PlanAppInfoItem.Num_Infant}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 吉野ポイントカード№ -->
              <b-form-group
                label="吉野ポイントカード№:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-PointCardNo"
              >
                <div>{{PlanAppInfoItem.PointCardNo}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- ポイントカード申込 -->
              <b-form-group
                label="ポイントカード申込:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-PointCardFlgName"
              >
                <div>{{PointCardFlgName}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 住所 -->
              <b-form-group
                label="住所:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-UserAddress"
              >
                <div>{{PlanAppInfoItem.UserAddress}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 生年月日 -->
              <b-form-group
                label="生年月日:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-BirthDate"
              >
                <div>{{PlanAppInfoItem.BirthDate}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 性別 -->
              <b-form-group
                label="性別:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-SexName"
              >
                <div>{{SexName}}</div>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 申し込み日 -->
              <b-form-group
                label="申し込み日:"
                label-size="md"
                description
                label-cols="5"
                label-align="left"
                label-for="input-AcceptDate"
              >
                <div>{{InsertDateStr}}</div>
              </b-form-group>
            </b-col>
          </b-row>
        </div>
      </template>
    </EditModdalBase>
  </div>
</template>
<script>
//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

//編集モーダル ベース
import EditModdalBase from "@components/common/EditModalBase";

//プラン申し込み情報
import PlanAppInfo from "@js/model/PlanAppInfo";

//運営者Biz
import ProviderBiz from "@js/biz/ProviderBiz";
import DateUtil from "@js/DateUtil";

export default {
  components: {
    FontAwesomeIcon,
    EditModdalBase,
  },
  data() {
    return {
      PlanAppInfoItem: {},
    };
  },
  computed: {
    Loading: function () {
      return this.$refs.editModalBase.Loading;
    },
    EditMode: function () {
      return this.$refs.editModalBase.EditMode;
    },
    EditModeName: function () {
      return this.$refs.editModalBase.EditModeName;
    },
    AcceptDateStr: function () {
      return DateUtil.GetDateStringZero(this.PlanAppInfoItem.AcceptDate);
    },
    InsertDateStr: function () {
      return DateUtil.GetDateStringZero(this.PlanAppInfoItem.InsertDate);
    },
    PointCardFlgName: function () {
      if (this.PlanAppInfoItem.PointCardFlg == 0) {
        return "なし";
      } else {
        return "あり";
      }
    },
    SexName: function () {
      if (this.PlanAppInfoItem.Sex == 0) {
        return "男性";
      } else {
        return "女性";
      }
    },
  },
  methods: {
    OpenRead: async function (editItem) {
      try {
        //プラン申し込み情報のGET
        var planAppInfoItem = await ProviderBiz.GetApplicationForProvider(
          editItem.ApplicationId
        );

        this.PlanAppInfoItem = planAppInfoItem;

        this.$refs.editModalBase.open("Read");
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
    CancelClicked: function () {
      this.$refs.editModalBase.close();
    },
  },
  mounted() {},
};
</script>