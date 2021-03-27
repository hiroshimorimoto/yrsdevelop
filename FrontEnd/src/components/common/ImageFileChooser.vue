<style lang="scss" scoped>
// .custom-file-input ~
.custom-file-label::after {
  content: "参照";
}
.img-preview {
  max-width: 500px;
  margin: 20px;
}
</style>

<template>
  <div>
    <b-form-group
      :label="label"
      label-size="md"
      description
      :label-cols="labeCols"
      label-align="left"
      label-for="input-Access"
    >
      <b-form-file
        v-model="file"
        ref="fileinput"
        size="md"
        :state="Boolean(file)"
        accept="image/*"
        placeholder="ファイルを選択するかドロップしてください"
        drop-placeholder="ファイルをドロップできます"
        @input="choosed"
      ></b-form-file>
      <div class="d-flex align-items-center justify-content-center img-preview">
        <span
          v-show="Loading"
          class="spinner-border spinner-border-lg"
          role="status"
          aria-hidden="true"
        ></span>
        <img v-show="!Loading" class="img-fluid" :src="DataUri" />
      </div>
    </b-form-group>
  </div>
</template>

<script>
import numeric from "vuelidate/lib/validators/numeric";
export default {
  name: "ImageFileChooser",
  props: {
    label: { type: String },
    labeCols: {
      type: Number,
      default: 2,
    },
    imageUrl: { type: String },
  },
  data() {
    return {
      file: null,
      selectedFiles: null,
      DataUri: "",
      Loading: false,
    };
  },
  methods: {
    SetPreviewUrl: function (DataUri) {
      this.DataUri = DataUri;
    },
    choosed: function () {
      if (this.file == null) return;
      this.Loading = true;

      const reader = new FileReader();
      reader.addEventListener("load", () => {
        // プレビュー用の変数に入れる。reader.resultはこの場合DataURLが帰る。そのままimageのsrcに突っ込めばいい
        this.DataUri = reader.result;
        this.Loading = false;

        const param = {
          FileElement: this.file,
          DataUri: this.DataUri,
        };
        this.$emit("FileChoosed", param);
      });

      // FileReaderでFileオブジェクトを読み込む
      reader.readAsDataURL(this.file);
    },
  },
  mounted: function () {
    this.DataUri = this.imageUrl;
  },
};
</script>