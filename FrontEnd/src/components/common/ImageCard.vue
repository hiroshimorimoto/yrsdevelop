<style lang="scss" scoped>
.img-card {
  width: 100%;
  height: 100%;
  margin: 10px;
}
.img-card:hover {
  cursor: pointer;
}
.img-card:hover .card {
  border-width: 3px;
  box-sizing: border-box;
}
.fa-trash-alt {
  color: blue;
  position: absolute;
  z-index: 1;
  right: 10px;
  bottom: 15px;
}
.fa-trash-alt:hover {
  color: red;
  cursor: pointer;
}
</style>
<template>
  <div class="img-card" @click="ImageCardClicked">
    <b-card
      title
      :img-src="ImageSrc"
      :img-alt="ImageEt.FileName"
      :border-variant="BorderVariant"
      img-top
      tag="article"
      class="mb-2"
    >
      <b-card-text>{{ImageEt.Comment}}</b-card-text>
    </b-card>
    <font-awesome-icon icon="trash-alt" class="fa-lg" @click="ImageCardDeleting" />
  </div>
</template>
<script>
//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

export default {
  name: "ImageCard",
  components: {
    FontAwesomeIcon,
  },
  props: {
    BorderVariant: { type: String, default: "warning" },
    ImageEt: { type: Object },
  },
  data() {
    return {
      CardTtext: "",
    };
  },
  computed: {
    ImageSrc: function () {
      if (this.ImageEt == null) {
        return null;
      } else {
        if (this.ImageEt.DataUri) {
          return this.ImageEt.DataUri;
        } else {
          return process.env.VUE_APP_API_BASE_URL + this.ImageEt.LocalImageUrl;
        }
      }
    },
  },
  methods: {
    ImageCardClicked: function () {
      this.$emit("ImageCardClicked", {
        sender: this,
        ImageBase: this.ImageEt,
      });
    },
    ImageCardDeleting: function () {
      this.$emit("ImageCardDeleting", {
        sender: this,
        ImageBase: this.ImageEt,
      });
    },
  },
};
</script>