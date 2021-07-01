<style lang="scss">
fieldset small {
  margin-top: 0px;
  padding-bottom: 7px;
}
</style>

<style lang="scss" scoped>
#input-Fee_Adult,
#input-Fee_Child,
#input-Fee_Infant {
  width: 150px;
}
input[type="number"] {
  text-align: right;
}
fieldset.form-group {
  margin-bottom: 0px;
}

.img-add-btn {
  // width: calc(100% - 10px);
  // height: calc(100% - 10px);
  margin: 10px;
}
</style>
<template>
  <div>
    <EditModdalBase
      ref="editModalBase"
      edit-modal-size="lg"
      :HeaderTitle="HeaderTitle"
      HeaderBgVariant="success"
      HeaderTextVariant="light"
      @OkClicked="OkClicked"
      @CancelClicked="CancelClicked"
      :OkButtonDisabled="IsButtonDisabled"
    >
      <template v-slot:EditModalBody>
        <div v-bind:disabled="Loading">
          <b-row>
            <b-col>
              <!-- 公開ステータス -->
              <div v-if="EditMode == 'Add'">
                <b-row align-v="center">
                  <b-col cols="5">
                    <h3>
                      <b-badge variant="primary">新規登録</b-badge>
                    </h3>
                  </b-col>
                </b-row>
              </div>
              <div v-if="EditMode != 'Add'">
                <b-row align-v="center">
                  <b-col cols="2">
                    <h3>
                      <b-badge :variant="PublicStatusVariant">{{
                        PublicStatusName
                      }}</b-badge>
                    </h3>
                  </b-col>
                  <b-col v-if="PublicStatus == 0">
                    <b-button
                      size="m"
                      variant="outline-primary"
                      @click="TogglePublicStatus(1)"
                    >
                      <span
                        v-show="Loading"
                        class="spinner-border spinner-border-sm"
                        role="status"
                        aria-hidden="true"
                      ></span>
                      今すぐ公開する&nbsp;
                      <font-awesome-icon icon="paper-plane" />
                    </b-button>
                  </b-col>
                  <b-col v-if="PublicStatus == 1">
                    <b-button
                      size="m"
                      variant="outline-danger"
                      @click="TogglePublicStatus(0)"
                    >
                      <span
                        v-show="Loading"
                        class="spinner-border spinner-border-sm"
                        role="status"
                        aria-hidden="true"
                      ></span>
                      非公開にする&nbsp;
                      <font-awesome-icon icon="paper-plane" />
                    </b-button>
                  </b-col>
                </b-row>
              </div>
            </b-col>
          </b-row>

          <b-row>
            <b-col>
              <b-tabs content-class="mt-3">
                <!-- 基本情報タブ -->
                <b-tab title="基本情報" active>
                  <b-row>
                    <b-col>
                      <b-form-checkbox v-model="HiddenFlg"
                        >検索結果一覧に表示されないようにする</b-form-checkbox
                      >
                    </b-col>
                    <b-col>
                      <b-form-checkbox v-model="AllPersonFlg"
                        >全員の個人情報を登録する</b-form-checkbox
                      >
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <!-- プランタイトル -->
                      <b-form-group
                        label="プラン名:"
                        label-size="md"
                        description="プラン名を入力してください"
                        label-cols="3"
                        label-align="left"
                        label-for="input-PlanTitle"
                      >
                        <b-form-input
                          id="input-PlanTitle"
                          type="text"
                          size="sm"
                          v-model="$v.PlanTitle.$model"
                          :state="ValidateState('PlanTitle')"
                          aria-describedby="PlanTitle-fb"
                        ></b-form-input>
                        <b-form-invalid-feedback id="PlanTitle-fb">
                          <small v-if="!$v.PlanTitle.required"
                            >入力してください</small
                          >
                          <small
                            class="invalid-feedback d-block"
                            v-if="
                              $v.PlanTitle.required && !$v.PlanTitle.maxLength
                            "
                            >100文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>

                  <b-row>
                    <b-col>
                      <!-- プラン開始・終了 & 受付可能日設定  -->
                      <b-form-group
                        label="プランの掲載期間："
                        label-size="md"
                        label-cols="3"
                        label-align="left"
                        description="※受付日が未設定の場合、プランを受付できません"
                      >
                        <div style="display: block" class="invalid-feedback">
                          <small
                            v-if="
                              !$v.PlanStartDate.ltEndDate ||
                              !$v.PlanEndDate.gtStartDate
                            "
                            >開始・終了の関係が不正です</small
                          >
                        </div>
                        <b-form-group
                          label="開始日:"
                          label-size="sm"
                          label-cols="2"
                          label-align="left"
                        >
                          <!-- デートピッカー2 -->
                          <DatePicker2
                            id="PlanStartDate"
                            description
                            :validator="$v.PlanStartDate"
                            v-model="PlanStartDate"
                          >
                            <template v-slot:invalid-feedback>
                              <small v-if="!$v.PlanStartDate.required"
                                >入力してください</small
                              >
                            </template>
                          </DatePicker2>
                        </b-form-group>

                        <b-form-group
                          label="終了日:"
                          label-size="sm"
                          label-cols="2"
                          label-align="left"
                        >
                          <!-- デートピッカー2 -->
                          <DatePicker2
                            id="PlanEndDate"
                            description
                            :validator="$v.PlanEndDate"
                            v-model="PlanEndDate"
                          >
                            <template v-slot:invalid-feedback>
                              <small v-if="!$v.PlanEndDate.required"
                                >入力してください</small
                              >
                            </template>
                          </DatePicker2>
                        </b-form-group>

                        <b-input-group size="sm">
                          <b-input-group-text bg-variant="light">{{
                            _AcceptDateComment
                          }}</b-input-group-text>
                          <b-input-group-append>
                            <b-button
                              variant="success"
                              :disabled="_AcceptDateDisabled"
                              @click="ShowAcceptDateModal"
                              >受付日の設定</b-button
                            >
                          </b-input-group-append>
                        </b-input-group>
                      </b-form-group>
                    </b-col>
                  </b-row>

                  <b-row>
                    <b-col>
                      <!-- 料金設定 -->
                      <b-form-group
                        label="料金設定:"
                        label-size="md"
                        description
                        label-cols="3"
                        label-align="left"
                        label-for="input-PlanTitle"
                      >
                        <b-form-group
                          label="大人:"
                          label-size="sm"
                          label-cols="2"
                          label-align="left"
                        >
                          <b-form-input
                            cols="1"
                            id="input-Fee_Adult"
                            type="number"
                            size="sm"
                            min="0"
                            v-model="$v.Fee_Adult.$model"
                            :state="ValidateState('Fee_Adult')"
                            aria-describedby="Fee_Adult-fb"
                          ></b-form-input>
                          <b-form-text>※中学生以上</b-form-text>
                          <b-form-invalid-feedback id="Fee_Adult-fb">
                            <small v-if="!$v.Fee_Adult.numeric"
                              >数値で入力してください</small
                            >
                          </b-form-invalid-feedback>
                        </b-form-group>
                        <b-form-group
                          label="児童:"
                          label-size="sm"
                          label-cols="2"
                          label-align="left"
                        >
                          <b-form-input
                            cols="1"
                            id="input-Fee_Child"
                            type="number"
                            size="sm"
                            min="0"
                            v-model="$v.Fee_Child.$model"
                            :state="ValidateState('Fee_Child')"
                            aria-describedby="Fee_Child-fb"
                          ></b-form-input>
                          <b-form-text>※２歳～小学生以下</b-form-text>
                          <b-form-invalid-feedback id="Fee_Child-fb">
                            <small v-if="!$v.Fee_Child.numeric"
                              >数値で入力してください</small
                            >
                          </b-form-invalid-feedback>
                        </b-form-group>
                        <b-form-group
                          label="幼児:"
                          label-size="sm"
                          label-cols="2"
                          label-align="left"
                        >
                          <b-form-input
                            cols="1"
                            id="input-Fee_Infant"
                            type="number"
                            size="sm"
                            min="0"
                            v-model="$v.Fee_Infant.$model"
                            :state="ValidateState('Fee_Infant')"
                            aria-describedby="Fee_Infant-fb"
                          ></b-form-input>
                          <b-form-text>※２歳未満</b-form-text>
                          <b-form-invalid-feedback id="Fee_Infant-fb">
                            <small v-if="!$v.Fee_Infant.numeric"
                              >数値で入力してください</small
                            >
                          </b-form-invalid-feedback>
                        </b-form-group>
                        <b-button
                          variant="success"
                          :disabled="IsDateFeeDisabled"
                          @click="ShowDateFeeModal"
                          >日別料金の設定</b-button
                        >
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <!-- トップ画像 -->
                      <ImageFileChooser
                        label="トップ画像:"
                        label-cols="2"
                        @FileChoosed="TopImageChoosed"
                        :image-url="TopImageUrl"
                        ref="topImageChooser"
                      ></ImageFileChooser>
                    </b-col>
                  </b-row>
                </b-tab>
                <!-- 詳細情報タブ -->
                <b-tab title="詳細情報">
                  <b-row>
                    <b-col>
                      <!-- 概要 -->
                      <b-form-group
                        label="概要:"
                        label-size="md"
                        description="概要はタイトル下に表示されます"
                        label-cols="2"
                        label-align="left"
                        label-for="input-Overview"
                      >
                        <b-form-textarea
                          id="input-Overview"
                          type="textarea"
                          size="sm"
                          rows="3"
                          max-rows="6"
                          v-model="$v.Overview.$model"
                          :state="ValidateState('Overview')"
                          aria-describedby="Overview-fb"
                        ></b-form-textarea>
                        <b-form-invalid-feedback id="Overview-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.Overview.maxLength"
                            >200文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row
                    ><b-col>
                      <!-- プラン内容 -->
                      <b-form-group
                        label="プラン内容:"
                        label-size="md"
                        description
                        label-cols="2"
                        label-align="left"
                        label-for="input-Contents"
                      >
                        <b-form-textarea
                          id="input-Contents"
                          type="textarea"
                          size="sm"
                          rows="10"
                          max-rows="30"
                          v-model="$v.Contents.$model"
                          :state="ValidateState('Contents')"
                          aria-describedby="Contents-fb"
                        ></b-form-textarea>
                        <b-form-invalid-feedback id="Contents-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.Contents.maxLength"
                            >1024文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row
                    ><b-col>
                      <!-- キャンセルポリシー -->
                      <b-form-group
                        label="キャンセルポリシー:"
                        label-size="md"
                        description
                        label-cols="2"
                        label-align="left"
                        label-for="input-CancelPolicy"
                      >
                        <b-form-textarea
                          id="input-CancelPolicy"
                          type="textarea"
                          size="sm"
                          rows="10"
                          max-rows="30"
                          v-model="$v.CancelPolicy.$model"
                          :state="ValidateState('CancelPolicy')"
                          aria-describedby="CancelPolicy-fb"
                        ></b-form-textarea>
                        <b-form-invalid-feedback id="CancelPolicy-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.CancelPolicy.maxLength"
                            >1024文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <!-- コース -->
                      <b-form-group
                        label="コース:"
                        label-size="md"
                        label-cols="2"
                        label-align="left"
                        label-for="input-PlanCourse"
                      >
                        <b-form-textarea
                          id="input-PlanCourse"
                          type="textarea"
                          size="sm"
                          rows="3"
                          max-rows="6"
                          v-model="$v.PlanCourse.$model"
                          :state="ValidateState('PlanCourse')"
                          aria-describedby="PlanCourse-fb"
                        ></b-form-textarea>
                        <b-form-invalid-feedback id="PlanCourse-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.PlanCourse.maxLength"
                            >256文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <!-- 所要時間 -->
                      <b-form-group
                        label="所要時間:"
                        label-size="md"
                        description
                        label-cols="3"
                        label-align="left"
                        label-for="input-TimeRequired"
                      >
                        <b-form-input
                          id="input-TimeRequired"
                          type="text"
                          size="sm"
                          v-model="$v.TimeRequired.$model"
                          :state="ValidateState('TimeRequired')"
                          aria-describedby="TimeRequired-fb"
                        ></b-form-input>
                        <b-form-invalid-feedback id="TimeRequired-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.TimeRequired.maxLength"
                            >256文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                    <b-col>
                      <b-form-group
                        label="最大申込人数:"
                        label-size="sm"
                        label-cols="4"
                        label-align="left"
                      >
                        <b-form-input
                          cols="1"
                          id="input-MaxApplicantsNum"
                          type="number"
                          size="sm"
                          min="0"
                          v-model="$v.MaxApplicantsNum.$model"
                          :state="ValidateState('MaxApplicantsNum')"
                          aria-describedby="MaxApplicantsNum-fb"
                        ></b-form-input>
                        <b-form-invalid-feedback id="MaxApplicantsNum-fb">
                          <small v-if="!$v.MaxApplicantsNum.numeric"
                            >数値で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                </b-tab>
                <b-tab title="カテゴリ">
                  <b-row>
                    <b-col>
                      <CategoryList
                        :SelectedCategoryIds="SelectedCategoryIds"
                        @CategorySelectedChanged="CategorySelectedChanged"
                      ></CategoryList>
                    </b-col>
                  </b-row>
                </b-tab>

                <b-tab title="地域" @click="AreaMapActivate">
                  <b-row>
                    <b-col>
                      <!-- 住所 -->
                      <b-form-group
                        label="住所:"
                        label-size="md"
                        description
                        label-cols="2"
                        label-align="left"
                        label-for="input-PlanAddress"
                      >
                        <b-form-input
                          id="input-PlanAddress"
                          type="text"
                          size="sm"
                          v-model="$v.PlanAddress.$model"
                          :state="ValidateState('PlanAddress')"
                          aria-describedby="PlanAddress-fb"
                        ></b-form-input>
                        <b-form-invalid-feedback id="PlanAddress-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.PlanAddress.maxLength"
                            >100文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <!-- アクセス -->
                      <b-form-group
                        label="アクセス:"
                        label-size="md"
                        description
                        label-cols="2"
                        label-align="left"
                        label-for="input-Access"
                      >
                        <b-form-textarea
                          id="input-Access"
                          type="textarea"
                          size="sm"
                          rows="2"
                          max-rows="3"
                          v-model="$v.Access.$model"
                          :state="ValidateState('Access')"
                          aria-describedby="Access-fb"
                        ></b-form-textarea>
                        <b-form-invalid-feedback id="Access-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.Access.maxLength"
                            >200文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <!-- 集合場所 -->
                      <b-form-group
                        label="集合場所:"
                        label-size="md"
                        description
                        label-cols="2"
                        label-align="left"
                        label-for="input-MeetingPlace"
                      >
                        <b-form-input
                          id="input-MeetingPlace"
                          type="text"
                          size="sm"
                          v-model="$v.MeetingPlace.$model"
                          :state="ValidateState('MeetingPlace')"
                          aria-describedby="MeetingPlace-fb"
                        ></b-form-input>
                        <b-form-invalid-feedback id="MeetingPlace-fb">
                          <small
                            class="invalid-feedback d-block"
                            v-if="!$v.MeetingPlace.maxLength"
                            >256文字以内で入力してください</small
                          >
                        </b-form-invalid-feedback>
                      </b-form-group>
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col>
                      <AreaMap
                        ref="AreaMap"
                        :SelectedAreaIds="SelectedAreaIds"
                      ></AreaMap>
                    </b-col>
                  </b-row>
                </b-tab>
                <b-tab title="写真">
                  <b-row>
                    <b-col>
                      <b-button
                        size="sm"
                        class="float-left img-add-btn"
                        variant="outline-warning"
                        @click="ShowImageEditModal_Add"
                        >添付画像の追加</b-button
                      >
                    </b-col>
                  </b-row>
                  <b-row>
                    <b-col
                      cols="12"
                      sm="6"
                      md="4"
                      lg="4"
                      xl="4"
                      v-for="planImage in VisibleTempImages"
                      :key="planImage.ImageNo"
                    >
                      <ImageCard
                        :ImageEt="planImage"
                        @ImageCardClicked="ImageCardClicked"
                        @ImageCardDeleting="ImageCardDeleting"
                      ></ImageCard>
                    </b-col>
                  </b-row>
                </b-tab>
              </b-tabs>
            </b-col>
          </b-row>
        </div>
      </template>
    </EditModdalBase>
    <!-- 受付可能日モーダル -->
    <AcceptDateModal
      ref="acceptDateModal"
      :plan-start-date="PlanStartDate"
      :plan-end-date="PlanEndDate"
      @AcceptDateSetted="AcceptDateSetted"
    ></AcceptDateModal>

    <!-- 日別料金設定モーダル -->
    <DateFeeModal
      ref="dateFeeModal"
      :plan-start-date="PlanStartDate"
      :plan-end-date="PlanEndDate"
      @DateFeeSetted="DateFeeSetted"
    ></DateFeeModal>

    <!-- 添付画像編集モーダル -->
    <ImageEditModal
      ref="imageEditModal"
      @ImageEdited="ImageEdited"
    ></ImageEditModal>
  </div>
