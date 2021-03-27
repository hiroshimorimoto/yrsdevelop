<style lang="scss">
@import "../../style/custom.scss";

hr.main {
  height: 1px;
  border-color: #ffc107;
  margin-top: 10px;
  border-width: 2px;
}
hr.sub {
  height: 1px;
  border-color: #ffc107;
  margin-top: 10px;
  border-width: 1px;
}

hr.dash {
  border: none;
  border-top: dashed 1px#ffc107;
  height: 1px;
  margin-left: 20px;
  margin-right: 20px;
}

.title-font {
  color: #ffc107;
  font-weight: bold;
  font-size: 1.5em;
}

.sub-title-font {
  // color: #ffc107;
  font-weight: bold;
  // font-size: 1.5em;
}

.field-title {
  display: inline-block;
  margin-top: 5px;
}
.field-annotation {
  display: inline-block;
  color: gray;
}

.required-badge {
  border-radius: 0.25rem;
  border: none;
  background-color: red;
  color: white;
  font-size: 1rem;
  padding: 0px 5px;
  margin: 0px 5px;
}
.input-group-text {
  height: 38px;
}
.custom-select {
  height: 38px;
}
.num-input {
  height: 38px;
}
.num-input[type="number"] {
  text-align: right;
}

.conf-value {
  height: 38px;
  padding: 5px 20px;
  border: 1px solid darkgray;
  display: inline-block;
  margin: 5px 0px;
  border-radius: 0.25rem;
}
.conf-value.num {
  text-align: right;
  margin: 0px 1px 0px 0px;
  border-radius: 0rem;
}
</style>

