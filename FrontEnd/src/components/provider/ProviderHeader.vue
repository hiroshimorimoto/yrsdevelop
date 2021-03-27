<style lang="scss" scoped>
</style>

<template>
  <b-navbar toggleable="sm" type="dark" variant="success">
    <b-navbar-brand href="#">{{menu_title}}</b-navbar-brand>

    <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

    <b-collapse id="nav-collapse" is-nav>
      <b-navbar-nav>
        <b-nav-item v-if="isLoggedin" href="#" @click="goLink('planList')">プラン一覧</b-nav-item>
        <b-nav-item v-if="isLoggedin" href="#" @click="goLink('appList')">申込一覧</b-nav-item>
        <b-nav-item v-if="isLoggedin" href="#" @click="goLink('privacy')">個人情報保護方針</b-nav-item>
      </b-navbar-nav>

      <b-navbar-nav class="ml-auto">
        <span
          v-show="Loading"
          class="spinner-border spinner-border-sm"
          role="status"
          aria-hidden="true"
        ></span>
        <b-nav-item v-if="isLoggedin" href="#" @click="doLogout">ログアウト</b-nav-item>
      </b-navbar-nav>
    </b-collapse>
  </b-navbar>
</template>

<script>
import providerLoginStore from "@js/store/ProviderLoginStore";

export default {
  data() {
    return {
      Loading: false,
      IsComplete: false,
      menu_title: "事業者",
      currentPage: "",
    };
  },
  computed: {
    isLoggedin: () => providerLoginStore.getters.isLoggedin,
  },
  methods: {
    goLink: function (path) {
      if (this.currentPage == path) return;
      this.currentPage = path;
      //イベント発火
      this.$emit("menuClicked", path);
    },
    doLogout: function () {
      this.Loading = true;
      this.$store
        .dispatch("logout")
        .then(() => {
          if (this.$store.getters.isLoggedin) {
          }
          this.Loading = false;
        })
        .catch((e) => {
          // this.ErrorMessage = e;
          this.Loading = false;
        });
    },
  },
};
</script>