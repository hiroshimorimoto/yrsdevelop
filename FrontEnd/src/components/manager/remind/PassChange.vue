<style lang="scss" scoped>
</style>
<template>
  <PassChangeBase ref="base" @passChangeClicked="doPassChange" LoginType="manager"></PassChangeBase>
</template>

<script>
import ManagerAuthBiz from "@js/biz/ManagerAuthBiz";
import PassChangeBase from "../../common/PassChangeBase";

export default {
  methods: {
    //パスワード変更
    doPassChange: async function() {
      this.$refs.base.ErrorMessage = "";
      this.$refs.base.Loading = true;

      try {
        await ManagerAuthBiz.ChangePassword(
          this.$refs.base.OldPassword,
          this.$refs.base.NewPassword,
          this.$refs.base.NewPasswordConf
        );

        this.$refs.base.Loading = false;
        this.$refs.base.IsComplete = true;
      } catch (e) {
        this.$refs.base.ErrorMessage = e;
        this.$refs.base.Loading = false;
      }
    }
  },
  components: {
    PassChangeBase: PassChangeBase
  }
};
</script>