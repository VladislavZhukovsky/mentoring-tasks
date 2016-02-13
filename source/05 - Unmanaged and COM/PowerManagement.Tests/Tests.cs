using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PowerManagement.Tests
{
    [TestClass]
    public class Tests
    {
        /// <summary>
        /// Task part 1.1 - getting system perameters
        /// </summary>
        //[TestMethod]
        public void TestMethod1()
        {
            IPowerManagementAdapter adapter = new PowerManagementAdapter();

            var result1 = adapter.GetLastSleepTime();
            Console.WriteLine("Last sleep time: {0}", result1);
            Console.WriteLine();

            var result2 = adapter.GetLastWakeTime();
            Console.WriteLine("Last wake time: {0}", result2);
            Console.WriteLine();

            var result3 = adapter.GetSystemBatteryState();
            Console.WriteLine("System Battery State");
            Console.WriteLine(result3);

            var result4 = adapter.GetSystemPowerInformation();
            Console.WriteLine("System Power Information");
            Console.WriteLine(result4);
        }

        //[TestMethod]
        public void TestMethod2()
        {
            IPowerManagementAdapter adapter = new PowerManagementAdapter();
            adapter.ReserveHibernationFile();
            adapter.RemoveHibernationFile();
        }

        //[TestMethod]
        public void TestMethod3()
        {
            IPowerManagementAdapter adapter = new PowerManagementAdapter();
            adapter.SetSuspendState();
            //adapter.SetHibernateState();
        }
    }
}
