using System.Windows;
using Prism.Mvvm;
namespace CadwiseTest2
{
    public partial class BanknotsLimitWindow : Window
    {
        public BanknotsLimit banknotsLimit = new BanknotsLimit();
        public BanknotsLimitWindow()
        {
            InitializeComponent();
            this.DataContext = banknotsLimit;
        }
        public void OkClick(object sender, RoutedEventArgs e)
        {
            if (Limit < 0)
            {
                MessageBox.Show("Limit must be a strictly positive number.");
            }
            else
            {
                this.DialogResult = true;
            }
        }
        public int Limit { get { return banknotsLimit.Limit; } set { banknotsLimit.Limit = value; } }
    }
    public partial class BanknotsLimit : BindableBase
    {
        public int Limit { get { return m_limit; } set { m_limit = value; RaisePropertyChanged("Limit"); } }
        private int m_limit = 4096;
    }
}
