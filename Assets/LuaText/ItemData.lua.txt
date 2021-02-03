--将物品的Json数据读取到Lua中的表中进行存储

--首先把json表从AB包中加载出来

--加载的json文件对象
local itemTxt = ABMgr:LoadRes("json","ItemData",typeof(TextAsset))
--获取它的文本信息 进行json解析
local itemList = Json.decode(itemTxt.text)

--加载出来是一个像数据结构的数据
--不方便通过id去获取里面的内容,所以,要用一张新表转存一次
ItemData = {}
for _, value in pairs(itemList) do
    ItemData[value.id] = value
end
