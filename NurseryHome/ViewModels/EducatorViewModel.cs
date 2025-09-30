using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using NurseryHome.Models;

namespace NurseryHome.ViewModels
{
    public class EducatorViewModel : INotifyPropertyChanged
    {
        private readonly int educatorId;

        public ObservableCollection<Group> EducatorGroups { get; set; } = new();
        public ObservableCollection<Program> Programs { get; set; } = new();
        public ObservableCollection<Material> Materials { get; set; } = new();
        public ObservableCollection<Announcement> Announcements { get; set; } = new();

        private Group? selectedGroup;
        public Group? SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
                LoadPrograms();
                LoadMaterials();
                LoadAnnouncements();
            }
        }

        public Program SelectedProgram { get; set; } = new();
        public string ProgramActivityName { get; set; } = "";
        public DateTime ProgramDate { get; set; } = DateTime.Today;
        public int SelectedGroupIdForProgram { get; set; }

        public ICommand AddProgramCommand { get; }
        public ICommand EditProgramCommand { get; }
        public ICommand DeleteProgramCommand { get; }
        public Announcement SelectedAnnouncement { get; set; } = new();
        public string AnnouncementTitle { get; set; } = "";
        public string AnnouncementDescription { get; set; } = "";

        public ICommand AddAnnouncementCommand { get; }
        public ICommand EditAnnouncementCommand { get; }
        public ICommand DeleteAnnouncementCommand { get; }     
        public Material SelectedMaterial { get; set; } = new();
        public string MaterialFileName { get; set; } = "";
        public string MaterialFilePath { get; set; } = "";
        public int SelectedGroupIdForMaterial { get; set; }

        public ICommand AddMaterialCommand { get; }
        public ICommand EditMaterialCommand { get; }
        public ICommand DeleteMaterialCommand { get; }

        public EducatorViewModel(int educatorId)
        {
            this.educatorId = educatorId;
            LoadGroups();

            LoadPrograms();
            LoadAnnouncements();
            LoadMaterials();

            // Programe
            AddProgramCommand = new RelayCommand(_ => AddProgram());
            EditProgramCommand = new RelayCommand(_ => EditProgram(), _ => SelectedProgram != null);
            DeleteProgramCommand = new RelayCommand(_ => DeleteProgram(), _ => SelectedProgram != null);

            // Anunturi
            AddAnnouncementCommand = new RelayCommand(_ => AddAnnouncement());
            EditAnnouncementCommand = new RelayCommand(_ => EditAnnouncement(), _ => SelectedAnnouncement != null);
            DeleteAnnouncementCommand = new RelayCommand(_ => DeleteAnnouncement(), _ => SelectedAnnouncement != null);

            // Materiale
            AddMaterialCommand = new RelayCommand(_ => AddMaterial());
            EditMaterialCommand = new RelayCommand(_ => EditMaterial(), _ => SelectedMaterial != null);
            DeleteMaterialCommand = new RelayCommand(_ => DeleteMaterial(), _ => SelectedMaterial != null);

        }

        private void LoadGroups()
        {
            EducatorGroups.Clear();
            foreach (var group in DatabaseManager.GetGroupsByEducatorId(educatorId))
                EducatorGroups.Add(group);
        }

        private void LoadPrograms()
        {
            Programs.Clear();
            foreach (var p in DatabaseManager.GetProgramsByEducator(educatorId))
                Programs.Add(p);

            // Actualizează selecția
            SelectedProgram = Programs.FirstOrDefault();
            OnPropertyChanged(nameof(SelectedProgram));
        }


        private void LoadMaterials()
        {
            Materials.Clear();
            foreach (var m in DatabaseManager.GetMaterialsByEducator(educatorId))
                Materials.Add(m);
        }

        private void LoadAnnouncements()
        {
            Announcements.Clear();
            foreach (var a in DatabaseManager.GetAnnouncementsByEducator(educatorId))
                Announcements.Add(a);
        }

        private void AddProgram()
        {
            var program = new Program
            {
                GroupId = SelectedGroupIdForProgram,
                Date = ProgramDate,
                ActivityName = ProgramActivityName
            };
            DatabaseManager.AddProgram(program);
            LoadPrograms();

            // Alege ultimul program adăugat ca selecție
            SelectedProgram = Programs.LastOrDefault();
            OnPropertyChanged(nameof(SelectedProgram));
        }


        private void EditProgram()
        {
            if (SelectedProgram != null)
            {
                DatabaseManager.UpdateProgram(SelectedProgram);
                LoadPrograms();
            }
        }

        private void DeleteProgram()
        {
            if (SelectedProgram != null)
            {
                DatabaseManager.DeleteProgram(SelectedProgram.Id);
                LoadPrograms();
            }
        }

        private void AddAnnouncement()
        {
            var announcement = new Announcement
            {
                EducatorId = educatorId,
                Title = AnnouncementTitle,
                Description = AnnouncementDescription
            };
            DatabaseManager.AddAnnouncement(announcement);
            LoadAnnouncements();
        }

        private void EditAnnouncement()
        {
            if (SelectedAnnouncement != null)
            {
                DatabaseManager.UpdateAnnouncement(SelectedAnnouncement);
                LoadAnnouncements();
            }
        }

        private void DeleteAnnouncement()
        {
            if (SelectedAnnouncement != null)
            {
                DatabaseManager.DeleteAnnouncement(SelectedAnnouncement.Id);
                LoadAnnouncements();
            }
        }

        private void AddMaterial()
        {
            var material = new Material
            {
                GroupId = SelectedGroupIdForMaterial,
                FileName = MaterialFileName,
                FilePath = MaterialFilePath
            };
            DatabaseManager.AddMaterial(material);
            LoadMaterials();
        }

        private void EditMaterial()
        {
            if (SelectedMaterial != null)
            {
                DatabaseManager.UpdateMaterial(SelectedMaterial);
                LoadMaterials();
            }
        }

        private void DeleteMaterial()
        {
            if (SelectedMaterial != null)
            {
                DatabaseManager.DeleteMaterial(SelectedMaterial.Id);
                LoadMaterials();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
