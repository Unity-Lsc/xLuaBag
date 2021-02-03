using System.Collections.Generic;
using System.Net.Mime;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LuaCopyEditor : Editor
{
    [MenuItem("XLua/Copy lua to txt")]
    public static void CopyLuaToTxt() {
        string path = Application.dataPath + "/LuaScripts/";
        if(!Directory.Exists(path)) return;
        //得到每一个lua文件的路径 拷贝到另一个文件夹中
        string[] filesArr = Directory.GetFiles(path,"*.lua");

        string newPath = Application.dataPath + "/LuaText/";
        if(!Directory.Exists(newPath)) {
            Directory.CreateDirectory(newPath);
        }else {
            string[] oldFiles = Directory.GetFiles(newPath,"*.txt");
            for (int i = 0; i < oldFiles.Length; i++)
            {
                File.Delete(oldFiles[i]);
            }
        }

        List<string> newPathList = new List<string>();
        string fileName = string.Empty;
        for (int i = 0; i < filesArr.Length; i++)
        {
            fileName = newPath + filesArr[i].Substring(filesArr[i].LastIndexOf("/") + 1) + ".txt";
            newPathList.Add(fileName);
            File.Copy(filesArr[i],fileName);
        }
        AssetDatabase.Refresh();

        for (int i = 0; i < newPathList.Count; i++)
        {
            AssetImporter importer = AssetImporter.GetAtPath(newPathList[i].Substring(newPathList[i].IndexOf("Asset")));
            if(importer != null) {
                importer.assetBundleName = "lua";
            }
        }

    }

}
