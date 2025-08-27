using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Repositories.Models;
using Services.Services;

namespace ResearchProjectManagement_182333
{
    public partial class DetailWindow : Window
    {
        private readonly ResearchProjectService _researchProjectService;
        private List<Researcher> _researchers = new();
        
        public ResearchProject? EditedOne { get; set; }

        public DetailWindow(ResearchProjectService? researchProjectService = null)
        {
            InitializeComponent();
            _researchProjectService = researchProjectService ?? new ResearchProjectService();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadResearchers();
            
            if (EditedOne != null)
            {
                LoadProjectData();
            }
            else
            {
                ClearFields();
                GenerateNewProjectId();
            }
        }

        private async Task LoadResearchers()
        {
            try
            {
                _researchers = await _researchProjectService.GetAllAsyncSub();
                LeadResearcher.ItemsSource = _researchers;
                LeadResearcher.DisplayMemberPath = "FullName";
                LeadResearcher.SelectedValuePath = "ResearcherId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProjectData()
        {
            if (EditedOne == null) return;

            ProjectId.Text = EditedOne.ProjectId.ToString();
            ProjectTitle.Text = EditedOne.ProjectTitle;
            ResearchField.Text = EditedOne.ResearchField;
            StartDate.SelectedDate = EditedOne.StartDate.ToDateTime(TimeOnly.MinValue);
            EndDate.SelectedDate = EditedOne.EndDate.ToDateTime(TimeOnly.MinValue);
            Budget.Text = EditedOne.Budget.ToString("F2");
            
            if (EditedOne.LeadResearcherId.HasValue)
            {
                LeadResearcher.SelectedValue = EditedOne.LeadResearcherId.Value;
            }

            ProjectId.IsReadOnly = true;
        }

        private void ClearFields()
        {
            ProjectId.Text = string.Empty;
            ProjectTitle.Text = string.Empty;
            ResearchField.Text = string.Empty;
            StartDate.SelectedDate = null;
            EndDate.SelectedDate = null;
            Budget.Text = string.Empty;
            LeadResearcher.SelectedIndex = -1;
            
            ProjectId.IsReadOnly = false;
        }

        private void GenerateNewProjectId()
        {
            try
            {
                var existingProjects = _researchProjectService.GetAll();
                var maxId = existingProjects.Any() ? existingProjects.Max(p => p.ProjectId) : 0;
                ProjectId.Text = (maxId + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var project = CreateProjectFromInput();

                if (EditedOne == null)
                {
                    await _researchProjectService.CreateAsync(project);
                    MessageBox.Show("Created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    project.ProjectId = EditedOne.ProjectId;
                    await _researchProjectService.UpdateAsync(project);
                    MessageBox.Show("Updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(ProjectId.Text) || !int.TryParse(ProjectId.Text, out _))
            {
                errors.Add("Invalid Project ID.");
            }

            if (string.IsNullOrWhiteSpace(ProjectTitle.Text))
            {
                errors.Add("Title required.");
            }

            if (string.IsNullOrWhiteSpace(ResearchField.Text))
            {
                errors.Add("Field required.");
            }

            if (!StartDate.SelectedDate.HasValue)
            {
                errors.Add("Start Date required.");
            }

            if (!EndDate.SelectedDate.HasValue)
            {
                errors.Add("End Date required.");
            }

            if (StartDate.SelectedDate.HasValue && EndDate.SelectedDate.HasValue)
            {
                if (EndDate.SelectedDate.Value <= StartDate.SelectedDate.Value)
                {
                    errors.Add("End Date must be after Start Date.");
                }
            }

            if (string.IsNullOrWhiteSpace(Budget.Text) || !decimal.TryParse(Budget.Text, out decimal budget) || budget <= 0)
            {
                errors.Add("Invalid Budget.");
            }

            if (LeadResearcher.SelectedValue == null)
            {
                errors.Add("Researcher required.");
            }

            if (EditedOne == null)
            {
                if (int.TryParse(ProjectId.Text, out int projectId))
                {
                    var existingProject = _researchProjectService.GetById(projectId);
                    if (existingProject != null)
                    {
                        errors.Add("ID exists.");
                    }
                }
            }

            if (errors.Any())
            {
                var errorMessage = string.Join("\n", errors);
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private ResearchProject CreateProjectFromInput()
        {
            return new ResearchProject
            {
                ProjectId = int.Parse(ProjectId.Text),
                ProjectTitle = ProjectTitle.Text.Trim(),
                ResearchField = ResearchField.Text.Trim(),
                StartDate = DateOnly.FromDateTime(StartDate.SelectedDate!.Value),
                EndDate = DateOnly.FromDateTime(EndDate.SelectedDate!.Value),
                LeadResearcherId = (int)LeadResearcher.SelectedValue!,
                Budget = decimal.Parse(Budget.Text)
            };
        }
    }
}
