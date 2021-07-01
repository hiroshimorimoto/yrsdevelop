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
    <PlanEditModal ref="planEditModal" @planEdited="planEdited"></PlanEditModal>
    <h5>プラン一覧</h5>
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
        <a href="#" @click="doEdit(row.item)">
          <font-awesome-icon icon="edit" class="fa-lg" />
        </a>
      </template>
      <!-- 制御列 -->
      <template v-slot:cell(delete-control)="row">
        <div class="col d-flex align-items-center justify-content-center">
          <a href="#" @click="doDelete(row.item)">
            <font-awesome-icon icon="trash-alt" class="fa-lg" />
          </a>
        </div>
      </template>
      <!-- 公開ステータス -->
      <template v-slot:cell(PublicStatus)="row">
        <h5 v-html="_getPublicStatusName(row.item.PublicStatus)"></h5>
      </template>
      <!-- プラン開始日 -->
      <template v-slot:cell(PlanStartDate)="row">
        <p>{{ _getFormatDateTime(row.item.PlanStartDate) }}</p>
      </template>
      <!-- プラン終了日 -->
      <template v-slot:cell(PlanEndDate)="row">
        <p>{{ _getFormatDateTime(row.item.PlanEndDate) }}</p>
      </template>
      <!-- 公開開始日 -->
      <template v-slot:cell(PublicStartDate)="row">
        <p>{{ _getFormatDateTime(row.item.PublicStartDate) }}</p>
      </template>
      <!-- 公開終了日 -->
      <template v-slot:cell(PubcliEndDate)="row">
        <p>{{ _getFormatDateTime(row.item.PubcliEndDate) }}</p>
      </template>
    </b-table>
    <div>
      <b-row>
        <b-col class="text-center">
          <b-button size="m" variant="success" @click="doAdd"
            >新規作成</b-button
          >
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
import ProviderBiz from "@js/biz/ProviderBiz";

//検索用Entity
import PlanListSearchEt from "@js/dto/PlanListSearchEt";
var searchEt = new PlanListSearchEt();

//編集用モーダル
import PlanEditModal from "@components/provider/PlanEditModal";

//列設定
const fields = [
  // { key: "PlanId", label: "プランID" },
  { key: "edit-control", label: "" },
  { key: "PublicStatus", label: "公開", sortable: true },
  { key: "PlanTitle", label: "タイトル" },
  { key: "Overview", label: "概要" },
  { key: "PlanStartDate", label: "プラン開始日", sortable: true },
  { key: "PlanEndDate", label: "プラン終了日", sortable: true },
  // { key: "PublicStartDate", label: "公開開始日", sortable: true },
  // { key: "PubcliEndDate", label: "公開終了日", sortable: true },
  { key: "delete-control", label: "" },
];

export default {
  components: {
    FontAwesomeIcon,
    PlanEditModal,
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
    doAdd: function (item) {
      this.$refs.planEditModal.OpenAdd(item);
    },
    doEdit: function (item) {
      this.$refs.planEditModal.OpenEdit(item);
    },
    doDelete: function (item) {
      this.$bvModal
        .msgBoxConfirm("このプランを削除しますか？", {
          title: "プラン削除確認",
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
            //プランの削除
            await ProviderBiz.DeletePlan(item.PlanId);

            this.$bvToast.toast("プランが削除されました", {
              variant: "success",
              solid: true,
              toaster: "b-toaster-top-center",
              autoHideDelay: 3000,
              noCloseButton: true,
              textCenter: true,
            });

            await this.getList();
          } catch (e) {
            this.$bvToast.toast("プランの削除中にエラーが発生しました\r" + e, {
              variant: "danger",
              solid: true,
              toaster: "b-toaster-top-center",
              autoHideDelay: 3000,
              noCloseButton: true,
              textCenter: true,
            });
          }
        })
        .catch((err) => {});
    },
    getList: async function () {
      try {
        const planList = await ProviderBiz.GetPlanList(searchEt);
        this.items = planList;
      } catch (e) {
        this.items = [];
      }
    },
    planEdited: function () {
      this.getList();
    },
    _getPublicStatusName: function (publicStatus) {
      if (publicStatus == 0) {
        //return '<span class="text-danger">非公開</span>';
        return '<span class="badge badge-danger">非公開</span>';
      } else if (publicStatus == 1) {
        // return '<span class="text-info">公開</span>';
        return '<span class="badge badge-primary">公開</span>';
      }
    },
    _getFormatDateTime: function (dateTime) {
      return DateUtil.GetDateStringZero(dateTime);
    },
  },
  mounted: async function () {
    this.getList();
  },
};
</script>