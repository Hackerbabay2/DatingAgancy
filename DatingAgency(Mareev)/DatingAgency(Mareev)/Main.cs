using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DatingAgency_Mareev_
{
    public partial class Main : Form
    {
        private DataBase _dataBase;
        private User _user;
        private User _selectedUser;
        private Pasport _pasport;
        private string _path = "UserDataBase.xml";
        private List<User> _users = new List<User>();
        private List<User> _suggestions = new List<User>();

        public Main(DataBase dataBase, User user)
        {
            InitializeComponent();
            _user = user;
            _pasport = _user.PasportData;
            _dataBase = dataBase;
            _users = _dataBase.UserList;
            var result = _users.Union(_dataBase.UserList);

            foreach (var usr in result)
            {
                _suggestions.Add(usr);
            }
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog imageLoad = new OpenFileDialog();
            imageLoad.Filter = ".png (*.png*)|*.png*|.jpg (*.jpg*)|*.jpg*";
            imageLoad.ShowDialog();

            try
            {
                Image image = Image.FromFile(imageLoad.FileName);
                pictureBox1.Image = image;
                _user.ImagePath = imageLoad.FileName;
                File.Delete(_path);
                SerializeXML(_dataBase,_path);
            }
            catch (Exception)
            {
                MessageBox.Show("Выберете фотографию");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadData();
            SexCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ShowNextPartner();
            ShowUsers(_user.YouLiked,YouLikedListView);
            ShowUsers(_user.YouLike,YouLikeListView);
        }

        private void SaveUserData()
        {
            _user.FullName = FullNameBox.Text;
            _user.Sex = SexCombo.Text;
            _user.Address = AddressBox.Text;
            _user.RequirementPartner = requirementPartnerBox.Text;
            _user.AboutMe = AboutMeBox.Text;
            File.Delete(_path);
            SerializeXML(_dataBase,_path);
        }

        private void LoadData()
        {
            if (File.Exists(_user.ImagePath))
            {
                pictureBox1.Image = Image.FromFile(_user.ImagePath);
            }
            else
            {
                pictureBox1.Image = Image.FromFile("accountIco.png");
            }
            FullNameBox.Text = _user.FullName;
            SexCombo.Text = _user.Sex;
            AddressBox.Text = _user.Address;
            requirementPartnerBox.Text = _user.RequirementPartner;
            AboutMeBox.Text = _user.AboutMe;
            IssuedByMask.Text = _pasport.IssuedBy;
            DateIssuedMask.Text = _pasport.DateIssued;
            DateBirthMask.Text = _pasport.DateBirth;
            SeriesMask.Text = _pasport.Series;
            NumberMask.Text = _pasport.Number;
        }

        private void SaveDataButton_Click(object sender, EventArgs e)
        {
            SaveUserData();
        }

        private void SavePassportDetailsButton_Click(object sender, EventArgs e)
        {
            _pasport.SetData(IssuedByMask.Text,DateIssuedMask.Text,DateBirthMask.Text,SeriesMask.Text,NumberMask.Text);
            File.Delete(_path);
            SerializeXML(_dataBase,_path);
        }

        private void ShowLoginButton_Click(object sender, EventArgs e)
        {
            LoginTextBox.Text = _user.Login;
        }

        private void ShowPasswordButton_Click(object sender, EventArgs e)
        {
            PasswordTextBox.Text = _user.Password;
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text.Length >= 8)
            {
                if (PasswordTextBox.Text != _user.Password)
                {
                    _user.Password = PasswordTextBox.Text;
                    File.Delete(_path);
                    SerializeXML(_dataBase,_path);
                }
                else
                {
                    MessageBox.Show("Напишите другой пароль");
                }
            }
            else
            {
                MessageBox.Show("Пароль слишком короткий");
            }
        }

        private void SerializeXML(DataBase database, string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataBase));

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fileStream, database);
            }
        }

        private void SearchYouLiketextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                _dataBase.ShowDataByAttribute(_user.YouLike,SearchYouLiketextBox.Text,YouLikeListView);
            }
        }

        private void searchYouLikedTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void searchYouLikedTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                _dataBase.ShowDataByAttribute(_user.YouLiked, searchYouLikedTextBox.Text, YouLikedListView);
            }
        }

        private void ShowNextPartner()
        {
            if (_user.Sex == "Женский")
            {
                if (_users.Count > 1)
                {
                    try
                    {
                        _selectedUser = _suggestions.First(u => u.Sex == "Мужской" && u != _user);
                        ShowDataPartner(_selectedUser);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Предложения с мужчинами закончились");
                        _selectedUser = null;
                    }
                }
                else
                {
                    MessageBox.Show("Предложения закончились");
                    _selectedUser = null;
                }
            }

            if (_user.Sex == "Мужской")
            {
                if (_users.Count > 1)
                {
                    try
                    {
                        _selectedUser = _suggestions.First(u => u.Sex == "Женский" && u != _user);
                        ShowDataPartner(_selectedUser);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Предложения с девушками закончились");
                        _selectedUser = null;
                    }
                }
                else
                {
                    MessageBox.Show("Предложения закончились");
                    _selectedUser = null;
                }
            }
        }

        private void ShowDataPartner(User user)
        {
            pictureBox2.Image = Image.FromFile(user.ImagePath);
            fullNameLabel.Text = $"ФИО: {user.FullName}";
            SexLabel.Text = $"Пол: {user.Sex}";
            addressLabel.Text = $"Адрес: {user.Address}";
            reqPartnerLabel.Text = $"Требования: {user.RequirementPartner}";
            richTextBox1.Text = $"О себе {user.AboutMe}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_selectedUser != null)
            {
                _user.AddYouLike(_selectedUser);
                _selectedUser.AddYouLiked(_user);
                File.Delete(_path);
                SerializeXML(_dataBase, _path);
                _suggestions.Remove(_selectedUser);
                ShowNextPartner();
                YouLikeListView.Items.Clear();
                ShowUsers(_user.YouLike,YouLikeListView);
            }
            else
            {
                MessageBox.Show("Предложения закончились");
            }
        }

        private void ShowUsers(List<string[]> users, ListView listView)
        {
            ListViewItem items = null;

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i] != null)
                {
                    items = new ListViewItem(new string[] { (i+1).ToString(),
                        users[i][0],
                        users[i][1],
                        users[i][2],
                        users[i][3],
                        users[i][4],
                    });
                    listView.Items.Add(items);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _suggestions.Remove(_selectedUser);
            ShowNextPartner();
        }

        private void YouLikeListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView lsw = (ListView)sender;

            if (e.Column == ListViewItemComparer.SortColumn)
            {
                if (ListViewItemComparer.Order == SortOrder.Ascending)
                {
                    ListViewItemComparer.Order = SortOrder.Descending;
                }
                else
                {
                    ListViewItemComparer.Order = SortOrder.Ascending;
                }
            }
            else
            {
                ListViewItemComparer.SortColumn = e.Column;
                ListViewItemComparer.Order = SortOrder.Ascending;
            }
            this.YouLikeListView.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }

        private void YouLikedListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView lsw = (ListView)sender;

            if (e.Column == ListViewItemComparer.SortColumn)
            {
                if (ListViewItemComparer.Order == SortOrder.Ascending)
                {
                    ListViewItemComparer.Order = SortOrder.Descending;
                }
                else
                {
                    ListViewItemComparer.Order = SortOrder.Ascending;
                }
            }
            else
            {
                ListViewItemComparer.SortColumn = e.Column;
                ListViewItemComparer.Order = SortOrder.Ascending;
            }
            this.YouLikedListView.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            YouLikeListView.Items.Clear();
            ShowUsers(_user.YouLike,YouLikeListView);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            YouLikedListView.Items.Clear();
            ShowUsers(_user.YouLiked,YouLikedListView);
        }
    }
}
