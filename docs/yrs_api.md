FORMAT: 1A
# 吉野リザーブ(仮)  API仕様書


# Data Structures
## PublicPlanInfo (object)
+ ProviderName : 事業者 (string) - 事業者名
+ ProviderIndustry : 観光業 (string) - 業種
+ ProviderAddress : 住所 (string) - 住所
+ TantoName : 担当者名 (string) - 担当者名
+ TantoMailAddress : hoge@moge.com (string) - 担当者メールアドレス
+ TantoPhone : 090-1234-5678 (string) - 電話番号
+ PlanId : 1 (number) - プランID
+ ProviderId : jigyo@sya.com (string) - 事業者ID
+ PublicStatus : 0 (number) - 公開ステータス
+ PlanTitle : プランタイトル (string) - プランタイトル
+ PlanStartDate : 2020/09/01 (string) - プラン開始日
+ PlanEndDate : 2020/09/30 (string) - プラン終了日
+ PublicStartDate :  (string) - 公開開始日
+ PublicEndDate :  (string) - 公開終了日
+ PlanAddress : 会場住所 (string) - 会場住所
+ Access : アクセス (string) - アクセス
+ Overview : 概要 (string) - 概要
+ Contents : プラン内容 (string) - プラン内容
+ Fee_Adult : 1800 (number) - 大人料金
+ Fee_Child : 900 (number) - 児童料金
+ Fee_Infant : 450 (number) - 幼児料金
+ DeleteFlg : 0 (number) - 削除フラグ
+ MeetingPlace : 駅前 (string) - 集合場所
+ TimeRequired : 60分 (string) - 所要時間
+ MaxApplicantsNum : 8 (number) - 最大申込人数
+ PublicTopImageUrl : /imgs/plans/{PlanId}/top (string) - トップ画像URI
+ MinAcceptDate : 2020/09/01 (string) - 最小 受付可能日
+ MaxAcceptDate : 2020/09/30 (string) - 最大 受付可能日
+ MaxFee_Adult : 0 (number) - 最大 大人料金
+ MinFee_Adult : 0 (number) - 最小 大人料金
+ MaxFee_Child : 0 (number) - 最大 児童料金
+ MinFee_Child : 0 (number) - 最小 児童料金
+ MaxFee_Infant : 0 (number) - 最大 幼児料金
+ MinFee_Infant : 0 (number) - 最小 幼児料金



## PlanDate (object)
+ PlanId : 1 (number) - プランID
+ AcceptDate : 2020/09/01 (string) - 受付可能日
+ Fee_Adult : 1800 (number) - 大人料金
+ Fee_Child : 900 (number) - 児童料金
+ Fee_Infant : 450 (number) - 幼児料金


## PlanImage (object)
+ PlanId : 1 (number) - プランID
+ ImageNo : 1 (number) - イメージNo
+ ImageLevel : 0 (number) - イメージレベル
+ SortOrder : 0 (number) - ソート順
+ FileName : top.jpg (string) - ファイル名
+ Comment : トップ画像 (string) - コメント
+ LocalImageFolder : /Plan/1/images (string) - ローカルイメージフォルダ
+ LocalImagePath : /Plan/1/images/top.jpg (string) - ローカルイメージパス
+ LocalImageUrl : /files/Plan/1/images/top.jpg (string) - ローカルイメージURL
+ PublicImageUrl :/imgs/plans/1/tmp/0 (string) - パブリックイメージURL


## PlanImageTmp (object)
+ PlanId : 1 (number) - プランID
+ ImageNo : 2 (number) - イメージNo
+ ImageLevel : 1 (number) - イメージレベル
+ SortOrder : 1 (number) - ソート順
+ FileName : temp.jpg (string) - ファイル名
+ Comment : 添付画像 (string) - コメント
+ LocalImageFolder : /Plan/1/images (string) - ローカルイメージフォルダ
+ LocalImagePath : /Plan/1/images/temp.jpg (string) - ローカルイメージパス
+ LocalImageUrl : /files/Plan/1/images/temp.jpg (string) - ローカルイメージURL
+ PublicImageUrl :/imgs/plans/1/tmp/2 (string) - パブリックイメージURL


## PlanListSearchEt (object)
+ AcceptDates : 2020/09/01,2020/09/02,2020/09/03 (array[string]) - 受付可能日リスト
+ CategoryIds : 1,4,6 (array[number]) - カテゴリリスト
+ AreaIds : 100,101,103,200 (array[number]) - エリアリスト
+ AcceptDateFrom : 2020/09/01 (string) - 受付可能日（From)
+ AcceptDateTo : 2020/09/30 (string) - 受付可能日（To)
+ MaxApplicantsNum : 3 (number) - 最大申込人数

