using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;
using CadwiseTest2;
namespace CadwiseUnitTest2
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestGetBanknots1()
        {
            var terminal = new BankTerminal()
            {
                BanknotsLimit = 1000,
                RequiredSum = 500,
                DefaultNominal = true,
            };
            terminal.AddBanknots(100, 5);
            terminal.AddBanknots(500, 1);
            terminal.GetBanknotsGreedy();
            var primer = new ObservableCollection<int> { 500 };
             CustomAssert.AreEqual(primer, terminal.BanknotsOut);
        }
        [TestMethod]
        public void TestGetBanknots2()
        {
            var terminal = new BankTerminal()
            {
                BanknotsLimit = 1000,
                RequiredSum = 500,
                DefaultNominal = false,
                PreferedNominal = 100
            };
            terminal.AddBanknots(100, 5);
            terminal.AddBanknots(500, 1);
            terminal.GetBanknotsGreedy();
            var primer = new ObservableCollection<int> { 100, 100, 100, 100, 100 };
            CustomAssert.AreEqual(primer, terminal.BanknotsOut);
        }
        [TestMethod]
        public void TestGetBanknots3()
        {
            var terminal = new BankTerminal()
            {
                BanknotsLimit = 1000,
                RequiredSum = 500,
                DefaultNominal = false,
                PreferedNominal = 1000
            };
            terminal.AddBanknots(100, 5);
            terminal.AddBanknots(500, 1);
            terminal.GetBanknotsGreedy();
            var primer = new ObservableCollection<int> { 500 };
            CustomAssert.AreEqual(primer, terminal.BanknotsOut);
        }
        [TestMethod]
        public void TestGetBanknots4()
        {
            var terminal = new BankTerminal()
            {
                BanknotsLimit = 1000,
                RequiredSum = 500,
                DefaultNominal = false,
                PreferedNominal = 1000
            };
            terminal.AddBanknots(100, 5);
            terminal.AddBanknots(500, 1);
            terminal.GetBanknotsGreedy();
            var primer = new ObservableCollection<int> { };
            CustomAssert.AreEqual(primer, terminal.BanknotsOut);
        }
        [TestMethod]
        public void TestBanknotsLimitException()
        {
            var terminal = new BankTerminal()
            {
                BanknotsLimit = 5,
                RequiredSum = 500,
                DefaultNominal = false,
                PreferedNominal = 1000
            };
            terminal.AddBanknots(100, 5);
            try
            {
                terminal.AddBanknots(500, 1);
                Assert.IsTrue(false); ;
            }
            catch (MyException e)
            {
                bool res = (e.Message == "Not enough space in terminal!");
                Assert.IsTrue(res);
            }
        }
        [TestMethod]
        public void TestWrongSumException()
        {
            try
            {
                var terminal = new BankTerminal()
                {
                    BanknotsLimit = 5,
                    RequiredSum = -100,
                    DefaultNominal = false,
                    PreferedNominal = 1000
                };
                terminal.GetBanknotsGreedy();
                Assert.IsTrue(false);
            }
            catch (MyException e)
            {
                bool res = (e.Message == "The sum must be a strictly positive number");
                Assert.IsTrue(res);
            }
        }
        [TestMethod]
        public void TestWrongNominalException()
        {
            try
            {
                var terminal = new BankTerminal()
                {
                    BanknotsLimit = 5,
                    RequiredSum = 100,
                    DefaultNominal = false,
                    PreferedNominal = 800
                };
                terminal.GetBanknotsGreedy();
                Assert.IsTrue(false);
            }
            catch (MyException e)
            {
                bool res = (e.Message == "Required nominal doesn't exist.");
                Assert.IsTrue(res);
            }
        }
    }
    public class CustomAssert
    {
        public static void AreEqual(ObservableCollection<int> primer, ObservableCollection<int> result)
        {
            bool res = primer.SequenceEqual<int>(result);
            Assert.IsTrue(res);
        }
    }
}
