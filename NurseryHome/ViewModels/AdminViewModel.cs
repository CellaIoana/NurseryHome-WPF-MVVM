using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using NurseryHome.Models;


namespace NurseryHome.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {

        private string fullName;
        private DateTime dateOfBirth = DateTime.Now;
        private int groupId;
        private int parentId;
        public ObservableCollection<Child> Children { get; set; } = new();
        private Child selectedChild;
        public Child SelectedChild
        {
            get => selectedChild;
            set
            {
                selectedChild = value;
                OnPropertyChanged(nameof(SelectedChild)); //trimitem semnal UI-ului sa actualizam legaturile de databinding
            }
        }

        public ICommand AddChildCommand { get; }
        public ICommand EditChildCommand { get; }
        public ICommand DeleteChildCommand { get; }
        public string FullName { get; set; } = "";
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        public int GroupId { get; set; }
        public int ParentId { get; set; }

        public ObservableCollection<User> Parents { get; set; } = new();
        public User SelectedParent { get; set; } = new();

        public ICommand AddParentCommand { get; }
        public ICommand EditParentCommand { get; }
        public ICommand DeleteParentCommand { get; }

        public string ParentFullName { get; set; } = "";
        public string ParentEmail { get; set; } = "";
        public string ParentPassword { get; set; } = "";
        public string ParentPhone { get; set; } = "";
        public string ParentAddress { get; set; } = "";

        public ObservableCollection<User> AllEducators { get; set; } = new();
        public ObservableCollection<User> AllParents { get; set; } = new();
        public ObservableCollection<ParentEducator> ParentEducatorLinks { get; set; } = new();

        //AllParents si AllEducators sunt folosite in dropdown-uri pt a alege un educator si un parinte
        //ParentEducatorLinks tine legaturile deja existente
        //LinkParentEducatorCommand face conexiunea intre cei doi
        public User SelectedEducator { get; set; }
        public ICommand LinkParentEducatorCommand { get; }

        public string EducatorFullName { get; set; } = "";
        public string EducatorPhone { get; set; } = "";
        public string EducatorEmail { get; set; } = "";
        public string EducatorPassword { get; set; } = "";
        public string EducatorAddress { get; set; } = "";

        public ICommand AddEducatorCommand { get; }
        public ICommand EditEducatorCommand { get; }
        public ICommand DeleteEducatorCommand { get; }

        public ObservableCollection<User> Educators { get; set; } = new();
        public User SelectedEducatorForCrud { get; set; } = new();

        public ObservableCollection<Group> Groups { get; set; } = new();
        public Group SelectedGroup { get; set; } = new();

        public string GroupName { get; set; } = "";
        public int GroupEducatorId { get; set; }