</template>
<script>
//アイコンライブラリ
import { library } from "@fortawesome/fontawesome-svg-core";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(fas);

//検証部品
import { validationMixin } from "vuelidate";
import {
  required,
  minLength,
  maxLength,
  numeric,
  email,
  minValue,
  maxValue,
} from "vuelidate/lib/validators";

//編集モーダル ベース
import EditModdalBase from "@components/common/EditModalBase";

//プラン
import PlanModel from "@js/dto/PlanModel";
import PlanInfo from "@js/model/PlanInfo";

//運営者Biz
import ProviderBiz from "@js/biz/ProviderBiz";
import DateUtil from "@js/DateUtil";

//カレンダー
import DatePicker2 from "@components/common/DatePicker2";

//プラン登録用コンポーネント
import AcceptDateModal from "./AcceptDateModal"; //受付可能日モーダル
import DateFeeModal from "./DateFeeModal"; //日別料金設定モーダル
import CategoryList from "@components/common/CategoryList"; //カテゴリ
import AreaMap from "@components/common/AreaMap"; //エリアマップ
import ImageFileChooser from "@components/common/ImageFileChooser"; //画像選択部品
import ImageEditModal from "@components/common/ImageEditModal"; //添付画像編集モーダル
import ImageCard from "@components/common/ImageCard"; //画像カード

