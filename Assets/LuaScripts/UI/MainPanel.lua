
require("UI/BagPanel")

BasePanel:subClass("MainPanel")

---初始化该面板,实例化对象 控件事件的监听
function MainPanel:Init(panelName)
    self.base.Init(self,panelName)
    if self.isInit == false then
        self.isInit = true
        self:GetComp("btnRole","Button").onClick:AddListener(function()
            self:OnBtnRoleClick()
        end);
        self:GetComp("btnSkill","Button").onClick:AddListener(function()
            self:OnBtnSkillClick()
        end);
    end
    
end

---点击玩家按钮
function MainPanel:OnBtnRoleClick()
    BagPanel:Show("BagPanel")
end

---点击技能按钮
function MainPanel:OnBtnSkillClick()
    print("点击技能按钮...")
    print(self.panelObj)
end
