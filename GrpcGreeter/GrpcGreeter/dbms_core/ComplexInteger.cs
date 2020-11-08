using System;
using System.Collections.Generic;
using System.Text;

namespace dbms_core
{
    public class ComplexInteger
    {
        string ComplexNumber { get; set; }

        public ComplexInteger(int a, int b)
        {
            if (b < 0)
            {
                ComplexNumber = a.ToString() + b + "i";
            }
            else
            {
                ComplexNumber = a.ToString() + Convert.ToString('+') + b + "i";
            }
        }

        public string GetComplexInteger()
        {
            return ComplexNumber;
        }
    }
}
