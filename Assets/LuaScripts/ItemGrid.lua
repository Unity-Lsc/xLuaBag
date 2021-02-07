--生成一个table 继承Object
Object:subClass("ItemGrid")

local EventSystem = require("EventSystem")

--成员变量
ItemGrid.obj = nil
ItemGrid.imgIcon = nil
ItemGrid.txtNum = nil
ItemGrid.btnIcon = nil
--成员函数
function ItemGrid:Init(parent, posX, posY)
    self.obj = ABMgr:LoadRes("ui", "ItemGrid", typeof(GameObject))
    --设置父对象
    self.obj.transform:SetParent(parent, false)
    --设置位置
    self.obj.transform.localPosition = Vector3(posX, posY, 0)
    --找控件
    self.imgIcon = self.obj.transform:Find("imgIcon"):GetComponent(typeof(Image))
    self.txtNum = self.obj.transform:Find("txtNum"):GetComponent(typeof(Text))
    self.btnIcon = self.imgIcon:GetComponent(typeof(Button))
end

---初始化道具信息
function ItemGrid:InitData(data)
    local itemCfg = ItemData[data.id]
    local strs = string.split(itemCfg.icon, "_")
    --加载图集
    local spriteAtlas = ABMgr:LoadRes("ui", strs[1], typeof(SpriteAtlas))
    --加载图标
    self.imgIcon.sprite = spriteAtlas:GetSprite(strs[2])
    --设置数量
    self.txtNum.text = data.num

    self.btnIcon.onClick:AddListener(function ()
        print("点击物品按钮...")
        EventSystem.SendEvent("OnItemEnter",itemCfg.name,itemCfg.des)
    end)

end

---销毁自身
function ItemGrid:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
end

