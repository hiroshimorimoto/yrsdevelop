<style lang="scss" scoped>
img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  margin: auto;
}

article.card:hover {
  cursor: pointer;
}

.card-body {
  overflow: hidden;
}
.card-body {
  h5,
  h4 {
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
  }

  .pre-contents {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
  }
}
</style>
<template>
  <b-card
    img-top
    :img-src="TopImageSrc"
    :img-alt="PublicPlanItem.PlanTitle"
    :title="PublicPlanItem.PlanTitle"
    title-tag="h4"
    :sub-title="SubTitle"
    sub-title-tag="h5"
    border-variant="warning"
    tag="article"
    class="mb-2"
    @click="PlanClicked"
  >
    <b-card-text>
      <div class="pre-contents text-muted">
        <small>{{PreContents}}</small>
      </div>
    </b-card-text>
  </b-card>
</template>
<script>
import PlanItemMixin from "@components/common/PlanItemMixin";
export default {
  name: "PlanCardItem",
  mixins: [PlanItemMixin],
};
</script>