<style lang="scss" scoped>
.title-box {
  display: block;
  width: 100%;

  border: 1px solid transparent;
  border-color: #ffc107;

  user-select: none;
  background-color: transparent;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  line-height: 1.5;
  border-radius: 0.25rem;
  transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out;
  // border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}

.search-title.no-button {
  cursor: default;
  padding-top: 12px;
  padding-bottom: 12px;
}
.section-row {
  margin-bottom: 20px;
}
.conf-btn {
  color: white;
  padding: 15px 30px;
  border-radius: 30px;
  font-weight: bold;
  font-size: large;
  margin: 20px;
}
.btn-area {
  width: 100%;
  margin: 10px;
  text-align: center;
}
.progress-wrapper {
  margin: 0 auto;
  height: 100px;
}
.step {
  display: inline-block;
  margin: auto;
  width: 90%;
  text-align: center;
  height: 50px;
}
.step .item {
  display: inline-block;
  background-color: #ffc107;
  margin: 25px 20px;
  height: 50px;
  width: 150px;
  opacity: 0.6;
  border: none;
}
.step .item > p {
  margin-top: 12px;
  color: white;
}

.triangle {
  display: inline-block;
  width: 50px;
  height: 0;
  border-left: 25px solid #ffc107;
  border-top: 25px solid transparent;
  border-bottom: 25px solid transparent;
  margin: -25px 1px -20px -25px;
  opacity: 0.6;
}

.step .item.current {
  opacity: 1;
}
.triangle.current {
  opacity: 1;
}

.comp-title-font {
  color: black;
  font-weight: bold;
  font-size: 2em;
  width: 100%;
  text-align: center;
  margin: 20px;
}

.comp-sub-title-font {
  color: black;
  font-weight: bold;
  font-size: 1.5em;
  width: 100%;
  text-align: center;
}
.order-no-font {
  color: #ffc107;
  font-weight: bold;
  font-size: 2em;
}
.order-no-wrapper {
  margin: 0px auto;
  display: table;
}

.order-no-wrapper p {
  display: inherit;
  margin: auto;
  color: dimgray;
}
</style>
<template>
  <div v-if="AppEditModel != null">
    <!-- 共通 /////////////////////////////////////////////////-->
    <b-row>
      <b-col>
        <div class="progress-wrapper">
          <div class="step">
            <div class="item" v-bind:class="{ current: Mode == 'Edit' }">
              <p>予約内容入力</p>
            </div>
            <div
              class="triangle"
              v-bind:class="{ current: Mode == 'Edit' }"
            ></div>
            <div class="item" v-bind:class="{ current: Mode == 'Conf' }">
              <p>予約内容確認</p>
            </div>
            <div
              class="triangle"
              v-bind:class="{ current: Mode == 'Conf' }"
            ></div>
            <div
              class="item"
              v-bind:class="{ current: Mode == 'Comp' || Mode == 'PayComp' }"
            >
              <p>予約完了</p>
            </div>
          </div>
        </div>
      </b-col>
    </b-row>

    <!-- 申込プラン内容 -->
    <b-row
      class="section-row"
      v-if="this.Mode != 'Comp' && this.Mode != 'PayComp'"
    >
      <b-col cols="0" lg="1" xl="1"></b-col>
      <b-col cols="12" lg="10" xl="10">
        <div class="main-pane">
          <div class="title-box">
            <span class="title-font">申込プラン内容</span>
            <hr class="main" />
            <AppEdit_Plan
              v-for="(planModel, index) in this.AppEditModel.PlanModelList"
              :key="planModel.PlanInfo.PlanId"
              :PlanModel="planModel"
              :PlanIndex="index"
              :PlanCount="AppEditModelRequest.PlanIdList.length"
              :AcceptDate="AppEditModelRequest.PlanIdList[index].AcceptDate"
              @PPLinkClick="PPLinkClick"
              @CPLinkClick="CPLinkClick"
            >
            </AppEdit_Plan>
          </div>
        </div>
      </b-col>
      <b-col cols="0" lg="1" xl="1"></b-col>
    </b-row>
    <!-- 共通 /////////////////////////////////////////////////-->

    <!-- <b-row>
      <b-col>
        <b>検証領域</b>
        <p>交通手段：{{ AppEditModel.ApplicationHeader.Transportation }}</p>
        <p>お支払方法：{{ AppEditModel.ApplicationHeader.PaymentMethods }}</p>
        <p>大人人数：{{ AppEditModel.ApplicationHeader.Num_Adult }}</p>
        <p>ユーザ名(姓){{ AppEditModel.ApplicationUsers[0].UserName1 }}</p>
        <p>ユーザ 生年月日{{ AppEditModel.ApplicationUsers[0].BirthDate }}</p>
        <p>{{ AppEditModel.EnableBankPay }}</p>
        <p>
          <input type="checkbox" v-model="AppEditModel.EnableBankPay" />
        </p>
        <p>{{ PPAcceptCheck }}</p>
      </b-col>
    </b-row> -->

    <!-- 予約内容入力 /////////////////////////////////////////////////-->
    <div v-if="Mode == 'Edit'">
      <!-- 申込メイン -->
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <span class="title-font">予約内容入力フォーム</span>
          <hr class="main" />

          <!-- 申込ヘッダ情報 -->
          <appEdit_Main
            :AppEditModel="this.AppEditModel"
            v-model="this.AppEditModel.ApplicationHeader"
            ref="AppEditMain"
          ></appEdit_Main>

          <!-- 代表者 -->
          <AppEdit_User
            :AppEditModel="this.AppEditModel"
            :UserIndex="0"
            v-model="this.AppEditModel.ApplicationUsers[0]"
            ref="AppEditUser_Main"
          ></AppEdit_User>

          <!-- 同行者 -->
          <AppEdit_User
            v-for="(number, index) in SubUserList.length"
            :key="number"
            :UserIndex="index + 1"
            v-model="SubUserList[index]"
            ref="AppEditUser_Sub"
          ></AppEdit_User>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>

      <!-- プライバシーポリシー -->
      <b-row>
        <b-col>
          <PrivacyPolicyInfo v-model="PPAcceptCheck" ref="PrivacyPolicyInfo">
          </PrivacyPolicyInfo>
        </b-col>
      </b-row>

      <!-- ボタン領域 -->
      <b-row>
        <b-col cols="3"> </b-col>
        <b-col cols="6">
          <div class="btn-area">
            <b-button
              variant="warning"
              :disabled="ConfButtonDisabled"
              @click="ConfAppEditModel"
              class="conf-btn"
              >予約内容を確認する
            </b-button>
          </div>
        </b-col>
        <b-col cols="3"></b-col>
      </b-row>
    </div>
    <!-- 予約内容入力 /////////////////////////////////////////////////-->

    <!-- 予約内容確認 /////////////////////////////////////////////////-->
    <div v-if="Mode == 'Conf'">
      <!-- 申込メイン -->
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <span class="title-font">予約内容確認</span>
          <hr class="main" />

          <!-- 申込ヘッダ情報 -->
          <appConf_Main
            :AppEditModel="this.AppEditModel"
            v-model="this.AppEditModel.ApplicationHeader"
          ></appConf_Main>

          <!-- 代表者 -->
          <AppConf_User
            :AppEditModel="this.AppEditModel"
            :UserIndex="0"
            v-model="this.AppEditModel.ApplicationUsers[0]"
          ></AppConf_User>

          <!-- 同行者 -->
          <AppConf_User
            v-for="(number, index) in SubUserList.length"
            :key="number"
            :UserIndex="index + 1"
            v-model="SubUserList[index]"
          ></AppConf_User>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>

      <!-- ボタン領域 -->
      <b-row>
        <b-col cols="3"> <b-button @click="BackEdit">戻る </b-button> </b-col>
        <b-col cols="6">
          <div class="btn-area">
            <b-button
              variant="warning"
              @click="RegistAppEditModel"
              class="conf-btn"
              v-bind:disabled="Loading"
            >
              <span
                v-show="Loading"
                class="spinner-border spinner-border-sm"
                role="status"
                aria-hidden="true"
              ></span>
              {{ RegistBtnTitle }}
            </b-button>
          </div>
        </b-col>
        <b-col cols="3"></b-col>
      </b-row>
    </div>
    <!-- 予約内容確認 /////////////////////////////////////////////////-->

    <!-- カード決済完了 /////////////////////////////////////////////////-->
    <div v-if="Mode == 'PayComp'">
      <!-- 申込メイン -->
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <span class="title-font">申込完了</span>
          <hr class="main" />
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <h2 class="comp-title-font">お申込が完了いたしました</h2>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <div class="order-no-wrapper">
            <span class="comp-sub-title-font">ご注文番号：</span>
            <span class="order-no-font">{{
              this.AppEditModel.ApplicationHeader.OrderNo
            }}</span>
          </div>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <div class="order-no-wrapper">
            <p>お申込ありがとうございます。</p>
            <br />
            <p>
              上記の「ご注文番号」をお控えいただきますようお願いいたします。
            </p>
            <br />
            <p>
              ご入力いただきましたメールアドレスへ「お申込完了メール」をお送りいたしました。
            </p>
            <p>
              しばらく経ってもお申込完了メールが届かない場合は「お問い合わせフォーム」またはお電話にてお問合せください。
            </p>
          </div>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <!-- ボタン領域 -->
      <b-row>
        <b-col cols="3"> </b-col>
        <b-col cols="6">
          <div class="btn-area">
            <b-button variant="warning" @click="GoToTopPage" class="conf-btn">
              TOPページへ戻る
            </b-button>
          </div>
        </b-col>
        <b-col cols="3"></b-col>
      </b-row>
    </div>
    <!-- カード決済 /////////////////////////////////////////////////-->

    <!-- カード決済エラー /////////////////////////////////////////////////-->
    <div v-if="Mode == 'PayCompError'">
      <!-- 申込メイン -->
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <span class="title-font">申込 決済エラー</span>
          <hr class="main" />
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <h2 class="comp-title-font">お申込の決済中にエラーが発生しました</h2>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <div class="order-no-wrapper">
            <span class="comp-sub-title-font">ご注文番号：</span>
            <span class="order-no-font">{{
              this.AppEditModel.ApplicationHeader.OrderNo
            }}</span>
          </div>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <div class="order-no-wrapper">
            <p>
              申し訳ございません。お申込みの決済処理中にエラーが発生いたしました。
            </p>
            <br />
            <p>
              上記の「ご注文番号」をお控えの上、「お問い合わせフォーム」またはお電話にてお問合せください。
            </p>
            <br />
            <p>エラー詳細：</p>
            <p>{{ PaymentErrMsg }}</p>
          </div>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <!-- ボタン領域 -->
      <b-row>
        <b-col cols="3"> </b-col>
        <b-col cols="6">
          <div class="btn-area">
            <b-button variant="warning" @click="GoToTopPage" class="conf-btn">
              TOPページへ戻る
            </b-button>
          </div>
        </b-col>
        <b-col cols="3"></b-col>
      </b-row>
    </div>
    <!-- カード決済 /////////////////////////////////////////////////-->

    <!-- 完了 /////////////////////////////////////////////////-->
    <div v-if="Mode == 'Comp'">
      <!-- 申込メイン -->
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <span class="title-font">申込完了</span>
          <hr class="main" />
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <h2 class="comp-title-font">お申込が完了いたしました</h2>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <div class="order-no-wrapper">
            <span class="comp-sub-title-font">ご注文番号：</span>
            <span class="order-no-font">{{
              this.AppEditModel.ApplicationHeader.OrderNo
            }}</span>
          </div>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <div class="order-no-wrapper">
            <span class="comp-sub-title-font">お申込日：</span>
            <span class="comp-sub-title-font">{{ OrderDate }}</span>
          </div>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>
      <b-row class="section-row">
        <b-col cols="0" lg="1" xl="1"></b-col>
        <b-col cols="12" lg="10" xl="10">
          <div class="order-no-wrapper">
            <p>お申込ありがとうございます。</p>
            <br />
            <p>
              上記の「ご注文番号」をお控えいただきますようお願いいたします。
            </p>
            <br />
            <p>
              ご入力いただきましたメールアドレスへ「お申込完了メール」をお送りいたしました。
            </p>
            <p>
              しばらく経ってもお申込完了メールが届かない場合は「お問い合わせフォーム」またはお電話にてお問合せください。
            </p>
          </div>
        </b-col>
        <b-col cols="0" lg="1" xl="1"></b-col>
      </b-row>

      <!-- ボタン領域 -->
      <b-row>
        <b-col cols="3"> </b-col>
        <b-col cols="6">
          <div class="btn-area">
            <b-button variant="warning" @click="GoToTopPage" class="conf-btn">
              TOPページへ戻る
            </b-button>
          </div>
        </b-col>
        <b-col cols="3"></b-col>
      </b-row>
    </div>
    <!-- 完了 /////////////////////////////////////////////////-->

    <PPModal ref="PPModal"></PPModal>
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

import DateUtil from "@js/DateUtil";

//利用者Biz
import CustomerBiz from "@js/biz/CustomerBiz";

//プライバシーポリシー情報
import PrivacyPolicyInfo from "@components/common/PrivacyPolicyInfo";

//共通コンポーネント
import AppEdit_Plan from "./AppEdit_Plan";

//プラン編集用コンポーネント
import AppEdit_Main from "./AppEdit_Main";
import AppEdit_User from "./AppEdit_User";

//プラン確認用コンポーネント
import AppConf_Main from "./AppConf_Main";
import AppConf_User from "./AppConf_User";

//個人情報保護方針、キャンセルポリシー表示用モーダル
import PPModal from "./PolicyModal";

//プランモデル 取得リクエスト
import AppEditModelRequest from "@js/dto/AppEditModelRequest";
import AppEditModelRequestItem from "@js/dto/AppEditModelRequestItem";

//ユーザ情報（同行者）登録用
import ApplicationUser from "@js/model/ApplicationUser";

//決済用
import PaymentUtil from "@js/PaymentUtil";

export default {
  name: "AppEdit",
  props: {
    AppEditModelRequest: Object,
    InitMode: {
      type: String,
      default: "Edit",
    },
    InitApplicationId: {
      type: Number,
      default: 0,
    },
  },
  components: {
    AppEdit_Plan,
    AppEdit_Main,
    AppEdit_User,
    AppConf_Main,
    AppConf_User,
    PrivacyPolicyInfo,
    PPModal,
  },
  data() {
    return {
      AppEditModel: null,
      UserCount: 1,
      PPAcceptCheck: false,
      Mode: "",
      PaymentErrMsg: null,
      Loading: false,
    };
  },
  watch: {
    "AppEditModel.ApplicationHeader.Num_Adult": {
      handler: function (val, oldVal) {
        if (!this.AppEditModel.IsAllPerson) return;
        if (!val || !oldVal) return;

        if (val <= 0) return;
        if (
          this.AppEditModel.MaxApplicantsNum > 0 &&
          val > this.AppEditModel.MaxApplicantsNum
        )
          return;

        if (this.UserCount != val) {
          if (this.UserCount < val) {
            for (var i = this.UserCount; i < val; i++) {
              //同行者Add
              var user = new ApplicationUser();
              user.ApplicationId = 0;
              user.UserNo = -1;

              this.AppEditModel.ApplicationUsers.push(user);
            }
          } else {
            //this.UserCount > val
            for (var i = val; i < this.UserCount; i++) {
              //ユーザRemove
              this.AppEditModel.ApplicationUsers.pop();
            }
          }

          this.UserCount = val;
          // console.log("Num_Adult!!!!!!!!!", this.UserCount);
        }
      },
      immediate: false,
    },
  },
  computed: {
    SubUserList() {
      if (this.AppEditModel == null) return [];

      var ret = [];
      this.AppEditModel.ApplicationUsers.forEach((appUser) => {
        if (appUser.UserNo < 0) {
          ret.push(appUser);
        }
      });

      return ret;
    },
    ConfButtonDisabled() {
      return !this.PPAcceptCheck;
    },
    // OrderNo() {
    //   return this.AppEditModel.ApplicationHeader.ApplicationId.toString().padStart(
    //     8,
    //     "0"
    //   );
    // },
    OrderDate() {
      return DateUtil.GetDateStringZeroJA_WithTime(
        this.AppEditModel.ApplicationHeader.InsertDate
      );
    },
    RegistBtnTitle() {
      if (this.AppEditModel.ApplicationHeader.PaymentMethods == 0) {
        return "予約する（カード決済画面へ）";
      } else {
        return "予約する";
      }
    },
  },
  methods: {
    getNewAppEditModel: async function () {
      var modelReq = this.AppEditModelRequest;
      try {
        //申込編集モデルの初期値をサーバから取得
        this.AppEditModel = await CustomerBiz.GetNewAppEditModel(modelReq);

        // //TODO:DEBUG
        // this.AppEditModel.ApplicationHeader.Num_Adult = 1;

        // var u = this.AppEditModel.ApplicationUsers[0];
        // u.ApplicationId = 0; //申込ID
        // u.UserNo = 0; //申込者No
        // u.UserName1 = "津田"; //名前（姓）
        // u.UserName2 = "敏男"; //名前（名）
        // u.UserRuby1 = "ツダ"; //フリガナ（姓）
        // u.UserRuby2 = "トシオ"; //フリガナ（名）
        // u.ZipCode = "3360911"; //郵便番号
        // u.UserAddress1 = "埼玉県"; //都道府県
        // u.UserAddress2 = "さいたま市緑区"; //市区町村
        // u.UserAddress3 = "9999-9"; //番地
        // u.BirthDate = "1975/05/26"; //生年月日
        // u.Age = 45; //年齢
        // u.UserPhone1 = "090-1234-5678"; //電話番号
        // u.UserPhone2 = ""; //緊急連絡先
        // u.UserMailAddress = "toshio2da@enjoit.co.jp"; //メールアドレス
        // u.Sex = 0; //性別
        // u.InsertDate = new Date(); //登録日
        // u.UpdateDate = 0; //更新日
        // u.DeleteFlg = 0; //削除フラグ

        // var su = new ApplicationUser();
        // su.ApplicationId = 0; //申込ID
        // su.UserNo = -1; //申込者No
        // su.UserName1 = "すずき"; //名前（姓）
        // su.UserName2 = "いちろう"; //名前（名）
        // su.UserRuby1 = "スズキ"; //フリガナ（姓）
        // su.UserRuby2 = "イチロウ"; //フリガナ（名）
        // su.ZipCode = "3360911"; //郵便番号
        // su.UserAddress1 = "福井県"; //都道府県
        // su.UserAddress2 = "福井市"; //市区町村
        // su.UserAddress3 = "9999-9"; //番地
        // su.BirthDate = "1970/02/04"; //生年月日
        // su.Age = 45; //年齢
        // su.UserPhone1 = "090-1234-5678"; //電話番号
        // su.UserPhone2 = ""; //緊急連絡先
        // su.UserMailAddress = "suzuki@enjoit.co.jp"; //メールアドレス
        // su.Sex = 0; //性別

        // this.AppEditModel.ApplicationUsers.push(su);
      } catch (e) {
        console.error(e);
        this.$bvToast.toast(e, {
          variant: "danger ",
          solid: true,
          toaster: "b-toaster-top-center",
          autoHideDelay: 5000,
          noCloseButton: true,
          textCenter: true,
        });
        return;
      }
      // this.$refs.planList.SetList(this.items);
    },

    getPayCompAppEditModel: async function () {
      try {
        //既存の申込編集モデルをサーバから取得
        this.AppEditModel = await CustomerBiz.GetPayCompAppEditModel(
          this.InitApplicationId
        );

        this.AppEditModelRequest = new AppEditModelRequest();
        for (var i = 0; i < this.AppEditModel.ApplicationPlans.length; i++) {
          let plan = this.AppEditModel.ApplicationPlans[i];

          let reqItem = new AppEditModelRequestItem();
          reqItem.PlanId = plan.PlanId; //プランID
          reqItem.AcceptDate = plan.AcceptDate; //申込日

          this.AppEditModelRequest.PlanIdList.push(reqItem);
        }

        //決済エラーの判別
        var maxPayment = null;
        for (var i = 0; i < this.AppEditModel.ApplicationPayments.length; i++) {
          if (
            maxPayment == null ||
            maxPayment.PaymentSeq <
              this.AppEditModel.ApplicationPayments[i].PaymentSeq
          ) {
            maxPayment = this.AppEditModel.ApplicationPayments[i];
          }
        }

        if (
          !maxPayment ||
          (maxPayment.ErrCode && maxPayment.ErrCode.length > 0)
        ) {
          //決済エラー
          this.PaymentErrMsg =
            "[" + maxPayment.ErrCode + "] " + maxPayment.ErrInfo;
          this.Mode = "PayCompError";
        }
      } catch (e) {
        console.error(e);
        this.$bvToast.toast(e, {
          variant: "danger ",
          solid: true,
          toaster: "b-toaster-top-center",
          autoHideDelay: 5000,
          noCloseButton: true,
          textCenter: true,
        });
        return;
      }
    },
    PPLinkClick(planId) {
      this.$refs.PPModal.openPrivacyPolicy(planId);
    },
    CPLinkClick(cancelPolicy) {
      this.$refs.PPModal.openCancelPolicy(cancelPolicy);
    },
    getRefsCollection() {
      var ret = [];
      if (this.$refs.AppEditMain && this.$refs.AppEditMain.$v)
        ret.push(this.$refs.AppEditMain.$v);

      if (this.$refs.AppEditUser_Main && this.$refs.AppEditUser_Main.$v)
        ret.push(this.$refs.AppEditUser_Main.$v);

      if (this.$refs.AppEditUser_Sub && this.$refs.AppEditUser_Sub.length > 0) {
        this.$refs.AppEditUser_Sub.forEach((subUser) => {
          if (subUser.$v) ret.push(subUser.$v);
        });
      }

      return ret;
    },
    checkConfButtonDisabled(refsCollection) {
      var anyDirty = false;
      refsCollection.forEach(($v) => {
        $v.$touch();

        // if ($v.$anyDirty) {
        //   anyDirty = true;
        //   return;
        // }
      });

      // if (!anyDirty) return true; //変更なしは 非活性

      var anyInvalid = false;
      refsCollection.forEach(($v) => {
        if ($v.$invalid) {
          anyInvalid = true;
          return;
        }
      });

      if (anyInvalid) return true; //１つでも検証無効があれば、非活性
      return false;
    },

    ConfAppEditModel() {
      var refsCollection = this.getRefsCollection();
      if (this.checkConfButtonDisabled(refsCollection)) {
        this.$bvToast.toast("入力内容にエラーがあります", {
          variant: "danger ",
          solid: true,
          toaster: "b-toaster-top-center",
          autoHideDelay: 5000,
          noCloseButton: true,
          textCenter: true,
        });
        return;
      }

      this.Mode = "Conf";
    },
    BackEdit() {
      this.Mode = "Edit";
    },
    async RegistAppEditModel() {
      this.Loading = true;
      try {
        //申込編集モデルをサーバへ登録
        this.AppEditModel = await CustomerBiz.PutAppEditModel(
          this.AppEditModel
        );

        if (this.AppEditModel.ApplicationHeader.PaymentMethods == 0) {
          //カード決済の場合
          //カード決済画面へ遷移
          this.GoToPayment();
        } else {
          this.Mode = "Comp";
        }
      } catch (e) {
        console.error(e);
        this.$bvToast.toast(e, {
          variant: "danger ",
          solid: true,
          toaster: "b-toaster-top-center",
          autoHideDelay: 5000,
          noCloseButton: true,
          textCenter: true,
        });
        return;
      } finally {
        this.Loading = false;
      }
    },
    GoToTopPage() {
      window.location = process.env.YRS_URL;
    },
    GoToPayment() {
      var appId = this.AppEditModel.ApplicationHeader.ApplicationId;
      var orderNo = this.AppEditModel.ApplicationHeader.OrderNo;
      var amount = this.AppEditModel.ApplicationHeader.Amount;
      PaymentUtil.openPayment(appId, orderNo, amount);
    },
  },
  mounted: async function () {
    this.Mode = this.InitMode;
    if (this.Mode == "PayComp") {
      this.getPayCompAppEditModel();
    } else {
      this.getNewAppEditModel();
    }
  },
};
</script>