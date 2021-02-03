using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 生成MD5校验文件
/// </summary>
public class CreateVerifyFile
{
    /// <summary>
    /// 生成校验文件
    /// </summary>
    [MenuItem("AssetBundleTools/Create Verify File")]
    public static void CreateFileMethod()
    {
        /*定义局部变量*/
        string abOutPath = string.Empty;//AssetBundle的输出路径
        string verifyFilePath = string.Empty;//校验文件的路径
        List<string> fileList = new List<string>();//存储所有合法文件的相对路径信息

        /*定义校验文件的输出路径*/
        abOutPath = PathTools.GetABOutPath();
        verifyFilePath = abOutPath + "/" + ABDefine.ProjectVerifyFile;
        /*如果本项目已经有校验文件,则进行覆盖*/
        if (File.Exists(verifyFilePath))
        {
            File.Delete(verifyFilePath);
        }
        /*遍历当前文件夹(校验文件的输出路径),所有的文件,生成MD5编码*/
        ListFile(new DirectoryInfo(abOutPath), ref fileList);
        /*把文件的名称与对应的MD5编码,写入校验文件*/
        WriteVerifyFile(verifyFilePath, abOutPath, fileList);
    }//CreateFileMethod_end

    /// <summary>
    /// 遍历当前文件夹(校验文件的输出路径),得到所有合法的文件
    /// </summary>
    /// <param name="fileSystemInfo">文件(夹)路径信息</param>
    /// <param name="fileList">输入输出参数 把所有合法的文件(相对路径)写入集合</param>
    private static void ListFile(FileSystemInfo fileSystemInfo, ref List<string> fileList)
    {
        //文件系统转为目录系统
        DirectoryInfo dirInfo = fileSystemInfo as DirectoryInfo;
        //获取文件夹下所有的文件信息(文件系统,包括文件与文件夹)
        FileSystemInfo[] fileSystems = dirInfo.GetFileSystemInfos();//这里我们把文件和文件夹 都看作文件系统信息

        foreach (FileSystemInfo item in fileSystems)
        {
            FileInfo fileInfo = item as FileInfo;
            if (fileInfo != null)
            {//文件
             //把win系统中路径分割符改为Unity的类型
                string strFileFullName = fileInfo.FullName.Replace("\\", "/");
                //过滤无效文件
                string fileExt = Path.GetExtension(strFileFullName);
                if (fileExt.EndsWith(".meta") || fileExt.EndsWith(".bak"))
                {
                    continue;
                }
                fileList.Add(strFileFullName);
            }
            else
            {//文件夹
                ListFile(item, ref fileList);//递归调用下一层文件夹
            }
        }
    }//ListFile_end

    /// <summary>
    /// 把文件的名称与对应的MD5编码值,写入校验文件
    /// </summary>
    /// <param name="verifyFileOutPath">写入校验文件的路径</param>
    /// <param name="abOutPath">AssetBundle的输出路径</param>
    /// <param name="fileList">存储所有合法文件的相对路径信息集合</param>
    private static void WriteVerifyFile(string verifyFileOutPath, string abOutPath, List<string> fileList)
    {
        using (FileStream fs = new FileStream(verifyFileOutPath, FileMode.CreateNew))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                for (int i = 0; i < fileList.Count; i++)
                {
                    //获取文件的名称
                    string strFile = fileList[i];
                    //生成此文件对应的MD5编码数值
                    string strFileMD5 = Helps.GetMD5Values(strFile);
                    //把文件中的全路径信息去除,保留相对路径
                    string strTrueFileName = strFile.Replace(abOutPath + "/", string.Empty);
                    //做参数检查,写入文件前
                    //写入文件
                    sw.WriteLine(strTrueFileName + "|" + strFileMD5);
                }//for_end
            }
        }
        //提示用户生成操作完毕
        Debug.Log("CreateVerifyFile.cs/WriteVerifyFile() 创建校验文件成功...");
        //刷新编辑器
        AssetDatabase.Refresh();
    }//WriteVerifyFile_end

}//Class_end
