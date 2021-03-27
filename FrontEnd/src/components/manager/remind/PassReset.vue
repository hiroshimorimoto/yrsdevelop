<style lang="scss" scoped>
</style>
<template>
  <PassResetBase ref="base" @passResetClicked="doPassReset" LoginType="manager">
    <template v-slot:info>
      お好きな 数字４桁の認証コードを入力し「送信」ボタンを押してください。
      <br />管理メールアドレスにパスワードリセットメールが送信されます。
    </template>
  </PassResetBase>
</template>

<script>
import ManagerAuthBiz from "@js/biz/ManagerAuthBiz";
import PassResetBase from "../../common/PassResetBase";

export default {
  methods: {
    //パスワードリセットリクエスト
    doPassReset: async function() {
      this.$refs.base.ErrorMessage = "";
      this.$refs.base.Loading = true;

      try {
        await ManagerAuthBiz.ResetPasswordRequest(this.AuthCode);

        this.$refs.base.Loading = false;
        this.$refs.base.IsComplete = true;
      } catch (e) {
        this.$refs.base.ErrorMessage = e;
        this.$refs.base.Loading = false;
      }
    },
    close: function() {
      window.close();
    }
  },
  mounted() {
    this.$bvModal.show("passResetModal");
  },
  components: {
    PassResetBase: PassResetBase
  }
};
</script>