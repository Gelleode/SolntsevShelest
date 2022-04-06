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
        }

        private void UpdateAgents()
        {
            List<Agent> agents = new List<Agent>(DatabaseContext.db.Agent);
            List<Discount> discounts = new List<Discount>(DatabaseContext.db.Agent.Select(s => new Discount { agentId = s.ID, 
                totalMoney = (from ps in s.ProductSale.Where(w => w.AgentID.Equals(s.ID))
                              join p in DatabaseContext.db.Product on ps.ProductID equals p.ID
                              select new { p.ID, p.MinCostForAgent, ps.ProductCount }), 
                productAmount = s.ProductSale.Where(w=>w.AgentID.Equals(s.ID)).Select(p=>p.ProductCount).DefaultIfEmpty().Sum() }));

            if (ComboFilter.SelectedIndex > 0)
                agents = agents.Where(p => p.AgentTypeID.Equals(ComboFilter.SelectedIndex)).ToList();
            if (ComboSort.SelectedIndex == 0)
                agents = agents.OrderBy(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 1)
                agents = agents.OrderByDescending(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 2)
                agents = agents;
            if (ComboSort.SelectedIndex == 3)
                agents = agents;
            if (ComboSort.SelectedIndex == 4)
                agents = agents.OrderBy(p => p.Priority).ToList();
            if (ComboSort.SelectedIndex == 5)
                agents = agents.OrderByDescending(p => p.Priority).ToList();
            LViewAgent.ItemsSource = agents.Where(p=>p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()));
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateAgents();
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateAgents();
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateAgents();
    }
}
