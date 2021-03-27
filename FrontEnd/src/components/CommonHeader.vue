<style lang="scss">
@import "../style/custom.scss";
</style>

<template>
  <div>
    <nav class="col-12 navbar navbar-expand-lg navbar-light bg-light">
      <span class="navbar-brand mb-0 h1">{{ system_title }}</span>
    </nav>
  </div>
</template>

<script>
//import ManagerHeader from "./manager/ManagerHeader";

export default {
  data() {
    return {
      system_title: "吉野リザーブ",
    };
  },
  //components: { ManagerHeader }
};
</script>