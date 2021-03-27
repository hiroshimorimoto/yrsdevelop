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

//プランモデル 取得リクエスト
import AppEditModelRequest from "@js/dto/AppEditModelRequest";
import AppEditModelRequestItem from "@js/dto/AppEditModelRequestItem";

var getParam = function (name, url) {
  if (!url) url = window.location.href;
  name = name.replace(/[\[\]]/g, "\\$&");
  var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
    results = regex.exec(url);
  if (!results) return null;
  if (!results[2]) return "";
  return decodeURIComponent(results[2].replace(/\+/g, " "));
};

//申込編集モデルの初期値をサーバから取得する為のリクエストを生成
var appEditModelRequest = new AppEditModelRequest();

var initPath = "/";
var initAppId = getParam("appId");

if (initAppId) {
  //決済完了画面
  initAppId = new Number(initAppId);
  initPath = "PayComp";
} else {
  var i = 0;
  while (true) {
    var key = "plan" + (i + 1).toString().padStart(2, "0");

    var query = getParam(key);
    if (query) {
      var querySplit = query.split(",");

      var planId = querySplit[0];
      var planDate = querySplit[1];

      let reqItem = new AppEditModelRequestItem();
      reqItem.PlanId = planId; //プランID
      reqItem.AcceptDate = planDate; //申込日

      appEditModelRequest.PlanIdList.push(reqItem);
    } else {
      break;
    }
    i++;
  }
}

//ルート設定
const routes = [
  {
    path: "/",
    component: () => import("./AppEdit"),
    props: {
      AppEditModelRequest: appEditModelRequest,
      InitMode: "Edit",
    },
  },
  {
    path: "/edit",
    component: () => import("./AppEdit"),
    props: {
      AppEditModelRequest: appEditModelRequest,
      InitMode: "Edit",
    },
  },
  {
    path: "/PayComp",
    component: () => import("./AppEdit"),
    props: {
      //AppEditModelRequest: appEditModelRequest,
      InitApplicationId: initAppId,
      InitMode: "PayComp",
    },
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
    this.currentPath = initPath;
  },
  router: router,
};
</script>