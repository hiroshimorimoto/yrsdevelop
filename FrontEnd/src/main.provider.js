import Vue from 'vue'
Vue.config.devtools = true;

// jquery
import jquery from "jquery";

//BreakPointイベント検出用Divを追加
var mqClasses = ["d-sm-block", "d-md-block", "d-lg-block", "d-xl-block"];
var mq = jquery(
  "<div id='mqDetector' style='visibility:hidden;width: 100%; position:absolute;'></div>"
).appendTo(jquery("body"));

//mq.append("<div class='d-sm-none d-block'></div>");
jquery.each(mqClasses, function (idx, val) {
  mq.append("<div class='" + val + " d-none'></div>");
});


import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
// Install BootstrapVue
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)

//jQueryの設定
// import jQuery from 'jquery'
// global.jquery = jQuery
// global.$ = jQuery
// window.$ = window.jQuery = require('jquery')


//axiosの設定
import axios from 'axios'
import VueAxios from 'vue-axios'
Vue.use(VueAxios, axios)
Vue.axios.defaults.withCredentials = true
Vue.axios.defaults.xsrfHeaderName = 'X-CSRF-Token'

//vuelidate
import Vuelidate from 'vuelidate'
Vue.use(Vuelidate)

//Store
import providerLoginStore from "@js/store/ProviderLoginStore";

//共通ヘッダ
import CommonHeader from './components/CommonHeader';

//マネージャ トップコンポーネント
import ProviderTop from './components/provider/ProviderTop'

//マネージャ パスワードリマインダ
import RemindTop from './components/provider/remind/RemindTop'

var path = location.pathname
//console.info(path);



new Vue({
  el: '#commonHeader', // アプリをマウントする要素(セレクタで指定)
  components: { CommonHeader }, //マウントするコンポーネント
  template: '<CommonHeader/>' // elの中に設定するテンプレート
})
if (path == "/provider/remind.html") {
  new Vue({
    el: '#app',
    components: { RemindTop },
    template: '<RemindTop/>',
  })
} else {
  //ログインチェック
  providerLoginStore.dispatch("checkLogin")
    .then(() => {
      new Vue({
        el: '#app',
        components: { ProviderTop },
        template: '<ProviderTop/>',
        store: providerLoginStore
      })
    });
}

