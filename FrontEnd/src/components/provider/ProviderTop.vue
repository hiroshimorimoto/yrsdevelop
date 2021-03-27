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
import SubHeader from "./ProviderHeader";

//ルート設定
const routes = [
  {
    path: "/",
    // component: planList
    component: () => import("./PlanList"),
  },
  {
    path: "/loginForm",
    // component: loginForm
    component: () => import("./ProviderLogin"),
  },
  {
    path: "/planList",
    // component: planList
    component: () => import("./PlanList"),
  },
  {
    path: "/appList",
    // component: appList
    component: () => import("./AppList"),
  },
  {
    path: "/privacy",
    // component: appList
    component: () => import("./PrivacyPolicy"),
  },];

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