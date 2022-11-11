//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月29日 10:34:57
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using ET.cfg.SceneConfig;
using ET.Client;
using UnityEngine;

namespace ET
{
    public static class FUI_LoginUtilities
    {
        // 选择进入游戏，则单局游戏Scene创建，为ClientScene的子物体，所有资源OperationHandle都挂在这个单局游戏的Scene上面，Scene销毁时统一进行释放
        public static async UniTaskVoid OnLogin(FUI_LoginComponent self)
        {
            Scene singleGameScene =
                SceneFactory.CreateSingleGameScene(++GlobalDefine.SingleGameSceneIndex, "SingleGameScene", self.Domain);

            SceneBaseConfig sceneBaseConfig = ConfigComponent.Instance.AllConfigTables.TbSceneBase[10001];

            List<string> resList = new List<string>();
            resList.Add(YooAssetProxy.GetYooAssetFormatResPath(sceneBaseConfig.SceneRecastNavDataFileName,
                YooAssetProxy.YooAssetResType.PathFind));
            resList.AddRange(YooAssetProxy.GetAssetPathsByTag(YooAssetProxy.YooAssetResType.SkillConfig.ToString()));

            await Game.EventSystem.PublishAsync(singleGameScene, new EventType.LoadingBegin()
            {
                SceneName = YooAssetProxy.GetYooAssetFormatResPath(sceneBaseConfig.SceneName,
                    YooAssetProxy.YooAssetResType.Scene),
                ResList = resList
            });
            
            ClientSceneManagerComponent.Instance.Get(1).GetComponent<FUIManagerComponent>().Remove(FUIPackage.Login);

            await Game.EventSystem.PublishAsync(singleGameScene, new EventType.LoadingFinish());

            await singleGameScene.GetComponent<PathFindComponent>()
                .LoadRecastGraphData(sceneBaseConfig.SceneRecastNavDataFileName);

            await Game.EventSystem.PublishAsync(singleGameScene, new EventType.EnterGameMapFinish());
        }
    }
}