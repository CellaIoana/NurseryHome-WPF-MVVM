using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NurseryHome.Models;

namespace NurseryHome.ViewModels
{
    public class ParentViewModel : INotifyPropertyChanged
    {
        private readonly int parentId;

        public ObservableCollection<Child> Children { get; set; } = new();
        public Child SelectedChild
        {
            get => selectedChild;
            set
            {
                selectedChild = value;
                OnPropertyChanged(nameof(SelectedChild));
                LoadChildData();
            }
        }
        private Child selectedChild;

        public User Educator { get; set; }

        public ObservableCollection<Absence> Absences { get; set; } = new();
        public ObservableCollection<Payment> Payments { get; set; } = new();
        public ObservableCollection<Announcement> Announcements { get; set; } = new();

        public ParentViewModel(int parentId)
        {
            this.parentId = parentId;
            LoadChildren();
        }

        private void LoadChildren()
        {
            Children.Clear();
            foreach (var c in DatabaseManager.GetChildrenByParentId(parentId))
                Children.Add(c);

            if (Children.Any())
                SelectedChild = Children.First();
        }

        private void LoadChildData()
        {
            if (SelectedChild == null)
                return;

            Absences.Clear();
            Payments.Clear();
            Announcements.Clear();

            foreach (var abs in DatabaseManager.GetAbsencesByChildId(SelectedChild.Id))
                Absences.Add(abs);

            foreach (var pay in DatabaseManager.GetPaymentsByChildId(SelectedChild.Id))
                Payments.Add(pay);

            foreach (var ann in DatabaseManager.GetAnnouncementsByGroupId(SelectedChild.GroupId))
                Announcements.Add(ann);

            Educator = DatabaseManager.GetEducatorByGroupId(SelectedChild.GroupId);
            OnPropertyChanged(nameof(Educator));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

