using System;
using System.Windows;
using Prism.Mvvm;
using Prism.Commands;
using System.Collections.ObjectModel;	
using System.Globalization;
using System.Windows.Data;
namespace CadwiseTest2
{
    public partial class MainWindow : Window
    {
        public BankTerminalVM vm = new BankTerminalVM();
        public MainWindow()
        {
            int limit = 4096;
            var limWindow = new BanknotsLimitWindow();
            if (limWindow.ShowDialog() == true)
                limit = limWindow.Limit;
            vm.BanknotsLimit = limit;
            InitializeComponent();
            this.DataContext = vm;
        }
    }
    public partial class BankTerminalVM : BindableBase
    {
        public BankTerminalVM()
        {
            m_model = new BankTerminal();
            m_model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            DepositeCommand = new DelegateCommand<object>(obj =>
            {
                try { m_model.AddBanknots((int)obj, 1); }
                catch (MyException e)
                {
                    MessageBox.Show(e.Message, "Command failed");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fatal error");
                }
            });
            GetSumCommand = new DelegateCommand<object>(obj =>
            {
                try { m_model.GetBanknotsGreedy(); }
                catch (MyException e)
                {
                    MessageBox.Show(e.Message, "Invalid input");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fatal error");
                }
            });
        }
        public int TotalSum { get { return m_model.TotalSum; } set { m_model.TotalSum = value; } }
        public int TotalBanknotsAmount { get { return m_model.TotalBanknotsAmount; } set { m_model.TotalBanknotsAmount = value; } }
        public int BanknotsLimit { get { return m_model.BanknotsLimit; } set { m_model.BanknotsLimit = value; } }
        public int RequiredSum { get { return m_model.RequiredSum; } set { m_model.RequiredSum = value; } }
        public int PreferedNominal { get { return m_model.PreferedNominal; } set { m_model.PreferedNominal = value; } }
        public bool DefaultNominal { get { return m_model.DefaultNominal; } set { m_model.DefaultNominal = value; } }
        public ObservableCollection<int> Deposits => m_model.Deposits;
        public ObservableCollection<int> BanknotsOut => m_model.BanknotsOut;
        public ObservableCollection<int> Banknots => m_model.Banknots;
        public DelegateCommand<object> DepositeCommand { get; set; }
        public DelegateCommand<object> GetSumCommand { get; set; }
        private BankTerminal m_model;
    }
    public class CashToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int cashValue = (int)value;
            if (cashValue <= 0)
                return "Can't give the sum!";
            return "Your cash: " + System.Convert.ToString(value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
