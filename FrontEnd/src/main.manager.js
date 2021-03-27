import Vue from 'vue'
Vue.config.devtools = true;

import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
// Install BootstrapVue
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)

//jQueryの設定
import jquery from 'jquery'
global.jquery = jquery
global.$ = jquery
window.$ = window.jquery = require('jquery');



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
import managerLoginStore from "@js/store/ManagerLoginStore";

//共通ヘッダ
import CommonHeader from './components/CommonHeader';

//マネージャ トップコンポーネント
import ManagerTop from './components/manager/ManagerTop'

//マネージャ パスワードリマインダ
import RemindTop from './components/manager/remind/RemindTop'





var path = location.pathname
//console.info(path);

new Vue({
  el: '#commonHeader', // アプリをマウントする要素(セレクタで指定)
  components: { CommonHeader }, //マウントするコンポーネント
  template: '<CommonHeader/>' // elの中に設定するテンプレート
})
if (path == "/manager/remind/remind.html") {
  new Vue({
    el: '#app',
    components: { RemindTop },
    template: '<RemindTop/>',
  })
} else {
  //ログインチェック
  managerLoginStore.dispatch("checkLogin")
    .then(() => {
      new Vue({
        el: '#app',
        components: { ManagerTop },
        template: '<ManagerTop/>',
        store: managerLoginStore
      })
    });
}