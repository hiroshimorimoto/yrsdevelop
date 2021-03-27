<style lang="scss" scoped>
@media (max-width: 812px) {
  .card {
    max-height: 200px;
  }
  .pre-contents {
    max-height: 110px;
  }
}

@media (max-width: 576px) {
  .card {
    max-height: 150px;
  }
  .pre-contents {
    max-height: 50px;
  }
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
  }
}

img {
  max-width: 33%;
  object-fit: cover;
}
</style>
<template>
  <b-card
    img-left
    :img-src="TopImageSrc"
    :img-alt="PublicPlanItem.PlanTitle"
    :title="PublicPlanItem.PlanTitle"
    title-tag="h4"
    :sub-title="SubTitle"
    sub-title-tag="h5"
    border-variant="warning"
    tag="article"
    class="mb-3"
    @click="PlanClicked"
  >
    <b-card-text>
      <div class="pre-contents text-muted">
        <small>{{PreContents}}</small>
      </div>
    </b-card-text>
  </b-card>
  <!-- </div> -->
</template>
<script>
import PlanItemMixin from "@components/common/PlanItemMixin";
export default {
  name: "PlanCardItem",
  mixins: [PlanItemMixin],
};
</script>