import PlanImage from "@js/model/PlanImage";

function initialState() {
  return {
    HeaderTitle: "",
    PlanId: "", //プランID
    ProviderId: "", //事業者ID
    PublicStatus: "", //公開ステータス
    PlanTitle: "", //プランタイトル
    PlanStartDate: "", //プラン開始日
    PlanEndDate: "", //プラン終了日
    PublicStartDate: "", //公開開始日
    PublicEndDate: "", //公開終了日
    PlanAddress: "", //会場住所
    Access: "", //アクセス
    Overview: "", //概要
    Contents: "", //プラン内容
    Fee_Adult: 0, //大人料金
    Fee_Child: 0, //児童料金
    Fee_Infant: 0, //幼児料金
    DeleteFlg: 0, //削除フラグ

    MeetingPlace: "", //集合場所
    TimeRequired: "", //所要時間
    MaxApplicantsNum: 0, //最大申込人数
    PlanCourse: "", // コース
    HiddenFlg: false, // 非表示フラグ
    AllPersonFlg: false, // 全個人情報フラグ
    CancelPolicy: "", // キャンセルポリシー

    AcceptDates: [], //受付可能日付
    SelectedCategoryIds: [], //カテゴリ
    SelectedAreaIds: [], //選択エリア
    TopImage: null, //選択済み トップ画像エンティティ
    TempImages: [], //選択済み 添付イメージファイルリスト

    SelectedTopImageFileName: null, //※登録用 画面選択された トップ画像 ファイル名
    TopImageUrl: "", //※初期表示用 選択済 トップ画像URL
  };
}

