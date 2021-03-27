<style lang="scss" scoped>
.fa-edit {
  color: gray;
}
.fa-edit:hover {
  color: blue;
  cursor: pointer;
}
.fa-trash-alt {
  color: gray;
}
.fa-trash-alt:hover {
  color: red;
  cursor: pointer;
}
</style>
<template>
  <div>
    <AppView ref="appView"></AppView>
    <h5>申込一覧</h5>
    <b-table
      stickyHeader
      stickyColumn
      select-mode="single"
      responsive
      striped
      bordered
      hover
      caption-top
      small
      outlined
      :items="items"
      :fields="fields"
      head-variant="light"
      id="list-table"
      ref="list-table"
      class="text-nowrap"
    >
      <!-- 制御列 -->
      <template v-slot:cell(edit-control)="row">
        <a href="#" @click="doView(row.item)">
          <font-awesome-icon icon="edit" class="fa-lg" />
        </a>
      </template>

      <!-- 制御列 -->
      <template v-slot:cell(delete-control)="row">
        <b-badge v-if="row.item.DeleteFlg == 1" variant="danger"
          >削除済み</b-badge
        >

        <div
          v-if="row.item.DeleteFlg == 0"
          class="col d-flex align-items-center justify-content-center"
        >
          <a href="#" @click="doDelete(row.item)">
            <font-awesome-icon icon="trash-alt" class="fa-lg" />
          </a>
        </div>
      </template>

      <!-- 来場日 -->
      <template v-slot:cell(AcceptDate)="row">
        <p>{{ _getFormatDateTime(row.item.AcceptDate) }}</p>
      </template>

      <!-- 生年月日 -->
      <template v-slot:cell(BirthDate)="row">
        <p>{{_getFormatDateTime(row.item.BirthDate)}}</p>
      </template>

      <!-- 交通手段 -->
      <template v-slot:cell(Transportation)="row">
        <p>{{_getFormatTransportation(row.item.Transportation)}}</p>
      </template>

      <!-- 支払方法 -->
      <template v-slot:cell(PaymentMethods)="row">
        <p>{{_getFormatPaymentMethods(row.item.PaymentMethods)}}</p>
      </template>
    </b-table>
  </div>
</template>

<script>
//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

//日付Util
import DateUtil from "@js/DateUtil";

//事業者Biz
import ProviderBiz from "@js/biz/ProviderBiz";

//参照用モーダル
import AppView from "@components/provider/AppView";

//列設定
const fields = [
  // { key: "PlanId", label: "プランID" },
  // { key: "edit-control", label: "" },
  { key: "ApplicationId", label: "注文番号", sortable: true },
  { key: "AcceptDate", label: "来場日", sortable: true },
  { key: "CheckInTime", label: "チェックイン時刻" },
  { key: "PlanTitle", label: "プランタイトル" },
  { key: "UserNo", label: "申込者No" },
  { key: "UserName1", label: "姓" },
  { key: "UserName2", label: "名" },
  { key: "UserPhone1", label: "電話番号", sortable: true },
  { key: "UserPhone2", label: "緊急連絡先", sortable: true },
  { key: "UserMailAddress", label: "メールアドレス", sortable: true },
  { key: "Num_Adult", label: "大人人数" },
  { key: "Num_Child", label: "子供人数" },
  { key: "Num_Infant", label: "幼児人数" },
  { key: "ZipCode", label: "〒", sortable: true },
  { key: "UserAddress1", label: "都道府県", sortable: true },
  { key: "UserAddress2", label: "市区町村" },
  { key: "UserAddress3", label: "番地" },
  { key: "BirthDate", label: "生年月日", sortable: true },
  { key: "Age", label: "年齢", sortable: true },
  { key: "Transportation", label: "交通手段" },
  { key: "PaymentMethods", label: "決済方法", sortable: true },
  //{ key: "ApplicationStatus", label: "申込ステータス", sortable: true },
  //{ key: "delete-control", label: "" },
];

export default {
  components: {
    FontAwesomeIcon,
    AppView,
  },
  data() {
    return {
      fields: fields,
      items: [],
      selectedRows: [],
    };
  },
  computed: {},
  methods: {
    doView: function (item) {
      this.$refs.appView.OpenRead(item);
    },
    doDelete: function (item) {
      this.$bvModal
        .msgBoxConfirm("この申込データを削除しますか？", {
          title: "申込データ削除確認",
          size: "sm",
          buttonSize: "sm",
          okVariant: "danger",
          okTitle: "はい",
          cancelTitle: "いいえ",
          footerClass: "p-2",
          hideHeaderClose: false,
          centered: true,
        })
        .then(async (value) => {
          if (!value) return;
          try {
            //申し込みデータの削除
            await ProviderBiz.DeleteApplication(item.ApplicationId);

            this.$bvToast.toast("申込データが削除されました", {
              variant: "success",
              solid: true,
              toaster: "b-toaster-top-center",
              autoHideDelay: 3000,
              noCloseButton: true,
              textCenter: true,
            });

            await this.getList();
          } catch (e) {
            this.$bvToast.toast(
              "申込データの削除中にエラーが発生しました\r" + e,
              {
                variant: "danger",
                solid: true,
                toaster: "b-toaster-top-center",
                autoHideDelay: 3000,
                noCloseButton: true,
                textCenter: true,
              }
            );
          }
        })
        .catch((err) => {});
    },
    getList: async function () {
      try {
        const planAppInfoList = await ProviderBiz.GetApplicationListForProvider();
        this.items = planAppInfoList;
      } catch (e) {
        this.items = [];
      }
    },
    // planEdited: function () {
    //   this.getList();
    // },
    _getFormatDateTime: function (dateTime) {
      return DateUtil.GetDateStringZero(dateTime);
    },
    _getFormatTransportation: function (transportation) {
      if (transportation == 0)
      {
        return "公共交通機関";
      }
      else
      {
        return "車・バイク";
      }
    },
    _getFormatPaymentMethods: function (paymentMethods) {
      if (paymentMethods == 0)
      {
        return "クレジット";
      }
      else if (paymentMethods == 1)
      {
        return "銀行振込";
      }
      else
      {
        return "現地支払";
      }
    },
  },
  mounted: async function () {
    this.getList();
  },
};
</script>