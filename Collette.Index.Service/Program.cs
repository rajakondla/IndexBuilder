using Collette.Index.Core;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Collette.Index.Service
{
    public class Program
    {
        public static void Main()
        {
            // read configurations

            Startup start = new Startup();
            start.ConfigureService();

            List<IPublisher> publishers = new List<IPublisher>();
            publishers.Add((IPublisher)ServiceLocator.Current.GetInstance(Service.GetFullPublisherName("ADIPublisher")));

            foreach (var get in publishers)
            {
                get.Publish();
            }
        }
    }
}
