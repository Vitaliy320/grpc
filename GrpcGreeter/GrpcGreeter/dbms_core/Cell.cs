using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace dbms_core
{
    public class Cell
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }


        public Cell()
        { 
        }

        public Cell(string value)
        {
            Value = value;
        }
        public Cell(string value, string type)
        {
            Value = value;
            Type = type;
        }
    }
}
