using System;
using Cysharp.Threading.Tasks;

namespace ET
{
	public interface IEvent
	{
		Type GetEventType();
	}
	
	public abstract class AEvent<E, A>: IEvent where E: Entity where A: struct
	{
		public Type GetEventType()
		{
			return typeof (A);
		}

		protected abstract UniTask Run(E entity, A a);

		public async UniTask Handle(E entity, A a)
		{
			try
			{
				await Run(entity, a);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}