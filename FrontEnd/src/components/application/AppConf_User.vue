<style lang="scss" scoped>
hr {
  height: 1px;
  border-color: #ffc107;
  margin-top: 10px;
}

.conf-value.text-field {
  width: 100%;
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
          <div class="conf-value text-field">{{ value.UserName1 }}</div>
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
          <div class="conf-value text-field">{{ value.UserName2 }}</div>
        </b-form-group>
      </b-col>
    </b-row>

    <!-- フリガナ（全角カナ） -->
    <b-row>
      <b-col>
        <span class="field-title text-field">フリガナ（全角カナ）</span>
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
          <div class="conf-value text-field">{{ value.UserRuby1 }}</div>
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
          <div class="conf-value text-field">{{ value.UserRuby2 }}</div>
        </b-form-group>
      </b-col>
    </b-row>

    <!-- 住所 -->
    <b-row>
      <b-col>
        <span class="field-title text-field">ご住所</span>
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
          <div class="conf-value text-field">{{ value.ZipCode }}</div>
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
          <div class="conf-value text-field">{{ value.UserAddress1 }}</div>
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
          <div class="conf-value text-field">{{ value.UserAddress2 }}</div>
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
          <div class="conf-value text-field">{{ value.UserAddress3 }}</div>
        </b-form-group>
      </b-col>
    </b-row>
    <!-- 生年月日・ご年齢 -->
    <b-row>
      <b-col>
        <span class="field-title">生年月日・ご年齢</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="2">
        <div class="conf-value text-field">{{ BirthDate_Ja }}</div>
      </b-col>

      <!-- 年齢 -->
      <b-col cols="2" style="padding-top: 5px">
        <b-input-group>
          <b-input-group-prepend>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >年齢</b-input-group-text
            >
          </b-input-group-prepend>
          <div class="conf-value num">{{ value.Age }}</div>
          <b-input-group-append>
            <b-input-group-text
              class="form-control form-control-sm"
              size="sm"
              variant="glay"
              >歳</b-input-group-text
            >
          </b-input-group-append>
        </b-input-group>
      </b-col>
    </b-row>

    <!-- お電話番号 -->
    <b-row>
      <b-col>
        <span class="field-title">お電話番号</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="3">
        <div class="conf-value text-field">{{ value.UserPhone1 }}</div>
      </b-col>
    </b-row>

    <!-- 緊急連絡先お電話番号 -->
    <b-row>
      <b-col>
        <span class="field-title">緊急連絡先お電話番号</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="3">
        <div class="conf-value text-field">{{ value.UserPhone2 }}</div>
      </b-col>
    </b-row>

    <!-- メールアドレス -->
    <b-row>
      <b-col>
        <span class="field-title">メールアドレス</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col cols="3">
        <div class="conf-value text-field">{{ value.UserMailAddress }}</div>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import DateUtil from "@js/DateUtil";

export default {
  name: "AppConf_User",
  components: {},
  props: {
    AppEditModel: Object,
    value: Object, //ApplicationUser
    UserIndex: Number,
  },

  data() {
    return {};
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
    BirthDate_Ja() {
      var d = DateUtil.GetDateValue(this.value.BirthDate);
      return DateUtil.GetDateStringZeroJA(d);
    },
  },
  methods: {},
  mounted: async function () {},
};
</script>