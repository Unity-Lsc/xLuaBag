require("ItemGrid")
local EventSystem = require("EventSystem")

--一个面板对应一个表
BasePanel:subClass("BagPanel")

BagPanel.content = nil
BagPanel.curType = -1
BagPanel.imgDes = nil
BasePanel.txtItemName = nil
BasePanel.txtItemDes = nil
BasePanel.btnItemClose = nil

--用来存储当前页签下的物品
BagPanel.items = {}

--成员方法
--初始化方法
function BagPanel:Init(panelName)
    self.base.Init(self,panelName)
    if self.isInit == false then
        self.isInit = true
        self.content = self:GetComp("scrollBag","ScrollRect").transform:Find("Viewport/Content"):GetComponent(typeof(Transform))
        self.imgDes = self.panelObj.transform:Find("imgDes"):GetComponent(typeof(Image))
        self.txtItemName = self.imgDes.transform:Find("txtItemName"):GetComponent(typeof(Text))
        self.txtItemDes = self.imgDes.transform:Find("txtItemDes"):GetComponent(typeof(Text))
        self.btnItemClose = self.imgDes.transform:Find("btnItemClose"):GetComponent(typeof(Button))
        
        --添加事件
        --关闭按钮事件
        self:GetComp("btnClose","Button").onClick:AddListener(function()
            self:Hide()
        end)
        --单选框事件
        self:GetComp("togEquip","Toggle").onValueChanged:AddListener(function(value)
            --self.togEquip.interactable = not value and true or false
            self:GetComp("togEquip","Toggle").interactable = (value and {false} or {true})[1]
            if value == true then
                self:ChangeType(1)
            end
        end)
        self:GetComp("togProp","Toggle").onValueChanged:AddListener(function(value)
            self:GetComp("togProp","Toggle").interactable = (value and {false} or {true})[1]
            if value == true then
                self:ChangeType(2)
            end
        end)
        self:GetComp("togGem","Toggle").onValueChanged:AddListener(function(value)
            self:GetComp("togGem","Toggle").interactable = (value and {false} or {true})[1]
            if value == true then
                self:ChangeType(3)
            end
        end)
        self.btnItemClose.onClick:AddListener(function ()
            self.imgDes.gameObject:SetActive(false)
        end)
        EventSystem.AddListener("OnItemEnter",function (name,des)
            self:OnItemEnter(name,des)
        end)
    end
end

--显示隐藏
---显示背包面板
function BagPanel:Show(panelName)
    self.base.Show(self,panelName)
    self.imgDes.gameObject:SetActive(false)
    if self.curType == -1 then
        self:ChangeType(1)
    end
end

-- 1装备 2道具 3宝石
function BagPanel:ChangeType(type)
    self.curType = type
    --先清空老格子
    for i = 1, #self.items do
        self.items[i]:Destroy()
    end
    self.items = {}

    local curItems = nil
    if type == 1 then
        curItems = PlayerData.equips
    elseif type == 2 then
        curItems = PlayerData.items
    elseif type == 3 then
        curItems = PlayerData.gems
    end

    if curItems ~= nil then
        for i = 1, #curItems do
            --根据数据,创建格子对象
            local grid = ItemGrid:new()
            --实例化对象,设置位置
            grid:Init(self.content,(i - 1)%4 * 170,math.floor((i - 1) / 4) * 170)
            --初始化它的信息 设置图标和数量
            grid:InitData(curItems[i])

            table.insert(self.items,grid)
        end
    end

end


function BagPanel:OnItemEnter(name,des)
    self.imgDes.gameObject:SetActive(true)
    self.txtItemName.text = name
    self.txtItemDes.text = des
end
