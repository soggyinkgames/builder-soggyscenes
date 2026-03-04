public interface ISymbolEventBus
{
    event Action<SymbolIdentity> SymbolRecognized;
    event Action<string> SymbolFailed;
    void RaiseRecognized(SymbolIdentity identity);
    void RaiseFailed(string reason);
}