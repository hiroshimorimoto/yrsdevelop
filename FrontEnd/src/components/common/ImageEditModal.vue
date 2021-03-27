<style lang="scss" scoped>
</style>
<template>
  <div>
    <b-modal
      id="ImageEditModal"
      size="md"
      centered
      no-close-on-backdrop
      no-close-on-esc
      header-bg-variant="success"
      header-text-variant="white"
    >
      <!-- ヘッダ -->
      <template v-slot:modal-header="{}">
        <h5 class="modal-title" id="editModalLabel">{{HeaderTitle}}</h5>
      </template>
      <!-- ボディ -->
      <template v-slot:default="{}">
        <b-row>
          <b-col>
            <!-- 画像 -->
            <ImageFileChooser
              label="画像:"
              label-cols="2"
              @FileChoosed="ImageChoosed"
              :image-url="ImageSrc"
              ref="topImageChooser"
            ></ImageFileChooser>
          </b-col>
        </b-row>
        <b-row>
          <b-col>
            <!-- コメント -->
            <b-form-group
              label="コメント:"
              label-size="md"
              description
              label-cols="2"
              label-align="left"
              label-for="input-Comment"
            >
              <b-form-textarea
                id="input-Comment"
                type="textarea"
                size="sm"
                rows="2"
                max-rows="3"
                v-model="$v.ImageBase.Comment.$model"
                :state="ValidateState('Comment')"
                aria-describedby="Comment-fb"
              ></b-form-textarea>
              <b-form-invalid-feedback id="Comment-fb">
                <small
                  class="invalid-feedback d-block"
                  v-if="!$v.ImageBase.Comment.maxLength"
                >256文字以内で入力してください</small>
              </b-form-invalid-feedback>
            </b-form-group>
          </b-col>
        </b-row>
      </template>
      <!-- フッタ -->
      <template v-slot:modal-footer>
        <b-button size="m" variant="secondary" @click="CancelClicked">キャンセル</b-button>
        <b-button size="m" variant="primary" @click="OkClicked" :disabled="IsButtonDisabled">設定</b-button>
      </template>
    </b-modal>
  </div>
</template>
<script>
//検証部品
import { validationMixin } from "vuelidate";
import { required, minLength, maxLength } from "vuelidate/lib/validators";

import ImageFileChooser from "@components/common/ImageFileChooser"; //画像選択部品

import ImageBase from "@js/model/ImageBase";

export default {
  mixins: [validationMixin],
  components: {
    ImageFileChooser,
  },
  data() {
    return {
      EditMode: "",
      HeaderTitle: "",

      ImageBase: new ImageBase(), //編集時 選択されたImageエンティティ
    };
  },
  validations: {
    ImageBase: {
      FileElement: required,
      Comment: { maxLength: maxLength(256) },
    },
  },
  computed: {
    IsButtonDisabled: function () {
      return this.$v.ImageBase.$invalid;
    },
    EditModeName: function () {
      var ret = "";
      switch (this.EditMode) {
        case "Add":
          ret = "新規登録";
          break;
        case "Edit":
          ret = "編集";
          break;
        case "Read":
          ret = "参照";
          break;
      }
      return ret;
    },
    ImageSrc: function () {
      if (this.ImageBase == null) {
        return null;
      } else {
        if (this.ImageBase.DataUri) {
          return this.ImageBase.DataUri;
        } else if (this.ImageBase.LocalImageUrl) {
          return (
            process.env.VUE_APP_API_BASE_URL + this.ImageBase.LocalImageUrl
          );
        } else {
          return null;
        }
      }
    },
  },
  methods: {
    OpenAdd: function () {
      this.EditMode = "Add";
      this.HeaderTitle = "写真 " + this.EditModeName;

      this.ImageBase = new ImageBase();
      this.ImageBase.EditMode = "Add";

      this.$bvModal.show("ImageEditModal");
    },

    OpenEdit: function (imageBase) {
      this.EditMode = "Edit";
      this.HeaderTitle = "写真 " + this.EditModeName;

      this.ImageBase = new ImageBase();
      Object.assign(this.ImageBase, imageBase);

      this.$bvModal.show("ImageEditModal");
    },
    OkClicked: async function () {
      const param = {
        EditMode: this.EditMode,
        ImageBase: this.ImageBase,
      };

      this.$emit("ImageEdited", param);
      this.$bvModal.hide("ImageEditModal");
    },
    ImageChoosed: function (param) {
      this.ImageBase.FileElement = param.FileElement;
      this.ImageBase.DataUri = param.DataUri;
      this.ImageBase.FileName = param.FileElement.name;
    },
    ValidateState: function (propName) {
      const { $dirty, $error } = this.$v.ImageBase[propName];
      return $dirty ? !$error : null;
    },
    CancelClicked: function () {
      this.$bvModal.hide("ImageEditModal");
    },
  },
};
</script>
