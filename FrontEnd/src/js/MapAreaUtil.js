import axios from 'axios'
const CANVAS_ID_BASE = "map-canvas-base";
const CANVAS_ID_HOVER = "map-canvas-hover";
const CANVAS_ID_SELECTED = "map-canvas-selected";

var AreaData = null;
var Areas = new Array();

export default class MapAreaUtil {

    constructor(imgId) {
        this.ImgId = imgId;
        // this.AreaData = null;
        // this.Areas = new Array();

        this.hdc = null;

        this._addArea = function (areas) {
            areas.forEach((area) => {
                //this.Areas.push(area);
                Areas.push(area);
                if (area.Areas && area.Areas.length > 0) {
                    this._addArea(area.Areas);
                }
            });
        };

        this.GetAreaData = async function () {
            //if (this.AreaData != null) {
            if (AreaData != null) {
                //console.log("GetAreaData RETURNED!!!");
                //return this;
                return {
                    AreaData: AreaData,
                    Areas: Areas
                }
            }

            //console.log("GetAreaData EXECUTED!!!");

            return new Promise((resolve, reject) => {

                //エリアデータJsonの解析
                const url = process.env.VUE_APP_API_BASE_URL + "/resource/area_data.json";
                axios.get(url)
                    .then((res) => {
                        //this.AreaData = res.data;
                        AreaData = res.data;
                        //this._addArea(this.AreaData.Areas);
                        this._addArea(AreaData.Areas);
                        //resolve(this);
                        resolve({
                            AreaData: AreaData,
                            Areas: Areas
                        });
                    })
                    .catch(err => {
                        reject(err);
                    })
            })
        };

        this._createCanvas = function (img, canvasId, zIndex) {
            //イメージサイズ
            var x, y, w, h;
            x = img.offsetLeft;
            y = img.offsetTop;
            w = img.clientWidth;
            h = img.clientHeight;

            var imgParent = img.parentNode;

            //キャンバスを検索（無ければ作成）
            var can = document.getElementById(canvasId);
            if (!can) {
                can = document.createElement("canvas");
                can.setAttribute('id', canvasId);
                imgParent.appendChild(can);
            }

            //キャンバスのスタイル設定
            can.style.zIndex = zIndex;

            can.style.left = x + 'px';

            can.style.top = y + 'px';
            can.style.height = h + "px";

            can.style.position = "absolute";
            can.style.width = w + "px";
            can.style.pointerEvents = "none";

            can.setAttribute('width', w + 'px');
            can.setAttribute('height', h + 'px');

            return can;
        };

        this.ClearBaseCanvas = function () {
            //選択可能エリアのキャンバスの取得
            var img = document.getElementById(this.ImgId);
            if (!img) return;

            var baseCanvas = this._createCanvas(img, CANVAS_ID_BASE, 1);
            var ctx = baseCanvas.getContext("2d");
            ctx.clearRect(0, 0, baseCanvas.width, baseCanvas.height);
        }

        //キャンバスの初期化
        this.InitCanvas = function (areaElms, selectedAreaIds) {
            var img = document.getElementById(this.ImgId);
            if (!img) return;

            //選択可能エリアのキャンバスの作成
            var baseCanvas = this._createCanvas(img, CANVAS_ID_BASE, 1);
            var ctx = baseCanvas.getContext("2d");

            //一旦全て薄く塗りつぶし、、、
            ctx.fillStyle = "rgba(255,255,255,0.7)";
            ctx.fillRect(0, 0, baseCanvas.width, baseCanvas.height);
            //選択可能領域は塗りつぶしをキャンセル（透明色で再塗りつぶし）
            ctx.globalCompositeOperation = "destination-out";
            for (let index = 0; index < areaElms.length; index++) {
                var areaElm = areaElms[index];
                if (areaElm.coords) {
                    this._selecttionAreaDraw(ctx, areaElm.coords);
                }
            }

            //マウスオーバーキャンバスの作成
            var hoverCanvas = this._createCanvas(img, CANVAS_ID_HOVER, 2);
            this.hdc = hoverCanvas.getContext('2d');


            //選択済み領域キャンバスの作成
            this.InitSelectedCanvas(selectedAreaIds);
        };

        //選択済み領域の描画
        this.InitSelectedCanvas = function (selectedAreaIds) {
            var img = document.getElementById(this.ImgId);
            if (!img) return;

            var selectedCanvas = this._createCanvas(img, CANVAS_ID_SELECTED, 3);

            var ctx = selectedCanvas.getContext("2d");
            //一旦クリア
            ctx.clearRect(0, 0, selectedCanvas.width, selectedCanvas.height);

            if (selectedAreaIds) {
                //this.Areas.forEach(e => {
                Areas.forEach(e => {
                    if (selectedAreaIds.indexOf(e.AreaId) >= 0) {
                        var areaElm = document.getElementById("map-area-" + e.AreaId)
                        if (areaElm) {
                            this._selectedAreaDraw(ctx, areaElm.coords);
                        }
                    }
                });
            }
        };

        //選択可能エリアの表示
        this._selecttionAreaDraw = function (ctx, coordStr) {
            this._createPath(ctx, coordStr);
            ctx.fillStyle = "rgba(0,0,0,1)";
            ctx.fill();//パス内の領域（選択可能領域）を塗りつぶす＝クリアする

            //輪郭も描画し境界をきれいにする
            this._createPath(ctx, coordStr);
            ctx.lineWidth = 5;
            ctx.strokeStyle = "rgba(0,0,0,1)";
            ctx.stroke();
        };

        //選択済みエリアの表示
        this._selectedAreaDraw = function (ctx, coordStr) {
            this._createPath(ctx, coordStr);
            ctx.fillStyle = "rgba(240,173,78,0.3)";//variant Warningと同じ色
            ctx.fill();//パス内の領域（選択可能領域）を塗りつぶす

            //輪郭も描画し境界をきれいにする
            this._createPath(ctx, coordStr);
            ctx.lineWidth = 4;
            ctx.strokeStyle = "rgba(240,173,78,1)";//variant Warningと同じ色
            ctx.stroke();
        };


        //マウスオーバー の描画
        this.AreaHover = function (coordStr, areaType) {
            switch (areaType) {
                case 'polygon':
                case 'poly':
                    this._drawPoly(coordStr);
                    break;

                case 'rect':
                    this._drawRect(coordStr);
            }
        };
        this._drawPoly = function (coordStr) {
            this._createPath(this.hdc, coordStr);
            this.hdc.lineWidth = 5;
            // this.hdc.strokeStyle = 'rgba(64,64,255,0.6)';
            this.hdc.strokeStyle = "rgba(240,173,78,1)";//variant Worningと同じ色
            this.hdc.stroke();
        };
        this._drawRect = function (coordStr) {
            var mCoords = coordStr.split(',');
            var top, left, bot, right;
            left = mCoords[0];
            top = mCoords[1];
            right = mCoords[2];
            bot = mCoords[3];
            this.hdc.strokeRect(left, top, right - left, bot - top);
        };



        this.AreaLeave = function () {
            var canvas = document.getElementById(CANVAS_ID_HOVER);
            this.hdc.clearRect(0, 0, canvas.width, canvas.height);
        };

        this._createPath = function (ctx, coordStr) {
            var mCoords = coordStr.split(',');
            var i, n;
            n = mCoords.length;

            ctx.beginPath();
            ctx.moveTo(mCoords[0], mCoords[1]);
            for (i = 2; i < n; i += 2) {
                ctx.lineTo(mCoords[i], mCoords[i + 1]);
            }
            ctx.lineTo(mCoords[0], mCoords[1]);
            ctx.closePath();
        }


    }
}