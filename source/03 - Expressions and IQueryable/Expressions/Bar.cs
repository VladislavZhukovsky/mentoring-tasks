using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task01
{
    public class Bar
    {
        public int number1;
        public int number2;
        public string str1;

        public int Number1
        {
            get { return number1; }
            set { number1 = value; }
        }
        public int Number2
        {
            get { return number2; }
            set { number2 = value; }
        }

        public string Str2 { get; set; }

        public object Obj { get; set; }
    }
}
