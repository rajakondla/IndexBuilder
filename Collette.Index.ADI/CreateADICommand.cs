using Collette.Index;
using Collette.IndexStore;
using Collette.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace Collette.Commands
{
    public class CreateADICommand : ICommand
    {
        public void Execute(BaseIndex index, Dependency dependecy)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (var constraint in dependecy.Constraints)
                {
                    index.Market = constraint.Market;
                    index.Fields = constraint.Fields;
                    sb.Append(index.Build());
                }

                // create jobject for index build
                IndexWrapper indexWrapper = new IndexWrapper();
                indexWrapper.Build(null);
                // create jobject and run the queue
                Queue.AddToQueue(null);
                // foreach market

                // create ADI object

                //
                // wait for all markets
                // submit to solr 
                // AddToQueue()
            }
            catch (Exception ex)
            {

            }

        }
    }
}
