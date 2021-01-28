--封装
Object = {}
--实例化方法
function Object:new()
    local obj = {}
    --给空对象设置元表 以及__index
    self.__index = self
    setmetatable(obj,self)
    return obj
end

--继承
function Object:subClass(className)
    --根据名字生成一张表(也就是类)
    _G[className] = {}
    local obj = _G[className]
    --设置自己的父类
    obj.base = self
    --给子类设置元表以及__index
    self.__index = self
    setmetatable(obj,self)
end