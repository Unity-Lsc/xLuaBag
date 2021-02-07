using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    // private void Start()
    // {

    //     LuaMgr.GetInstance().Init();
    //     LuaMgr.GetInstance().DoLuaFile("Main");

    // }

    public void ReceiveInfoStartRunning()
    {
        Debug.Log("StartGame...");
        LuaMgr.GetInstance().Init();
        LuaMgr.GetInstance().DoLuaFile("Main");
    }

}
