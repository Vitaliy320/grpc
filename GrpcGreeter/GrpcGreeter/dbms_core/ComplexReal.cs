using System;
using System.Collections.Generic;
using System.Text;

namespace dbms_core
{
    class ComplexReal
    {
        string ComplexNumber { get; set; }
        string RealValue { get; set; }
        public ComplexReal(int a, int b)
        {
            if (b < 0)
            {
                ComplexNumber = a.ToString() + b + "i";
            }
            else
            {
                ComplexNumber = a.ToString() + Convert.ToString('+') + b + "i";
            }
            RealValue = a.ToString();
        }

        public string GetComplexReal()
        {
            return RealValue;
        }
    }
}