export default {
  mixins: [validationMixin],
  components: {
    FontAwesomeIcon,
    EditModdalBase,
    DatePicker2,
    AcceptDateModal,
    DateFeeModal,
    CategoryList,
    AreaMap,
    ImageFileChooser,
    ImageEditModal,
    ImageCard,
  },
  data() {
    return initialState();
    //   return {
    //     HeaderTitle: "",
    //     PlanId: "", //プランID
    //     ProviderId: "", //事業者ID
    //     PublicStatus: "", //公開ステータス
    //     PlanTitle: "", //プランタイトル
    //     PlanStartDate: "", //プラン開始日
    //     PlanEndDate: "", //プラン終了日
    //     PublicStartDate: "", //公開開始日
    //     PublicEndDate: "", //公開終了日
    //     PlanAddress: "", //会場住所
    //     Access: "", //アクセス
    //     Overview: "", //概要
    //     Contents: "", //プラン内容
    //     Fee_Adult: 0, //大人料金
    //     Fee_Child: 0, //児童料金
    //     Fee_Infant: 0, //幼児料金
    //     DeleteFlg: 0, //削除フラグ
    //     MeetingPlace: "", //集合場所
    //     TimeRequired: "", //所要時間
    //     MaxApplicantsNum: 0, //最大申込人数
    //     PlanCourse: "", // コース
    //     HiddenFlg: false, // 非表示フラグ
    //     AllPersonFlg: false, // 全個人情報フラグ
    //     CancelPolicy: "", // キャンセルポリシー
    //     AcceptDates: [], //受付可能日付
    //     SelectedCategoryIds: [], //カテゴリ
    //     SelectedAreaIds: [], //選択エリア
    //     TopImage: null, //選択済み トップ画像エンティティ
    //     TempImages: [], //選択済み 添付イメージファイルリスト
    //     SelectedTopImageFileName: null, //※登録用 画面選択された トップ画像 ファイル名
    //     TopImageUrl: "", //※初期表示用 選択済 トップ画像URL
    //   };
  },
  validations: {
    PlanTitle: { required, maxLength: maxLength(100) }, //プランタイトル
    PlanStartDate: {
      required: required,
      ltEndDate: (v, c) => c.IsltEndDate(v, c),
    }, //プラン開始日
    PlanEndDate: {
      required: required,
      gtStartDate: (v, c) => c.IsgtStartDate(v, c),
    }, //プラン終了日
    PlanAddress: { maxLength: maxLength(100) }, //会場住所
    Access: { maxLength: maxLength(200) }, //アクセス
    Overview: { maxLength: maxLength(200) }, //概要
    Contents: { maxLength: maxLength(1024) }, //プラン内容
    Fee_Adult: { numeric }, //大人料金
    Fee_Child: { numeric }, //児童料金
    Fee_Infant: { numeric }, //幼児料金

    MeetingPlace: { maxLength: maxLength(256) }, //集合場所
    TimeRequired: { maxLength: maxLength(256) }, //所要時間
    MaxApplicantsNum: { numeric }, //最大申込人数
    PlanCourse: { maxLength: maxLength(256) }, //コース
    CancelPolicy: { maxLength: maxLength(1024) }, //キャンセルポリシー
  },
  computed: {
    VisibleTempImages: function () {
      return this.TempImages.filter((e) => !e.DeleteFlg);
    },
    IsButtonDisabled: function () {
      return this.$v.$invalid || this.$refs.editModalBase.Loading;
    },
    Loading: function () {
      return this.$refs.editModalBase.Loading;
    },
    EditMode: function () {
      return this.$refs.editModalBase.EditMode;
    },
    EditModeName: function () {
      return this.$refs.editModalBase.EditModeName;
    },
    PublicStatusVariant: function () {
      if (this.PublicStatus == 0) {
        //非公開
        return "danger";
      } else if (this.PublicStatus == 1) {
        //公開
        return "primary";
      }
    },
    PublicStatusName: function () {
      if (this.PublicStatus == 0) {
        return "非公開";
      } else if (this.PublicStatus == 1) {
        return "公開中";
      }
    },
    IsDateFeeDisabled: function () {
      return this._AcceptDateCount <= 0;
    },
    _PlanIdDisabled: function () {
      if (this.EditMode == "Add") {
        return false;
      } else {
        return true;
      }
    },
    _PlanIdDescription: function () {
      if (this.EditMode == "Add") {
        return "(自動採番)";
      } else {
        return "";
      }
    },
    _AcceptDateDisabled: function () {
      if (!this.PlanStartDate || !this.PlanEndDate) return true;
      return false;
    },
    _AcceptDateCount: function () {
      if (!this.PlanStartDate || !this.PlanEndDate) return 0;

      var ret = 0;
      var start = new Date(this.PlanStartDate);
      var end = new Date(this.PlanEndDate);
      var termDays = (end - start) / 86400000 + 1;
      return termDays;

      return 0;
    },
    _AcceptDateComment: function () {
      var days = this._AcceptDateCount;
      if (days <= 0) {
        return "(期間未選択)";
      }

      var ret = "全期間 " + days + "日";

      if (this.AcceptDates.length <= 0) {
        ret += " (受付日未設定)";
      } else if (days == this.AcceptDates.length) {
        ret += " 全日 受付可能";
      } else {
        ret += " 中 " + this.AcceptDates.length + "日 受付可能";
      }

      return ret;
    },
  },
  methods: {
    IsltEndDate: function (value, _this) {
      return value <= _this.PlanEndDate;
    },
    IsgtStartDate: function (value, _this) {
      return value >= _this.PlanStartDate;
    },
    OpenAdd: function () {
      //一旦フォームをクリア
      Object.assign(this.$data, initialState());

      var editItem = new PlanInfo();

      editItem.PlanId = -1;
      editItem.PublicStatus = 0;

      var now = new Date();
      editItem.PlanStartDate = DateUtil.GetDateStringZero(now);
      editItem.PlanEndDate = DateUtil.GetDateStringZero(now);

      //値をコピー
      Object.assign(this.$data, editItem);
      this.AcceptDates = [];
      this.DateFeeList = [];

      this.SelectedCategoryIds = [];
      this.SelectedAreaIds = [];

      //トップ画像
      this.TopImage = null;
      this.TopImageUrl = null;

      //添付画像群
      this.TempImages = [];

      this.$refs.editModalBase.open("Add");
      this.HeaderTitle = "プラン " + this.EditModeName;
    },
    OpenEdit: async function (editItem) {
      //一旦フォームをクリア
      Object.assign(this.$data, initialState());

      try {
        //プラン情報のGET
        var planModel = await ProviderBiz.GetPlanModel(editItem.PlanId);
        planModel.PlanEt.PlanStartDate = DateUtil.GetDateStringZero(
          planModel.PlanEt.PlanStartDate
        );
        planModel.PlanEt.PlanEndDate = DateUtil.GetDateStringZero(
          planModel.PlanEt.PlanEndDate
        );

        //値をコピー
        Object.assign(this.$data, planModel.PlanEt);
        this.AcceptDates = DateUtil.ToDateArray(planModel.AcceptDates);
        this.DateFeeList = planModel.DateFeeList;
        this.SelectedCategoryIds = planModel.SelectedCategoryIds;
        this.SelectedAreaIds = planModel.SelectedAreaIds;

        //トップ画像
        this.TopImage = null;
        this.TopImageUrl = null;

        if (planModel.TopImage != null) {
          this.TopImage = planModel.TopImage;
          this.TopImageUrl =
            process.env.VUE_APP_API_BASE_URL + planModel.TopImage.LocalImageUrl;
        }

        //添付画像群
        this.TempImages = planModel.TempImages;

        this.$refs.editModalBase.open("Edit");
        this.HeaderTitle = "プラン " + this.EditModeName;
      } catch (e) {
        this.$bvToast.toast(e, {
          variant: "danger ",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 3000,
          noCloseButton: true,
          textCenter: true,
        });
      }
    },
    OpenRead: async function (editItem) {
      try {
        //プラン情報のGET
        var planModel = await ProviderBiz.GetPlanModel(editItem.PlanId);

        //値をコピー
        Object.assign(this.$data, planModel.PlanEt);
        this.AcceptDates = DateUtil.ToDateArray(planModel.AcceptDates);
        this.DateFeeList = planModel.DateFeeList;
        this.SelectedCategoryIds = planModel.SelectedCategoryIds;
        this.SelectedAreaIds = planModel.SelectedAreaIds;

        this.$refs.editModalBase.open("Edit");
        this.HeaderTitle = "プラン " + this.EditModeName;
      } catch (e) {
        this.$bvToast.toast(e, {
          variant: "danger ",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 3000,
          noCloseButton: true,
          textCenter: true,
        });
      }
    },
    OkClicked: async function () {
      var planModel = new PlanModel();

      var planEt = new PlanInfo();
      Object.assign(planEt, this.$data);

      //各種DataをPostエンティティに設定
      planModel.EditMode = this.EditMode;
      planModel.PlanEt = planEt;
      planModel.AcceptDates = this.AcceptDates;
      planModel.DateFeeList = this.DateFeeList;

      planModel.SelectedCategoryIds = this.SelectedCategoryIds;
      planModel.SelectedAreaIds = this.SelectedAreaIds;
      planModel.TopImage = this.TopImage;

      planModel.SelectedTopImageFileName = null;
      planModel.TempImages = this.TempImages;

      //ファイルリスト
      var fileElements = [];
      if (this.SelectedTopImageElement != null) {
        fileElements.push(this.SelectedTopImageElement);
        planModel.SelectedTopImageFileName = this.SelectedTopImageFileName;
      }
      for (var planImage of this.TempImages) {
        fileElements.push(planImage.FileElement);
      }

      this.$refs.editModalBase.ErrorMessage = "";
      this.$refs.editModalBase.Loading = true;

      try {
        //プラン情報のPOST
        await ProviderBiz.PostPlan(planModel, fileElements);

        this.$refs.editModalBase.Loading = false;
        this.$refs.editModalBase.IsComplete = true;

        this.$emit("planEdited");
        this.$bvToast.toast("プランが登録されました", {
          variant: "success",
          solid: true,
          toaster: "b-toaster-top-right",
          autoHideDelay: 3000,
          noCloseButton: true,
          textCenter: true,
        });

        this.$refs.editModalBase.close();
      } catch (e) {
        this.$refs.editModalBase.ErrorMessage = e;
        this.$refs.editModalBase.Loading = false;
      }
    },
    TogglePublicStatus: async function (publicStatus) {
      this.$refs.editModalBase.ErrorMessage = "";
      this.$refs.editModalBase.Loading = true;

      //プラン情報のPOST
      try {
        var planInfo = await ProviderBiz.PostPlanPublicStatus(
          this.PlanId,
          publicStatus
        );

        this.$refs.editModalBase.Loading = false;
        this.$refs.editModalBase.IsComplete = true;

        if (planInfo.PublicStatus == publicStatus) {
          this.PublicStatus = publicStatus;

          this.$emit("planEdited");

          var msg =
            publicStatus == 0
              ? "プランを非公開にしました"
              : "プランを公開しました";

          this.$bvToast.toast(msg, {
            variant: "success",
            solid: true,
            toaster: "b-toaster-top-center",
            autoHideDelay: 3000,
            noCloseButton: true,
            textCenter: true,
          });
        }
      } catch (e) {
        this.$refs.editModalBase.ErrorMessage = e;
        this.$refs.editModalBase.Loading = false;
      }
    },
    AreaMapActivate: function () {
      setTimeout(() => {
        this.$refs.AreaMap.InitAreaMap();
      }, 0);
    },
    ShowAcceptDateModal: function () {
      this.$refs.acceptDateModal.open(
        this.PlanStartDate,
        this.PlanEndDate,
        this.AcceptDates
      );
    },
    AcceptDateSetted: function (acceptDate) {
      this.AcceptDates = acceptDate;
    },
    ShowDateFeeModal: function () {
      this.$refs.dateFeeModal.open(
        this.PlanId,
        this.Fee_Adult,
        this.Fee_Child,
        this.Fee_Infant,
        this.AcceptDates,
        this.DateFeeList
      );
    },
    DateFeeSetted: function (dateFeeList) {
      this.DateFeeList = dateFeeList;
    },
    ShowImageEditModal_Add: function () {
      this.$refs.imageEditModal.OpenAdd();
    },
    ImageCardClicked: function ({ sender, ImageBase }) {
      this.$refs.imageEditModal.OpenEdit(ImageBase);
    },
    ImageEdited: function (param) {
      if (param.EditMode == "Add") {
        var newImage = new PlanImage();
        Object.assign(newImage, param.ImageBase);

        newImage.EditMode = "Add";
        newImage.PlanId = this.PlanId;
        newImage.ImageNo = this.TempImages.length + 1; //初期値
        newImage.ImageLevel = 1; //Top以外
        newImage.SortOrder = this.TempImages.length + 1;

        this.TempImages.push(newImage);
      } else if (param.EditMode == "Edit") {
        let tempImg = this.TempImages.find(
          (e) => e.ImageNo == param.ImageBase.ImageNo
        );
        if (tempImg) {
          Object.assign(tempImg, param.ImageBase);

          if (tempImg.EditMode == "Add" || tempImg.EditMode == "Deleted") {
          } else {
            tempImg.EditMode = "Edit";
          }
        }
      }
    },
    ImageCardDeleting: function ({ sender, ImageBase }) {
      let tempImg = this.TempImages.find((e) => e.ImageNo == ImageBase.ImageNo);
      if (tempImg) {
        if (tempImg.EditMode == "Add") {
          const i = this.TempImages.indexOf(tempImg);
          this.TempImages.splice(i, 1);
        } else {
          tempImg.DeleteFlg = true;
          tempImg.EditMode = "Deleted";
        }
      }
    },
    CategorySelectedChanged: function (selectedCategoryIds) {
      this.SelectedCategoryIds = selectedCategoryIds;
    },
    TopImageChoosed: function (param) {
      this.SelectedTopImageElement = param.FileElement;
      this.SelectedTopImageFileName = param.FileElement.name; //TODO:プロパティチェック！
      if (this.TopImage != null) {
        this.TopImage.DeleteFlg = true;
      }
    },
    ValidateState: function (propName) {
      const { $dirty, $error } = this.$v[propName];
      return $dirty ? !$error : null;
    },
    CancelClicked: function () {
      this.$refs.editModalBase.close();
    },
  },
  mounted() {},
};
</script>