namespace mm.effect
{
    public static class EffectExtension
    {
        public static IEffectFactory.Handler CreateHandler(this IEffectFactory factory)
        {
            return new IEffectFactory.Handler(factory);
        }

        public static T GetControl<T>(this IEffect effect)
        => effect is T control ? control : default;
    }
}
