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
            Manager.MainFrame = MainFrame;
        }

        private void UpdateAgents()
        {
            List<Agent> agents = new List<Agent>(DatabaseContext.db.Agent);
            List<Discount> discounts = new List<Discount>(DatabaseContext.db.Agent.Select(s => new Discount { agentId = s.ID,
                totalMoney = (from ps in s.ProductSale.Where(w => w.AgentID.Equals(s.ID))
                              join p in DatabaseContext.db.Product on ps.ProductID equals p.ID
                              select new { p.ID, money = p.MinCostForAgent * ps.ProductCount }).Select(p=>p.money).DefaultIfEmpty().Sum(),
                productAmount = s.ProductSale.Where(w => w.AgentID.Equals(s.ID)).Select(p => p.ProductCount).DefaultIfEmpty().Sum() }));
            var curAgents = from a in agents
                            join d in discounts on a.ID equals d.agentId
                            select new { ID = a.ID, Logo = a.Logo, Title = a.Title, ProductCount = d.productAmount, Phone = a.Phone, Priority = a.Priority, Discount = d.discount, AgentTypeID = a.AgentTypeID, AgentType = a.AgentType};

            if (ComboFilter.SelectedIndex > 0)
                curAgents = curAgents.Where(p => p.AgentTypeID.Equals(ComboFilter.SelectedIndex)).ToList();
            if (ComboSort.SelectedIndex == 0)
                curAgents = curAgents.OrderBy(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 1)
                curAgents = curAgents.OrderByDescending(p => p.Title).ToList();
            if (ComboSort.SelectedIndex == 2)
                curAgents = curAgents.OrderBy(p => p.Discount).ToList();
            if (ComboSort.SelectedIndex == 3)
                curAgents = curAgents.OrderByDescending(p => p.Discount).ToList();
            if (ComboSort.SelectedIndex == 4)
                curAgents = curAgents.OrderBy(p => p.Priority).ToList();
            if (ComboSort.SelectedIndex == 5)
                curAgents = curAgents.OrderByDescending(p => p.Priority).ToList();
            LViewAgent.ItemsSource = curAgents.Where(p=>p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()));
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateAgents();
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateAgents();
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateAgents();
        private void LViewAgent_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Manager.MainFrame.Navigate(new AddEditPage(Convert.ToInt32(LViewAgent.SelectedItem.ToString().Split(' ')[3].Remove(LViewAgent.SelectedItem.ToString().Split(' ')[3].Length - 1))));

        private void MainFrame_ContentRendered(object sender, EventArgs e) { }
    }
}
