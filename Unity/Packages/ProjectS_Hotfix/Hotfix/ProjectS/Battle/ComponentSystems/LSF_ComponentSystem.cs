// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月17日, 星期日
// --------------------------

namespace ET
{
    public class LSF_ComponentSystem_AwakeSystem0 : AwakeSystem<LSF_Component>
    {
        public override void Awake(LSF_Component self)
        {
        }
    }


    public class LSF_ComponentSystem_FxiedUpdate0 : FixedUpdateSystem<LSF_Component>
    {
        public override void FixedUpdate(LSF_Component self)
        {
            self.CurrentFrame++;
        }
    }

    public class LSF_ComponentSystem_DestroySystem0 : DestroySystem<LSF_Component>
    {
        public override void Destroy(LSF_Component self)
        {
        }
    }

    public static class LSF_ComponentSystem_Utilities
    {
    }
}