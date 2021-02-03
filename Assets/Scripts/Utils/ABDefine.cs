using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 委托定义区 */
[XLua.CSharpCallLua]
public delegate void DelLoadComplete(string abName);


/* 枚举类型定义 */

public class ABDefine
{
    //框架常量
    public static string ASSETBUNDLE_MANIFEST = "AssetBundleManifest";
    //向下通知常量
    public static string ReceiveInfoStartRunning = "ReceiveInfoStartRunning";
    //校验文件名
    public static string ProjectVerifyFile = "ProjectVerifyFile.txt";
}
