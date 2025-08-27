using System.Windows;
using System.Windows.Controls;
using Repositories.Models;
using Services.Services;

namespace ResearchProjectManagement_182333
{
    public partial class MainWindow : Window
    {
        private readonly ResearchProjectService _researchProjectService = new();
        private UserAccount? _currentUser;
        private List<ResearchProject> _allProjects = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _currentUser = this.Tag as UserAccount;
            ConfigureUIForRole();
            FillGrid();
        }

        private void ConfigureUIForRole()
        {
            if (_currentUser == null) return;

            switch (_currentUser.Role)
            {
                case 1:
                    CreateButton.IsEnabled = true;
                    UpdateButton.IsEnabled = true;
                    DeleteButton.IsEnabled = true;
                    break;
                case 2:
                    CreateButton.IsEnabled = true;
                    UpdateButton.IsEnabled = true;
                    DeleteButton.IsEnabled = false;
                    break;
                case 3:
                    CreateButton.IsEnabled = false;
                    UpdateButton.IsEnabled = false;
                    DeleteButton.IsEnabled = false;
                    break;
                default:
                    CreateButton.IsEnabled = false;
                    UpdateButton.IsEnabled = false;
                    DeleteButton.IsEnabled = false;
                    break;
            }
        }

        private void FillGrid()
        {
            try
            {
                ModelDataGrid.ItemsSource = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                _allProjects = _researchProjectService.GetAllIncludeOrderBy(x => x.ProjectId);
                ModelDataGrid.ItemsSource = _allProjects;
                ModelDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            PerformSearch();
        }

        private void PerformSearch()
        {
            try
            {
                string searchText = SearchTextBox.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(searchText))
                {
                    ModelDataGrid.ItemsSource = _allProjects;
                }
                else
                {
                    var filteredProjects = _allProjects.Where(p => 
                        (p.ProjectTitle != null && p.ProjectTitle.ToLower().Contains(searchText)) ||
                        (p.ResearchField != null && p.ResearchField.ToLower().Contains(searchText))
                    ).ToList();
                    ModelDataGrid.ItemsSource = filteredProjects;
                }
                ModelDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser?.Role != 1 && _currentUser?.Role != 2)
            {
                MessageBox.Show("No permission!", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var detailWindow = new DetailWindow(_researchProjectService)
            {
                Label = { Content = "Create" }
            };
            detailWindow.ShowDialog();
            FillGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser?.Role != 1 && _currentUser?.Role != 2)
            {
                MessageBox.Show("No permission!", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ModelDataGrid.SelectedItem is ResearchProject researchProject)
            {
                var detailWindow = new DetailWindow(_researchProjectService)
                {
                    EditedOne = researchProject,
                    Label = { Content = "Edit" }
                };
                detailWindow.ShowDialog();
                FillGrid();
            }
            else
            {
                MessageBox.Show("Select a project first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser?.Role != 1)
            {
                MessageBox.Show("No permission!", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ModelDataGrid.SelectedItem is not ResearchProject researchProject)
            {
                MessageBox.Show("Select a project first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Delete: {researchProject.ProjectTitle}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _researchProjectService.Remove(researchProject);
                FillGrid();
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}