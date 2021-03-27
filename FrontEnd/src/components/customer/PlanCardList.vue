<style lang="scss" scoped>
div.row > div {
  padding: 1px;
}

@media (min-width: 576px) {
  .card-columns {
    -moz-column-count: 1;
    column-count: 1;
  }
}

@media (min-width: 812px) {
  .card-columns {
    -moz-column-count: 2;
    column-count: 2;
  }
}

@media (min-width: 992px) {
  .card-columns {
    -moz-column-count: 2;
    column-count: 2;
  }
}
@media (min-width: 1200px) {
  .card-columns {
    -moz-column-count: 3;
    column-count: 3;
  }
}
</style>
<template>
  <div>
    <b-card-group columns>
      <PlanCardItem
        v-for="(publicPlanItem) in this.items"
        :key="publicPlanItem.PlanId"
        :PublicPlanItem="publicPlanItem"
        @PlanClicked="PlanClicked"
      ></PlanCardItem>
    </b-card-group>
  </div>
</template>

<script>
//プランカード表示コンポーネント
import PlanCardItem from "./PlanCardItem";

export default {
  name: "PlanCardList",
  props: {
    items: { type: Array },
  },
  components: {
    PlanCardItem,
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