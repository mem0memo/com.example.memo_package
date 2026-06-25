using System;

namespace mm.core.effect
{
    public interface IEffectFactory
    {
        IEffect Create();

        void Release(IEffect effect);

        public class Handler : IDisposable
        {
            private IEffectFactory factory;
            private IEffect effect;

            public Handler(IEffectFactory factory)
            {
                this.factory = factory;
            }

            public IEffect Get()
            {
                if (effect == null)
                {
                    effect = factory.Create();
                }
                return effect;
            }

            public void Dispose()
            {
                if (effect != null)
                {
                    factory.Release(effect);
                    effect = null;
                }
            }
        }
    }
}
