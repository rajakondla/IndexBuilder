using Collette.Index;
using Collette.Index.Core;
using Collette.Index.Service;
using Collette.Listener;
using Newtonsoft.Json;
using System;


namespace ConsoleApp1
{
    public class BuildIndex
    {
        static void Main()
        {
            Program.Main();

            BaseIndex index = (BaseIndex)ServiceLocator.Current.GetInstance(Service.GetFullIndexName("ADI"));

            index.Process();
        }

    }

}