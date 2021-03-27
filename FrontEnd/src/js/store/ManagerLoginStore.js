import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

import ManagerAuthBiz from '@js/biz/ManagerAuthBiz'

const ManagerLoginStore = new Vuex.Store({
    state: {
        _managerLoginInfo: null
    },
    //GETTERS
    getters: {
        isLoggedin: state => !(state._managerLoginInfo == null),
        //managerLoginInfo: (state) => state._managerLoginInfo
    },
    //SETTERS(commit)
    mutations: {
        managerLoginInfo: (state, managerLoginInfo) => state._managerLoginInfo = managerLoginInfo
    },
    //ACTIONS(dispatch)
    actions: {
        checkLogin: async ctx => {
            //ログインチェック
            try {
                const res = await ManagerAuthBiz.CheckLogin();
                ctx.commit("managerLoginInfo", res);
            } catch (e) {
                ctx.commit("managerLoginInfo", null);
            }
        },
        login: async (ctx, { managerPassword }) => {
            //ログイン
            try {
                const res = await ManagerAuthBiz.Login(managerPassword);
                ctx.commit("managerLoginInfo", res);
            } catch (e) {
                ctx.commit("managerLoginInfo", null);
                throw e;
            }
        },
        logout: async ctx => {
            //ログインチェック
            try {
                const res = await ManagerAuthBiz.Logout();
                ctx.commit("managerLoginInfo", null);
            } catch (e) {
                ctx.commit("managerLoginInfo", null);
            }
        },
    }
});

export default ManagerLoginStore;