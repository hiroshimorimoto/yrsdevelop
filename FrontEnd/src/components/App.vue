<style lang="scss">
@import "../style/custom.scss";
</style>
<template>
  <div class="container">
    <div class="row">
      <!-- <div class="col-2"></div> -->
      <div class="col-12">
        <MainHeader></MainHeader>
      </div>
      <!-- <div class="col-2"></div> -->
    </div>
  </div>
</template>

<script>
import MainHeader from "./MainHeader";

export default {
  components: { MainHeader }
};
</script>