<style lang="scss" scoped>
input[placeholder="YYYY"],
input[placeholder="年"] {
  max-width: 70px;
  text-align: right;
}
input[placeholder="DD"],
input[placeholder="MM"],
input[placeholder="月"],
input[placeholder="日"] {
  max-width: 50px;
  text-align: right;
}

.form-control-sm {
  padding: 0.25rem 0.5rem;
  font-size: 0.875rem;
  line-height: 1.5;
  border-radius: 0.2rem;
}
</style>
<template>
  <b-form-group
    label-size="sm"
    :description="description"
    label-align="left"
    :label-for="ComponentId"
  >
    <b-input-group>
      <b-form-input
        :id="ComponentId"
        :state="_state()"
        v-model="YearValue"
        type="number"
        min="2000"
        max="2999"
        size="sm"
        placeholder="年"
        autocomplete="off"
        :aria-describedby="FeedbackId"
      ></b-form-input>
      <b-form-input
        :id="ComponentId"
        :state="_state()"
        v-model="MonthValue"
        type="number"
        min="1"
        max="12"
        size="sm"
        placeholder="月"
        autocomplete="off"
        :aria-describedby="FeedbackId"
      ></b-form-input>
      <b-form-input
        :id="ComponentId"
        :state="_state()"
        v-model="DateValue"
        type="number"
        min="1"
        :max="DateMax"
        :disabled="DateMax <= 0 || DateMax == '' "
        size="sm"
        placeholder="日"
        autocomplete="off"
        :aria-describedby="FeedbackId"
      ></b-form-input>
      <b-input-group-append>
        <b-input-group-text
          class="form-control form-control-sm"
          size="sm"
          variant="glay"
        >{{DoWName}}</b-input-group-text>
        <b-form-datepicker
          size="sm"
          v-model="PickerValue"
          button-only
          locale="ja-jp"
          @context="onContext"
        ></b-form-datepicker>
      </b-input-group-append>
    </b-input-group>
    <div class="invalid-feedback d-block" :id="FeedbackId">
      <small class="d-block" v-if="DateObjValue == null">日付が不正です</small>
      <slot name="invalid-feedback"></slot>
    </div>
  </b-form-group>
</template>
<script>
import DateUtil from "../../js/DateUtil";
import { validationMixin } from "vuelidate";

export default {
  mixins: [validationMixin],
  props: {
    value: {
      type: String,
      default: ""
    },
    id: {
      type: String,
      default: "DatePicker"
    },
    description: {
      type: String,
      default: ""
    },
    validator: {
      type: Object
    }
  },
  data() {
    return {
      PickerValue: null,
      year: "",
      month: "",
      date: ""
    };
  },
  computed: {
    YearValue: {
      get() {
        return this.year;
      },
      set(val) {
        if (this.year == val) return;
        this.year = val;
      }
    },
    MonthValue: {
      get() {
        return this.month;
      },
      set(val) {
        if (this.month == val) return;
        this.month = val;
      }
    },
    DateValue: {
      get() {
        return this.date;
      },
      set(val) {
        if (this.date == val) return;
        this.date = val;
      }
    },
    DateObjValue: function() {
      if (!this.year || !this.month || !this.date) {
        return null;
      } else {
        return new Date(this.year, this.month - 1, this.date);
      }
    },
    DoWName: function() {
      if (this.DateObjValue == null) return "";
      return DateUtil.GetDayOfWeekChar(this.DateObjValue);
    },
    DateMax: function() {
      var ret = "";
      if (!this.YearValue || !this.MonthValue) return "";
      try {
        ret = new Date(this.YearValue, this.MonthValue, 0).getDate();
        if (this.DateValue <= 0) this.DateValue = "";
        if (this.DateValue > ret) this.DateValue = ret;
      } catch (error) {
        ret = "";
      }
      return ret;
    },
    ComponentId: function() {
      return "input-" + this.Id;
    },
    FeedbackId: function() {
      return "fb-" + this.Id;
    }
  },
  validations: {
    value: {
      dateValue: DateUtil.checkDateYMD
    }
  },
  watch: {
    value: {
      immediate: true,
      handler: function() {
        //modelの更新通知
        if (this.value && DateUtil.checkDateYMD(this.value)) {
          var date = new Date(this.value);

          if (
            this.DateObjValue == null ||
            date.getTime() != this.DateObjValue.getTime()
          ) {
            this.YearValue = date.getFullYear();
            this.MonthValue = date.getMonth() + 1;
            this.DateValue = date.getDate();

            this.PickerValue = date;
          }
        } else {
          this.YearValue = "";
          this.MonthValue = "";
          this.DateValue = "";

          this.PickerValue = null;
        }
      }
    },
    DateObjValue: {
      immediate: false,
      handler: function() {
        var notifyValue;
        if (this.DateObjValue == null) {
          notifyValue = "";
        } else {
          notifyValue = DateUtil.GetDateStringZero(this.DateObjValue);
        }

        if (this.value != notifyValue) {
          this.PickerValue = this.DateObjValue;
          this.$emit("input", notifyValue);
        }
      }
    }
  },
  methods: {
    onContext: function(ctx) {
      if (ctx.selectedDate == null) return;
      this.YearValue = ctx.selectedDate.getFullYear();
      this.MonthValue = ctx.selectedDate.getMonth() + 1;
      this.DateValue = ctx.selectedDate.getDate();
    },
    _state: function() {
      if (this.$v.$invalid) return false;
      if (this.validator) {
        const { $dirty, $error } = this.validator;
        return $dirty ? !$error : null;
      }
    }
  }
};
</script>