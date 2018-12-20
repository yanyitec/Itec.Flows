using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public interface IDealer
    {
        Guid Id { get; }
        string Name { get; }
        string ToJSON();
    }
}
