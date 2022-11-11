// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月16日, 星期六
// --------------------------

using Cysharp.Threading.Tasks;
using ET.Client;
using UnityEngine;

namespace ET
{
    public class PathFindComponentSystems : AwakeSystem<PathFindComponent>
    {
        public override void Awake(PathFindComponent self)
        {
            self.DoAwake().Forget();
        }
    }

    public class PathFindComponentDestroySystems : DestroySystem<PathFindComponent>
    {
        public override void Destroy(PathFindComponent self)
        {
            self.DoDestroy();
        }
    }

    public static class PathFindUtitlies
    {
        public static async UniTask LoadRecastGraphData(this PathFindComponent self, string graphName)
        {
            TextAsset textAsset = await self.DomainScene().GetComponent<YooAssetComponent>()
                .LoadAssetAsync<TextAsset>(
                    YooAssetProxy.GetYooAssetFormatResPath(graphName, YooAssetProxy.YooAssetResType.PathFind));
            
            self.PathFindInstance.data.DeserializeGraphs(textAsset.bytes);
            Log.Info($"加载RecastNav数据图：{graphName}完成");
        }
        
        public static async UniTask DoAwake(this PathFindComponent self)
        {
            GameObject prefab = await self.DomainScene().GetComponent<YooAssetComponent>()
                .LoadAssetAsync<GameObject>("PathFind_PathFindCore");

            self.PathFindInstance = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.PathFind, true).GetComponent<AstarPath>();
        }
        
        public static void DoDestroy(this PathFindComponent self)
        {
            UnityEngine.Object.Destroy(self.PathFindInstance.gameObject);
            self.PathFindInstance = null;
        }
    }
}