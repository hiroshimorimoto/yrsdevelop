using Microsoft.CodeAnalysis.CSharp.Syntax;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;

using YrsWeb.Controllers;
using YrsWeb.Lib.Common;
using YrsWeb.Lib.Dto;
using YrsWeb.Lib.Models;
using Microsoft.Azure; // Namespace for Azure Configuration Manager
using Microsoft.Azure.Storage; // Namespace for Storage Client Library
using Microsoft.Azure.Storage.File; // Namespace for Azure Files
using Microsoft.AspNetCore.Http;

namespace YrsWeb.Biz
{
    public class FileBiz : AbstractBiz
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="dbContext"></param>
        public FileBiz(BaseController controller, YRS_DBEntities dbContext) : base(controller, dbContext) { }


        public static CloudFileDirectory FindDir(CloudFileDirectory parentDir, string folderName, bool isCreate)
        {
            List<string> folderNames = folderName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            return FindDir(parentDir, folderNames, isCreate);
        }

        public static CloudFileDirectory FindDir(CloudFileDirectory parentDir, List<string> folderNames, bool isCreate)
        {
            if (parentDir == null) return null;
            if (folderNames.Count <= 0) return parentDir;

            if (!parentDir.Exists())
            {
                if (isCreate)
                {
                    parentDir.Create();
                }
                else
                {
                    return null;
                }
            }

            CloudFileDirectory subDir = parentDir.GetDirectoryReference(folderNames[0]);
            if (!subDir.Exists())
            {
                if (isCreate)
                {
                    subDir.Create();
                }
                else
                {
                    return null;
                }
            }

            if (folderNames.Count <= 1)
            {
                return subDir;
            }
            else
            {
                folderNames.RemoveAt(0);
                return FindDir(subDir, folderNames, isCreate);
            }
        }

        private StorageFileInfo CreateStorageFileInfo(CloudFileShare share, CloudFile cloudFile)
        {
            cloudFile.FetchAttributes();
            StorageFileInfo fi = new StorageFileInfo();
            fi.ShareName = share.Name;
            fi.Name = cloudFile.Name;
            fi.MetaData = cloudFile.Metadata;
            fi.Size = cloudFile.Properties.Length;
            fi.ContentType = cloudFile.Properties.ContentType;
            if (cloudFile.Properties.LastModified.HasValue)
                fi.LastModified = cloudFile.Properties.LastModified.Value.LocalDateTime;

            return fi;
        }

        public List<StorageFileInfo> GetFileList(string shareName, string folderPath)
        {
            List<StorageFileInfo> ret = new List<StorageFileInfo>();

            CloudFileShare share = base.FileClient.GetShareReference(shareName);
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            CloudFileDirectory folder = FindDir(rootDir, folderPath, true);

            //FileRequestOptions opt = new FileRequestOptions();
            List<IListFileItem> results = new List<IListFileItem>(folder.ListFilesAndDirectories());
            foreach (IListFileItem listItem in results)
            {
                //Console.WriteLine("- {0} (type: {1})", listItem.Uri, listItem.GetType());
                if (listItem is CloudFile cloudFile)
                {
                    StorageFileInfo fi = this.CreateStorageFileInfo(share, cloudFile);

                    ret.Add(fi);
                }
            }

            return ret;
        }

        public List<StorageDirectoryInfo> GetDirectoryList(string shareName, string parentFolderPath)
        {
            List<StorageDirectoryInfo> ret = new List<StorageDirectoryInfo>();

            CloudFileShare share = base.FileClient.GetShareReference(shareName);
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();

            CloudFileDirectory folder = FindDir(rootDir, parentFolderPath, false);
            if (folder == null || !folder.Exists()) return ret;


            //FileRequestOptions opt = new FileRequestOptions();
            List<IListFileItem> results = new List<IListFileItem>(folder.ListFilesAndDirectories());
            foreach (IListFileItem listItem in results)
            {
                //Console.WriteLine("- {0} (type: {1})", listItem.Uri, listItem.GetType());
                if (listItem is CloudFileDirectory cloudDirectory)
                {
                    StorageDirectoryInfo di = new StorageDirectoryInfo();
                    di.ShareName = share.Name;
                    di.Name = cloudDirectory.Name;
                    di.MetaData = cloudDirectory.Metadata;

                    ret.Add(di);
                }
            }

            return ret;
        }


        public List<StorageFileInfo> DeleteSubFiles(string shareName, string parentFolderPath)
        {
            List<StorageFileInfo> ret = new List<StorageFileInfo>();

            CloudFileShare share = base.FileClient.GetShareReference(shareName);
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            CloudFileDirectory folder = FindDir(rootDir, parentFolderPath, true);

            //FileRequestOptions opt = new FileRequestOptions();
            List<IListFileItem> results = new List<IListFileItem>(folder.ListFilesAndDirectories());
            foreach (IListFileItem listItem in results)
            {
                //Console.WriteLine("- {0} (type: {1})", listItem.Uri, listItem.GetType());
                if (listItem is CloudFile cloudFile)
                {
                    StorageFileInfo fi = this.CreateStorageFileInfo(share, cloudFile);

                    ret.Add(fi);

                    //削除
                    cloudFile.DeleteIfExists();
                }
            }

            return ret;
        }

        public StorageFileInfo DeleteFile(string shareName, string folderPath, string fileName)
        {
            StorageFileInfo ret = new StorageFileInfo();

            CloudFileShare share = base.FileClient.GetShareReference(shareName);
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            CloudFileDirectory folder = FindDir(rootDir, folderPath, false);
            if (folder == null || !folder.Exists()) return ret;

            CloudFile cloudFile = folder.GetFileReference(fileName);
            if (!cloudFile.Exists()) return ret;

            StorageFileInfo fi = this.CreateStorageFileInfo(share, cloudFile);

            //削除
            cloudFile.DeleteIfExists();

            return fi;
        }


        public StorageFileInfo GetFileInfo(string shareName, string folderPath, string fileName)
        {
            StorageFileInfo ret = new StorageFileInfo();

            CloudFileShare share = base.FileClient.GetShareReference(shareName);
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            CloudFileDirectory folder = FindDir(rootDir, folderPath, false);
            if (folder == null || !folder.Exists()) return ret;

            CloudFile cloudFile = folder.GetFileReference(fileName);
            if (!cloudFile.Exists()) return ret;

            StorageFileInfo fi = this.CreateStorageFileInfo(share, cloudFile);

            return fi;
        }
        public List<StorageFileInfo> SaveFile(string shareName, string folderPath, IFormFile file)
        {
            return this.SaveFiles(shareName, folderPath, new List<IFormFile> { file });
        }

        public List<StorageFileInfo> SaveFiles(string shareName, string folderPath, List<IFormFile> files)
        {
            List<StorageFileInfo> ret = new List<StorageFileInfo>();
            CloudFileShare share = base.FileClient.GetShareReference(shareName);
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

            return ret;

        }
    }
}
