﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public interface IActivity
    {
        IDealer GetDealer(IReadOnlyState inputs, IReadOnlyState parameters , IReadOnlyState states , IDealer currentUser, object context);
        bool CheckStart(IReadOnlyState inputs, IReadOnlyState parameters, IReadOnlyState states, IDealer currentUser, object context);
        bool Execute(IState state,IDealer dealer, object context);

        bool CheckOver(IReadOnlyState outputs, IDealer dealer, object context);
    }
}
