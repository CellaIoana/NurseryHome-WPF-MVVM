using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace NurseryHome.Models
{
    public static class DatabaseManager
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["CresaDB"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void AddChild(Child child)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "EXEC sp_AddChild @FullName, @DateOfBirth, @GroupId, @ParentId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FullName", child.FullName);
            cmd.Parameters.AddWithValue("@DateOfBirth", child.DateOfBirth);
            cmd.Parameters.AddWithValue("@GroupId", child.GroupId);
            cmd.Parameters.AddWithValue("@ParentId", child.ParentId);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateChild(Child child)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "UPDATE Children SET FullName=@FullName, DateOfBirth=@DateOfBirth, GroupId=@GroupId, ParentId=@ParentId WHERE Id=@Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FullName", child.FullName);
            cmd.Parameters.AddWithValue("@DateOfBirth", child.DateOfBirth);
            cmd.Parameters.AddWithValue("@GroupId", child.GroupId);
            cmd.Parameters.AddWithValue("@ParentId", child.ParentId);
            cmd.Parameters.AddWithValue("@Id", child.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteChild(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM Children WHERE Id=@Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public static List<Child> GetChildren()
        {
            List<Child> children = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Children";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                children.Add(new Child
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    DateOfBirth = reader.GetDateTime(2),
                    GroupId = reader.GetInt32(3),
                    ParentId = reader.GetInt32(4)
                });
            }
            return children;
        }

        public static List<User> GetParents()
        {
            List<User> parents = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Users WHERE Role = 'parinte'";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                parents.Add(new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Role = reader.GetString(reader.GetOrdinal("Role"))
                });
            }
            return parents;
        }

        public static void AddParent(User parent)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "INSERT INTO Users (FullName, Email, Password, Phone, Address, Role) VALUES (@FullName, @Email, @Password, @Phone, @Address, @Role)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FullName", parent.FullName);
            cmd.Parameters.AddWithValue("@Email", parent.Email);
            cmd.Parameters.AddWithValue("@Password", parent.Password);
            cmd.Parameters.AddWithValue("@Phone", parent.Phone);
            cmd.Parameters.AddWithValue("@Address", parent.Address);
            cmd.Parameters.AddWithValue("@Role", "parinte");
            cmd.ExecuteNonQuery();
        }

        public static void UpdateParent(User parent)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "UPDATE Users SET FullName = @FullName, Email = @Email, Password = @Password, Phone = @Phone, Address = @Address WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FullName", parent.FullName);
            cmd.Parameters.AddWithValue("@Email", parent.Email);
            cmd.Parameters.AddWithValue("@Password", parent.Password);
            cmd.Parameters.AddWithValue("@Phone", parent.Phone);
            cmd.Parameters.AddWithValue("@Address", parent.Address);
            cmd.Parameters.AddWithValue("@Id", parent.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteParent(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM Users WHERE Id = @Id AND Role = 'parinte'";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public static List<ParentEducator> GetParentEducatorLinks()
        {
            List<ParentEducator> links = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM ParentEducator";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                links.Add(new ParentEducator
                {
                    Id = reader.GetInt32(0),
                    ParentId = reader.GetInt32(1),
                    EducatorId = reader.GetInt32(2)
                });
            }
            return links;
        }

        public static void AddParentEducatorLink(int parentId, int educatorId)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "INSERT INTO ParentEducator (ParentId, EducatorId) VALUES (@ParentId, @EducatorId)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ParentId", parentId);
            cmd.Parameters.AddWithValue("@EducatorId", educatorId);
            cmd.ExecuteNonQuery();
        }


        public static void DeleteParentEducatorLink(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM ParentEducator WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public static List<User> GetUsersByRole(string role)
        {
            List<User> users = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Users WHERE Role = @Role";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Role", role);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Role = reader.GetString(reader.GetOrdinal("Role"))
                });
            }
            return users;
        }

        public static void AddEducator(User educator)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "INSERT INTO Users (FullName, Email, Password, Phone, Address, Role) " +
                           "VALUES (@FullName, @Email, @Password, @Phone, @Address, @Role)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FullName", educator.FullName);
            cmd.Parameters.AddWithValue("@Email", educator.Email);
            cmd.Parameters.AddWithValue("@Password", educator.Password);
            cmd.Parameters.AddWithValue("@Phone", educator.Phone);
            cmd.Parameters.AddWithValue("@Address", educator.Address);
            cmd.Parameters.AddWithValue("@Role", "educator");
            cmd.ExecuteNonQuery();
        }

        public static void UpdateEducator(User educator)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "UPDATE Users SET FullName = @FullName, Email = @Email, Password = @Password, " +
                           "Phone = @Phone, Address = @Address WHERE Id = @Id AND Role = 'educator'";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FullName", educator.FullName);
            cmd.Parameters.AddWithValue("@Email", educator.Email);
            cmd.Parameters.AddWithValue("@Password", educator.Password);
            cmd.Parameters.AddWithValue("@Phone", educator.Phone);
            cmd.Parameters.AddWithValue("@Address", educator.Address);
            cmd.Parameters.AddWithValue("@Id", educator.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteEducator(int educatorId)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM Users WHERE Id = @Id AND Role = 'educator'";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", educatorId);
            cmd.ExecuteNonQuery();
        }

        // Obține toate grupele
        public static List<Group> GetGroups()
        {
            List<Group> groups = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Groups";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                groups.Add(new Group
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    EducatorId = reader.GetInt32(2)
                });
            }
            return groups;
        }

        // Adaugă grupă
        public static void AddGroup(Group group)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "INSERT INTO Groups (Name, EducatorId) VALUES (@Name, @EducatorId)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", group.Name);
            cmd.Parameters.AddWithValue("@EducatorId", group.EducatorId);
            cmd.ExecuteNonQuery();
        }

        // Actualizează grupă
        public static void UpdateGroup(Group group)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "UPDATE Groups SET Name = @Name, EducatorId = @EducatorId WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", group.Name);
            cmd.Parameters.AddWithValue("@EducatorId", group.EducatorId);
            cmd.Parameters.AddWithValue("@Id", group.Id);
            cmd.ExecuteNonQuery();
        }

        // Șterge grupă
        public static void DeleteGroup(int groupId)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM Groups WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", groupId);
            cmd.ExecuteNonQuery();
        }

        public static List<Child> GetChildrenByParentId(int parentId)
        {
            List<Child> children = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Children WHERE ParentId = @ParentId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ParentId", parentId);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                children.Add(new Child
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    DateOfBirth = reader.GetDateTime(2),
                    GroupId = reader.GetInt32(3),
                    ParentId = reader.GetInt32(4)
                });
            }
            return children;
        }

        public static List<Absence> GetAbsencesByChildId(int childId)
        {
            List<Absence> absences = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Absences WHERE ChildId = @ChildId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ChildId", childId);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                absences.Add(new Absence
                {
                    Id = reader.GetInt32(0),
                    ChildId = reader.GetInt32(1),
                    Date = reader.GetDateTime(2),
                    Reason = reader.GetString(3)
                });
            }
            return absences;
        }

        public static List<Payment> GetPaymentsByChildId(int childId)
        {
            List<Payment> payments = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Payments WHERE ChildId = @ChildId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ChildId", childId);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                payments.Add(new Payment
                {
                    Id = reader.GetInt32(0),
                    ChildId = reader.GetInt32(1),
                    Date = reader.GetDateTime(2),
                    Amount = reader.GetDecimal(3)
                });
            }
            return payments;
        }

        public static List<Announcement> GetAnnouncementsByGroupId(int groupId)
        {
            List<Announcement> announcements = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Announcements WHERE EducatorId IN (SELECT EducatorId FROM Groups WHERE Id = @GroupId)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                announcements.Add(new Announcement
                {
                    Id = reader.GetInt32(0),
                    EducatorId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3)
                });
            }
            return announcements;
        }

        public static User GetEducatorByGroupId(int groupId)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = @"SELECT u.* FROM Users u
                     INNER JOIN Groups g ON u.Id = g.EducatorId
                     WHERE g.Id = @GroupId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    Password = reader.GetString(2),
                    FullName = reader.GetString(3),
                    Phone = reader.GetString(4),
                    Address = reader.GetString(5),
                    Role = reader.GetString(6)
                };
            }
            return null!;
        }

        public static List<Group> GetGroupsByEducatorId(int educatorId)
        {
            List<Group> groups = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Groups WHERE EducatorId = @EducatorId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EducatorId", educatorId);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                groups.Add(new Group
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    EducatorId = reader.GetInt32(2)
                });
            }
            return groups;
        }

        public static List<Program> GetProgramsByEducator(int educatorId)
        {
            List<Program> programs = new();
            using var conn = GetConnection();
            conn.Open();
            string query = @"SELECT p.* FROM Programs p
                     JOIN Groups g ON p.GroupId = g.Id
                     WHERE g.EducatorId = @EducatorId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@EducatorId", educatorId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                programs.Add(new Program
                {
                    Id = reader.GetInt32(0),
                    GroupId = reader.GetInt32(1),
                    Date = reader.GetDateTime(2),
                    ActivityName = reader.GetString(3)
                });
            }
            return programs;
        }

        public static List<Material> GetMaterialsByEducator(int educatorId)
        {
            List<Material> materials = new();
            using var conn = GetConnection();
            conn.Open();
            string query = @"SELECT m.* FROM Materials m
                     JOIN Groups g ON m.GroupId = g.Id
                     WHERE g.EducatorId = @EducatorId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@EducatorId", educatorId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                materials.Add(new Material
                {
                    Id = reader.GetInt32(0),
                    GroupId = reader.GetInt32(1),
                    FileName = reader.GetString(2),
                    FilePath = reader.GetString(3)
                });
            }
            return materials;
        }

        public static List<Announcement> GetAnnouncementsByEducator(int educatorId)
        {
            List<Announcement> announcements = new();
            using var conn = GetConnection();
            conn.Open();
            string query = "SELECT * FROM Announcements WHERE EducatorId = @EducatorId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@EducatorId", educatorId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                announcements.Add(new Announcement
                {
                    Id = reader.GetInt32(0),
                    EducatorId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.GetString(3)
                });
            }
            return announcements;
        }

        public static void AddProgram(Program program)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "INSERT INTO Programs (GroupId, Date, ActivityName) VALUES (@GroupId, @Date, @ActivityName)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@GroupId", program.GroupId);
            cmd.Parameters.AddWithValue("@Date", program.Date);
            cmd.Parameters.AddWithValue("@ActivityName", program.ActivityName);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateProgram(Program program)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "UPDATE Programs SET GroupId = @GroupId, Date = @Date, ActivityName = @ActivityName WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@GroupId", program.GroupId);
            cmd.Parameters.AddWithValue("@Date", program.Date);
            cmd.Parameters.AddWithValue("@ActivityName", program.ActivityName);
            cmd.Parameters.AddWithValue("@Id", program.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteProgram(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM Programs WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public static void AddAnnouncement(Announcement a)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "INSERT INTO Announcements (EducatorId, Title, Description) VALUES (@EducatorId, @Title, @Description)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EducatorId", a.EducatorId);
            cmd.Parameters.AddWithValue("@Title", a.Title);
            cmd.Parameters.AddWithValue("@Description", a.Description);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateAnnouncement(Announcement a)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "UPDATE Announcements SET Title = @Title, Description = @Description WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Title", a.Title);
            cmd.Parameters.AddWithValue("@Description", a.Description);
            cmd.Parameters.AddWithValue("@Id", a.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteAnnouncement(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM Announcements WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public static void AddMaterial(Material m)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "INSERT INTO Materials (GroupId, FileName, FilePath) VALUES (@GroupId, @FileName, @FilePath)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@GroupId", m.GroupId);
            cmd.Parameters.AddWithValue("@FileName", m.FileName);
            cmd.Parameters.AddWithValue("@FilePath", m.FilePath);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateMaterial(Material m)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "UPDATE Materials SET FileName = @FileName, FilePath = @FilePath WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FileName", m.FileName);
            cmd.Parameters.AddWithValue("@FilePath", m.FilePath);
            cmd.Parameters.AddWithValue("@Id", m.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteMaterial(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string query = "DELETE FROM Materials WHERE Id = @Id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

    }
}