        public ICommand AddGroupCommand { get; }
        public ICommand EditGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }

        public AdminViewModel()
        {
            AddChildCommand = new RelayCommand(_ => AddChild());
            EditChildCommand = new RelayCommand(_ => EditChild());
            DeleteChildCommand = new RelayCommand(_ => DeleteChild());

            AddParentCommand = new RelayCommand(_ => AddParent());
            EditParentCommand = new RelayCommand(_ => EditParent());
            DeleteParentCommand = new RelayCommand(_ => DeleteParent());

            LinkParentEducatorCommand = new RelayCommand(_ => LinkParentToEducator());

            AddEducatorCommand = new RelayCommand(_ => AddEducator());
            EditEducatorCommand = new RelayCommand(_ => EditEducator());
            DeleteEducatorCommand = new RelayCommand(_ => DeleteEducator());

            AddGroupCommand = new RelayCommand(_ => AddGroup());
            EditGroupCommand = new RelayCommand(_ => EditGroup());
            DeleteGroupCommand = new RelayCommand(_ => DeleteGroup());

            LoadGroups();
            LoadEducators();
            LoadAllParents();
            LoadParentEducatorLinks();

            LoadChildren();
            LoadParents();
        }

        private void LoadChildren()
        {
            Children.Clear();
            foreach (var c in DatabaseManager.GetChildren())
                Children.Add(c);
        }

        private void AddChild()
        {
            if (!string.IsNullOrWhiteSpace(FullName))
            {
                var child = new Child
                {
                    FullName = FullName,
                    DateOfBirth = DateOfBirth,
                    GroupId = GroupId,
                    ParentId = ParentId
                };
                DatabaseManager.AddChild(child);
                LoadChildren();
            }
        }

        private void EditChild()
        {
            if (SelectedChild != null)
            {
                DatabaseManager.UpdateChild(SelectedChild);
                LoadChildren();
            }
        }
        private void LoadParents()
        {
            Parents.Clear();
            foreach (var p in DatabaseManager.GetParents())
                Parents.Add(p);
        }

        private void AddParent()
        {
            if (!string.IsNullOrWhiteSpace(ParentFullName))
            {
                var parent = new User
                {
                    FullName = ParentFullName,
                    Email = ParentEmail,
                    Password = ParentPassword,
                    Phone = ParentPhone,
                    Address = ParentAddress,
                    Role = "parinte"
                };
                DatabaseManager.AddParent(parent);
                LoadParents();
            }
        }

        private void EditParent()
        {
            if (SelectedParent != null)
            {
                DatabaseManager.UpdateParent(SelectedParent);
                LoadParents();
            }
        }

        private void DeleteParent()
        {
            if (SelectedParent != null)
            {
                DatabaseManager.DeleteParent(SelectedParent.Id);
                LoadParents();
            }
        }
        private void DeleteChild()
        {
            if (SelectedChild != null)
            {
                DatabaseManager.DeleteChild(SelectedChild.Id);
                LoadChildren();
            }
        }

        private void LoadEducators()
        {
            AllEducators.Clear();
            foreach (var user in DatabaseManager.GetUsersByRole("educator"))
                AllEducators.Add(user);
        }

        private void LoadAllParents()
        {
            AllParents.Clear();
            foreach (var user in DatabaseManager.GetUsersByRole("parinte"))
                AllParents.Add(user);
        }

        private void LoadParentEducatorLinks()
        {
            ParentEducatorLinks.Clear();
            foreach (var link in DatabaseManager.GetParentEducatorLinks())
                ParentEducatorLinks.Add(link);
        }

        private void LinkParentToEducator()
        {
            if (SelectedParent != null && SelectedEducator != null)
            {
                DatabaseManager.AddParentEducatorLink(SelectedParent.Id, SelectedEducator.Id);
                LoadParentEducatorLinks();
            }
        }

        private void AddEducator()
        {
            if (!string.IsNullOrWhiteSpace(EducatorFullName) && !string.IsNullOrWhiteSpace(EducatorEmail))
            {
                var educator = new User
                {
                    FullName = EducatorFullName,
                    Email = EducatorEmail,
                    Password = EducatorPassword,
                    Phone = EducatorPhone,
                    Address = EducatorAddress,
                    Role = "educator"
                };
                DatabaseManager.AddEducator(educator);
                LoadEducators();
            }
        }

        private void EditEducator()
        {
            if (SelectedEducator != null)
            {
                DatabaseManager.UpdateEducator(SelectedEducator);
                LoadEducators();
            }
        }

        private void DeleteEducator()
        {
            if (SelectedEducator != null)
            {
                DatabaseManager.DeleteEducator(SelectedEducator.Id);
                LoadEducators();
            }
        }

        private void LoadGroups()
        {
            Groups.Clear();
            foreach (var g in DatabaseManager.GetGroups())
                Groups.Add(g);
        }

        private void AddGroup()
        {
            if (!string.IsNullOrWhiteSpace(GroupName))
            {
                var group = new Group
                {
                    Name = GroupName,
                    EducatorId = GroupEducatorId
                };
                DatabaseManager.AddGroup(group);
                LoadGroups();
            }
        }

        private void EditGroup()
        {
            if (SelectedGroup != null)
            {
                DatabaseManager.UpdateGroup(SelectedGroup);
                LoadGroups();
            }
        }

        private void DeleteGroup()
        {
            if (SelectedGroup != null)
            {
                DatabaseManager.DeleteGroup(SelectedGroup.Id);
                LoadGroups();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
