using Collette.Index;
using Collette.Index.Core;
using Collette.Index.Service;
//using Collette.Listener;
using Newtonsoft.Json;
using System;


namespace ConsoleApp1
{
    public class BuildIndex
    {
        static void Main()
        {
            Program.Main(); 
            // read component from queue
            var component = "ADI";
            BaseIndex index = (BaseIndex)ServiceLocator.Current.GetInstance(Service.GetFullIndexName(component));

            index.Process();
           
        }

    }

}