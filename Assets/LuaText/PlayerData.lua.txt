PlayerData = {}

PlayerData.equips = {}
PlayerData.items= {}
PlayerData.gems = {}

---为玩家数据写了一个初始化方法,以后直接改这里的数据来源即可
function PlayerData:Init()
    table.insert(self.equips,{id = 1,num = 1})
    table.insert(self.equips,{id = 2,num = 1})

    table.insert(self.items,{id = 3,num = 66})
    table.insert(self.items,{id = 4,num = 99})

    table.insert(self.gems,{id = 5,num = 10})
    table.insert(self.gems,{id = 6,num = 9})
end