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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolntsevShelest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BtnChangePriority.Visibility = Visibility.Hidden;
            var allTypes = DatabaseContext.db.AgentType.ToList();

            allTypes.Insert(0, new AgentType
            {
                Title = "Все типы"
            });

            ComboFilter.ItemsSource = allTypes.Select(p => new { Title = p.Title }).ToArray();
            ComboFilter.SelectedIndex = 0;
            List<string> allFilters = new List<string>() { "По возрастанию имени", "По убыванию имени", "По возрастанию скидки", "По убыванию скидки", "По возрастанию приоритета", "По убыванию приоритета" };
            ComboSort.ItemsSource = allFilters;
            ComboSort.SelectedIndex = 0;
            UpdateAgents();
            Manager.MainFrame = MainFrame;
        }

        private void UpdateAgents()
        {
            var curDate = DateTime.Now.Date.AddDays(-365);
            var me = DatabaseContext.db.Agent.ToList();
            List<AgentWithDiscount> curAgents = new List<AgentWithDiscount>(DatabaseContext.db.Agent.Select(s => new AgentWithDiscount { Agent = s,
                TotalMoney = s.ProductSale.Where(w => w.AgentID.Equals(s.ID) && w.SaleDate >= curDate).Join(DatabaseContext.db.Product, ps => ps.ProductID, p => p.ID, (ps, p) => new { money = p.MinCostForAgent * ps.ProductCount }).Select(p => p.money).DefaultIfEmpty().Sum(),
                ProductCount = s.ProductSale.Where(w => w.AgentID.Equals(s.ID) && w.SaleDate >= curDate).Select(p => p.ProductCount).DefaultIfEmpty().Sum()}));

            if (ComboFilter.SelectedIndex > 0)
                curAgents = curAgents.Where(p => p.Agent.AgentTypeID.Equals(ComboFilter.SelectedIndex)).ToList();
            switch (ComboSort.SelectedIndex)
            {
                case 0:
                    curAgents = curAgents.OrderBy(p => p.Agent.Title).ToList();
                    break;
                case 1:
                    curAgents = curAgents.OrderByDescending(p => p.Agent.Title).ToList();
                    break;
                case 2:
                    curAgents = curAgents.OrderBy(p => p.Discount).ToList();
                    break;
                case 3:
                    curAgents = curAgents.OrderByDescending(p => p.Discount).ToList();
                    break;
                case 4:
                    curAgents = curAgents.OrderBy(p => p.Agent.Priority).ToList();
                    break;
                case 5:
                    curAgents = curAgents.OrderByDescending(p => p.Agent.Priority).ToList();
                    break;
            }
            if (TBoxSearch.Text != null)
                curAgents = curAgents.Where(p => p.Agent.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewAgent.ItemsSource = curAgents;
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateAgents();
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateAgents();
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateAgents();
        private void LViewAgent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddEditWindow addEditWindow = new AddEditWindow((LViewAgent.SelectedItem as AgentWithDiscount).Agent);
            addEditWindow.ShowDialog();
        }
        private void MainFrame_ContentRendered(object sender, EventArgs e) => UpdateAgents();
        public void UpdateAfterClose() => UpdateAgents();
        private void LViewAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LViewAgent.SelectedItems.Count > 1)
                BtnChangePriority.Visibility = Visibility.Visible;
            else
                BtnChangePriority.Visibility = Visibility.Hidden;
        }
        private void BtnChangePriority_Click(object sender, RoutedEventArgs e)
        {
            EditPriorityWindow editPriorityWindow = new EditPriorityWindow(LViewAgent.SelectedItems.Cast<AgentWithDiscount>().Select(s => s.Agent).ToList());
            if (editPriorityWindow.ShowDialog() == true)
                UpdateAgents();
        }
    }
}
