<style scoped>
.plan-tile {
  height: 100%;
}
.plan-tile:hover {
  cursor: pointer;
}

.plan-tile img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  margin: auto;
}
</style>
<template>
  <div class="plan-tile" @click="PlanClicked">
    <b-img-lazy :src="TopImageSrc" />
  </div>
</template>
<script>
import PlanItemMixin from "@components/common/PlanItemMixin";
export default {
  name: "PlanTileItem",
  mixins: [PlanItemMixin],
};
</script>