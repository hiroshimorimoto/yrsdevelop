<style lang="scss" scoped>
</style>
<template>
  <PassResetBase ref="base" @passResetClicked="doPassReset" LoginType="provider">
    <template v-slot:info>
      ・ログインID（登録されたメールアドレス）
      <br />・お好きな 数字４桁の認証コード
      <br />を入力し「送信」ボタンを押してください。
      <br />登録されたメールアドレスにパスワードリセットメールが送信されます。
    </template>
  </PassResetBase>
</template>

<script>
import ProviderAuthBiz from "@js/biz/ProviderAuthBiz";
import PassResetBase from "../../common/PassResetBase";

export default {
  methods: {
    //パスワードリセットリクエスト
    doPassReset: async function () {
      this.$refs.base.ErrorMessage = "";
      this.$refs.base.Loading = true;

      try {
        await ProviderAuthBiz.ResetPasswordRequest(
          this.$refs.base.LoginId,
          this.$refs.base.AuthCode
        );

        this.$refs.base.Loading = false;
        this.$refs.base.IsComplete = true;
      } catch (e) {
        this.$refs.base.ErrorMessage = e;
        this.$refs.base.Loading = false;
      }
    },
  },
  mounted() {
    this.$bvModal.show("passResetModal");
  },
  components: {
    PassResetBase: PassResetBase,
  },
};
</script>