<style lang="scss" scoped>
.input-group > .input-group-prepend > .btn-group > .btn,
.input-group > .input-group-append:not(:last-child) > .btn-group > .btn,
.input-group
  > .input-group-append:last-child
  > .btn-group:not(:last-child):not(.dropdown-toggle)
  > .btn {
  border-top-right-radius: 0.25rem;
  border-bottom-right-radius: 0.25rem;
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
        v-model="$v.InputValue.$model"
        type="text"
        placeholder="YYYY/MM/DD"
        autocomplete="off"
        :aria-describedby="FeedbackId"
      ></b-form-input>
      <b-input-group-append>
        <b-form-datepicker v-model="PickerValue" button-only locale="ja-jp" @context="onContext"></b-form-datepicker>
      </b-input-group-append>
      <b-form-invalid-feedback :id="FeedbackId">
        <small v-if="!$v.InputValue.dateValue">日付が不正です</small>
        <slot name="invalid-feedback"></slot>
      </b-form-invalid-feedback>
    </b-input-group>
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
      InputValue: ""
    };
  },
  validations: {
    InputValue: {
      dateValue: DateUtil.checkDateYMD
    }
  },
  watch: {
    value: {
      immediate: true,
      handler: function() {
        //modelの更新通知
        if (DateUtil.checkDateYMD(this.InputValue)) {
          this.InputValue = this.value;
          this.PickerValue = DateUtil.GetDateValue(this.InputValue);
        } else {
          this.InputValue = "";
          this.PickerValue = null;
        }
      }
    },
    InputValue: {
      immediate: false,
      handler: function() {
        //modelの更新通知
        if (DateUtil.checkDateYMD(this.InputValue)) {
          this.$emit("input", this.InputValue);
          this.PickerValue = DateUtil.GetDateValue(this.InputValue);
        } else {
          this.InputValue = "";
          this.PickerValue = null;
        }
      }
    }
  },
  computed: {
    ComponentId: function() {
      return "input-" + this.Id;
    },
    FeedbackId: function() {
      return "fb-" + this.Id;
    }
  },
  methods: {
    onContext: function(ctx) {
      this.InputValue = DateUtil.GetDateStringZero(ctx.selectedDate);
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