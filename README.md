# 本项目包含多个解决方案

ET 7.0 + FGUI + luban + hybridclr + YooAsset + NKGMoba + UniTask，并提供常用的编辑器工具。

# 用前须知

 - 此项目为本人出于个人喜好和开发需求进行的魔改，并不保证普适性。
 - 目前master为单机版本，服务端未作适配，所以服务端是跑不起来的。

# 环境 && 版本

 - Unity：2020.3.33
 - 服务端：.Net 6.0
 - 客户端：.Net Framework 4.7.2
 - IDE：Rider 2022.1.2
 - 必要：项目使用了多个第三方插件，请自行购买导入：
   - /Unity/Assets/Plugins/Sirenix
   - /Unity/Assets/Plugins/AstarPathfindingProject
   - /Unity/Assets/Plugins/Animancer
   - /Unity/Assets/Plugins/ShaderControl
   - 最后对ProjectS_Hotfix.asmdef做如下引用即可 ![image](https://user-images.githubusercontent.com/35335061/180807422-0ca3a32b-fdf3-4866-83d5-1ff4a569fee8.png)


# 项目截图（IL2CPP PC真机）
![QQ截图20220721230643](https://user-images.githubusercontent.com/35335061/180248732-70230143-7e42-4fc3-bcfd-3d93a8cef5ee.png)

# TODO && Features

- [x] 接入 [ET 7.0](https://github.com/egametang/ET)
- [x] 接入 [hybridclr C#代码热更方案](https://github.com/focus-creative-games/hybridclr)
- [x] 接入 [YooAssset 资源热更方案](https://github.com/tuyoogame/YooAsset)
- [x] 接入 [FGUI UI方案](https://www.fairygui.com/) ，并提供MVVM UI框架
- [x] 接入 [luban 配置表方案](https://github.com/focus-creative-games/luban)，并提供周边工具链
- [x] 接入 [UniTask 异步方案](https://github.com/Cysharp/UniTask), 并基于UniTask对所有异步模块进行改造
- [x] 全面移植 [NKGMoba技能系统](https://gitee.com/NKG_admin/NKGMobaBasedOnET)，及其周边工具链
- [x] 提供一个Entities可视化Debug工具

# 备忘

由于框架本身做了一些打包流程和程序集拆分相关的优化，所以在接入hybridclr时需要做一些修改

 - 注释用于构建时过滤程序集的脚本 ：[FilterHotFixAssemblies.cs](https://github.com/wqaetly/ET/blob/et7_fgui_yooasset_luban_huatuo/Unity/Packages/com.focus-creative-games.hybridclr_unity/Editor/BuildProcessors/FilterHotFixAssemblies.cs) 因为如果HybridCLRGlobalSettings.asset里本身所引用的热更程序集/热更asmdef就不进包的话（例如设置成Only Include Editor Plaform），就会抛出异常，从而导致整个构建过程的Dll是错误的

# 使用指引

项目使用Monkey Command对所有的编辑器指令进行封装，无需每次选择MenuItem进行调用，直接按 《'》 键呼出Monkey Command界面，然后搜索指定指令回车调用即可
![image](https://user-images.githubusercontent.com/35335061/180249168-1616fafc-d58f-4620-9886-64fed1d2d2ae.png)

## Entities Debug工具
![QQ截图20220828194025](https://user-images.githubusercontent.com/35335061/187073093-c1f78181-4955-4543-ba80-f1ae35c0caf2.png)

## 出包流程

### 热更Dll构建

![image](https://user-images.githubusercontent.com/35335061/180249294-3376d37e-8ad5-4e27-816a-190a186adbac.png)

### EXE构建

![image](https://user-images.githubusercontent.com/35335061/180249408-8d0fc3bd-3be4-4742-b511-21c35cecbcff.png)

### AB打包

![image](https://user-images.githubusercontent.com/35335061/180249336-6b54ec78-9b04-454c-b070-78b160958b28.png)

### 本地File WebServer 调试

![image](https://user-images.githubusercontent.com/35335061/180249525-f916056d-e510-427a-a55a-a09abe36f9ac.png)

## 配置表相关

### 导出二进制配置，二进制读取代码

![image](https://user-images.githubusercontent.com/35335061/180249739-2263495d-364d-4776-aac0-680a4800f286.png)

### 导出Json配置，Json读取代码

![image](https://user-images.githubusercontent.com/35335061/180249838-14add5e8-1cab-4a7a-aeba-87fbcaed7503.png)

### 导出编辑器专用Json配置，读取代码

![image](https://user-images.githubusercontent.com/35335061/180249925-d4d68fc2-f8b9-479c-87fb-a136e2dd4b44.png)

## 技能编辑器

双击即可打开

![image](https://user-images.githubusercontent.com/35335061/180250056-bf80ced0-dc70-4521-991e-51dc84c2da63.png)

![image](https://user-images.githubusercontent.com/35335061/180250346-e0baa3a2-796a-4c81-9552-a8c2abb2f1a9.png)


# 引用

博客教程：https://www.lfzxb.top/et7-fgui-yooasset-luban-huatuo/
