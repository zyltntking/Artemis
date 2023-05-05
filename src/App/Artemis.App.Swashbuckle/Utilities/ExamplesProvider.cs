namespace Artemis.App.Swashbuckle.Utilities;

public interface IExamplesProvider
{
    object GetExample();
}

public abstract class BodyExamplesProvider : IExamplesProvider
{
    public object GetExample()
    {
        return new { body = GetBodyExample() };
    }

    protected abstract object GetBodyExample();
}

public abstract class BaseExamplesProvider<TInnerValue> : IExamplesProvider
{
    public object GetExample()
    {
        return GetTypedExamples();
    }

    public abstract ResponseObj<TInnerValue> GetTypedExamples();
}

public class ResponseObj<TInnerValue>
{
    public BodyType<TInnerValue> Body { get; set; }

    public class BodyType<TInnerBodyValue>
    {
        public TInnerBodyValue Value { get; set; }
    }
}