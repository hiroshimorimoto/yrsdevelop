using System;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;


using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.Azure; // Namespace for Azure Configuration Manager
using Microsoft.Azure.Storage; // Namespace for Storage Client Library
using Microsoft.Azure.Storage.File; // Namespace for Azure Files
using System.Data.Entity.Infrastructure;
using YrsWeb.Lib.Models;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Common;
using Newtonsoft.Json.Linq;
using System.IO;
using YrsWeb.Biz;

namespace YrsWeb.Controllers
{
    public partial class ApiController
    {
        [HttpPost("File/Upload")]
        [Produces("application/json")]
        public ApiResult<string> Upload(string shareName, string folderPath, List<IFormFile> files)
        {
            ApiResult<string> result = new ApiResult<string>();
            try
            {
                List<StorageFileInfo> ret = new List<StorageFileInfo>();
                CloudFileShare share = this.FileClient.GetShareReference(shareName);
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                CloudFileDirectory folder = FileBiz.FindDir(rootDir, folderPath, true);
                folder.CreateIfNotExists();

                foreach (IFormFile file in files)
                {
                    CloudFile cloudFile = folder.GetFileReference(file.FileName);
                    if (cloudFile.Exists())
                    {//既存の場合は削除
                        cloudFile.Delete();
                    }

                    StorageFileInfo fi = new StorageFileInfo();
                    fi.ShareName = share.Name;
                    fi.Name = cloudFile.Name;
                    fi.MetaData = cloudFile.Metadata;
                    using (Stream s = file.OpenReadStream())
                    {
                        fi.Size = s.Length;
                        cloudFile.UploadFromStream(s);
                    }

                    ret.Add(fi);
                }

                result.ResultData = JsonConvert.SerializeObject(ret);
                return result;
            }
            catch (Exception ex)
            {
                throw new YrsWebException(ex.Message, ex);
            }
        }

        [HttpPost("File/Download")]
        [Produces("application/octet-stream")]
        public IActionResult Download(string shareName, string folderPath, string fileName)
        {
            try
            {
                CloudFileShare share = this.FileClient.GetShareReference(shareName);
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                CloudFileDirectory folder = FileBiz.FindDir(rootDir, folderPath, false);
                if (folder == null || !folder.Exists())
                    throw new YrsWebException(String.Format("ディレクトリ「{0}」が見つかりません", folderPath));

                CloudFile cloudFile = folder.GetFileReference(fileName);
                if (!cloudFile.Exists())
                    throw new YrsWebException(String.Format("ファイル「{0}」が見つかりません", fileName));

                return this.File(cloudFile.OpenRead(), "application/octet-stream");
            }
            catch (YrsWebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new YrsWebException(ex.Message, ex);
            }
        }
    }
}
