using Collette.Index;
using System;
using System.Collections.Generic;
using System.Text;

namespace Collette.Commands
{
    public interface ICommand
    {
        void Execute(BaseIndex index, Condition dependency);
    }
}
