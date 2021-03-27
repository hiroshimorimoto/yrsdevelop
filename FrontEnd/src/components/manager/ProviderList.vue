<style lang="scss" scoped>
table.b-table {
}
</style>
<template>
  <div>
    <ProviderEditModal ref="providerEditModal" @providerEdited="providerEdited"></ProviderEditModal>
    <h5>事業者一覧</h5>
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
      <template v-slot:cell(control)="row">
        <a href="#" @click="doEdit(row.item)">
          <font-awesome-icon icon="edit" class="fa-2x" />
        </a>
      </template>

      <!-- 登録ステータス -->
      <template v-slot:cell(RegistStatus)="row">
        <p v-html="_getRegistStatusName(row.item.RegistStatus)"></p>
      </template>
      <!-- 仮登録日 -->
      <template v-slot:cell(PreRegistDate)="row">
        <p>{{_getFormatDateTime(row.item.PreRegistDate)}}</p>
      </template>
      <!-- 本登録日 -->
      <template v-slot:cell(ProRegistDate)="row">
        <p>{{_getFormatDateTime(row.item.ProRegistDate)}}</p>
      </template>
    </b-table>
    <div class>
      <b-row>
        <b-col class="text-center">
          <b-button size="m" variant="primary" @click="doAdd">新規作成</b-button>
        </b-col>
      </b-row>
    </div>
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
import ManagerBiz from "@js/biz/ManagerBiz";
//検索用Entity
import ProviderListSearchEt from "@js/dto/ProviderListSearchEt";
var searchEt = new ProviderListSearchEt();

//編集用モーダル
import ProviderEditModal from "@components/manager/ProviderEditModal";

//列設定
const fields = [
  { key: "control", label: "" },
  { key: "ProviderId", label: "事業者ID" },
  { key: "RegistStatus", label: "登録ステータス", sortable: true },
  // { key: "ProviderPassword", label: "パスワード" },
  { key: "ProviderName", label: "事業者名" },
  { key: "ProviderIndustry", label: "業種" },
  { key: "ProviderAddress", label: "住所" },
  { key: "TantoName", label: "担当者名" },
  { key: "TantoMailAddress", label: "担当者メールアドレス" },
  { key: "TantoPhone", label: "電話番号" },
  { key: "PreRegistDate", label: "仮登録日", sortable: true },
  { key: "ProRegistDate", label: "本登録日", sortable: true },
];

export default {
  data() {
    return {
      fields: fields,
      items: [],
      selectedRows: [],
    };
  },
  computed: {},
  methods: {
    doAdd: function (item) {
      this.$refs.providerEditModal.openAdd(item);
    },
    doEdit: function (item) {
      this.$refs.providerEditModal.openEdit(item);
    },
    getList: async function () {
      try {
        const providerList = await ManagerBiz.GetProviderList(searchEt);
        this.items = providerList;
      } catch (e) {
        this.items = [];
      }
    },
    providerEdited: function () {
      this.getList();
    },
    _getRegistStatusName: function (registStatus) {
      if (registStatus == 2) {
        return '<span class="text-primary">登録済み</span>';
      } else if (registStatus == 1) {
        return '<span class="text-info">登録待ち</span>';
      } else {
        return '<span class="text-warning">仮登録</span>';
      }
    },
    _getFormatDateTime: function (dateTime) {
      return DateUtil.GetDateStringZero(dateTime);
    },
  },
  mounted: async function () {
    this.getList();
  },
  components: {
    FontAwesomeIcon,
    ProviderEditModal,
  },
};
</script>