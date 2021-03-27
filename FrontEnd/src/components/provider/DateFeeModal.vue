<style lang="scss" scoped>
input[type="number"] {
  text-align: right;
}
div.col-2 {
  padding-right: 0px;
  padding-left: 0px;
}
div.title {
  text-align: center;
}
div.title.slim {
  text-align: center;
  padding-right: 0px;
  padding-left: 0px;
}
div.fee-default {
  text-align: right;
  color: lightgray;
}
</style>
<template>
  <!-- 日別料金設定モーダル -->
  <b-modal
    id="DateFeeModal"
    size="md"
    scrollable
    centered
    no-close-on-backdrop
    no-close-on-esc
    header-bg-variant="success"
    header-text-variant="white"
  >
    <!-- ヘッダ -->
    <template v-slot:modal-header="{}">
      <h5 class="modal-title" id="editModalLabel">日別料金の設定</h5>
    </template>
    <!-- ボディ -->
    <template v-slot:default="{}">
      <b-row>
        <b-col cols="4" class="title">日付</b-col>
        <b-col cols="1" class="title slim">個別料金</b-col>
        <b-col cols="2" class="title">大人</b-col>
        <b-col cols="2" class="title">児童</b-col>
        <b-col cols="2" class="title">幼児</b-col>
      </b-row>
      <b-row v-for="planDate in dateFeeList" :key="planDate.AcceptDate">
        <b-col cols="4">{{GetDateStringZero(planDate.AcceptDate)}}</b-col>
        <b-col cols="1" class="title">
          <!-- <b-form-checkbox v-model="planDate.HasFee" value="true" unchecked-value="false"></b-form-checkbox> -->
          <b-form-checkbox v-model="planDate.HasFee"></b-form-checkbox>
        </b-col>
        <b-col cols="2" class="fee-default" v-if="!planDate.HasFee">{{$data.fee_Adult}}</b-col>
        <b-col cols="2" class="fee-default" v-if="!planDate.HasFee">{{$data.fee_Child}}</b-col>
        <b-col cols="2" class="fee-default" v-if="!planDate.HasFee">{{$data.fee_Infant}}</b-col>
        <b-col cols="2" class="fee-default" v-if="planDate.HasFee">
          <b-form-input
            id="input-Fee_Adult"
            type="number"
            size="sm"
            min="0"
            v-model="planDate.Fee_Adult"
            aria-describedby="Fee_Adult-fb"
          ></b-form-input>
        </b-col>
        <b-col cols="2" v-if="planDate.HasFee">
          <b-form-input
            id="input-Fee_Child"
            type="number"
            size="sm"
            min="0"
            v-model="planDate.Fee_Child"
            aria-describedby="Fee_Child-fb"
          ></b-form-input>
        </b-col>
        <b-col cols="2" v-if="planDate.HasFee">
          <b-form-input
            id="input-Fee_Infant"
            type="number"
            size="sm"
            min="0"
            v-model="planDate.Fee_Infant"
            aria-describedby="Fee_Infant-fb"
          ></b-form-input>
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

import DateUtil from "@js/DateUtil";

import PlanDate from "@js/model/PlanDate";

//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

//検証部品
import { validationMixin } from "vuelidate";

export default {
  mixins: [validationMixin],
  components: { DateUtil, FontAwesomeIcon },
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
      planId: 0, //プランID
      fee_Adult: 0, //大人料金
      fee_Child: 0, //児童料金
      fee_Infant: 0, //幼児料金
      selectedDates: [],
      dateFeeList: [],
    };
  },
  computed: {},
  methods: {
    open: function (
      planId,
      fee_Adult,
      fee_Child,
      fee_Infant,
      selectedDates,
      dateFeeList
    ) {
      this.planId = planId;
      this.fee_Adult = fee_Adult;
      this.fee_Child = fee_Child;
      this.fee_Infant = fee_Infant;
      this.selectedDates = selectedDates;

      if (dateFeeList) this.dateFeeList = dateFeeList.concat();
      if (!this.dateFeeList) this.dateFeeList = [];

      this.selectedDates.forEach((acceptDate) => {
        let dateFee = this.dateFeeList.find((e) => {
          return (
            DateUtil.GetDateStringZero(acceptDate) ==
            DateUtil.GetDateStringZero(e.AcceptDate)
          );
        });

        if (dateFee) {
          dateFee.HasFee = true;
        } else {
          //既存の日別料金Etが存在しない場合は 新規にEtを追加
          let planDate = new PlanDate();

          planDate.PlanId = this.planId;
          planDate.AcceptDate = DateUtil.GetDateStringZero(acceptDate);
          planDate.HasFee = false;
          planDate.Fee_Adult = fee_Adult;
          planDate.Fee_Child = fee_Child;
          planDate.Fee_Infant = fee_Infant;
          this.dateFeeList.push(planDate);
        }
      });

      this.$bvModal.show("DateFeeModal");
    },
    close: function () {
      this.$bvModal.hide("DateFeeModal");
    },
    OkClicked: function () {
      var ret = [];
      this.dateFeeList.forEach((dateFee) => {
        if (dateFee.HasFee) {
          ret.push(dateFee);
        }
      });

      this.$emit("DateFeeSetted", ret); //イベント発火
      this.$bvModal.hide("DateFeeModal");
    },
    CancelClicked: function () {
      //this.$emit("CancelClicked"); //イベント発火
      this.$bvModal.hide("DateFeeModal");
    },
    GetDateStringZero: function (date) {
      return (
        DateUtil.GetDateStringZero(date) +
        " (" +
        DateUtil.GetDayOfWeekChar(date) +
        ")"
      );
    },
  },
};
</script>