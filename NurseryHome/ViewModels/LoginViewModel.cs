using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using NurseryHome.Models;
using NurseryHome.Views;

namespace NurseryHome.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string email;
        private string password;

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(param => Login());
        }

        private void Login()
        {
            try
            {
                using (SqlConnection conn = DatabaseManager.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string role = reader["Role"].ToString().Trim().ToLower();
                        MessageBox.Show($"ROLUL EXTRAS ESTE: {role}");

                        Window nextWindow = null;

                        switch (role)
                        {
                            case "admin":
                                nextWindow = new AdminDashboard();
                                break;
                            case "educator":
                                int Id = Convert.ToInt32(reader["Id"]);
                                nextWindow = new EducatorDashboard(Id);
                                break;
                            case "parinte":
                                int userId = Convert.ToInt32(reader["Id"]);
                                nextWindow = new ParentDashboard(userId);
                                break;
                            default:
                                MessageBox.Show("Rol necunoscut!");
                                return;
                        }

                        nextWindow.Show();

                        // Închidem login-ul
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.Title == "Autentificare")
                            {
                                window.Close();
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email sau parolă incorecte.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la login: " + ex.Message);
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
