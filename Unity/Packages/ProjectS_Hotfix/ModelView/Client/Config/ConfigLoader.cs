using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace ET.Client
{
    [Event(SceneType.Process)]
    public class LoadConfig : AEvent<Scene, EventType.LoadConfig>
    {
        protected override async UniTask Run(Scene entity, EventType.LoadConfig a)
        {
            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();

            configComponent.AllConfigTables = new cfg.Tables();
            await configComponent.AllConfigTables.LoadAsync(LuBanEntry.LoadBytesBuf);
        }
    }
}