<script>
import PublicPlanInfo from "@js/model/PublicPlanInfo";

export default {
  props: {
    PublicPlanItem: { type: Object },
  },
  data() {
    return {};
  },
  computed: {
    TopImageSrc: function () {
      if (this.PublicPlanItem == null) {
        return null;
      } else {
        return (
          process.env.VUE_APP_API_BASE_URL +
          this.PublicPlanItem.PublicTopImageUrl
        );
      }
    },
    SubTitle: function () {
      if (
        !this.PublicPlanItem.Overview ||
        this.PublicPlanItem.Overview.length <= 0
      ) {
        return "　";
      } else {
        return this.PublicPlanItem.Overview;
      }
    },
    PreContents: function () {
      if (
        !this.PublicPlanItem.Contents ||
        this.PublicPlanItem.Contents.length <= 0
      ) {
        return "　";
      } else {
        return this.PublicPlanItem.Contents;
        // if (this.PublicPlanItem.Contents.length >= 100) {
        //   return this.PublicPlanItem.Contents.substr(0, 99);
        // } else {
        //   return this.PublicPlanItem.Contents;
        // }
      }
    },
  },
  methods: {
    PlanClicked: function () {
      this.$emit("PlanClicked", this.PublicPlanItem);
    },
  },
};
</script>