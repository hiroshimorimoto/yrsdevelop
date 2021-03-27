<style lang="scss">
span.vc-day-content {
  cursor: default;
}
.lock-div {
  z-index: 2;
  background-color: rgba(0, 0, 0, 0);
  width: 100%;
  height: 230px;
  top: 40px;
  position: absolute;
}
</style>
<template>
  <b-row>
    <b-col cols="6" style="min-width:270px;">
      <div class="lock-div" v-if="ReadOnly"></div>
      <v-date-picker
        :input-props="inputProps"
        mode="multiple"
        :formats="formats"
        v-model="selectedDates"
        :min-date="StartDate"
        :max-date="EndDate"
        is-inline
      ></v-date-picker>
    </b-col>
    <b-col cols="5" style="min-width:190px;" v-if="ListVisible">
      <p>選択されている日付</p>
      <div style="height:230px;overflow: scroll;">
        <ul class="list-group">
          <li v-for="(date,index) in selectedDates" :key="index">
            <a @click="removeDate(date)">
              <font-awesome-icon icon="minus-circle" style="color:red" />
            </a>
            {{ GetDateStringZero(date) }}
          </li>
        </ul>
      </div>
    </b-col>
  </b-row>
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
  name: "DatesChooser",
  components: { VCalendar, DateUtil, FontAwesomeIcon },
  props: {
    StartDate: {
      type: String,
      default: null,
    },
    EndDate: {
      type: String,
      default: null,
    },
    SelectedDates: {
      type: Array,
      default: [],
    },
    ListVisible: {
      type: Boolean,
      default: true,
    },
    ReadOnly: {
      type: Boolean,
      default: false,
    },
  },
  model: {
    prop: "SelectedDates",
    event: "SelectedDatesChanged",
  },
  data() {
    return {
      formats: {
        input: ["YYYY/MM/DD"],
      },
      selectedDates: [],
    };
  },
  computed: {
    inputProps: function () {
      return { class: "input", name: "event_dates", readonly: this.ReadOnly };
    },
  },
  methods: {
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
  },
  watch: {
    selectedDates: {
      immediate: false,
      handler: function () {
        this.$emit("SelectedDatesChanged", this.selectedDates);
      },
    },
    SelectedDates: {
      immediate: true,
      handler: function () {
        this.selectedDates = this.SelectedDates;
      },
    },
  },
  mounted() {
    //this.selectedDates = this.SelectedDates;
  },
};
</script>