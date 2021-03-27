<style lang="scss" scoped>
</style>
<template>
  <div>
    <div>
      <b-modal
        id="passChangeModal"
        centered
        no-close-on-backdrop
        no-close-on-esc
        :header-bg-variant="headerBgVariant"
        :header-text-variant="headerTextVariant"
      >
        <!-- ヘッダ -->
        <template v-slot:modal-header="{  }">
          <h4 class="modal-title" id="passChangeModalLabel">{{LoginTypeName}} パスワード変更</h4>
        </template>
        <!-- ボディ -->
        <template v-slot:default="{}">
          <div v-if="!IsComplete">
            <!-- ログインID （動的）-->
            <b-row v-if="showLoginId">
              <b-col class="text-center">
                <!-- ログインID -->
                <b-form-group
                  label="ログインID:"
                  description="ログインID(メールアドレス)を入力してください"
                  label-cols-sm="5"
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
                  label="現パスワード:"
                  description="現在のパスワードを入力してください"
                  label-cols="5"
                  label-align="center"
                  label-for="input-oldPassword"
                >
                  <b-form-input
                    id="input-oldPassword"
                    type="password"
                    v-model="$v.OldPassword.$model"
                    :state="validateState('OldPassword')"
                    aria-describedby="OldPassword-fb"
                    v-bind:disabled="Loading"
                  ></b-form-input>
                  <b-form-invalid-feedback id="OldPassword-fb">
                    <span class="invalid-feedback d-block" v-show="!$v.OldPassword.require">入力してください</span>
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
                    <span class="invalid-feedback d-block" v-show="!$v.NewPassword.require">入力してください</span>
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
                    <span
                      class="invalid-feedback d-block"
                      v-if="!$v.NewPasswordConf.required"
                    >入力してください</span>
                    <span
                      class="invalid-feedback d-block"
                      v-if="$v.NewPasswordConf.required && !$v.NewPasswordConf.sameAs"
                    >パスワードが異なります</span>
                  </b-form-invalid-feedback>
                </b-form-group>
              </b-col>
            </b-row>
          </div>
          <!-- 完了メッセージ -->
          <div v-if="IsComplete">
            <b-row>
              <b-col class="text-center">
                <p class="text-info">パスワードが変更されました。<br/>ログイン画面からログインしてください。</p>
              </b-col>
            </b-row>
          </div>
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
            v-show="!IsComplete"
            size="m"
            variant="primary"
            @click="doPassChange"
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
  </div>
</template>

<script>
import ManagerAuthBiz from "@js/biz/ManagerAuthBiz";
import { validationMixin } from "vuelidate";
import { required, email, sameAs } from "vuelidate/lib/validators";
import RemindMixin from "./RemindMixin";

export default {
  mixins: [validationMixin, RemindMixin],
  data() {
    return {
      LoginId: "",
      OldPassword: "",
      NewPassword: "",
      NewPasswordConf: ""
    };
  },
  validations: function() {
    var ret = {
      OldPassword: { required },
      NewPassword: { required },
      NewPasswordConf: {
        required: required,
        sameAs: sameAs("NewPassword")
      }
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
    //パスワード変更
    doPassChange: async function() {
      this.$emit("passChangeClicked"); //イベント発火
    }
  },
  mounted() {
    this.$bvModal.show("passChangeModal");
  }
};
</script>