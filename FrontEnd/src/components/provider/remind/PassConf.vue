<style lang="scss" scoped>
</style>
<template>
  <PassConfBase ref="base" @passConfClicked="doPassConf" LoginType="provider">
    <template v-slot:info>
      ・ログインID（登録されたメールアドレス）
      <br />・数字４桁の認証コード
      <br />・新しいパスワード
      <br />を入力し「変更」ボタンを押してください。
    </template>
  </PassConfBase>
</template>


<script>
import ProviderAuthBiz from "@js/biz/ProviderAuthBiz";
import PassConfBase from "../../common/PassConfBase";

//パスワードリセット確認DTO
import ResetPasswordConfirmEt from "@js/dto/ResetPasswordConfirmEt";

export default {
  methods: {
    doPassConf: async function () {
      this.$refs.base.ErrorMessage = "";
      this.$refs.base.Loading = true;

      try {
        var et = new ResetPasswordConfirmEt();

        const keyCode = this.$route.query.k;
        et.KeyCode = keyCode;
        et.UserId = this.$refs.base.LoginId;
        et.AuthCode = this.$refs.base.AuthCode;
        et.NewPassword = this.$refs.base.NewPassword;
        et.NewPasswordConf = this.$refs.base.NewPasswordConf;

        //パスワード変更確認
        await ProviderAuthBiz.ResetPasswordConfirm(et);

        this.$refs.base.Loading = false;
        this.$refs.base.IsComplete = true;
      } catch (e) {
        this.$refs.base.ErrorMessage = e;
        this.$refs.base.Loading = false;
      }
    },
  },
  components: {
    PassConfBase: PassConfBase,
  },
};
</script>