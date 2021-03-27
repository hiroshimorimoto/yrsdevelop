<style lang="scss" scoped>
div.main-pane {
  padding: 5px;
}
.search-title {
  text-align: left;
}
.search-title.no-button {
  cursor: default;
  padding-top: 12px;
  padding-bottom: 12px;
}
.search-title span {
  color: white;
  margin-left: 10px;
}
.search-title svg {
  color: white;
}

.search-title div.title-right {
  display: inline-block;
  // float: right;
}
.search-title div.title-right.list-mode-button {
  cursor: pointer;
}
.search-title div.title-right.list-mode-button svg {
  margin-left: 20px;
}
.search-title div.title-right.list-mode-button .active {
  color: #0000aa;
}
</style>
<template>
  <div>
    <b-row>
      <b-col cols="0" lg="1" xl="1"></b-col>
      <b-col cols="12" lg="10" xl="10">
        <div class="main-pane">
          <b-button class="search-title" v-b-toggle.search-area block variant="warning">
            <b-icon icon="search" scale="1"></b-icon>
            <div class="title-right">
              <b-icon v-if="!searchAreaShow" icon="chevron-double-down" scale="1"></b-icon>
              <b-icon v-if="searchAreaShow" icon="chevron-double-up" scale="1"></b-icon>
            </div>
            <span>検索</span>
          </b-button>
          <b-collapse
            ref="searchArea"
            id="search-area"
            @shown="SeachAreaCollapseChanged"
            @hidden="SeachAreaCollapseChanged"
          >
            <b-row>
              <b-col cols="1" sm="1"></b-col>
              <b-col cols="10" sm="10" v-if="loaded && IsMobileWidth">
                <SearchAreaMobile ref="seachArea" @SearchClick="SearchClick"></SearchAreaMobile>
              </b-col>
              <b-col cols="10" sm="10" v-if="loaded && !IsMobileWidth">
                <SeachArea ref="seachArea" @SearchClick="SearchClick"></SeachArea>
              </b-col>
              <b-col cols="1" sm="1"></b-col>
            </b-row>
          </b-collapse>
        </div>
        <div class="main-pane" id="result-area">
          <b-button block variant="warning" class="search-title no-button">
            <b-icon icon="emoji-smile" scale="1.5"></b-icon>
            <span>体験</span>
            <div class="title-right list-mode-button">
              <b-icon
                icon="grid3x3-gap"
                scale="2"
                @click="ListModeSet('tile')"
                :class="ListModeBtnClass('tile')"
              ></b-icon>
              <b-icon
                icon="view-stacked"
                scale="2"
                @click="ListModeSet('grid')"
                :class="ListModeBtnClass('grid')"
              ></b-icon>
              <b-icon
                icon="file"
                scale="2"
                @click="ListModeSet('card')"
                :class="ListModeBtnClass('card')"
              ></b-icon>
            </div>
          </b-button>
          <CustomerPlanList ref="planList" :ListMode="ListMode" @PlanClicked="PlanClicked"></CustomerPlanList>
        </div>
      </b-col>
      <b-col cols="0" lg="1" xl="1"></b-col>
    </b-row>
  </div>
</template>

<script>
// jquery
import $ from "jquery";

//検索用Entity
import PlanListSearchEt from "@js/dto/PlanListSearchEt";

//利用者Biz
import CustomerBiz from "@js/biz/CustomerBiz";

//検索条件コンポーネント
import SeachArea from "./SearchArea";
import SearchAreaMobile from "./SearchAreaMobile";

//プランカード表示コンポーネント
import CustomerPlanList from "./CustomerPlanList";

export default {
  name: "CustomerMain",
  components: {
    SeachArea,
    SearchAreaMobile,
    CustomerPlanList,
  },
  data() {
    return {
      SearchEt: null,
      items: [],
      searchAreaShow: false,
      mapInited: false,
      ListMode: "tile", //"tile" or "grid" or "card"
      mqBreakPoint: "sm",
      loaded: false,
    };
  },
  computed: {
    IsMobileWidth: function () {
      if (this.mqBreakPoint == "sm") {
        return true;
      } else {
        return false;
      }
    },
  },
  methods: {
    getList: async function () {
      try {
        this.items = await CustomerBiz.FindPlanList(this.SearchEt);
      } catch (e) {
        this.items = [];
      }
      this.$refs.planList.SetList(this.items);
    },
    SearchClick: async function (planListSearchEt) {
      Object.assign(this.SearchEt, planListSearchEt);
      try {
        this.items = await CustomerBiz.FindPlanList(this.SearchEt);
      } catch (e) {
        this.items = [];
      }
      this.$refs.planList.SetList(this.items);

      var speed = 500;
      var target = $("#result-area");
      var position = target.offset().top;
      $("html, body").animate({ scrollTop: position }, speed, "swing");
    },
    PlanClicked: function (publicPlanItem) {
      var planId = publicPlanItem.PlanId;
      var url = "./planview.html?pid=" + planId;
      window.open(url, "planview" + planId);
    },
    SeachAreaCollapseChanged: function () {
      this.searchAreaShow = this.$refs.searchArea.show;
      if (this.searchAreaShow && !this.mapInited) {
        this.$refs.seachArea.InitMap();
        this.mapInited = true;
      }
    },
    ListModeSet: function (modeName) {
      this.ListMode = modeName;
    },
    ListModeBtnClass: function (modeName) {
      if (modeName == this.ListMode) {
        return "active";
      } else {
        return "";
      }
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
  mounted: async function () {
    this.SearchEt = new PlanListSearchEt();
    this.getList();

    $(window).on("resize", this.checkResize);
    this.checkResize();
    this.$nextTick(() => {
      this.loaded = true;
    });
  },
};
</script>