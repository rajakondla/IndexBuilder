using Collette.Index;
using Collette.Utilities;
using Newtonsoft.Json.Linq;

namespace Collette.Index
{
    public class ADIPublisher:IPublisher
    {
        public void Publish()
        {
            // read configuration
            // 
            // ICommand cmd=ServiceLocator.GetService("ADI");

            // msgqueue.push(serialize(cmd));

            JObject jObj = new JObject();
            
            jObj.Add(new JProperty("type","ADI"));
            Queue.Push(jObj);
        }
    }
}
