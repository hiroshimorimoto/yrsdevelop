<style lang="scss" scoped>
</style>
<template>
  <div>
    <EditModdalBase
      ref="editModalBase"
      :HeaderTitle="HeaderTitle"
      HeaderBgVariant="primary"
      HeaderTextVariant="light"
      @OkClicked="OkClicked"
      @CancelClicked="CancelClicked"
      :OkButtonDisabled="isButtonDisabled"
    >
      <template v-slot:EditModalBody>
        <div v-bind:disabled="Loading">
          <b-row>
            <b-col>
              <!-- 登録ステータス -->
              <div v-if="$refs.editModalBase.EditMode == 'Add'">
                <b-row align-v="center">
                  <b-col cols="3">
                    <h3>
                      <b-badge variant="primary">新規登録</b-badge>
                    </h3>
                  </b-col>
                  <b-col>
                    <b-form-checkbox v-model="IsSendRegistMail">登録完了時に通知メールを送信する</b-form-checkbox>
                  </b-col>
                </b-row>
              </div>
              <div v-if="$refs.editModalBase.EditMode != 'Add'">
                <b-row align-v="center">
                  <b-col cols="3">
                    <h3>
                      <b-badge v-bind:variant="RegistStatusVariant">{{RegistStatusName}}</b-badge>
                    </h3>
                  </b-col>
                  <b-col v-if="RegistStatus < 2">
                    <b-button
                      size="sm"
                      variant="outline-primary"
                      @click="sendRegistMail"
                      v-bind:disabled="isButtonDisabled"
                    >
                      <span
                        v-show="Loading"
                        class="spinner-border spinner-border-sm"
                        role="status"
                        aria-hidden="true"
                      ></span>
                      通知メールを送信する&nbsp;
                      <font-awesome-icon icon="paper-plane" />
                      <small v-if="RegistMailCount > 0">({{RegistMailCount}}回目)</small>
                    </b-button>
                  </b-col>
                </b-row>
              </div>
            </b-col>
          </b-row>
          <b-row>
            <hr />
          </b-row>
          <b-row>
            <b-col>
              <!-- 事業者ID -->
              <b-form-group
                label="事業者ID:"
                label-size="sm"
                :description="_ProviderIdDescription"
                label-cols="2"
                label-align="right"
                label-for="input-ProviderId"
              >
                <b-form-input
                  id="input-ProviderId"
                  type="text"
                  size="sm"
                  v-model="$v.ProviderId.$model"
                  :state="validateState('ProviderId')"
                  aria-describedby="ProviderId-fb"
                  v-bind:disabled="_ProviderIdDisabled"
                ></b-form-input>
                <b-form-invalid-feedback
                  id="ProviderId-fb"
                  v-if="$v.ProviderId.$dirty && !_ProviderIdDisabled"
                >
                  <small class="invalid-feedback d-block" v-if="!$v.ProviderId.required">入力してください</small>
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.ProviderId.required && !$v.ProviderId.email"
                  >有効なメールアドレスを入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
          <b-row>
            <b-col>
              <!-- 事業者名 -->
              <b-form-group
                label="事業者名:"
                label-size="sm"
                description="事業者名を入力してください"
                label-cols="2"
                label-align="right"
                label-for="input-ProviderName"
              >
                <b-form-input
                  id="input-ProviderName"
                  type="text"
                  size="sm"
                  v-model="$v.ProviderName.$model"
                  :state="validateState('ProviderName')"
                  aria-describedby="ProviderName-fb"
                ></b-form-input>
                <b-form-invalid-feedback id="ProviderName-fb" v-if="$v.ProviderName.$dirty">
                  <small class="invalid-feedback d-block" v-if="!$v.ProviderName.required">入力してください</small>
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.ProviderName.required && !$v.ProviderName.maxLength"
                  >50文字以内で入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
          <b-row>
            <b-col>
              <!-- 業種 -->
              <b-form-group
                label="業種:"
                label-size="sm"
                description="業種を入力してください"
                label-cols="2"
                label-align="right"
                label-for="input-ProviderIndustry"
              >
                <b-form-input
                  id="input-ProviderIndustry"
                  type="text"
                  size="sm"
                  v-model="$v.ProviderIndustry.$model"
                  :state="validateState('ProviderIndustry')"
                  aria-describedby="ProviderIndustry-fb"
                ></b-form-input>
                <b-form-invalid-feedback id="ProviderIndustry-fb" v-if="$v.ProviderIndustry.$dirty">
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.ProviderIndustry.required && !$v.ProviderIndustry.maxLength"
                  >50文字以内で入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 住所 -->
              <b-form-group
                label="住所:"
                label-size="sm"
                description="住所を入力してください"
                label-cols="2"
                label-align="right"
                label-for="input-ProviderAddress"
              >
                <b-form-textarea
                  id="input-ProviderAddress"
                  type="text"
                  size="sm"
                  aria-multiline="3"
                  v-model="$v.ProviderAddress.$model"
                  :state="validateState('ProviderAddress')"
                  aria-describedby="ProviderAddress-fb"
                ></b-form-textarea>
                <b-form-invalid-feedback id="ProviderAddress-fb" v-if="$v.ProviderAddress.$dirty">
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.ProviderAddress.required && !$v.ProviderAddress.maxLength"
                  >50文字以内で入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 担当者名 -->
              <b-form-group
                label="担当者名:"
                label-size="sm"
                description="担当者名を入力してください"
                label-cols="2"
                label-align="right"
                label-for="input-TantoName"
              >
                <b-form-input
                  id="input-TantoName"
                  type="text"
                  size="sm"
                  v-model="$v.TantoName.$model"
                  :state="validateState('TantoName')"
                  aria-describedby="TantoName-fb"
                ></b-form-input>
                <b-form-invalid-feedback id="TantoName-fb" v-if="$v.TantoName.$dirty">
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.TantoName.required && !$v.TantoName.maxLength"
                  >50文字以内で入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 担当者メールアドレス -->
              <b-form-group
                label="担当者メールアドレス:"
                label-size="sm"
                description="担当者メールアドレスを入力してください"
                label-cols="5"
                label-align="right"
                label-for="input-TantoMailAddress"
              >
                <b-form-input
                  id="input-TantoMailAddress"
                  type="text"
                  size="sm"
                  v-model="$v.TantoMailAddress.$model"
                  :state="validateState('TantoMailAddress')"
                  aria-describedby="TantoMailAddress-fb"
                ></b-form-input>
                <b-form-invalid-feedback id="TantoMailAddress-fb" v-if="$v.TantoMailAddress.$dirty">
                  <small
                    class="invalid-feedback d-block"
                    v-if="!$v.TantoMailAddress.email"
                  >有効なメールアドレスを入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <!-- 電話番号 -->
              <b-form-group
                label="電話番号:"
                label-size="sm"
                description="電話番号を入力してください"
                label-cols="2"
                label-align="right"
                label-for="input-TantoPhone"
              >
                <b-form-input
                  id="input-TantoPhone"
                  type="text"
                  size="sm"
                  v-model="$v.TantoPhone.$model"
                  :state="validateState('TantoPhone')"
                  aria-describedby="TantoPhone-fb"
                ></b-form-input>
                <b-form-invalid-feedback id="TantoPhone-fb" v-if="$v.TantoPhone.$dirty">
                  <small
                    class="invalid-feedback d-block"
                    v-if="$v.TantoPhone.required && !$v.TantoPhone.maxLength"
                  >50文字以内で入力してください</small>
                </b-form-invalid-feedback>
              </b-form-group>
            </b-col>
          </b-row>
        </div>
      </template>
    </EditModdalBase>
  </div>
