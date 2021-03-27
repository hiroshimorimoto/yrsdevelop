<style lang="scss" scoped>
</style>
<template>
  <div>
    <PlanTileList
      v-show="ListMode=='tile'"
      ref="planList"
      :items="items"
      @PlanClicked="PlanClicked"
    ></PlanTileList>
    <PlanGridList
      v-show="ListMode=='grid'"
      ref="planList"
      :items="items"
      @PlanClicked="PlanClicked"
    ></PlanGridList>
    <PlanCardList
      v-show="ListMode=='card'"
      ref="planList"
      :items="items"
      @PlanClicked="PlanClicked"
    ></PlanCardList>
  </div>
</template>

<script>
//プランリスト
import PlanTileList from "./PlanTileList"; //タイル
import PlanGridList from "./PlanGridList"; //グリッド
import PlanCardList from "./PlanCardList"; //カード

export default {
  name: "CustomerPlanList",
  props: {
    ListMode: {
      type: String,
      required: true,
      default: "tile", //or "grid" or "card"
    },
  },
  components: {
    PlanTileList,
    PlanGridList,
    PlanCardList,
  },
  data() {
    return {
      items: [],
    };
  },
  computed: {},
  methods: {
    SetList: async function (items) {
      // this.$refs.planList.items = items;
      this.items = items;
    },
    PlanClicked: function (publicPlanItem) {
      this.$emit("PlanClicked", publicPlanItem);
    },
  },
};
</script>