<style lang="scss" scoped>
div.row > div {
  padding: 1px;
}
</style>
<template>
  <div>
    <b-row>
      <b-col
        cols="6"
        sm="6"
        md="4"
        lg="3"
        xl="3"
        v-for="(publicPlanItem) in this.items"
        :key="publicPlanItem.PlanId"
      >
        <PlanTileItem :PublicPlanItem="publicPlanItem" @PlanClicked="PlanClicked"></PlanTileItem>
      </b-col>
    </b-row>
  </div>
</template>

<script>
//プランカード表示コンポーネント
import PlanTileItem from "./PlanTileItem";

export default {
  name: "PlanTileList",
  props: {
    items: { type: Array },
  },
  components: {
    PlanTileItem,
  },
  computed: {},
  methods: {
    SetList: async function (items) {
      this.items = items;
    },
    PlanClicked: function (publicPlanItem) {
      this.$emit("PlanClicked", publicPlanItem);
    },
  },
};
</script>