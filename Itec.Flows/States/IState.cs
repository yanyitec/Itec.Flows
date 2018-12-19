namespace Itec.Flows
{
    public interface IState: IReadOnlyState
    {
        
        new string this[string key] { get; set; }

        
        IState Remove(string key);
        IState Set<T>(string key, T value);
        IState Set(string key, object value);
        IState SetString(string key, string value);

        
    }
}