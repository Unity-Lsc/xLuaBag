--常用别名在这里定位
require("Object") --面向对象
require("SplitTools") --字符串拆分
Json = require("JsonUtility") --Json解析

--Unity相关
GameObject = CS.UnityEngine.GameObject
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
Vector2 = CS.UnityEngine.Vector2
Vector3 = CS.UnityEngine.Vector3
TextAsset = CS.UnityEngine.TextAsset

--图集对象类
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas

--UI相关
UI = CS.UnityEngine.UI
Image = UI.Image
Button = UI.Button
Text = UI.Text
Toggle = UI.Toggle
ScrollRect = UI.ScrollRect

--自己写的C#脚本相关
ABMgr = CS.ABMgr.GetInstance()  --直接得到ABMge的单例对象


RootCanvas = GameObject.Find("RootCanvas").transform

