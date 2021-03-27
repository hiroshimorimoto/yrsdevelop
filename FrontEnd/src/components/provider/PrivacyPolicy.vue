<style lang="scss" scoped>
</style>
<template>
  <div>
    <h5>個人情報保護方針</h5>
    <b-row>
      <b-col>
        <b-form-textarea
          id="input-PersonalinfoManagement"
          type="textarea"
          size="sm"
          rows="10"
          max-rows="30"
          v-model="$v.PersonalinfoManagement.$model"
          :state="ValidateState('PersonalinfoManagement')"
          aria-describedby="PersonalinfoManagement-fb"
        ></b-form-textarea>
        <b-form-invalid-feedback id="PersonalinfoManagement-fb">
          <small v-if="!$v.PersonalinfoManagement.required"
            >入力してください</small
          >
          <small
            class="invalid-feedback d-block"
            v-if="
              $v.PersonalinfoManagement.required &&
              !$v.PersonalinfoManagement.maxLength
            "
            >5000文字以内で入力してください</small
          >
        </b-form-invalid-feedback>
      </b-col>
    </b-row>
    <b-row><b-col>&nbsp;</b-col> </b-row>
    <b-row>
      <b-col class="text-center">
        <b-button size="m" variant="success" @click="doSave">登録</b-button>
      </b-col>
    </b-row>
  </div>
</template>
<script>
// jquery
import $ from "jquery";
//検証部品
import { validationMixin } from "vuelidate";
import { required, maxLength } from "vuelidate/lib/validators";

import ProviderBiz from "@js/biz/ProviderBiz";

export default {
  mixins: [validationMixin],
  name: "PrivacyPolicy",
  components: {},
  data() {
    return {
      PersonalinfoManagement: "",
    };
  },
  validations: {
    PersonalinfoManagement: { required, maxLength: maxLength(5000) },
  },
  methods: {
    ValidateState(propName) {
      const { $dirty, $error } = this.$v[propName];
      return $dirty ? !$error : null;
    },
    async doSave() {
      try {
        //プラン情報のPOST
        await ProviderBiz.PostPrivacyPolicy(this.PersonalinfoManagement);

        this.$bvToast.toast("個人情報保護方針 が変更されました", {
          variant: "success",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 3000,
          noCloseButton: true,
          textCenter: true,
        });
      } catch (e) {
        console.error(e);
        this.$bvToast.toast("個人情報保護方針 の更新中にエラーが発生しました", {
          variant: "danger",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 5000,
          noCloseButton: true,
          textCenter: true,
        });
      }
    },
  },
  mounted: async function () {
    this.PersonalinfoManagement = await ProviderBiz.GetPrivacyPolicy();
  },
};
</script>