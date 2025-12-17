# 导入 Interpreter
from asteval import Interpreter
import re

if __name__ == "__main__":
    # 步骤1：创建 Interpreter 实例
    aeval = Interpreter()

    # 步骤2：定义服务器数据字典列表
    server_data_list = [
        {
            "natneg": "1",
            "gamever": "2.00",
            "gravity": 800,
            "mapname": "gmtmap2",
            "gamemode": "openplaying",
            "gamename": "gmtest2",
            "gametype": "arena",
            "hostname": "Server 1",
            "hostport": "25000",
            "localip0": "172.19.0.4",
            "numteams": 2,
            "teamplay": 1,
            "fraglimit": 0,
            "localport": 11111,
            "rankingon": 1,
            "timelimit": 40,
            "maxplayers": 32,
            "numplayers": 8,
            "statechanged": 1,
        },
        {
            "natneg": "1",
            "gamever": "2.00",
            "gravity": 600,
            "mapname": "gmtmap2",
            "gamemode": "openplaying",
            "gamename": "gmtest2",
            "gametype": "arena",
            "hostname": "Server 2",
            "hostport": "25000",
            "localip0": "172.19.0.4",
            "numteams": 2,
            "teamplay": 1,
            "fraglimit": 0,
            "localport": 11111,
            "rankingon": 1,
            "timelimit": 40,
            "maxplayers": 32,
            "numplayers": 4,
            "statechanged": 1,
        }
    ]

    # 步骤3：定义条件
    condition = "gravity > 700 and numplayers > 5 and gamemode == 'openplaying'"

    # 步骤4：提取条件中的键
    pattern = r'(\w+)\s*([<>!=]+)\s*([\'\"]?)(.*?)\3'
    matches = re.findall(pattern, condition)
    keys = [match[0] for match in matches]
    print(f"提取的键: {keys}")

    # 步骤5：评估每个服务器数据字典的条件
    for server in server_data_list:
        # 更新上下文：动态加载提取的键
        for key in keys:
            if key in server:
                aeval.symtable[key] = server[key]
        result = aeval(condition)
        print(f"服务器 {server['hostname']} (Gravity: {server['gravity']}, Players: {server['numplayers']}): {result}")
