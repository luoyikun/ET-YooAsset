using System;
using Cysharp.Threading.Tasks;
using ET.EventType;
using Sirenix.Serialization;
using UnityEngine;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class OnSceneLoadingBegin: AEvent<Scene, LoadingBegin>
    {
        protected override async UniTask Run(Scene entity, LoadingBegin a)
        {
            await entity.GetComponent<NP_TreeDataRepositoryComponent>().LoadSkillCanvas();
        }
    }

    public class NP_RuntimeTreeRepositoryAwakeSystem : AwakeSystem<NP_TreeDataRepositoryComponent>
    {
        public override void Awake(NP_TreeDataRepositoryComponent self)
        {

        }
    }

    public static class NP_TreeDataRepositoryComponentSystems
    {
        /// <summary>
        /// 获取一棵树的所有数据（默认形式）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NP_DataSupportor GetNP_TreeData(this NP_TreeDataRepositoryComponent self, long id)
        {
            if (self.NpRuntimeTreesDatas.ContainsKey(id))
            {
                return self.NpRuntimeTreesDatas[id];
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }

        /// <summary>
        /// 获取一棵树的所有数据（仅深拷贝黑板数据内容，而忽略例如BuffNodeDataDic的数据内容）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NP_DataSupportor GetNP_TreeData_DeepCopyBBValuesOnly(this NP_TreeDataRepositoryComponent self,
            long id)
        {
            NP_DataSupportor result = new NP_DataSupportor();
            if (self.NpRuntimeTreesDatas.ContainsKey(id))
            {
                result.BuffNodeDataDic = self.NpRuntimeTreesDatas[id].BuffNodeDataDic;
                result.NpDataSupportorBase = self.NpRuntimeTreesDatas[id].NpDataSupportorBase.DeepCopy();
                return result;
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }

        /// <summary>
        /// 获取一棵树的所有数据（通过深拷贝形式）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NP_DataSupportor GetNP_TreeData_DeepCopy(this NP_TreeDataRepositoryComponent self, long id)
        {
            if (self.NpRuntimeTreesDatas.ContainsKey(id))
            {
                return self.NpRuntimeTreesDatas[id].DeepCopy();
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }
        
        public static async UniTask LoadSkillCanvas(this NP_TreeDataRepositoryComponent self)
        {
            YooAssetComponent yooAssetComponent = self.DomainScene().GetComponent<YooAssetComponent>();
            
            foreach (var skillCanvasConfig in YooAssetProxy.GetAssetPathsByTag("SkillConfig"))
            {
                TextAsset textAsset =
                    await yooAssetComponent.LoadAssetAsync<TextAsset>(skillCanvasConfig);
            
                if (textAsset.bytes.Length == 0) Log.Info("没有读取到文件");
                try
                {
                    NP_DataSupportor MnNpDataSupportor = SerializationUtility.DeserializeValue<NP_DataSupportor>(textAsset.bytes, DataFormat.Binary);
            
                    Log.Info($"反序列化行为树:{skillCanvasConfig}完成");
            
                    self.NpRuntimeTreesDatas.Add(MnNpDataSupportor.NpDataSupportorBase.NPBehaveTreeDataId, MnNpDataSupportor);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    throw;
                }
            }
        }
    }
}