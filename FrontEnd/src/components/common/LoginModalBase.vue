<style lang="scss" scoped>
</style>
<template>
  <div>
    <b-modal
      id="loginModal"
      centered
      no-close-on-backdrop
      no-close-on-esc
      :header-bg-variant="headerBgVariant"
      :header-text-variant="headerTextVariant"
    >
      <!-- ヘッダ -->
      <template v-slot:modal-header="{}">
        <h4 class="modal-title" id="loginModalLabel">{{LoginTypeName}} ログイン</h4>
      </template>
      <!-- ボディ -->
      <template v-slot:default="{}">
        <!-- ログインID （動的）-->
        <b-row v-if="showLoginId">
          <b-col>
            <!-- ログインID -->
            <b-form-group
              label="ログインID:"
              description="ログインID(メールアドレス)を入力してください"
              label-cols-sm="4"
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
        <!-- パスワード -->
        <b-row>
          <b-col>
            <b-form-group
              label="パスワード:"
              description="パスワードを入力してください"
              label-cols-sm="4"
              label-align="center"
              label-for="input-password"
            >
              <b-form-input
                id="input-password"
                type="password"
                v-model="$v.Password.$model"
                :state="validateState('Password')"
                aria-describedby="Password-fb"
                v-bind:disabled="Loading"
              ></b-form-input>
              <b-form-invalid-feedback id="Password-fb">
                <span class="invalid-feedback d-block" v-show="!$v.Password.require">入力してください</span>
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
        </b-row>
        <!-- メッセージエリア -->
        <b-row>
          <b-col class="text-center">
            <p class="text-danger">{{ErrorMessage}}</p>
          </b-col>
        </b-row>
        <!-- リンクエリア -->
        <b-row>
          <b-col class="text-center">
            <a @click="goPassChange" href="javascript:void(0)">パスワード変更</a>
          </b-col>
        </b-row>
        <b-row>
          <b-col class="text-center">
            <b-link @click="goPassReset" href="javascript:void(0)">パスワードを忘れた方はこちら</b-link>
          </b-col>
        </b-row>
      </template>

      <!-- フッタ -->
      <template v-slot:modal-footer>
        <b-button
          size="m"
          variant="primary"
          @click="loginClicked"
          v-bind:disabled="isButtonDisabled"
        >
          <span
            v-show="Loading"
            class="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></span>
          ログイン
        </b-button>
      </template>
    </b-modal>
  </div>
</template>
<script>
import { validationMixin } from "vuelidate";
import { required, email } from "vuelidate/lib/validators";
import RemindMixin from "../common/RemindMixin";
export default {
  mixins: [validationMixin, RemindMixin],
  data() {
    return {
      LoginId: "",
      Password: ""
    };
  },
  validations: function() {
    var ret = {
      Password: { required }
    };
    if (this.showLoginId) {
      ret.LoginId = {
        required: required,
        email: email
      };
    }
    return ret;
  },
  computed: {},
  methods: {
    //ログイン処理
    loginClicked: function() {
      this.$emit("loginClicked"); //イベント発火
    }
  },
  mounted() {
    this.$bvModal.show("loginModal");
  }
};
</script>