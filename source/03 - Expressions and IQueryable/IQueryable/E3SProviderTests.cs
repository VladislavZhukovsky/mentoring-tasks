using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample03.E3SClient.Entities;
using Sample03.E3SClient;
using System.Configuration;
using System.Linq;

namespace Sample03
{
    [TestClass]
    public class E3SProviderTests
    {
        //[TestMethod]
        public void TaskPart1()
        {
            //Changes are in class ExpressionToFTSRequestTranslator
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            Console.WriteLine("");
            foreach (var emp in employees.Where(e => "EPBYMINW4226" == e.workstation ))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        //[TestMethod]
        public void TaskPart2StartsWith()
        {
            //Changes are in class ExpressionToFTSRequestTranslator
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            Console.WriteLine("StartsWith");
            foreach (var emp in employees.Where(e => e.workstation.StartsWith("EPBYMINW422")))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        //[TestMethod]
        public void TaskPart2EndsWith()
        {
            //Changes are in class ExpressionToFTSRequestTranslator
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            Console.WriteLine("EndsWith");
            foreach (var emp in employees.Where(e => e.workstation.EndsWith("226")))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        //[TestMethod]
        public void TaskPart2Contains()
        {
            //Changes are in class ExpressionToFTSRequestTranslator
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            Console.WriteLine("Contains");
            foreach (var emp in employees.Where(e => e.workstation.Contains("PBYMINW422")))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }

        /// <summary>
        /// Changes are in classes:
        ///     E3SQueryClient
        ///     FTSRequestGenerator
        ///     ExpressionToFTSRequestTranslator
        ///     
        /// </summary>
        //[TestMethod]
        public void TaskPart3And()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            foreach (var emp in employees.Where(e => e.workstation.StartsWith("EPBYMINW42") && e.workstation.EndsWith("7")))
            {
                Console.WriteLine("{0} {1}", emp.nativename, emp.startworkdate);
            }
        }
    }
}
