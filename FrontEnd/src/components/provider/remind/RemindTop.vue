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
import SubHeader from "../ProviderHeader";

//コンテンツ コンポーネント
// import passChange from "./PassChange";
// import passReset from "./PassReset";
// import passConf from "./PassConf";

//ルート設定
const routes = [
  {
    path: "/passchange",
    // component: passChange
    component: () => import("./PassChange")
  },
  {
    path: "/passreset",
    // component: passReset
    component: () => import("./PassReset")
  },
  {
    path: "/passconf",
    // component: passConf
    component: () => import("./PassConf")
  }
];

var router = new VueRouter({
  routes: routes
});

export default {
  data() {
    return {
      _currentPath: null
    };
  },
  components: {
    SubHeader: SubHeader
  },
  computed: {
    currentPath: {
      get: function() {
        return this._currentPath;
      },
      set: function(newVal) {
        this._currentPath = newVal;
        this.$router.replace(this._currentPath);
      }
    }
  },
  methods: {
    headerClicked: function(path) {
      this.currentPath = path;
    }
  },
  mounted() {
    //this.currentPath = "/passchange";
  },
  router: router
};
</script>