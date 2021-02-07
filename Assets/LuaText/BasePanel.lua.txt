Object:subClass("BasePanel")

BasePanel.panelObj = nil
BasePanel.isInit = false
--控件字典
BasePanel.components = {}


function BasePanel:Init(panelName)
    if self.panelObj == nil then
        self.panelObj = ABMgr:LoadRes("ui",panelName,typeof(GameObject))
        self.panelObj.transform:SetParent(RootCanvas,false)

        --对所有UI组件进行存储
        local allComs = self.panelObj:GetComponentsInChildren(typeof(UIBehaviour))
        for i = 0, allComs.Length - 1 do
            local compName = allComs[i].name
            --按照名字的规则,去找控件
            if string.find(compName,"btn") ~= nill or
                string.find(compName,"tog") ~= nill or
                string.find(compName,"img") ~= nill or
                string.find(compName,"scroll") ~= nill or
                string.find(compName,"txt") ~= nill then
                    --为了让我们得到组件的时候,确定组件的类型 需要将类型进行存储
                    local typeName = allComs[i]:GetType().Name

                    --避免一个对象上有多个组件,出现覆盖问题  用table来进行存储
                    if self.components[compName] ~= nill then
                        self.components[compName][typeName] = allComs[i]
                    else
                        self.components[compName] = {[typeName] = allComs[i]}
                    end
            end
        end
    end
end

---获取组件
---@param name string
---@param typeName string
function BasePanel:GetComp(name,typeName)
    if self.components[name] ~= nil then
        local targetComps = self.components[name]
        if targetComps[typeName] ~= nil then
            return targetComps[typeName]
        end
    end
    return nil
end

---显示面板
---@type string panelName
function BasePanel:Show(panelName)
    print(self)
    self:Init(panelName)
    self.panelObj:SetActive(true)
end

---隐藏面板
function BasePanel:Hide()
    self.panelObj:SetActive(false)
end