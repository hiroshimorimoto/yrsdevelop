<style lang="scss" scoped>
</style>
<template>
  <div>
    <b-row v-if="TitleVisible">
      <b-col>
        <p>地域選択：</p>
      </b-col>
    </b-row>
    <div v-if="areaData != null">
      <b-row>
        <b-col cols="0" md="1" lg="1"></b-col>
        <b-col cols="6" md="5" lg="5" style="min-width:165px;" v-if="TreeVisible">
          <!-- <div v-if="areaData != null" :style="{height:mapHeight + 'px',overflow: 'scroll'}"> -->
          <!-- 選択エリアツリー -->
          <AreaMapTree
            v-for="area in areaData.Areas"
            :key="area.AreaId"
            :item="area"
            @MapTreeCheckedChanged="MapTreeCheckedChanged"
            @NodeCreated="NodeCreated"
            :ref="'child-area-' + area.AreaId"
          ></AreaMapTree>
        </b-col>
        <b-col cols="6" lg="4">
          <div class="map-wrapper">
            <div class="map-border">
              <img
                id="main-map"
                src="/resource/map.png"
                usemap="#area-data"
                style="width:auto;z-index:-1;height:390px"
              />
              <map name="area-data" id="area-data">
                <area
                  v-for="area in areas"
                  :id="'map-area-' + area.AreaId"
                  :key="area.AreaId"
                  :alt="area.AreaName"
                  :title="area.AreaName"
                  :coords="area.coords"
                  @click="MapClick(area)"
                  @mouseover="AreaHover($event)"
                  @mouseout="AreaLeave()"
                  shape="poly"
                />
              </map>
            </div>
          </div>
        </b-col>
      </b-row>
    </div>
  </div>
</template>
<script>
// axios
import axios from "axios";
// jquery
import $ from "jquery";

//イメージマップ
import ImageMap from "image-map";

//マップエリア ユーティリティ
import MapAreaUtil from "@js/MapAreaUtil";
var mapAreaUtil = new MapAreaUtil("main-map");
//エリアツリー
import AreaMapTree from "./AreaMapTree";

export default {
  components: {
    AreaMapTree,
  },
  props: {
    SelectedAreaIds: {
      type: Array,
      default: () => [],
    },
    TitleVisible: {
      type: Boolean,
      default: true,
    },
    TreeVisible: {
      type: Boolean,
      default: true,
    },
    ReadOnly: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      areaData: null,
      areas: [],
      treeNodes: {},
      mqBreakPoint: "",
    };
  },
  computed: {
    mapHeight: function () {
      var ret = $("#main-map").height();
      return ret;
    },
  },
  methods: {
    InitAreaMap: function () {
      ImageMap("#main-map", 500);
      var areaElms = $("area[shape]");
      if (areaElms && areaElms.length > 0) {
        setTimeout(() => {
          mapAreaUtil.InitCanvas(areaElms, this.SelectedAreaIds);
        }, 0);
      }
    },
    NodeCreated: function (treeNode) {
      this.treeNodes[treeNode.item.AreaId] = treeNode;
      treeNode.IsSelected =
        this.SelectedAreaIds.indexOf(treeNode.item.AreaId) >= 0;
    },
    MapClick: function (area) {
      if (this.ReadOnly) return;
      const found = this.SelectedAreaIds.indexOf(area.AreaId);
      if (found >= 0) {
        //remove
        area.IsSelected = false;
        this.SelectedAreaIds.splice(found, 1);
      } else {
        area.IsSelected = true;
        this.SelectedAreaIds.push(area.AreaId);
      }

      //TreeNodeの選択
      if (this.treeNodes[area.AreaId]) {
        this.treeNodes[area.AreaId].IsSelected = area.IsSelected;
      }

      //選択済み領域の再描画
      mapAreaUtil.InitSelectedCanvas(this.SelectedAreaIds);
    },
    MapTreeCheckedChanged: function (area) {
      const found = this.SelectedAreaIds.indexOf(area.AreaId);
      if (area.IsSelected) {
        if (found < 0) {
          this.SelectedAreaIds.push(area.AreaId);
        }
      } else {
        if (found >= 0) {
          this.SelectedAreaIds.splice(found, 1);
        }
      }

      //選択済み領域の再描画
      mapAreaUtil.InitSelectedCanvas(this.SelectedAreaIds);
    },
    AreaHover: function (event) {
      if (this.ReadOnly) return;
      mapAreaUtil.AreaHover(event.toElement.coords, "poly");
    },
    AreaLeave: function (event) {
      mapAreaUtil.AreaLeave();
    },
    checkResize: function () {
      var mq = $("#mqDetector");
      var visibles = mq.find(":visible");
      if (visibles.length <= 0) {
        this.mqBreakPoint = "sm";
      } else {
        var visible = visibles.get(visibles.length - 1).className;
        var breakPoint = visible.slice(2, 4);
        this.mqBreakPoint = breakPoint;
      }
    },
  },
  watch: {
    mqBreakPoint: {
      immediate: true,
      handler: function () {
        this.InitAreaMap();
      },
    },
  },
  mounted: function () {
    //mapAreaUtil.GetAreaData().then((areaDataUtil) => {
    mapAreaUtil.GetAreaData().then(({ AreaData, Areas }) => {
      // this.areaData = areaDataUtil.AreaData;
      // this.areas = areaDataUtil.Areas;
      this.areaData = AreaData;
      this.areas = Areas;

      $(window).on("resize", this.checkResize);
      this.checkResize();
    });
  },
};
</script>