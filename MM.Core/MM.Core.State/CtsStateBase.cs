using System.Threading;

namespace mm.core.state
{
    public class CtsStateBase : IState
    {
        private CancellationTokenSource cts;

        public void Start()
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();

            OnStart(cts.Token);
        }

        public void Stop()
        {
            OnStop();
            cts?.Cancel();
            cts?.Dispose();
            cts = null;
        }

        public void Update(double deltaTime)
        {
            OnUpdate(deltaTime, cts == null ? default : cts.Token);
        }

        protected virtual void OnStart(CancellationToken token)
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void OnUpdate(double deltaTime, CancellationToken token)
        {
        }
    }
}