## PublicPlanModel (object)
+ PlanInfo : (PublicPlanInfo) - プラン情報
+ AcceptDates : 2020/09/01,2020/09/02,2020/09/03 (array[string]) - 受付可能日リスト
+ DateFeeList : (array[PlanDate]) - 日別料金リスト
+ CategoryIds : 1,4,6 (array[number]) - カテゴリリスト
+ AreaIds : 100,101,103,200 (array[number]) - エリアリスト
+ TopImage : (PlanImage) - トップ画像情報
+ TempImages : (array[PlanImage]) - 添付画像リスト


## Category_M (object)
+ CategoryId : 1 (number) - カテゴリID
+ CategoryName : カテゴリ (string) - カテゴリ名
+ CategoryLevel : 0 (number) - カテゴリレベル

## ApplicationInfo (object)
+ ApplicationId : 1 (number) - 申込ID
+ PlanId : 1 (number) - プランID
+ ProviderId : provider@hoge.com (string) - 事業者ID
+ ApplicationStatus : 0 (number) - 申込ステータス
+ UserName : 申込者名 (string) - ユーザ名
+ UserMailAddress : user@hoge.com - ユーザメールアドレス
+ UserPhone : 1112223333 - ユーザ電話番号
+ AcceptDate : 2020/09/05 (string) - 来場日
+ Num_Stays : 0 (number) - 泊数
+ Num_Adult : 0 (number) - 大人人数
+ Num_Child : 0 (number) - 児童人数
+ Num_Infant : 0 (number) - 幼児人数
+ Fee_Adult : 0 (number) - 大人料金(申込時)
+ Fee_Child : 0 (number) - 児童料金(申込時)
+ Fee_Infant : 0 (number) - 幼児料金(申込時)
+ PointCardNo : 123456789 (string) - 吉野ポイントカード№
+ PointCardFlg : 0  (number)- ポイントカード申込フラグ
+ ZipCode : 123-4567 (string) - 郵便番号
+ UserAddress : 住所 (string) - 住所
+ BirthDate : 1990/01/01 (string) - 生年月日
+ Sex : 0 (number) - 性別
+ InsertDate : 2020/09/05 (string) - 登録日
+ UpdateDate : 2020/09/05 (string) - 更新日
+ DeleteFlg : 0 (number) - 削除フラグ

# Group 画像取得 API
## トップ画像 [/imgs/plans/{planId}/top]
### トップ画像 [GET]
* 指定されたプランIDのプランのトップ画像を返します
* 指定されたプランIDのプランが見つからない（または公開されていない）場合や、トップ画像が設定されていない場合 [404 NotFond]が返ります
+ Parameters
    + planId: 1 (number, required) - プランID
