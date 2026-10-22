using System.Runtime.CompilerServices;

namespace Sandbox.Tasks;

internal static partial class SyncContext
{
	public static class FrameStage
	{
		public static FrameStageAwaiter Update = new FrameStageAwaiter();
		public static FrameStageAwaiter FixedUpdate = new FrameStageAwaiter();
		public static FrameStageAwaiter PreRender = new FrameStageAwaiter();

		public class FrameStageAwaiter
		{
			public ulong Value { get; private set; }

			public Action Queue { get; set; }

			public void Trigger()
			{
				Value++;

				var q = Queue;
				Queue = default;

				if ( q is not null )
				{
					foreach ( var d in q.GetInvocationList() )
					{
						d.DynamicInvoke();
					}
				}
			}

			public async Task Await() => await new UpdateAwaiter( this, Value );
		}

		struct UpdateAwaiter : INotifyCompletion
		{
			ulong startValue;
			FrameStageAwaiter source;

			internal UpdateAwaiter( FrameStageAwaiter source, ulong startValue )
			{
				this.source = source;
				this.startValue = startValue;
			}

			public void OnCompleted( Action continuation )
			{
				source.Queue += continuation;
			}

			internal bool IsCompleted => startValue != source.Value;
			internal UpdateAwaiter GetAwaiter() => this;
			public void GetResult() { }
		}

	}
}
