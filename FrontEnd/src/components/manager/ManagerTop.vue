<style lang="scss" scoped>
</style>

<template>
  <div>
    <SubHeader @menuClicked="headerClicked"></SubHeader>
    <router-view id="body"></router-view>
  </div>
</template>

<script>
//Vue
import Vue from "vue";
//Vueルーター
import VueRouter from "vue-router";
Vue.use(VueRouter);

//ヘッダ
import SubHeader from "./ManagerHeader";

//ルート設定
const routes = [
  {
    path: "/",
    //component: providerList
    component: () => import("./ProviderList"),
  },
  {
    path: "/loginForm",
    // component: managerLogin
    component: () => import("./ManagerLogin"),
  },
  {
    path: "/providerList",
    // component: providerList
    component: () => import("./ProviderList"),
  },
  {
    path: "/reserveList",
    // component: reserveList
    component: () => import("./ReserveList"),
  },
];

var router = new VueRouter({
  routes: routes,
});

export default {
  data() {
    return {
      _currentPath: null,
    };
  },
  components: {
    SubHeader: SubHeader,
  },
  computed: {
    isLoggedIn: function () {
      this.$store.getters.isLoggedin;
    },
    currentPath: {
      get: function () {
        return this._currentPath;
      },
      set: function (newVal) {
        //ログインされていない場合は ログインフォームを表示
        if (!this.$store.getters.isLoggedin) {
          newVal = "loginForm";
        }
        if (this._currentPath == newVal) return;
        this._currentPath = newVal;
        this.$router.replace(this._currentPath);
      },
    },
  },
  methods: {
    headerClicked: function (path) {
      this.currentPath = path;
    },
  },
  mounted() {
    this.currentPath = "/";

    this.$store.watch(
      (state, getters) => getters.isLoggedin,
      (newValue, oldValue) => {
        this.currentPath = "/";
      }
    );
  },
  router: router,
};
</script>