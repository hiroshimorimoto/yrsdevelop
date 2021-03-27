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
    -moz-column-count: 1;
    column-count: 1;
  }
}

@media (min-width: 992px) {
  .card-columns {
    -moz-column-count: 1;
    column-count: 1;
  }
}
@media (min-width: 1200px) {
  .card-columns {
    -moz-column-count: 1;
    column-count: 1;
  }
}
</style>
<template>
  <div>
    <PlanGridItem
      v-for="(publicPlanItem) in this.items"
      :key="publicPlanItem.PlanId"
      :PublicPlanItem="publicPlanItem"
      @PlanClicked="PlanClicked"
    ></PlanGridItem>
  </div>
</template>

<script>
//プラングリッド表示コンポーネント
import PlanGridItem from "./PlanGridItem";

export default {
  name: "PlanGridList",
  props: {
    items: { type: Array },
  },
  components: {
    PlanGridItem,
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