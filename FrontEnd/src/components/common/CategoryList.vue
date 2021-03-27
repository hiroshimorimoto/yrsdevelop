<style lang="scss" scoped>
.badge {
  font-size: 150%;
  margin: 10px;
}
.read-only:hover {
  cursor: default;
}
</style>

<template>
  <div>
    <div v-if="ReadOnly">
      <div
        v-for="categoryItem in CategoryItems"
        :key="categoryItem.CategoryId"
        :class="GetItemClass(categoryItem.CategoryId)"
      >{{"#" + categoryItem.CategoryName}}</div>
    </div>
    <div v-if="!ReadOnly">
      <a
        v-for="categoryItem in CategoryItems"
        :key="categoryItem.CategoryId"
        :class="GetItemClass(categoryItem.CategoryId)"
        @click="ToggleSelect(categoryItem.CategoryId)"
      >{{"#" + categoryItem.CategoryName}}</a>
    </div>
  </div>
</template>
<script>
import CustomerBiz from "@js/biz/CustomerBiz";

export default {
  props: {
    SelectedCategoryIds: {
      type: Array,
      default: () => [],
    },
    ReadOnly: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      CategoryItems: [],
      Loading: false,
      ErrorMessage: "",
      selectedCategoryIds: [],
    };
  },
  methods: {
    GetCategoryItems: async function () {
      this.ErrorMessage = "";
      this.Loading = true;

      //プラン情報のPOST
      try {
        this.CategoryItems = await CustomerBiz.GetCategoryList();

        this.Loading = false;
        this.IsComplete = true;
      } catch (e) {
        this.ErrorMessage = e;
        this.Loading = false;
      }
    },
    IsSelected: function (categoryId) {
      return this.selectedCategoryIds.indexOf(categoryId) >= 0;
    },
    ToggleSelect: function (categoryId) {
      if (this.IsSelected(categoryId)) {
        this.selectedCategoryIds = this.selectedCategoryIds.filter(
          (e) => e != categoryId
        );
      } else {
        this.selectedCategoryIds.push(categoryId);
      }

      //v-modelの更新(sync対応)
      this.$emit("CategorySelectedChanged", this.selectedCategoryIds);
    },
    GetItemClass: function (categoryId) {
      var ret = "badge badge-pill ";
      if (this.IsSelected(categoryId)) {
        ret += "badge-warning";
      } else {
        ret += "badge-light";
      }

      if (this.ReadOnly) {
        ret += " read-only";
      }
      return ret;
    },
  },
  mounted: function () {
    this.GetCategoryItems();
    this.selectedCategoryIds = this.SelectedCategoryIds;
  },
};
</script>