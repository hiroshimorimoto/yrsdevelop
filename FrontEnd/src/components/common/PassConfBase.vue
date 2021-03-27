<style lang="scss" scoped>
</style>
<template>
  <div>
    <b-modal
      id="passConfModal"
      centered
      no-close-on-backdrop
      no-close-on-esc
      :header-bg-variant="headerBgVariant"
      :header-text-variant="headerTextVariant"
    >
      <!-- ヘッダ -->
      <template v-slot:modal-header="{  }">
        <h4 class="modal-title" id="passConfModalLabel">{{LoginTypeName}} パスワード再設定</h4>
      </template>
      <!-- ボディ -->
      <template v-slot:default="{}">
        <div v-if="!IsComplete">
          <b-row>
            <b-col class="text-center">
              <small class="text-info">
                <slot name="info"></slot>
              </small>
            </b-col>
          </b-row>
          <!-- ログインID （動的）-->
          <b-row v-if="showLoginId">
            <b-col class="text-center">
              <!-- ログインID -->
              <b-form-group
                label="ログインID:"
                description="ログインID(メールアドレス)を入力してください"
                label-cols="5"
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
                description="数字４桁の認証コードを入力してください"
                label-cols="5"
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
                  class="width:80px"
                ></b-form-input>
                <b-form-invalid-feedback id="authCode-fb" v-if="$v.AuthCode.$dirty">
                  <small class="invalid-feedback d-block" v-if="!$v.AuthCode.req">入力してください</small>
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.AuthCode.req && (!$v.AuthCode.min || !AuthCode.max)"
                  >4桁で入力してください</small>
                  <span class="invalid-feedback d-block" v-if="!$v.AuthCode.num">数値で入力してください</span>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
          <b-row>
            <b-col class="text-center">
              <b-form-group
                label="新パスワード:"
                description="新しいパスワードを入力してください"
                label-cols="5"
                label-align="center"
                label-for="input-newPassword"
              >
                <b-form-input
                  id="input-newPassword"
                  type="password"
                  v-model="$v.NewPassword.$model"
                  :state="validateState('NewPassword')"
                  aria-describedby="NewPassword-fb"
                  v-bind:disabled="Loading"
                ></b-form-input>
                <b-form-invalid-feedback id="NewPassword-fb">
                  <small class="invalid-feedback d-block" v-show="!$v.AuthCode.required">入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
          <b-row>
            <b-col class="text-center">
              <b-form-group
                label="新パスワード(確認):"
                description="新しいパスワードを再度入力してください"
                label-cols="5"
                label-align="center"
                label-for="input-newPasswordConf"
              >
                <b-form-input
                  id="input-newPasswordConf"
                  type="password"
                  v-model="$v.NewPasswordConf.$model"
                  :state="validateState('NewPasswordConf')"
                  aria-describedby="newPasswordConf-fb"
                  v-bind:disabled="Loading"
                ></b-form-input>
                <b-form-invalid-feedback id="newPasswordConf-fb" v-if="$v.NewPasswordConf.$dirty">
                  <small
                    class="invalid-feedback d-block"
                    v-if="!$v.NewPasswordConf.required"
                  >入力してください</small>
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.NewPasswordConf.required && !$v.NewPasswordConf.sameAs"
                  >パスワードが異なります</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
        </div>
        <!-- 完了メッセージ -->
        <b-row v-if="IsComplete">
          <b-col class="text-center">
            <p class="text-info">パスワードが変更されました。ログイン画面からログインしてください。</p>
          </b-col>
        </b-row>
        <!-- メッセージエリア -->
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
          @click="passConfClicked"
          v-bind:disabled="isButtonDisabled"
        >
          <span
            v-show="Loading"
            class="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></span>
          変更
        </b-button>
        <!-- 完了メッセージ -->
        <b-button size="m" variant="primary" @click="goLogin" v-if="IsComplete">ログイン画面へ</b-button>
      </template>
    </b-modal>
  </div>
</template>

<script>
import ManagerAuthBiz from "@js/biz/ManagerAuthBiz";
import { validationMixin } from "vuelidate";
import {
  required,
  minLength,
  maxLength,
  numeric,
  sameAs,
  email,
} from "vuelidate/lib/validators";

//パスワードリセット確認DTO
import ResetPasswordConfirmEt from "@js/dto/ResetPasswordConfirmEt";
import RemindMixin from "./RemindMixin";

export default {
  mixins: [validationMixin, RemindMixin],
  data() {
    return {
      LoginId: "",
      AuthCode: "",
      NewPassword: "",
      NewPasswordConf: "",
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
      NewPassword: { required },
      NewPasswordConf: {
        required: required,
        sameAs: sameAs("NewPassword"),
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
    //パスワード変更
    passConfClicked: async function () {
      this.$emit("passConfClicked"); //イベント発火
    },
  },
  mounted() {
    this.$bvModal.show("passConfModal");
  },
};
</script>