<style lang="scss" scoped>
</style>
<template>
  <div>
    <b-modal
      id="passResetModal"
      centered
      no-close-on-backdrop
      no-close-on-esc
      :header-bg-variant="headerBgVariant"
      :header-text-variant="headerTextVariant"
    >
      <!-- ヘッダ -->
      <template v-slot:modal-header="{  }">
        <h4 class="modal-title" id="passResetModalLabel">{{LoginTypeName}} パスワード再設定</h4>
      </template>
      <!-- ボディ -->
      <template v-slot:default>
        <div v-if="!IsComplete">
          <b-row>
            <b-col class="text-center">
              <small class="text-info">
                <slot name="info"></slot>
              </small>
            </b-col>
          </b-row>
          <b-row>
            <hr />
          </b-row>
          <!-- ログインID （動的）-->
          <b-row v-if="showLoginId">
            <b-col class="text-center">
              <!-- ログインID -->
              <b-form-group
                label="ログインID:"
                description="ログインID(メールアドレス)を入力してください"
                label-cols="4"
                label-align="center"
                label-for="input-password"
              >
                <b-form-input
                  id="input-password"
                  type="text"
                  v-model="$v.LoginId.$model"
                  :state="validateState('LoginId')"
                  aria-describedby="LoginId-fb"
                  v-bind:disabled="Loading"
                ></b-form-input>
                <b-form-invalid-feedback id="LoginId-fb" v-if="$v.LoginId.$dirty">
                  <small class="invalid-feedback d-block" v-if="!$v.LoginId.required">入力してください</small>
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.LoginId.required && !$v.LoginId.email"
                  >有効なメールアドレスを入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
          <b-row>
            <b-col class="text-center">
              <b-form-group
                label="認証コード:"
                description="お好きな４桁の数値を入力してください"
                label-cols="4"
                label-align="center"
                label-for="input-authCode"
              >
                <b-form-input
                  id="input-authCode"
                  type="text"
                  v-model="$v.AuthCode.$model"
                  :state="validateState('AuthCode')"
                  aria-describedby="authCode-fb"
                  v-bind:disabled="Loading"
                ></b-form-input>
                <b-form-invalid-feedback id="authCode-fb" v-if="$v.AuthCode.$dirty">
                  <span class="invalid-feedback d-block" v-if="!$v.AuthCode.req">入力してください</span>
                  <span
                    class="invalid-feedback d-block"
                    v-if="$v.AuthCode.req && (!$v.AuthCode.min || !$v.AuthCode.max)"
                  >4桁で入力してください</span>
                  <span class="invalid-feedback d-block" v-if="!$v.AuthCode.num">数値で入力してください</span>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
        </div>
        <!-- メッセージエリア -->
        <b-row v-if="IsComplete">
          <b-col class="text-center">
            <p class="text-info">パスワードリセットメールが送信されました。メールのリンクから新しいパスワードを設定してください</p>
          </b-col>
        </b-row>
        <!-- エラーメッセージエリア -->
        <b-row>
          <b-col class="text-center">
            <p class="text-danger">{{ErrorMessage}}</p>
          </b-col>
        </b-row>
      </template>
      <!-- フッタ -->
      <template v-slot:modal-footer>
        <b-button
          v-if="!IsComplete"
          size="m"
          variant="primary"
          @click="passResetClicked"
          v-bind:disabled="isButtonDisabled"
        >
          <span
            v-show="Loading"
            class="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></span>
          送信
        </b-button>
        <b-button v-if="IsComplete" size="m" variant="primary" @click="close">閉じる</b-button>
      </template>
    </b-modal>
  </div>
</template>

<script>
//import ManagerAuthBiz from "@js/biz/ManagerAuthBiz";
import { validationMixin } from "vuelidate";
import {
  required,
  minLength,
  maxLength,
  numeric,
  email,
} from "vuelidate/lib/validators";
import RemindMixin from "../common/RemindMixin";

export default {
  mixins: [validationMixin, RemindMixin],
  data() {
    return {
      LoginId: "",
      AuthCode: "",
    };
  },
  validations: function () {
    var ret = {
      AuthCode: {
        req: required,
        min: minLength(4),
        max: maxLength(4),
        num: numeric,
      },
    };
    if (this.showLoginId) {
      ret.LoginId = {
        required: required,
        email: email,
      };
    }
    return ret;
  },
  computed: {},
  methods: {
    //パスワードリセットリクエスト
    passResetClicked: async function () {
      this.$emit("passResetClicked"); //イベント発火
    },
    close: function () {
      this.$bvModal.hide("passResetModal");
      //window.close();
    },
  },
  mounted() {
    this.$bvModal.show("passResetModal");
  },
};
</script>