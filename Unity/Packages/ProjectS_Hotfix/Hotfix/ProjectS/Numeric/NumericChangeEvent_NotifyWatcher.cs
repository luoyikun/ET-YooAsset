using Cysharp.Threading.Tasks;

namespace ET
{
	// 分发数值监听
	[Event(SceneType.Map)]  // 服务端Map需要分发
	[Event(SceneType.Current)] // 客户端CurrentScene也要分发
	public class NumericChangeEvent_NotifyWatcher: AEvent<Unit, EventType.NumericChange>
	{
		protected override async UniTask Run(Unit unit, EventType.NumericChange args)
		{
			NumericWatcherComponent.Instance.Run(unit, args);
			await UniTask.CompletedTask;
		}
	}
}
