using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task01
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new MappingGenerator();
            var func = generator.Generate<Foo, Bar>();

            var foo = new Foo();
            foo.number1 = 1;
            foo.number2 = 2;
            foo.str1 = "str1";
            foo.Str2 = "str2";
            foo.Obj = new object();

            var bar = func.Map(foo);

            Console.WriteLine("Bar.number1 = {0}", bar.number1);
            Console.WriteLine("Bar.number2 = {0}", bar.number2);
            Console.WriteLine("Bar.str = {0}", bar.str1);
            Console.WriteLine("Bar.Str = {0}", bar.Str2);
            Console.WriteLine("Bar.Obj = {0}", bar.Obj.ToString());

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
