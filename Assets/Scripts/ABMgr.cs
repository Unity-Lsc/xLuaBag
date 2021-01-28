using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// AssetBundle管理类
/// </summary>
public class ABMgr : SingletonAutoMono<ABMgr>
{
    private AssetBundle mainAB = null;//主包
    private AssetBundleManifest mainManifest = null;//获取依赖包的配置文件

    //存储加载过的AB包
    private Dictionary<string, AssetBundle> abDict = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// AB包的存放路径
    /// </summary>
    private string PathUrl {
        get {
            return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 对应的平台名
    /// </summary>
    private string MainPlatformName {
        get {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    /// <summary>
    /// 加载资源包
    /// </summary>
    /// <param name="abName">ab包名</param>
    private void LoadAB(string abName) {
        //加载主包
        if (mainAB == null) {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainPlatformName);
            mainManifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        AssetBundle ab = null;
        //获取依赖包的相关信息
        string[] strs = mainManifest.GetAllDependencies(abName);//所有依赖包的名字
        for (int i = 0; i < strs.Length; i++) {
            //判断依赖包是否加载过
            if (!abDict.ContainsKey(strs[i])) {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDict.Add(strs[i], ab);
            }
        }
        //加载目标包
        if (!abDict.ContainsKey(abName)) {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDict.Add(abName, ab);
        }
    }

    /// <summary>
    /// 同步加载 不指定类型
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="resName">资源名</param>
    public Object LoadRes(string abName,string resName) {
        //加载AB包
        LoadAB(abName);

        //加载资源
        Object obj = abDict[abName].LoadAsset(resName);
        if(obj is GameObject) {
            return Instantiate(obj);
        }else {
            return obj;
        }
    }

    /// <summary>
    /// 同步加载 根据type指定类型
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="objType">类型</param>
    public Object LoadRes(string abName, string resName, System.Type objType) {
        //加载AB包
        LoadAB(abName);

        //加载资源
        Object obj = abDict[abName].LoadAsset(resName, objType);
        if (obj is GameObject) {
            return Instantiate(obj);
        } else {
            return obj;
        }
    }

    /// <summary>
    /// 同步加载 根据泛型指定类型
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="resName">资源名</param>
    public T LoadRes<T>(string abName, string resName) where T : Object {
        //加载AB包
        LoadAB(abName);

        //加载资源
        T obj = abDict[abName].LoadAsset<T>(resName);
        if (obj is GameObject) {
            return Instantiate(obj);
        } else {
            return obj;
        }
    }

    /// <summary>
    /// 异步加载
    /// 这里的异步加载 加载AB包时,使用的还是同步加载
    /// 只是从AB包中加载资源时,使用了异步
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callBack">加载完毕回调</param>
    public void LoadResAsync(string abName,string resName,UnityAction<Object> callBack) {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName,string resName,UnityAction<Object> callBack) {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest request = abDict[abName].LoadAssetAsync(resName);
        yield return request;
        Object obj = request.asset;
        //异步加载结束后 通过委托把资源传递给外部使用
        if (obj is GameObject) {
            callBack(Instantiate(obj));
        } else {
            callBack(obj);
        }
    }

    /// <summary>
    /// 异步加载 通过type指定类型
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="type">指定的类型</param>
    /// <param name="callBack">加载完毕回调</param>
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack) {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack) {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest request = abDict[abName].LoadAssetAsync(resName, type);
        yield return request;
        Object obj = request.asset;
        //异步加载结束后 通过委托把资源传递给外部使用
        if (obj is GameObject) {
            callBack(Instantiate(obj));
        } else {
            callBack(obj);
        }
    }

    /// <summary>
    /// 异步加载
    /// 这里的异步加载 加载AB包时,使用的还是同步加载
    /// 只是从AB包中加载资源时,使用了异步
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callBack">加载完毕回调</param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest request = abDict[abName].LoadAssetAsync<T>(resName);
        yield return request;
        Object obj = request.asset;
        //异步加载结束后 通过委托把资源传递给外部使用
        if (obj is GameObject) {
            callBack(Instantiate(obj) as T);
        } else {
            callBack(obj as T);
        }

    }

    /// <summary>
    /// 单个包卸载
    /// </summary>
    /// <param name="abName">要卸载的AB包名</param>
    public void UnLoad(string abName) {
        if(abDict.ContainsKey(abName)) {
            abDict[abName].Unload(false);
            abDict.Remove(abName);
        }
    }

    /// <summary>
    /// 所有包卸载
    /// </summary>
    public void ClearAB() {
        AssetBundle.UnloadAllAssetBundles(false);
        abDict.Clear();
        mainAB = null;
        mainManifest = null;
    }

}
