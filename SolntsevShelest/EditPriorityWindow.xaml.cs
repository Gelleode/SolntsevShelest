using SolntsevShelest.Utilities;
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
        private static List<Agent> _currentAgents;
        public EditPriorityWindow(List<Agent> agents)
        {
            InitializeComponent();
            _currentAgents = agents;
            TBoxPrior.Text = _currentAgents.Select(a => a.Priority).Max().ToString();
        }
        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32(TBoxPrior.Text);
            }
            catch
            {
                MessageBox.Show("Неправильный ввод");
                return;
            }
            foreach (Agent agent in _currentAgents)
                agent.Priority = Convert.ToInt32(TBoxPrior.Text);
            try
            {
                DatabaseContext.db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            DialogResult = true;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
