using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure; // Namespace for Azure Configuration Manager
using Microsoft.Azure.Storage; // Namespace for Storage Client Library
using Microsoft.Azure.Storage.File; // Namespace for Azure Files

using YrsWeb.Biz;

namespace YrsWeb.Lib.Common
{
	public class FileStorageUtil
	{

        public static CloudFile FindCloudFile(CloudFileClient fileClient,string shareName,string path)
        {
            string folderPath, fileName;

            List<string> folderNames = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            fileName = folderNames.Last();
            folderNames.RemoveAt(folderNames.Count - 1);
            folderPath = String.Join("/", folderNames);

            try
            {
                CloudFileShare share = fileClient.GetShareReference(shareName);
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                CloudFileDirectory folder = FileBiz.FindDir(rootDir, folderPath, false);
                if (folder == null || !folder.Exists())
                    throw new YrsWebException(String.Format("ディレクトリ「{0}」が見つかりません", folderPath));

                CloudFile cloudFile = folder.GetFileReference(fileName);
                if (!cloudFile.Exists())
                    throw new YrsWebException(String.Format("ファイル「{0}」が見つかりません", fileName));

                //return this.File(cloudFile.OpenRead(), "application/octet-stream");
                return cloudFile;
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
