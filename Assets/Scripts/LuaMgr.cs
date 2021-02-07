using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// Lua管理器
/// </summary>
public class LuaMgr : BaseManager<LuaMgr>
{

    private LuaEnv luaEnv = null;

    /// <summary>
    /// 得到Lua中的_G
    /// </summary>
    public LuaTable Global {
        get {
            return luaEnv.Global;
        }
    }

    /// <summary>
    /// 初始化解析器
    /// </summary>
    public void Init() {
        if (luaEnv != null) return;
        luaEnv = new LuaEnv();
        //luaEnv.AddLoader(MyCustomLoader);
        luaEnv.AddLoader(MyCustomABLoader);
    }

    /// <summary>
    /// 执行Lua语言
    /// </summary>
    /// <param name="str"></param>
    public void DoString(string str) {
        if(luaEnv == null) {
            Debug.LogError("Lua解析器未初始化...");
            return;
        }
        luaEnv.DoString(str);
    }

    /// <summary>
    /// 传入Lua文件名 执行Lua语言
    /// </summary>
    /// <param name="fileName">Lua文件名</param>
    public void DoLuaFile(string fileName) {
        string str = string.Format("require('{0}')", fileName);
        luaEnv.DoString(str);
    }

    private byte[] MyCustomABLoader(ref string filePath) {
        TextAsset ta = ABMgr.GetInstance().LoadRes<TextAsset>("lua", filePath + ".lua");
        if(ta != null) {
            return ta.bytes;
        }
        Debug.Log("MyCustomABLoader重定向失败,文件名为:" + filePath);
        return null;
    }

    private byte[] MyCustomLoader(ref string filePath) {
        string path = Application.dataPath + "/LuaScripts/" + filePath + ".lua";

        if (File.Exists(path)) {
            //Debug.Log(filePath);
            return File.ReadAllBytes(path);
        } else {
            Debug.Log("MyCustomLoader重定向失败,文件名:" + filePath);
        }

        return null;
    }

    /// <summary>
    /// 释放Lua垃圾
    /// </summary>
    public void Tick() {
        if (luaEnv == null) {
            Debug.LogError("Lua解析器未初始化...");
            return;
        }
        luaEnv.Tick();
    }

    /// <summary>
    /// 释放Lua解析器
    /// </summary>
    public void Dispose() {
        if (luaEnv == null) {
            Debug.LogError("Lua解析器未初始化...");
            return;
        }
        luaEnv.Dispose();
        luaEnv = null;
    }

}
