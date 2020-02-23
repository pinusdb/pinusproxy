using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace PinusProxy.Utils
{
  public class DateTime2LongConverter : DateTimeConverterBase
  {
    public override bool CanRead { get { return false; } }
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(DateTime);
    }
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value is DateTime)
      {
        DateTime timeVal = (DateTime)value;
        writer.WriteValue((timeVal.ToUniversalTime().Ticks - 621355968000000000) / 10000);
      }
      else
      {
        throw new JsonSerializationException("Unexpected value when converting date");
      }
    }
  }
}
