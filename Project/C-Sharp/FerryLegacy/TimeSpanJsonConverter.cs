using Newtonsoft.Json;
using System;
using System.Xml;

namespace FerryLegacy
{
    /**
     * This file is added to be a Json converter
     * for the TimeSpan type as it is not a current
     * type handled by Json converters in the 
     * Newtonsoft.Json package
     */
    class TimeSpanJsonConverter :JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan);
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof(TimeSpan))
                throw new ArgumentException();

            var spanString = reader.Value as string;
            if (spanString == null)
                return null;
            return XmlConvert.ToTimeSpan(spanString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var duration = (TimeSpan)value;
            writer.WriteValue(XmlConvert.ToString(duration));
        }
    }
}
