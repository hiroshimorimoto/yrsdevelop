<style lang="scss">
@import "../../style/custom.scss";
hr {
  height: 1px;
  border-color: #ffc107;
  margin-top: 10px;
}
</style>
<template>
  <div v-if="value != null">
    <b-row>
      <b-col>
        <hr class="sub" />
      </b-col>
    </b-row>

    <!-- ユーザ情報タイトル -->
    <b-row>
      <b-col>
        <span class="sub-title-font">{{ UserTitle }}</span>
      </b-col>
    </b-row>

    <!-- 名前 -->
    <b-row>
      <b-col>
        <span class="field-title">お名前（外国籍の方は英数字入力）</span>
        <span class="required-badge">必須</span>
      </b-col>
    </b-row>
    <b-row>
      <!-- 姓 -->
      <b-col cols="3">
        <b-form-group
          label="姓"
          label-size="md"
          description=""
          label-cols="3"
          label-align="right"
          label-for="input-UserName1"
        >
          <b-form-input
            id="input-UserName1"
            type="text"
            size="xm"
            v-model="$v.value.UserName1.$model"
            :state="ValidateState('UserName1')"
            aria-describedby="UserName1-fb"
            placeholder="吉野"
          ></b-form-input>
          <b-form-invalid-feedback id="UserName1-fb">
            <small v-if="!$v.value.UserName1.required">入力してください</small>
            <small
              class="invalid-feedback d-block"
              v-if="
                $v.value.UserName1.required && !$v.value.UserName1.maxLength
              "
              >50文字以内で入力してください</small
            >
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
      <!-- 名 -->
      <b-col cols="3">
        <b-form-group
          label="名"
          label-size="md"
          description=""
          label-cols="3"
          label-align="right"
          label-for="input-UserName2"
        >
          <b-form-input
            id="input-UserName2"
            type="text"
            size="xm"
            v-model="$v.value.UserName2.$model"
            :state="ValidateState('UserName2')"
            aria-describedby="UserName2-fb"
            placeholder="太郎"
          ></b-form-input>
          <b-form-invalid-feedback id="UserName2-fb">
            <small v-if="!$v.value.UserName2.required">入力してください</small>
            <small
              class="invalid-feedback d-block"
              v-if="
                $v.value.UserName2.required && !$v.value.UserName2.maxLength
              "
              >50文字以内で入力してください</small
            >
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
    </b-row>

    <!-- フリガナ（全角カナ） -->
    <b-row>
      <b-col>
        <span class="field-title">フリガナ（全角カナ）</span>
        <span class="required-badge">必須</span>
      </b-col>
    </b-row>
    <b-row>
      <!-- セイ -->
      <b-col cols="3">
        <b-form-group
          label="セイ"
          label-size="md"
          description=""
          label-cols="3"
          label-align="right"
          label-for="input-UserRuby1"
        >
          <b-form-input
            id="input-UserRuby1"
            type="text"
            size="xm"
            v-model="$v.value.UserRuby1.$model"
            :state="ValidateState('UserRuby1')"
            aria-describedby="UserRuby1-fb"
            placeholder="ヨシノ"
          ></b-form-input>
          <b-form-invalid-feedback id="UserRuby1-fb">
            <small v-if="!$v.value.UserRuby1.required">入力してください</small>
            <small
              class="invalid-feedback d-block"
              v-if="
                $v.value.UserRuby1.required && !$v.value.UserRuby1.maxLength
              "
              >50文字以内で入力してください</small
            >
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
      <!-- メイ -->
      <b-col cols="3">
        <b-form-group
          label="メイ"
          label-size="md"
          description=""
          label-cols="3"
          label-align="right"
          label-for="input-UserRuby2"
        >
          <b-form-input
            id="input-UserRuby2"
            type="text"
            size="xm"
            v-model="$v.value.UserRuby2.$model"
            :state="ValidateState('UserRuby2')"
            aria-describedby="UserRuby2-fb"
            placeholder="タロウ"
          ></b-form-input>
          <b-form-invalid-feedback id="UserRuby2-fb">
            <small v-if="!$v.value.UserRuby2.required">入力してください</small>
            <small
              class="invalid-feedback d-block"
              v-if="
                $v.value.UserRuby2.required && !$v.value.UserRuby2.maxLength
              "
              >50文字以内で入力してください</small
            >
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
    </b-row>

    <!-- 住所 -->
    <b-row>
      <b-col>
        <span class="field-title">ご住所</span>
        <span class="required-badge">必須</span>
      </b-col>
    </b-row>
    <b-row>
      <!-- 郵便番号 -->
      <b-col cols="3">
        <b-form-group
          label="郵便番号"
          label-size="md"
          description=""
          label-cols="4"
          label-align="right"
          label-for="input-ZipCode"
        >
          <b-form-input
            id="input-ZipCode"
            type="text"
            size="xm"
            v-model="$v.value.ZipCode.$model"
            :state="ValidateState('ZipCode')"
            aria-describedby="ZipCode-fb"
            placeholder="0000000"
          ></b-form-input>
          <b-form-invalid-feedback id="ZipCode-fb">
            <small v-if="!$v.value.ZipCode.required">入力してください</small>
            <small
              class="invalid-feedback d-block"
              v-if="$v.value.ZipCode.required && !$v.value.ZipCode.numeric"
            >
              数値で入力してください
            </small>
            <small
              class="invalid-feedback d-block"
              v-if="$v.value.ZipCode.required && !$v.value.ZipCode.maxLength"
            >
              7文字以内で入力してください
            </small>
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
      <!-- 都道府県 -->
      <b-col cols="3">
        <b-form-group
          label="都道府県"
          label-size="md"
          description=""
          label-cols="4"
          label-align="right"
          label-for="input-UserAddress1"
        >
          <b-form-select
            variant="success"
            v-model="value.UserAddress1"
            :options="PREFS"
            placeholder="選択してください"
          ></b-form-select>
          <b-form-invalid-feedback id="UserAddress1-fb">
            <small v-if="!$v.value.UserAddress1.required">
              入力してください
            </small>
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
      <!-- 市区町村 -->
      <b-col cols="4">
        <b-form-group
          label="市区町村"
          label-size="md"
          description=""
          label-cols="4"
          label-align="right"
          label-for="input-UserAddress2"
        >
          <b-form-input
            id="input-UserAddress2"
            type="text"
            size="xm"
            v-model="$v.value.UserAddress2.$model"
            :state="ValidateState('UserAddress2')"
            aria-describedby="UserAddress2-fb"
            placeholder="吉野郡吉野町"
          ></b-form-input>
          <b-form-invalid-feedback id="UserAddress2-fb">
            <small v-if="!$v.value.UserAddress2.required">
              入力してください
            </small>
            <small
              class="invalid-feedback d-block"
              v-if="
                $v.value.UserAddress2.required &&
                !$v.value.UserAddress2.maxLength
              "
              >100文字以内で入力してください
            </small>
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
    </b-row>
    <!-- 番地・マンション名等 -->
    <b-row>
      <b-col cols="5">
        <b-form-group
          label="番地・マンション名等"
          label-size="md"
          description=""
          label-cols="4"
          label-align="right"
          label-for="input-UserAddress3"
        >
          <b-form-input
            id="input-UserAddress3"
            type="text"
            size="xm"
            v-model="$v.value.UserAddress3.$model"
            :state="ValidateState('UserAddress3')"
            aria-describedby="UserAddress3-fb"
            placeholder="77-1"
          ></b-form-input>
          <b-form-invalid-feedback id="UserAddress3-fb">
            <small v-if="!$v.value.UserAddress3.required">
              入力してください
            </small>
            <small
              class="invalid-feedback d-block"
              v-if="
                $v.value.UserAddress3.required &&
                !$v.value.UserAddress3.maxLength
              "
              >100文字以内で入力してください
            </small>
          </b-form-invalid-feedback>
        </b-form-group>
      </b-col>
    </b-row>
    <!-- 生年月日・ご年齢 -->
    <b-row>
      <b-col>
        <span class="field-title">生年月日・ご年齢</span>
        <span class="required-badge">必須</span>
        <br />
        <span class="field-annotation">
          ※ご年齢はプラン申込時点でのものをご記入ください
        </span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="5">
        <!-- 年 -->
        <b-input-group>
          <b-form-select
            variant="success"
            v-model="birthYear"
            :options="BIRTH_YEARS"
            :state="ValidateState('BirthDate')"
            aria-describedby="BirthDate-fb"
          ></b-form-select>

          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >年</b-input-group-text
            >
          </b-input-group-append>

          <!-- 月 -->
          <b-form-select
            variant="success"
            v-model="birthMonth"
            :options="BIRTH_MONTH"
            :state="ValidateState('BirthDate')"
            aria-describedby="BirthDate-fb"
          ></b-form-select>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >月</b-input-group-text
            >
          </b-input-group-append>

          <!-- 日 -->
          <b-form-select
            variant="success"
            v-model="birthDate"
            :options="birthDateList"
            :state="ValidateState('BirthDate')"
            aria-describedby="BirthDate-fb"
          ></b-form-select>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >日</b-input-group-text
            >
          </b-input-group-append>
          <b-form-invalid-feedback id="BirthDate-fb">
            <small v-if="!$v.value.BirthDate.required">
              正しい日付で入力してください
            </small>
          </b-form-invalid-feedback>
        </b-input-group>
      </b-col>

      <!-- 年齢 -->
      <b-col cols="2">
        <b-input-group>
          <b-form-input
            cols="2"
            id="input-Age"
            type="number"
            size="sm"
            min="0"
            class="num-input"
            v-model="$v.value.Age.$model"
            :state="ValidateState('Age')"
            aria-describedby="Age-fb"
          ></b-form-input>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >歳</b-input-group-text
            >
          </b-input-group-append>
          <b-form-invalid-feedback id="Age-fb">
            <small v-if="!$v.value.Age.required">入力してください </small>
            <small v-if="!$v.value.Age.numeric">数値で入力してください </small>
          </b-form-invalid-feedback>
        </b-input-group>
      </b-col>
    </b-row>

    <!-- お電話番号 -->
    <b-row>
      <b-col>
        <span class="field-title">お電話番号</span>
        <span class="required-badge">必須</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="3">
        <b-form-input
          id="input-UserPhone1"
          type="text"
          size="xm"
          v-model="$v.value.UserPhone1.$model"
          :state="ValidateState('UserPhone1')"
          aria-describedby="UserPhone1-fb"
          placeholder="0746-34-2522"
        ></b-form-input>

        <b-form-invalid-feedback id="UserPhone1-fb">
          <small v-if="!$v.value.UserPhone1.required">入力してください</small>
          <small
            class="invalid-feedback d-block"
            v-if="
              $v.value.UserPhone1.required && !$v.value.UserPhone1.maxLength
            "
            >20文字以内で入力してください</small
          >
        </b-form-invalid-feedback>
      </b-col>
    </b-row>

    <!-- 緊急連絡先お電話番号 -->
    <b-row>
      <b-col>
        <span class="field-title">緊急連絡先お電話番号</span>
        <br />
        <span class="field-annotation">
          ※緊急な連絡が必要な場合に必ず連絡のとれるお電話番号をご入力ください。<br />
          ご入力がない場合は、「お電話番号」欄にご入力いただきました電話番号が緊急連絡先として登録されます。
        </span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="3">
        <b-form-input
          id="input-UserPhone2"
          type="text"
          size="xm"
          v-model="$v.value.UserPhone2.$model"
          :state="ValidateState('UserPhone2')"
          aria-describedby="UserPhone2-fb"
        ></b-form-input>
        <b-form-invalid-feedback id="UserPhone2-fb">
          <small
            class="invalid-feedback d-block"
            v-if="!$v.value.UserPhone2.maxLength"
            >20文字以内で入力してください</small
          >
        </b-form-invalid-feedback>
      </b-col>
    </b-row>

    <!-- メールアドレス -->
    <b-row>
      <b-col>
        <span class="field-title">メールアドレス</span>
        <span class="required-badge">必須</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="3">
        <b-form-input
          id="input-UserMailAddress"
          type="text"
          size="xm"
          v-model="$v.value.UserMailAddress.$model"
          :state="ValidateState('UserMailAddress')"
          aria-describedby="UserMailAddress-fb"
          placeholder="info@yoshino-reserve.com"
        ></b-form-input>

        <b-form-invalid-feedback id="UserMailAddress-fb">
          <small v-if="!$v.value.UserMailAddress.required"
            >入力してください</small
          >

          <small
            class="invalid-feedback d-block"
            v-if="
              $v.value.UserMailAddress.required &&
              !$v.value.UserMailAddress.email
            "
            >有効なメールアドレスを入力してください
          </small>
          <small
            class="invalid-feedback d-block"
            v-if="
              $v.value.UserMailAddress.required &&
              !$v.value.UserMailAddress.maxLength
            "
            >20文字以内で入力してください
          </small>
        </b-form-invalid-feedback>
      </b-col>
    </b-row>
  </div>
