print("Main.lua 准备就绪")
--初始化所有准备好的类别名
require("InitClass")
--初始化道具信息
require("ItemData")
--初始化玩家信息
require("PlayerData")
PlayerData:Init()

--添加主页面
require("UI/MainPanel")
MainPanel:Show()