using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池模块
/// </summary>
public class PoolMgr : SingletonAutoMono<PoolMgr> {
    
    private Dictionary<string,List<GameObject>> poolDict = new Dictionary<string, List<GameObject>>();

    /// <summary>
    /// 从缓存池中获取物体
    /// </summary>
    /// <param name="path">获取的物体路径</param>
    public GameObject Get(string path) {
        GameObject obj = null;
        if(poolDict.ContainsKey(path) && poolDict[path].Count > 0) {
            obj = poolDict[path][0];
            poolDict[path].RemoveAt(0);
        } else {
            GameObject prefab = Resources.Load<GameObject>(path);
            obj = GameObject.Instantiate(prefab);
            obj.name = prefab.name;
            if(!poolDict.ContainsKey(path)) {
                poolDict.Add(path,new List<GameObject>() {obj});
            }else {
                poolDict[path].Add(obj);
            }
        }
        obj.SetActive(true);
        return obj;
    }

    public GameObject Get(string path,Action<GameObject> callback) {
        GameObject obj = null;
        if(poolDict.ContainsKey(path) && poolDict[path].Count > 0) {
            obj = poolDict[path][0];
            poolDict[path].RemoveAt(0);
            callback(obj);
            obj.SetActive(true);
        } else {
            StartCoroutine(LoadResAsync(path,callback));
        }
        return obj;
    }

    IEnumerator LoadResAsync(string path,Action<GameObject> callback) {
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        yield return request;
        GameObject obj = request.asset as GameObject;
        if(poolDict.ContainsKey(path)) {
            poolDict[path].Add(GameObject.Instantiate(obj));
        }else {
            poolDict.Add(path,new List<GameObject>(){GameObject.Instantiate(obj)});
        }
        callback(GameObject.Instantiate(obj));
    }

    /// <summary>
    /// 将物体放入缓存池中
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="obj">存入的物体</param>
    public void Push(string path,GameObject obj) {
        obj.SetActive(false);
        if(poolDict.ContainsKey(path)) {
            poolDict[path].Add(obj);
        } else {
            poolDict.Add(path,new List<GameObject>() {obj});
        }
    }

}