</template>
<script>
//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

//検証部品
import { validationMixin } from "vuelidate";
import {
  required,
  minLength,
  maxLength,
  numeric,
  email
} from "vuelidate/lib/validators";

//編集モーダル ベース
import EditModdalBase from "@components/common/EditModalBase";

// 事業者マスタ
import Provider_M from "@js/model/Provider_M";

//事業者Biz
import ManagerBiz from "@js/biz/ManagerBiz";

export default {
  mixins: [validationMixin],
  data() {
    return {
      HeaderTitle: "",
      ProviderId: "", //事業者ID
      RegistStatus: -1, //登録ステータス
      ProviderPassword: "", //パスワード
      ProviderName: "", //事業者名
      ProviderIndustry: "", //業種
      ProviderAddress: "", //住所
      TantoName: "", //担当者名
      TantoMailAddress: "", //担当者メールアドレス
      TantoPhone: "", //電話番号
      PreRegistDate: "", //仮登録日
      ProRegistDate: "", //本登録日
      RegistMailCount: 0, //登録完了メール送信数
      IsSendRegistMail: false //登録メール送信フラグ
    };
  },
  validations: {
    ProviderId: { required, email },
    ProviderName: { required, maxLength: maxLength(50) },
    ProviderIndustry: { maxLength: maxLength(50) },
    ProviderAddress: { maxLength: maxLength(100) },
    TantoName: { maxLength: maxLength(50) },
    TantoMailAddress: { email },
    TantoPhone: { maxLength: maxLength(20) }
  },
  computed: {
    isButtonDisabled: function() {
      return this.$v.$invalid || this.$refs.editModalBase.Loading;
    },
    RegistStatusVariant: function() {
      if (this.RegistStatus == 2) {
        return "primary";
      } else if (this.RegistStatus == 1) {
        return "info";
      } else {
        return "warning";
      }
    },
    RegistStatusName: function() {
      if (this.RegistStatus == 2) {
        return "登録済み";
      } else if (this.RegistStatus == 1) {
        return "登録待ち";
      } else {
        return "仮登録";
      }
    },
    _ProviderIdDisabled: function() {
      if (this.$refs.editModalBase.EditMode == "Add") {
        return false;
      } else {
        return true;
      }
    },
    _ProviderIdDescription: function() {
      if (this.$refs.editModalBase.EditMode == "Add") {
        return "事業者ID(メールアドレス)を入力してください";
      } else {
        return "";
      }
    }
  },
  methods: {
    openAdd: function() {
      var editItem = new Provider_M();

      //値をコピー
      Object.assign(this.$data, editItem);
      this.$refs.editModalBase.open("Add");
      this.HeaderTitle = "事業者 " + this.$refs.editModalBase.EditModeName;
    },
    openEdit: function(editItem) {
      //値をコピー
      Object.assign(this.$data, editItem);

      this.$refs.editModalBase.open("Edit");
      this.HeaderTitle = "事業者 " + this.$refs.editModalBase.EditModeName;
    },
    openRead: function(editItem) {},
    OkClicked: async function() {
      var providerEt = new Provider_M();
      Object.assign(providerEt, this.$data);

      this.$refs.editModalBase.ErrorMessage = "";
      this.$refs.editModalBase.Loading = true;

      //事業者情報のPOST
      try {
        await ManagerBiz.PostProvider(
          this.$refs.editModalBase.EditMode,
          false,
          providerEt
        );

        this.$refs.editModalBase.Loading = false;
        this.$refs.editModalBase.IsComplete = true;

        this.$emit("providerEdited");
        this.$bvToast.toast("事業者が登録されました", {
          variant: "success",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 3000,
          noCloseButton: true,
          textCenter: true
        });

        this.$refs.editModalBase.close();
      } catch (e) {
        this.$refs.editModalBase.ErrorMessage = e;
        this.$refs.editModalBase.Loading = false;
      }
    },
    sendRegistMail: async function() {
      var providerEt = new Provider_M();
      //値のコピー
      Object.assign(providerEt, this.$data);

      this.$refs.editModalBase.ErrorMessage = "";
      this.$refs.editModalBase.Loading = true;

      try {
        //通知メール送信
        await ManagerBiz.SendRegistMail(this.ProviderId);

        this.$refs.editModalBase.Loading = false;
        this.$refs.editModalBase.IsComplete = true;
        this.$emit("providerEdited");

        this.$bvToast.toast("通知メールが送信されました", {
          variant: "success",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 3000,
          noCloseButton: true,
          textCenter: true
        });
      } catch (e) {
        this.$refs.editModalBase.ErrorMessage = e;
        this.$refs.editModalBase.Loading = false;
      }
    },
    CancelClicked: function() {
      this.$refs.editModalBase.close();
    },
    validateState: function(propName) {
      const { $dirty, $error } = this.$v[propName];
      return $dirty ? !$error : null;
    }
  },
  mounted() {
    //this.$refs.editModalBAse.open("Edit");
  },
  components: {
    FontAwesomeIcon,
    EditModdalBase
  }
};
</script>