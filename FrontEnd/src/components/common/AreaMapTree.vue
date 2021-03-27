<style lang="scss" scoped>
ul {
  padding-inline-start: 20px;
}
</style>
<template>
  <div>
    <div class="form-inline d-flex align-items-center" style="cursor:pointer;">
      <div v-if="hasChild" style="margin-right:5px;">
        <font-awesome-icon v-if="!isOpen" @click="toggle" icon="plus-square" class="fa-1x" />
        <font-awesome-icon v-if="isOpen" @click="toggle" icon="minus-square" class="fa-1x" />
      </div>
      <div v-if="!hasChild" style="margin-right:5px;">
        <font-awesome-icon @click="toggle" icon="caret-right" class="fa-1x" />
      </div>
      <b-form-checkbox v-model="IsSelected" :class="{bold: hasChild}" :disabled="!checkEnabled">
        <div style="cursor:pointer;">{{ item.AreaName }}</div>
      </b-form-checkbox>
    </div>
    <div>
      <ul v-show="isOpen" v-if="hasChild" style="padding-left:25px;">
        <AreaMapTree
          v-for="(area, index) in item.Areas"
          :key="index"
          :item="area"
          :ref="'child-area-' + area.AreaId"
          @MapTreeCheckedChanged="childCheckedChanged"
          @NodeCreated="ChildNodeCreated"
        ></AreaMapTree>
      </ul>
    </div>
  </div>
</template>
<script>
//エリアツリー(リカーシブル)
import AreaMapTree from "./AreaMapTree";
import imageMap from "image-map";

//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

export default {
  name: "AreaMapTree",
  components: {
    FontAwesomeIcon,
    AreaMapTree,
  },
  props: {
    item: Object,
  },
  data: function () {
    return {
      isOpen: true,
      IsSelected: false,
    };
  },
  computed: {
    hasChild: function () {
      if (!this.item.Areas) return false;
      return this.item.Areas.length > 0;
    },
    checkEnabled: function () {
      //return this.hasChild || (this.item.coords && this.item.coords.length > 0);
      return true;
    },
    isGroup: function () {
      return this.hasChild; // && (!this.item.coords || this.item.coords.length <= 0)
    },
  },
  watch: {
    IsSelected: {
      immediate: false,
      handler: function (newValue) {
        this.item.IsSelected = newValue;
        // if (this.isGroup) {
        //   this.item.Areas.forEach((area) => {
        //     var childNode = this.$refs["child-area-" + area.AreaId];
        //     if (childNode) childNode[0].IsSelected = newValue;
        //   });
        // } else {
        //   this.$emit("MapTreeCheckedChanged", this.item);
        // }

        if (this.hasChild) {
          this.item.Areas.forEach((area) => {
            var childNode = this.$refs["child-area-" + area.AreaId];
            if (childNode) childNode[0].IsSelected = newValue;
          });
        }

        this.$emit("MapTreeCheckedChanged", this.item);
      },
    },
  },
  methods: {
    toggle: function () {
      if (this.hasChild) {
        this.isOpen = !this.isOpen;
      }
    },
    childCheckedChanged: function (area) {
      //イベントを親へ伝播
      this.$emit("MapTreeCheckedChanged", area);
    },
    ChildNodeCreated: function (area) {
      //イベントを親へ伝播
      this.$emit("NodeCreated", area);
    },
  },
  mounted: function () {
    this.$emit("NodeCreated", this);
  },
};
</script>