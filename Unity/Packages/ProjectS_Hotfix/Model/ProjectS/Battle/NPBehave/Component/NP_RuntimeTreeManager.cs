//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 14:54:26
//------------------------------------------------------------

using System.Collections.Generic;

namespace ET
{
    public class NP_RuntimeTreeManager: Entity, IAwake, IDestroy
    {
        public Dictionary<long, NP_RuntimeTree> RuntimeTrees = new Dictionary<long, NP_RuntimeTree>();

        /// <summary>
        /// 已经添加过的行为树，第一个id为配置id，第二个id为运行时id
        /// </summary>
        private Dictionary<long, long> m_HasAddedTrees = new Dictionary<long, long>();

        /// <summary>
        /// 添加行为树
        /// </summary>
        /// <param name="runTimeID">行为树运行时ID</param>
        /// <param name="rootId">行为树在预配置中的id，即根节点id</param>
        /// <param name="npRuntimeTree">要添加的行为树</param>
        public void AddTree(long runTimeID, long rootId, NP_RuntimeTree npRuntimeTree)
        {
            RuntimeTrees.Add(runTimeID, npRuntimeTree);
            this.m_HasAddedTrees.Add(rootId, runTimeID);
        }

        /// <summary>
        /// 通过运行时ID请求行为树
        /// </summary>
        /// <param name="runTimeid">运行时ID</param>
        /// <returns></returns>
        public NP_RuntimeTree GetTreeByRuntimeID(long runTimeid)
        {
            if (RuntimeTrees.ContainsKey(runTimeid))
            {
                return RuntimeTrees[runTimeid];
            }

            Log.Error($"通过运行时ID请求行为树请求的ID不存在，id是{runTimeid}");
            return null;
        }

        /// <summary>
        /// 通过预制id请求行为树(将要废弃)
        /// </summary>
        /// <param name="prefabid">预制id</param>
        /// <returns></returns>
        public NP_RuntimeTree GetTreeByPrefabID(long prefabid)
        {
            if (this.m_HasAddedTrees.ContainsKey(prefabid))
            {
                return RuntimeTrees[this.m_HasAddedTrees[prefabid]];
            }

            Log.Error($"通过预制id请求行为树,请求的ID不存在，id是{prefabid}");
            return null;
        }
        
        public void RemoveTree(long id)
        {
            if (RuntimeTrees.ContainsKey(id))
            {
                RuntimeTrees[id].Dispose();
                RuntimeTrees.Remove(id);
            }
            else
            {
                Log.Error($"请求删除的ID不存在，id是{id}");
            }
        }

        public override void Dispose()
        {
            if(IsDisposed)
                return;
            base.Dispose();
            foreach (var runtimeTree in RuntimeTrees)
            {
                runtimeTree.Value.Dispose();
            }
            RuntimeTrees.Clear();
            this.m_HasAddedTrees.Clear();
        }
    }
}