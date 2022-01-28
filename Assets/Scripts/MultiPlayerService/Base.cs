using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Multiplayer
{
    public class BaseMessage
    {
        public BaseMessage()
        {

        }

        public BaseMessage(object data)
        {
            Data = JToken.FromObject(data, new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            Name = data.GetType().Name;
        }

        public string Name { get; set; }
        public Roles From { get; set; }
        public JToken Data { get; set; }
    }

    public static class Extentions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}