</template>

<script>
// jquery
import $ from "jquery";

//検証部品
import { validationMixin } from "vuelidate";
import {
  required,
  minLength,
  maxLength,
  numeric,
  email,
  minValue,
  maxValue,
} from "vuelidate/lib/validators";

import DateUtil from "@js/DateUtil";

//定数
import { PREFS, BIRTH_MONTH, BIRTH_DATES } from "./AppEdit_User_Const";

export default {
  mixins: [validationMixin],
  name: "AppEdit_User",
  components: {},
  props: {
    AppEditModel: Object,
    value: Object, //ApplicationUser
    UserIndex: Number,
  },
  validations: {
    value: {
      UserName1: { required, maxLength: maxLength(50) },
      UserName2: { required, maxLength: maxLength(50) },
      UserRuby1: { required, maxLength: maxLength(50) },
      UserRuby2: { required, maxLength: maxLength(50) },
      ZipCode: { required, numeric, maxLength: maxLength(7) },
      UserAddress1: { required },
      UserAddress2: { required, maxLength: maxLength(100) },
      UserAddress3: { required, maxLength: maxLength(100) },
      BirthDate: { required },
      Age: { required, numeric },
      UserPhone1: { required, maxLength: maxLength(20) },
      UserPhone2: { maxLength: maxLength(20) },
      UserMailAddress: { required, email, maxLength: maxLength(256) },
    },
  },

  data() {
    return {
      birthYear: "",
      birthMonth: "",
      birthDate: "",
    };
  },
  watch: {
    birthYear(oldValue, newValue) {
      this.birthDateCheck();
    },
    birthMonth(oldValue, newValue) {
      this.birthDateCheck();
    },
    birthDate(oldValue, newValue) {
      this.birthDateCheck();
    },
  },
  computed: {
    UserTitle() {
      var ret = "";
      if (this.UserIndex == 0) {
        ret = "代表者様情報（大人1）";
      } else {
        ret = "ご同行者様情報（大人";
        ret += this.UserIndex + 1;
        ret += "）";
      }
      return ret;
    },
    PREFS() {
      return PREFS;
    },
    BIRTH_YEARS() {
      const years = 100;
      var dateNow = new Date();
      var nowYear = dateNow.getFullYear();
      var ret = [];
      for (var y = years; y >= 0; y--) {
        ret.push(nowYear - y);
      }
      return ret;
    },
    BIRTH_MONTH() {
      return BIRTH_MONTH;
    },
    birthDateList: function () {
      if (!this.birthMonth) return [];
      return BIRTH_DATES[this.birthMonth];
    },
  },
  methods: {
    ValidateState(propName) {
      const { $dirty, $error } = this.$v.value[propName];
      return $dirty ? !$error : null;
    },
    updateModel(newValue) {
      // inputイベントによってv-modelが更新され、するとバインドされているthis.valueも更新される。
      this.$emit("input", newValue);
    },
    birthDateCheck() {
      var strDate =
        this.birthYear + "/" + this.birthMonth + "/" + this.birthDate;
      if (DateUtil.SimpleCheckDateYMD(strDate)) {
        this.value.BirthDate = DateUtil.GetDateStringZero(strDate);
      } else {
        this.value.BirthDate = null;
      }

      //if (this.$v.value.BirthDate.$model != this.value.BirthDate) {
      this.$v.value.BirthDate.$model = this.value.BirthDate;
      //}
    },
  },
  mounted: async function () {
    if (DateUtil.SimpleCheckDateYMD(this.value.BirthDate)) {
      this.value.BirthDate = DateUtil.GetDateStringZero(this.value.BirthDate);
      var d = new Date(this.value.BirthDate);
      this.birthYear = d.getFullYear().toString();
      this.birthMonth = (d.getMonth() + 1).toString().padStart(2, "0");
      this.birthDate = d.getDate().toString().padStart(2, "0");
    } else {
      this.value.BirthDate = null;
    }
  },
};
</script>