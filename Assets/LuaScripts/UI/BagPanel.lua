--一个面板对应一个表
BagPanel = {}

--成员变量
--面板对象
BagPanel.panelObj = nil
--各个控件
BagPanel.btnClose = nil
BagPanel.togEquip = nil
BagPanel.togProp = nil
BagPanel.togGem = nil
BagPanel.scrollBag = nil
BagPanel.content = nil

--成员方法
--初始化方法
function BagPanel:Init()
    if self.panelObj == nil then
        self.panelObj = ABMgr:LoadRes("ui","BagPanel",typeof(GameObject))
        self.panelObj.transform:SetParent(RootCanvas,false)
        local tran = self.panelObj.transform
        self.btnClose = tran:Find("btnClose"):GetComponent(typeof(Button))
        self.togEquip = tran:Find("Group/togEquip"):GetComponent(typeof(Toggle))
        self.togProp = tran:Find("Group/togProp"):GetComponent(typeof(Toggle))
        self.togGem = tran:Find("Group/togGem"):GetComponent(typeof(Toggle))
        self.scrollBag = tran:Find("scrollBag"):GetComponent(typeof(ScrollRect))
        self.content = self.scrollBag.transform:Find("Viewport/Content"):GetComponent(typeof(Transform))
        --添加事件
        --关闭按钮事件
        self.btnClose.onClick:AddListener(function()
            self:Hide()
        end)
        --单选框事件
        self.togEquip.onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(1)
            end
        end)
        self.togProp.onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(2)
            end
        end)
        self.togGem.onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(3)
            end
        end)
    end
end

--显示隐藏
---显示背包面板
function BagPanel:Show()
    self:Init()
    self.panelObj:SetActive(true)
end

---隐藏背包面板
function BagPanel:Hide()
    self.panelObj:SetActive(false)
end


function BagPanel:ChangeType(type)
    print("当前页签为:" .. type)
end
