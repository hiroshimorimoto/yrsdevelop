<style lang="scss" scoped>
div.input-group-text {
  background-color: whitesmoke;
}
</style>
<template>
  <!-- 受付可能編集モーダル -->
  <b-modal
    id="AcceptDateModal"
    size="md"
    centered
    no-close-on-backdrop
    no-close-on-esc
    header-bg-variant="success"
    header-text-variant="white"
  >
    <!-- ヘッダ -->
    <template v-slot:modal-header="{}">
      <h5 class="modal-title" id="editModalLabel">受付可能日の設定</h5>
    </template>
    <!-- ボディ -->
    <template v-slot:default="{}">
      <b-row>
        <b-col>
          <b-form-group label="プラン受付期間:" label-size="sm" label-cols="4" label-align="right">
            <div
              style="padding-top:3px; font-weight: bold;"
            >{{GetDateStringZero(planStartDate)}} ～ {{GetDateStringZero(planEndDate)}}</div>
          </b-form-group>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <DatesChooser :StartDate="planStartDate" :EndDate="planEndDate" v-model="selectedDates"></DatesChooser>
        </b-col>
      </b-row>
      <b-row>
        <hr />
      </b-row>
      <b-row>
        <b-col>
          <b-input-group size="sm">
            <b-input-group-prepend>
              <b-input-group-text>期間内全ての日付を</b-input-group-text>
            </b-input-group-prepend>
            <b-input-group-append>
              <b-dropdown text="(選択)" variant="success" size="sm">
                <b-dropdown-item @click="addAllDate">選択する</b-dropdown-item>
                <b-dropdown-item @click="removeAllDate">選択解除する</b-dropdown-item>
              </b-dropdown>
            </b-input-group-append>
          </b-input-group>
          <br />
        </b-col>
      </b-row>
      <b-row>
        <b-col cols="8">
          <b-input-group size="sm">
            <b-input-group-prepend>
              <b-input-group-text bg-variant="light">期間内全ての</b-input-group-text>
            </b-input-group-prepend>
            <b-form-select variant="success" v-model="selectedDoW" :options="DoW"></b-form-select>

            <b-input-group-append>
              <b-input-group-text variant="light">曜日を</b-input-group-text>
              <b-dropdown text="(選択)" variant="success" size="sm">
                <b-dropdown-item @click="addAllDoWDate">選択する</b-dropdown-item>
                <b-dropdown-item @click="removeAllDoWDate">選択解除する</b-dropdown-item>
              </b-dropdown>
            </b-input-group-append>
          </b-input-group>
        </b-col>
      </b-row>
    </template>
    <!-- フッタ -->
    <template v-slot:modal-footer>
      <b-button size="m" variant="secondary" @click="CancelClicked">キャンセル</b-button>
      <b-button size="m" variant="primary" @click="OkClicked">設定</b-button>
    </template>
  </b-modal>
</template>
<script>
import Vue from "vue";
import VCalendar from "v-calendar";
Vue.use(VCalendar);

import DateUtil from "@js/DateUtil";

//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

import DatesChooser from "@components/common/DatesChooser";

const DoW = [
  { value: 0, text: "日" },
  { value: 1, text: "月" },
  { value: 2, text: "火" },
  { value: 3, text: "水" },
  { value: 4, text: "木" },
  { value: 5, text: "金" },
  { value: 6, text: "土" },
];

export default {
  components: { VCalendar, DateUtil, DatesChooser, FontAwesomeIcon },
  // props: {
  //   PlanStartDate: {
  //     type: String,
  //     default: null
  //   },
  //   PlanEndDate: {
  //     type: String,
  //     default: null
  //   }
  // },
  data() {
    return {
      formats: {
        input: ["YYYY/MM/DD"],
      },
      planStartDate: "",
      PlanEndDAte: "",
      selectedDates: [],
      allSetIndex: 0,
      selectedDoW: 0,
      DoW: DoW,
    };
  },
  computed: {
    allSetTitle: function () {
      if (!this.allSetIndex || this.allSetIndex <= 0)
        return "(選択してください)";
    },
  },
  methods: {
    open: function (planStartDate, planEndDate, selectedDates) {
      this.planStartDate = planStartDate;
      this.planEndDate = planEndDate;
      this.selectedDates = selectedDates;
      this.$bvModal.show("AcceptDateModal");
    },
    close: function () {
      this.$bvModal.hide("AcceptDateModal");
    },
    OkClicked: function () {
      this.$emit("AcceptDateSetted", this.selectedDates); //イベント発火
      this.$bvModal.hide("AcceptDateModal");
    },
    CancelClicked: function () {
      //this.$emit("CancelClicked"); //イベント発火
      this.$bvModal.hide("AcceptDateModal");
    },
    GetDateStringZero: function (date) {
      return (
        DateUtil.GetDateStringZero(date) +
        " (" +
        DateUtil.GetDayOfWeekChar(date) +
        ")"
      );
    },
    removeDate: function (date) {
      this.selectedDates = this.selectedDates.filter((e) => e != date);
    },
    addAllDate: function () {
      var target = new Date(this.planStartDate);
      const endDate = new Date(this.planEndDate);

      //一旦全てクリア
      this.selectedDates = [];

      //開始～終了までの日付を全て追加
      while (target.getTime() <= endDate.getTime()) {
        this.selectedDates.push(DateUtil.GetDateStringZero(target));
        target.setDate(target.getDate() + 1); //1日加算
      }
    },
    removeAllDate: function () {
      //全てクリア
      this.selectedDates = [];
    },
    addAllDoWDate: function () {
      var target = new Date(this.planStartDate);
      const endDate = new Date(this.planEndDate);
      //開始～終了までの日付をループ
      while (target.getTime() <= endDate.getTime()) {
        if (target.getDay() == this.selectedDoW) {
          var strDate = DateUtil.GetDateStringZero(target);
          if (this.selectedDates.indexOf(strDate) < 0) {
            this.selectedDates.push(strDate);
          }
        }
        target.setDate(target.getDate() + 1); //1日加算
      }
    },
    removeAllDoWDate: function () {
      this.selectedDates = this.selectedDates.filter(
        (e) => new Date(e).getDay() != this.selectedDoW
      );
    },
  },
};
</script>