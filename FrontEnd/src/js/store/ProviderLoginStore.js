import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

import ProviderAuthBiz from '@js/biz/ProviderAuthBiz'

const ProviderLoginStore = new Vuex.Store({
    state: {
        _providerLoginInfo: null
    },
    //GETTERS
    getters: {
        isLoggedin: state => !(state._providerLoginInfo == null),
        //providerLoginInfo: (state) => state._providerLoginInfo
    },
    //SETTERS(commit)
    mutations: {
        providerLoginInfo: (state, providerLoginInfo) => state._providerLoginInfo = providerLoginInfo
    },
    //ACTIONS(dispatch)
    actions: {
        checkLogin: async ctx => {
            //ログインチェック
            try {
                const res = await ProviderAuthBiz.CheckLogin();
                ctx.commit("providerLoginInfo", res);
            } catch (e) {
                ctx.commit("providerLoginInfo", null);
            }
        },
        login: async (ctx, { providerId, providerPassword }) => {
            //ログイン
            try {
                const res = await ProviderAuthBiz.Login(providerId, providerPassword);
                ctx.commit("providerLoginInfo", res);
            } catch (e) {
                ctx.commit("providerLoginInfo", null);
                throw e;
            }
        },
        logout: async ctx => {
            //ログアウト
            try {
                const res = await ProviderAuthBiz.Logout();
                ctx.commit("providerLoginInfo", null);
            } catch (e) {
                ctx.commit("providerLoginInfo", null);
            }
        },
    }
});

export default ProviderLoginStore;