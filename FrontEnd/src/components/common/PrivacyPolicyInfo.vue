<style lang="scss" scoped>
.title {
  font-weight: bold;
  margin: 10px;
}
.info-border {
  border: 1px solid gray;
  margin: 5px 20px;
  padding: 10px;
  border-radius: 0.25rem;
}
span {
  display: inline-block;
  margin: 5px 20px;
}

.check-area {
  margin: 10px auto;
  width: 90%;
  background-color: lightgray;
  text-align: center;
  padding: 10px;
}
</style>
<template>
  <div class="info-border">
    <b-row>
      <b-col>
        <span class="title">個人情報の取扱について</span>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <span>
          入力いただくお客様の個人情報は、当該旅行サービスのご案内にのみ使用し、それ以外の目的で使用することはございません。また、お申込頂いた当該旅行サービスの手続きに必要な範囲内で、手配代行者に対しお客様の個人情報と提供いたします。
        </span>
        <span>
          上記にご同意頂ける場合は「個人情報の取り扱いについて同意します」にチェックをいれてください。
        </span>
      </b-col>
    </b-row>
    <b-row>
      <b-col class="check-col">
        <div class="check-area">
          <b-form-checkbox v-model="bufCheck" ref="chk">
            個人情報の取り扱いについて同意します
          </b-form-checkbox>
        </div>
      </b-col>
    </b-row>
  </div>
</template>

<script>
//検証部品
import { validationMixin } from "vuelidate";

const IsChecked = function (value) {
  return value;
};

export default {
  mixins: [validationMixin],
  name: "PrivacyPolicyInfo",
  props: {
    value: {
      type: Boolean,
      default: false,
    },
  },
  components: {},
  data() {
    return {
      bufCheck: false,
    };
  },
  watch: {
    bufCheck: {
      handler: function () {
        this.$emit("input", this.bufCheck);
      },
      immediate: false,
    },
  },
  validations: {
    bufCheck: { isCheck: IsChecked },
  },
  computed: {},
  methods: {},
  mounted: async function () {},
};
</script>