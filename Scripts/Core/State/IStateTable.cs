namespace mm
{
    public interface IStateTable
    {
        void Change(IState prev, IState next);

        IState Next(IState current);

        public class Empty : IStateTable
        {
            public void Change(IState prev, IState next)
            {
            }

            public IState Next(IState current) => default;
        }
    }
}
