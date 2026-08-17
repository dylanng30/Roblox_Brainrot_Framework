namespace Dylanng
{
    public abstract class UITop : UIBase
    {
        
    }

    public abstract class UITop<TData> : UITop, IUIData<TData>
    {
        public TData Data { get; private set; }

        public void Setup(TData data)
        {
            Data = data;
            OnSetup(data);
        }

        protected virtual void OnSetup(TData data) { }
    }
}