
require("UI/BagPanel")

MainPanel = {}

--关联的面板对象
MainPanel.panelObj = nil
--对应的面板控件
MainPanel.btnRole = nil
MainPanel.btnSkill = nil

---初始化该面板,实例化对象 控件事件的监听
function MainPanel:Init()
    if self.panelObj == nil then
        --1.实例化面板对象 设置父对象
        self.panelObj = ABMgr:LoadRes("ui", "MainPanel", typeof(GameObject))
        self.panelObj.transform:SetParent(RootCanvas, false)
        --2.找到对应控件
        self.btnRole = self.panelObj.transform:Find("btnRole"):GetComponent(typeof(Button))
        self.btnSkill = self.panelObj.transform:Find("btnSkill"):GetComponent(typeof(Button))
        --3.为控件添加事件监听
        --如果直接.传入自己的函数,那么在函数内部,没有办法用self获取内容
        --self.btnRole.onClick:AddListener(self.OnBtnRoleClick)
        self.btnRole.onClick:AddListener(function()
            self:OnBtnRoleClick()
        end);
        self.btnSkill.onClick:AddListener(function()
            self:OnBtnSkillClick()
        end);
    end
end

---主面板的显示
function MainPanel:Show()
    self:Init()
    self.panelObj:SetActive(true)
end

---主面板隐藏
function MainPanel:Hide()
    self.panelObj:SetActive(false)
end

---点击玩家按钮
function MainPanel:OnBtnRoleClick()
    BagPanel:Show()
end

---点击技能按钮
function MainPanel:OnBtnSkillClick()
    print("点击技能按钮...")
    print(self.panelObj)
end
