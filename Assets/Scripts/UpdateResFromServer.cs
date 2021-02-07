using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 从服务器下载更新最新的资源文件 获取资源(ab,LUA文件,配置文件Json/XML...)
/// 1.下载校验文件到客户端
/// 2.根据校验文件,客户端逐条读取资源文件,然后与本客户端相同的资源文件进行MD5编码比对
/// 3.如果客户端没有服务端的文件,直接下载即可
/// 4.客户端存在与服务端相同的文件,但是MD5编码比对不一致.说名服务端对应的资源文件发生了更新,则客户端下载最新的资源文件
/// </summary>
public class UpdateResFromServer : MonoBehaviour
{
    //是否启用本脚本(是否联网下载服务器更新资源)
    public bool enableSelf = true;//默认启用
                                  //PC平台的资源下载路径
    private string downLoadPath = string.Empty;
    //HTTP 服务器地址
    private string serverUrl = PathTools.SERVER_URL;

    private void Awake()
    {
        if (enableSelf)
        {
            //PC平台的资源下载路径
            downLoadPath = PathTools.GetABOutPath();
            //检测资源进行对比更新
            StartCoroutine(DownlaodResAndCheckUpdate(serverUrl));
        }
        else
        {
            Debug.Log("禁用热更服务...");
            //通知其他游戏主逻辑,开始运行
            BroadcastMessage(ABDefine.ReceiveInfoStartRunning, SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator DownlaodResAndCheckUpdate(string url)
    {
        /* 步骤1: 下载校验文件到客户端 */
        if (string.IsNullOrEmpty(serverUrl))
        {
            yield break;
        }
        //服务器校验文件路径
        string fileUrl = serverUrl + "/" + ABDefine.ProjectVerifyFile;
        //下载校验文件到客户端
        WWW www = new WWW(fileUrl);
        yield return www;
        //网络错误检查
        if (www.error != null && !string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("www加载网络错误,请检查服务器链接,URL是否正确,网络状态是否良好...  错误信息:" + www.error);
            yield break;
        }
        //判断客户端本地是否有此目录
        if (!Directory.Exists(downLoadPath))
        {
            Directory.CreateDirectory(downLoadPath);
        }
        //开始下载校验文件,且写入本地
        File.WriteAllBytes(downLoadPath + "/" + ABDefine.ProjectVerifyFile, www.bytes);

        /* 步骤2 根据校验文件,客户端逐条读取资源文件,然后与本客户端相同的资源文件进行MD5编码比对 */
        //读取资源文件的内容
        string strServerFileText = www.text;
        string[] lines = strServerFileText.Split('\n');//按照换行进行截取
        for (int i = 0; i < lines.Length; i++)
        {
            //如果校验文件出现空行 跳过继续进行
            if (string.IsNullOrEmpty(lines[i])) continue;
            //得到校验文件每行的文件名与MD5编码
            string[] fileAndMD5 = lines[i].Split('|');
            string strServerFileName = fileAndMD5[0].Trim();//服务端文件名称
            string strServerMD5 = fileAndMD5[1].Trim();//服务端文件的MD5校验码
                                                       //得到本地的这个文件路径
            string strLocalFilePath = downLoadPath + "/" + strServerFileName;

            /* 步骤3 如果客户端没有服务端的文件,直接下载即可 */
            if (!File.Exists(strLocalFilePath))
            {
                //对于本地不存在的文件夹 进行创建
                string dir = Path.GetDirectoryName(strLocalFilePath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                Debug.Log("下载客户端没有的文件:" + strLocalFilePath);
                //通过www 正式开始下载服务端的文件  并且写入本地指定路径
                yield return StartCoroutine(DownLoadFileByWWW(serverUrl + "/" + strServerFileName, strLocalFilePath));
            }
            else
            {
                //根据客户端本地文件名称 得到本地的MD5编码
                string strLocalMD5 = Helps.GetMD5Values(strLocalFilePath);
                //服务端MD5编码,与本地生成的MD5编码做比较
                if (!strLocalMD5.Equals(strServerMD5))
                {
                    //如果比较不一致 删除本地对应文件
                    File.Delete(strLocalFilePath);
                    Debug.Log("下载和服务端存在差异的文件:" + strLocalFilePath);
                    //从服务器下载新的文件
                    yield return StartCoroutine(DownLoadFileByWWW(serverUrl + "/" + strServerFileName, strLocalFilePath));
                }
            }

        }//for_end

        yield return new WaitForEndOfFrame();
        Debug.Log("资源更新完成,开始进行主逻辑...");
        //向下广播 通知游戏启动主逻辑
        BroadcastMessage(ABDefine.ReceiveInfoStartRunning, SendMessageOptions.DontRequireReceiver);
    }//DownlaodResAndCheckUpdate_end

    /// <summary>
    /// 通过WWW下载文件 并且写入本地指定路径
    /// </summary>
    /// <param name="wwwUrl">WWW的URL地址</param>
    /// <param name="localFilePath">本地文件地址</param>
    /// <returns></returns>
    IEnumerator DownLoadFileByWWW(string wwwUrl, string localFilePath)
    {
        WWW www = new WWW(wwwUrl);
        yield return www;
        if (www.error != null && !string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("www加载网络错误,请检查服务器链接,URL是否正确,网络状态是否良好...  错误信息:" + www.error);
            yield break;
        }
        //下载完成 写入本地文件
        File.WriteAllBytes(localFilePath, www.bytes);
    }//DownLoadFileByWWW_end

}//Class_end
