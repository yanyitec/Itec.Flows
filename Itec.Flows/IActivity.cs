using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public interface IActivity
    {
        bool CheckStart(IReadOnlyDictionary<string,string> inputs, IDealer dealer, object context);
        IReadOnlyDictionary<string, string> Execute(IState state,IDealer dealer, object context);

        bool CheckOver(IReadOnlyDictionary<string, string> outputs, IDealer dealer, object context);
    }
}
