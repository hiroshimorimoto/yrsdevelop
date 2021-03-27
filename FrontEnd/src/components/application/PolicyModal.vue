<style lang="scss" scoped>
.text-area {
  white-space: pre-wrap;
}
</style>
<template>
  <div>
    <EditModdalBase
      ref="editModalBase"
      :HeaderTitle="HeaderTitle"
      HeaderBgVariant="warning"
      HeaderTextVariant="dark"
      @CancelClicked="CancelClicked"
    >
      <template v-slot:EditModalBody>
        <div>
          <b-row>
            <b-col>
              <div class="text-area">{{ PolicyText }}</div>
            </b-col>
          </b-row>
        </div>
      </template>
    </EditModdalBase>
  </div>
</template>
<script>
//編集モーダル ベース
import EditModdalBase from "@components/common/EditModalBase";

import CustomerBiz from "@js/biz/CustomerBiz";

export default {
  name: "PolicyModal",
  data() {
    return {
      PolicyText: "",
      HeaderTitle: "",
    };
  },
  computed: {},
  methods: {
    async openPrivacyPolicy(planId) {
      try {
        this.PolicyText = await CustomerBiz.GetPrivacyPolicyFromPlanId(planId);
        this.HeaderTitle = "個人情報保護方針";

        this.$refs.editModalBase.open("Read");
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
    async openCancelPolicy(cancelPolicy) {
      try {
        this.PolicyText = cancelPolicy;
        this.HeaderTitle = "キャンセルポリシー";

        this.$refs.editModalBase.open("Read");
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
    CancelClicked() {
      this.$refs.editModalBase.close();
    },
  },
  mounted() {},
  components: {
    EditModdalBase,
  },
};
</script>