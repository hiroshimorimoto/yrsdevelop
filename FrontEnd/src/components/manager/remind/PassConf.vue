<style lang="scss" scoped>
</style>
<template>
  <PassConfBase ref="base" @passConfClicked="doPassConf" LoginType="manager">
    <template v-slot:info>
      数字４桁の認証コードと新しいパスワードを入力し
      <br />「変更」ボタンを押してください。
    </template>
  </PassConfBase>
</template>


<script>
import ManagerAuthBiz from "@js/biz/ManagerAuthBiz";
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
        et.AuthCode = this.$refs.base.AuthCode;
        et.NewPassword = this.$refs.base.NewPassword;
        et.NewPasswordConf = this.$refs.base.NewPasswordConf;

        //パスワード変更確認
        await ManagerAuthBiz.ResetPasswordConfirm(et);

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