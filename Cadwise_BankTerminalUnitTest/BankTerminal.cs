using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace CadwiseTest2
{
    public class MyException : Exception
    {
        public MyException(string message):base(message)
        { }
    }
    public partial class BankTerminal : BindableBase
    {
        public void AddBanknots(int nominal, int amount)
        {
            if (TotalBanknotsAmount + amount > BanknotsLimit)
                throw new MyException("Not enough space in terminal!");
            TotalBanknotsAmount += amount;
            TotalSum += nominal * amount;
            if (m_banknots.ContainsKey(nominal))
                m_banknots[nominal] += amount;
            else
                m_banknots.Add(nominal, amount);
            for (int i = 0; i < amount; i++)
            {
                m_deposits.Add(nominal);
                RaisePropertyChanged("Deposits");
            }
        }
        public void GetBanknotsGreedy()
        {
            if (RequiredSum <= 0)
                throw new MyException("The sum must be a strictly positive number");
            if (!m_banknots.ContainsKey(PreferedNominal))
                throw new MyException("Required nominal doesn't exist.");
            int actualSum = 0;
            var newBanknots = new SortedDictionary<int, int>(m_banknots);
            var currentBanknotsOut = new List<int>();
            foreach (var banknot in Enumerable.Reverse(m_banknots))
            {
                while (actualSum + banknot.Key <= RequiredSum && newBanknots[banknot.Key] > 0) //Value - количество, Key - номинал
                {
                    newBanknots[banknot.Key]--;
                    currentBanknotsOut.Add(banknot.Key);
                    actualSum += banknot.Key;
                }
            }
            if(!DefaultNominal)
                foreach (int nominal in currentBanknotsOut.ToArray())
                {
                    if (nominal % PreferedNominal == 0 && nominal > PreferedNominal)
                    {
                        int requiredAmount = nominal / PreferedNominal;
                        if (requiredAmount <= newBanknots[PreferedNominal])
                        {
                            newBanknots[PreferedNominal] -= requiredAmount;
                            newBanknots[nominal]++;
                            currentBanknotsOut.Remove(nominal);
                            for (int i = 0; i < requiredAmount; i++)
                                currentBanknotsOut.Add(PreferedNominal);
                        }
                    }
                }
            if (actualSum == RequiredSum)
            {
                m_banknots = newBanknots;
                foreach (int nominal in currentBanknotsOut)
                {
                    m_banknotsOut.Add(nominal);
                    RaisePropertyChanged("BanknotsOut");
                    TotalBanknotsAmount--;
                }
                TotalSum -= actualSum;
            }
            else
            {
                m_banknotsOut.Add(-1);
                RaisePropertyChanged("BanknotsOut");
            }
        }
        public ObservableCollection<int> Deposits { get { return new ObservableCollection<int>(m_deposits); } }
        public ObservableCollection<int> BanknotsOut { get { return new ObservableCollection<int>(m_banknotsOut); } }
        public int RequiredSum { get { return m_requiredSum; } set { m_requiredSum = value; RaisePropertyChanged("RequiredSum"); } }
        public int PreferedNominal { get { return m_preferedNominal; } set { m_preferedNominal = value; RaisePropertyChanged("PreferedNominal"); } }
        public bool DefaultNominal { get { return m_defaultNominal; } set { m_defaultNominal = value; RaisePropertyChanged("PreferedNominal"); } }
        public int TotalSum { get { return m_totalSum; } set { m_totalSum = value; RaisePropertyChanged("TotalSum"); } }
        public int TotalBanknotsAmount { get { return m_totalBanknotsAmount; } set { m_totalBanknotsAmount = value; RaisePropertyChanged("TotalBanknotsAmount"); } }
        public int BanknotsLimit { get { return m_banknotsLimit; } set { m_banknotsLimit = value; RaisePropertyChanged("RequiredSum"); } }
        public ObservableCollection<int> Banknots { get { return new ObservableCollection<int>(m_banknots.Keys); } }
        private List<int> m_deposits = new List<int>();
        private List<int> m_banknotsOut = new List<int>();
        private int m_requiredSum = 0;
        private int m_preferedNominal = 5000;
        private bool m_defaultNominal = true;
        private int m_totalSum = 0;
        private int m_totalBanknotsAmount = 0;
        private int m_banknotsLimit = 4096;
        private SortedDictionary<int, int> m_banknots = new SortedDictionary<int, int>()
        {
            {10,0},
            {50,0},
            {100,0},
            {500,0},
            {1000,0},
            {5000,0}
        };
    }
}
