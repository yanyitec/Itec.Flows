namespace Itec.Flows
{
    public interface IState
    {
        string this[string key] { get; set; }

        int Count { get; }

        string Get(string key);
        bool Set(string key, string value);
    }
}