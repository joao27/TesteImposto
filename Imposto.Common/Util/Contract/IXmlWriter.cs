namespace Imposto.Common.Util.Contract
{
    public interface IXmlWriter<T>
    {
        bool Record(string file, T @object);
    }
}
