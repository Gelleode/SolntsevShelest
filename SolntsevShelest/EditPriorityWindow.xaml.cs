using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SolntsevShelest
{
    /// <summary>
    /// Логика взаимодействия для EditPriorityWindow.xaml
    /// </summary>
    public partial class EditPriorityWindow : Window
    {
        public EditPriorityWindow(List<Agent> agents)
        {
            var MaxPrior = agents.Select(a => a.Priority).Max();
            InitializeComponent();
            TBoxPrior.Text = MaxPrior.ToString();
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
