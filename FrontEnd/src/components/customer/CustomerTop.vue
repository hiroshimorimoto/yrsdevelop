<style lang="scss" scoped>
</style>

<template>
  <div>
    <router-view id="body"></router-view>
  </div>
</template>

<script>
//Vue
import Vue from "vue";
//Vueルーター
import VueRouter from "vue-router";
Vue.use(VueRouter);

//ルート設定
const routes = [
  {
    path: "/",
    component: () => import("./CustomerMain"),
  },
  {
    path: "/Main",
    component: () => import("./CustomerMain"),
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
  components: {},
  computed: {
    currentPath: {
      get: function () {
        return this._currentPath;
      },
      set: function (newVal) {
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
  },
  router: router,
};
</script>