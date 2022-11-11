using System.Collections.Generic;

namespace ET
{
    public class NP_RuntimeTreeAwakeSystem : AwakeSystem<NP_RuntimeTree, NP_DataSupportor, NP_SyncComponent, Unit>
    {
        public override void Awake(NP_RuntimeTree self, NP_DataSupportor belongNP_DataSupportor,
            NP_SyncComponent npSyncComponent, Unit belongToUnit)
        {
            self.BelongToUnit = belongToUnit;
            self.BelongNP_DataSupportor = belongNP_DataSupportor;
            self.NpSyncComponent = npSyncComponent;
        }
    }

    public class NP_RuntimeTreeDestroySystem : DestroySystem<NP_RuntimeTree>
    {
        public override void Destroy(NP_RuntimeTree self)
        {
            self.Finish().Forget();
        }
    }
}