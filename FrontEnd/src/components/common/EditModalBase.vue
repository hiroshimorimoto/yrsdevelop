<style lang="scss" scoped>
</style>
<template>
  <div>
    <b-modal
      id="editModal"
      :size="EditModalSize"
      scrollable
      no-close-on-backdrop
      no-close-on-esc
      :header-bg-variant="HeaderBgVariant"
      :header-text-variant="HeaderTextVariant"
    >
      <!-- ヘッダ -->
      <template v-slot:modal-header="{}">
        <h4 class="modal-title" id="editModalLabel">{{HeaderTitle}}</h4>
      </template>
      <!-- ボディ -->
      <template v-slot:default="{}">
        <b-container>
          <slot name="EditModalBody"></slot>
        </b-container>
        <!-- 完了メッセージ -->
        <div v-if="IsComplete">
          <b-row>
            <b-col class="text-center">
              <p class="text-info">{{CompleteMessage}}</p>
            </b-col>
          </b-row>
        </div>
        <!-- エラーメッセージエリア -->
        <b-row>
          <b-col class="text-center">
            <p class="text-danger">{{ErrorMessage}}</p>
          </b-col>
        </b-row>
      </template>

      <!-- フッタ -->
      <template v-slot:modal-footer>
        <b-button
          v-show="CancelButtonVisible"
          size="m"
          variant="secondary"
          @click="CancelClicked"
          v-bind:disabled="CancelButtonDisabled"
        >{{CancelButtonTitle}}</b-button>
        <b-button
          v-show="OkButtonVisible"
          size="m"
          variant="primary"
          @click="OkClicked"
          v-bind:disabled="OkButtonDisabled"
        >
          <span
            v-show="Loading"
            class="spinner-border spinner-border-sm"
            role="status"
            aria-hidden="true"
          ></span>
          {{OkButtonTitle}}
        </b-button>
      </template>
    </b-modal>
  </div>
</template>
<script>
const PROPS = {
  Add: {
    OkButtonTitle: "新規追加",
    OkButtonVisible: true,
    CancelButtonTitle: "キャンセル",
    CancelButtonVisible: true,
  },
  Edit: {
    OkButtonTitle: "登録",
    OkButtonVisible: true,
    CancelButtonTitle: "キャンセル",
    CancelButtonVisible: true,
  },
  Read: {
    OkButtonTitle: "",
    OkButtonVisible: false,
    CancelButtonTitle: "閉じる",
    CancelButtonVisible: true,
  },
};

export default {
  name: "EditModalBase",
  props: {
    EditModalSize: {
      type: String,
      default: "md",
    },
    HeaderTitle: String,
    HeaderBgVariant: String,
    HeaderTextVariant: String,
    OkButtonDisabled: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      EditMode: "", //Add,Edit,Read
      CompleteMessage: "",
      ErrorMessage: "",
      Loading: false,
      IsComplete: false,
      CancelButtonDisabled: false,
    };
  },
  computed: {
    OkButtonTitle: function () {
      return PROPS[this.EditMode].OkButtonTitle;
    },
    OkButtonVisible: function () {
      return PROPS[this.EditMode].OkButtonVisible;
    },
    CancelButtonTitle: function () {
      return PROPS[this.EditMode].CancelButtonTitle;
    },
    CancelButtonVisible: function () {
      return PROPS[this.EditMode].CancelButtonVisible;
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
  },
  methods: {
    OkClicked: function () {
      this.$emit("OkClicked"); //イベント発火
    },
    CancelClicked: function () {
      this.$emit("CancelClicked"); //イベント発火
    },
    open: function (editMode) {
      this.EditMode = editMode;
      this.$bvModal.show("editModal");
    },
    close: function () {
      this.$bvModal.hide("editModal");
    },
  },
  mounted: function () {},
};
</script>