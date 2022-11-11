//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:44:40
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 行为树数据仓库组件
    /// </summary>
    public class NP_TreeDataRepositoryComponent: Entity, IAwake
    {
        public const string NPDataServerPath = "../Config/SkillConfigs/";

        /// <summary>
        /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
        /// </summary>
        public Dictionary<long, NP_DataSupportor> NpRuntimeTreesDatas = new Dictionary<long, NP_DataSupportor>();
    }
}