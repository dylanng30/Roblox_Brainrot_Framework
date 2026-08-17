namespace Dylanng
{
    public abstract class UIScreen : UIBase
    {
        
    }

    public abstract class UIScreen<TData> : UIScreen, IUIData<TData>
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