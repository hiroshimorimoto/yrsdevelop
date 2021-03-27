<script>
export default {
  props: {
    LoginType: String
  },
  data() {
    return {
      PROPS: {
        manager: {
          name: "運営者",
          headerBgVariant: "primary",
          headerTextVariant: "light"
        },
        provider: {
          name: "事業者",
          headerBgVariant: "success",
          headerTextVariant: "light",
          showLoginId: true
        },
        customer: {
          name: "",
          headerBgVariant: "warning",
          headerTextVariant: "light",
          showLoginId: true
        }
      },
      ErrorMessage: "",
      Loading: false,
      IsComplete: false
    };
  },
  computed: {
    isButtonDisabled: function() {
      return this.$v.$invalid || this.Loading;
    },
    showLoginId: function() {
      return this.PROPS[this.LoginType].showLoginId || false;
    },
    LoginTypeName: function() {
      return this.PROPS[this.LoginType].name;
    },
    headerBgVariant: function() {
      return this.PROPS[this.LoginType].headerBgVariant;
    },
    headerTextVariant: function() {
      return this.PROPS[this.LoginType].headerTextVariant;
    }
  },
  methods: {
    validateState: function(propName) {
      const { $dirty, $error } = this.$v[propName];
      return $dirty ? !$error : null;
    },
    goLogin: function() {
      document.location = "/" + this.LoginType + "/index.html";
    },
    goPassChange: function() {
      //this.$emit("goPassChange"); //イベント発火
      document.location = "/" + this.LoginType + "/remind.html#/PassChange";
    },
    goPassReset: function() {
      //this.$emit("goPassReset"); //イベント発火
      document.location = "/" + this.LoginType + "/remind.html#/PassReset";
    }
  }
};
</script>