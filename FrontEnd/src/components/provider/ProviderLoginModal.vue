<style lang="scss" scoped>
</style>
<template>
  <LoginModalBase ref="base" @loginClicked="doLogin" LoginType="provider"></LoginModalBase>
</template>
<script>
import LoginModalBase from "../common/LoginModalBase";
export default {
  methods: {
    //ログイン処理
    doLogin: function() {
      this.$refs.base.ErrorMessage = "";
      this.$refs.base.Loading = true;

      this.$store
        .dispatch("login", {
          providerId: this.$refs.base.LoginId,
          providerPassword: this.$refs.base.Password
        })
        .then(() => {
          if (this.$store.getters.isLoggedin) {
            this.$bvModal.hide("loginModal");
          }
          //this.$refs.base.Loading = false;
        })
        .catch(e => {
          this.$refs.base.ErrorMessage = e;
          this.$refs.base.Loading = false;
        });
    }
  },
  components: {
    LoginModalBase: LoginModalBase
  }
};
</script>