+ Response 200 (image/*)
+ Response 404 (text/plain; charset=utf-8) 
    画像が見つかりません Plan[{planId}]


## 添付画像 [/imgs/plans/{planId}/tmp/{imgNo}]
### 添付画像 [GET]
* 指定されたプランIDのプランの添付画像(イメージNoによってはTop画像)を返します
* 指定されたプランIDのプランが見つからない（または公開されていない）場合 [404 NotFond]が返ります
* 指定されたイメージNoの画像が見つからない場合 [404 NotFond]が返ります
+ Parameters
    + planId: 1 (number, required) - プランID
    + imgNo: 1 (number, required) - イメージNo
+ Response 200 (image/*)
+ Response 404 (text/plain; charset=utf-8) 
    画像が見つかりません Plan[{planId}],Img[{imgNo}]



# Group データ取得 API
データ取得API(/jobj/*)では、インタフェースが統一されています。  
現時点でサポートしているフォーマットはapplication/jsonのみです。

Response 200 (application/json) の場合（または サーバでハンドリングできるエラーの場合）  
下記フォーマットのJSONデータが返されます

## 共通フォーマット
**正常終了(200)の例**
```json
{
    "resultData":"<文字列としてエスケープされたJSONデータ>",
    "hasError": false,
    "errorMessage": null
}
```

**異常終了(404)の例**
```json
{
    "resultData":null,
    "hasError": true,
    "errorMessage": "プランが見つかりません Plan[1]"
}
```
+ **resultData**　:　処理結果 JSON データ (string)   
    + 検索結果の一覧等、実際の処理結果JSON文字列です。  
    文字列なので 取得後 JSONObject等にParseする必要があります。
        ```javascript
        //JavaScriptの例
        const resultData = responce.resultData;
        var obj = JSON.parse(resultData);
        ```

+ **hasError**　:　(boolean,required)
    + サーバサイドでエラーが発生した場合、trueが設定されます。  
    httpStatusCodeが200（正常終了）の場合でも trueの場合があます。
+ **errorMessage**　:　エラーメッセージ (string) -エラーメッセージ  
    + サーバサイドでエラーが発生した場合のエラーメッセージが設定されます。  
    通常はnullです。



<br/><br/>

## カテゴリリスト取得 [/jobj/categories]
### カテゴリリスト取得  [GET]
* カテゴリーのリストを返します。
+ Parameters
+ Response 200 (application/json)
    + Attributes
        + resultData: (array[Category_M])
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ

## プランリスト簡易検索 [/jobj/plans]
### プランリスト簡易検索 [GET]
* 公開されているプランで オススメ条件に該当するプランのリストを返します。
+ Parameters
+ Response 200 (application/json)
    + Attributes
        + resultData: (array[PublicPlanInfo])
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ

## プランリスト詳細検索 [/jobj/plans]
### プランリスト詳細検索  [POST]
* 公開されているプランで 指定された条件に該当するプランのリストを返します。
* 検索条件データの各プロパティを null(配列の場合はLength0)を設定するとその検索条件は無条件として処理されます。
* プロパティ同士は AND条件 で検索されます。
* 配列内は OR条件 として検索されます。
+ Request (application/json)
    + Attributes (PlanListSearchEt)
+ Response 200 (application/json)
    + Attributes
        + resultData: (array[PublicPlanInfo])
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ



## プランモデル取得 [/jobj/plans/{planId}]
### プランモデル取得 [GET]
* 指定されたプランIDのプラン情報を返します
* 指定されたプランIDのプランが見つからない（または公開されていない）場合 [404 NotFond]が返ります
+ Parameters
    + planId: 1 (number, required) - プランID
+ Response 200 (application/json)
    + Attributes
        + resultData: PublicPlanModel (PublicPlanModel)
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ
+ Response 404 (text/plain; charset=utf-8) 
    プランが見つかりません Plan[{planId}]

## 画像モデルリスト取得 [/jobj/plans/{planId}/imgs]
### 画像モデルリスト取得 [GET]
* 指定されたプランIDのプランの画像モデルリストを返します
+ Parameters
    + planId: 1 (number, required) - プランID
+ Response 200 (application/json)
    + Attributes
        + resultData: (array[PlanImage])
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ
+ Response 404 (text/plain; charset=utf-8) 
    プランが見つかりません Plan[{planId}]

## トップ画像モデル取得 [/jobj/plans/{planId}/imgs/top]
### トップ画像モデル取得 [GET]
* 指定されたプランIDのプランのトップ画像データを返します
* 指定されたプランIDのプランが見つからない（または公開されていない）場合や、トップ画像が設定されていない場合 [404 NotFond]が返ります
+ Parameters
    + planId: 1 (number, required) - プランID
+ Response 200 (application/json)
    + Attributes
        + resultData: (PlanImage)
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ
+ Response 404 (text/plain; charset=utf-8) 
    画像が見つかりません Plan[{planId}]


## 添付画像モデル取得 [/jobj/plans/{planId}/imgs/tmp/{imgNo}]
### 添付画像モデル取得 [GET]
* 指定されたプランIDのプランの添付画像(イメージNoによってはTop画像)データを返します
* 指定されたプランIDのプランが見つからない（または公開されていない）場合 [404 NotFond]が返ります
* 指定されたイメージNoの画像が見つからない場合 [404 NotFond]が返ります
+ Parameters
    + planId: 1 (number, required) - プランID
    + imgNo: 2 (number, required) - イメージNo
+ Response 200 (application/json)
    + Attributes
        + resultData: (PlanImageTmp)
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ
+ Response 404 (text/plain; charset=utf-8) 
    画像が見つかりません Plan[{planId}],Img[{imgNo}]

## 申込情報 [/jobj/apps]
### 申込情報登録  [POST]
* 申込情報を新規登録します。
* ApplicationId に0以下を設定してください。
* 既存のApplicationIdを指定した状態でPOSTするとエラーとなります。
+ Request (application/json)
    + Attributes (ApplicationInfo)
+ Response 200 (application/json)
    + Attributes
        + resultData: (ApplicationInfo)
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ


<!-- ## 申込情報 [/jobj/apps]
### 申込情報登録  [PUT]
* 申込情報を更新します。
* ApplicationId に既存の値を設定してください。
* 既存ではApplicationIdを指定した状態でPUTするとエラーとなります。
+ Request (application/json)
    + Attributes (ApplicationInfo)
+ Response 200 (application/json)
    + Attributes
        + resultData: (ApplicationInfo)
        + hasError: false (boolean, required)
        + errorMessage: null (string,option) -エラーメッセージ -->