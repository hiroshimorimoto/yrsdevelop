import ApiUtil from '@js/ApiUtil';
import FileUtil from '@js/FileUtil';

import BizMethodInfo from "@js/dto/BizMethodInfo.js";


export default class FileBiz {
    constructor() {
    }

    static get BIZ_NAME() { return "File"; }

    static async GetFileList(folderPath) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetFileList");
        bmi.AddStringParam("shareName", FileUtil.ShareName);
        bmi.AddStringParam("folderPath", folderPath);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(FileBiz.BIZ_NAME, bmi);
    }

    static async GetDirectoryList(parentFolderPath) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetDirectoryList");
        bmi.AddStringParam("shareName", FileUtil.ShareName);
        bmi.AddStringParam("parentFolderPath", parentFolderPath);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(FileBiz.BIZ_NAME, bmi);
    }

    static async DeleteSubFiles(parentFolderPath) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("DeleteSubFiles");
        bmi.AddStringParam("shareName", FileUtil.ShareName);
        bmi.AddStringParam("parentFolderPath", parentFolderPath);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(FileBiz.BIZ_NAME, bmi);
    }

    static async DeleteFile(folderPath, fileName) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("DeleteFile");
        bmi.AddStringParam("shareName", FileUtil.ShareName);
        bmi.AddStringParam("folderPath", folderPath);
        bmi.AddStringParam("fileName", fileName);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(FileBiz.BIZ_NAME, bmi);
    }

    static async GetFileInfo(folderPath, fileName) {
        //Bizメソッドパラメータ
        var bmi = new BizMethodInfo("GetFileInfo");
        bmi.AddStringParam("shareName", FileUtil.ShareName);
        bmi.AddStringParam("folderPath", folderPath);
        bmi.AddStringParam("fileName", fileName);

        //Bizメソッド APIコール
        return ApiUtil.callApi_BizMethodInfo(FileBiz.BIZ_NAME, bmi);
    }

    static async Upload(folderPath, file) {
        return FileUtil.Upload(folderPath, file);
    }
}