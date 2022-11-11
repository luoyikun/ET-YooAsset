using Cysharp.Threading.Tasks;
using ET;
using FairyGUI;

namespace ET
{
    public class FUIInitComponent: Entity
    {
        public async UniTask Init()
        {
            await UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();
            
        }
    }
}