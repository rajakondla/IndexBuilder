using Collette.Index;
using Collette.IndexStore;
using Collette.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Collette.Listener
{
    //public class IndexProcessor
    //{
    //    public void Execute(BaseIndex index, Condition dependency = null)
    //    {
    //        try
    //        {

    //            //var sb = new StringBuilder();
    //            ConcurrentBag<string> conList = new ConcurrentBag<string>();
    //            if (dependency != null)
    //            {
    //                var tasks = new List<Task>();
    //                foreach (var constraint in dependency?.Constraints)
    //                {
    //                    tasks.Add(new Task(async () =>
    //                    {
    //                        index.Market = constraint.Market;
    //                        //index.Fields = constraint.Fields;
    //                        conList.Add(await index.Build());
    //                    }));
    //                }

    //                Task.WaitAll(tasks.ToArray());
    //            }
    //            else
    //            {
    //                conList.Add(index.Build().Result);
    //            }

    //            var result = string.Join("", conList.ToArray());
    //            var indexWrapper = new IndexWrapper();

    //            // create jobject for index build

    //            indexWrapper.Build(null);
    //            // create jobject and run the queue
    //            Queue.Push(null);
    //            // foreach market

    //            // create ADI object

    //            //
    //            // wait for all markets
    //            // submit to solr 
    //            // AddToQueue()

    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //}

    //public class QueueListener
    //{
    //    public void Process()
    //    {
    //        // Listener started
    //        //ICommand cmd = null;// Msg.Pop();
    //        //cmd.Execute();

    //        // Listener end
    //    }
    //}
}
