using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Services.Services;
using Repositories.Basic;
using Repositories.Models;

namespace ResearchProjectManagement_182333
{
    public partial class LoginWindow : Window
    {
        private readonly UserAccountService _userAccountService;

        public LoginWindow()
        {
            InitializeComponent();
            var accountRepository = new GenericRepository<UserAccount>();
            _userAccountService = new UserAccountService(accountRepository);
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Password;

                txtStatus.Text = "";

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Invalid Email or Password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var userAccount = await _userAccountService.GetUserAccount(email, password);
                
                if (userAccount == null)
                {
                    MessageBox.Show("Invalid Email or Password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (userAccount.Role == 4)
                {
                    MessageBox.Show("No permission!", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (userAccount.Role >= 1 && userAccount.Role <= 3)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Tag = userAccount;
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid user role!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
