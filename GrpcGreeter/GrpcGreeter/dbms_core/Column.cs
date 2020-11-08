using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace dbms_core
{
    public class Column
    {
        public enum Types
        {
            Integer,
            Real,
            Char,
            String,
            ComplexInteger,
            ComplexReal
        }
        [JsonPropertyName("columnName")]
        public string ColumnName { get; set; }
        [JsonPropertyName("columnType")]
        public string ColumnType { get; set; }

        //public Column(string name, string type)
        //{
        //    ColumnName = name;
        //    switch (type)
        //    {
        //        case "int":
        //            //ColumnType = int;
        //            break;
        //        case "real":
        //            ColumnType = Types.Real;
        //            break;
        //        case "char":
        //            ColumnType = Types.Char;
        //            break;
        //        case "string":
        //            ColumnType = Types.String;
        //            break;
        //        case "cint":
        //            ColumnType = Types.ComplexInteger;
        //            break;
        //        case "creal":
        //            ColumnType = Types.ComplexReal;
        //            break;
        //    }
        //}

        public string AssignType(string type)
        {
            switch (type)
            {
                case "integer":
                    return "integer";
                case "real":
                    return "real";
                case "char":
                    return "char";
                case "string":
                    return "string";
                case "cinteger":
                    return "cinteger";
                case "creal":
                    return "creal";
                default: return "wrong_type";
            }
        }
    }
}
