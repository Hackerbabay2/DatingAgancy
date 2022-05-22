using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DatingAgency_Mareev_
{
    [Serializable]
    public class DataBase
    {
        public List<User> UserList { get; set; } = new List<User>();

        public DataBase() { }

        public void ShowDataByAttribute(List<string[]> users, string attrubute, ListView listView)
        {
            var result = users.Where(u => u.Contains(attrubute)).ToList();
            ListViewItem items = null;

            if (result.Count > 0)
            {
                listView.Items.Clear();
                for(int i = 0; i < result.Count; i++)
                {
                    items = new ListViewItem(new string[]
                    {
                        (i+1).ToString(),
                        result[i][0],
                        result[i][1],
                        result[i][2],
                        result[i][3],
                        result[i][4]
                    });
                    listView.Items.Add(items);
                }
            }
            else
            {
                MessageBox.Show("Ничего не найдено");
            }
        }

        public ListViewItem GetListUser(User user)
        {
            ListViewItem item = null;

            item = new ListViewItem(new string[] 
            {
                user.FullName,
                user.Sex,
                user.Address,
                user.RequirementPartner,
                user.AboutMe
            });

            return item;
        }

        public User GetUser(string login, string password)
        {
            foreach (User user in UserList)
            {
                if (user.Login == login)
                {
                    if (user.Password == password)
                        return user;
                }
            }

            return null;
        }

        public bool CheckUser(string login, string password)
        {
            foreach (User user in UserList)
            {
                if (user.Login == login)
                {
                    if (user.Password == password)
                        return true;
                }
            }
            return false;
        }

        public bool CheckRepeatLogin(string login)
        {
            foreach (User user in UserList)
            {
                if (user.Login == login)
                    return true;
            }
            return false;
        }

        
        public bool TryAddUser(string login, string password)
        {
            if (!CheckRepeatLogin(login))
            {
                if (login != "" && password != "")
                {
                    if (password.Length > 8)
                    {
                        UserList.Add(new User(login, password));
                        MessageBox.Show("Вы зарегестрировались, входите!");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Пароль слишком короткий");
                        return false;
                    }
                }
                MessageBox.Show("Заполните все поля");
                return false;
            }
            else
            {
                MessageBox.Show("Этот логин уже занят");
                return false;
            }
        }
    }

    [Serializable]
    public class User
    {
        [XmlElement("User")]

        public string Login { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public string FullName { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string RequirementPartner { get; set; }
        public string AboutMe { get; set; }
        public Pasport PasportData { get; set; }
        public List<string[]> YouLiked { get; set; } = new List<string[]>();
        public List<string[]> YouLike { get; set; } = new List<string[]>();

        public User() { }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
            PasportData = new Pasport();
        }

        public void SetVisibleData(string image, string fullName, string sex, string address, string requirementParther, string aboutMe)
        {
            ImagePath = image;
            FullName = fullName;
            Sex = sex;
            Address = address;
            RequirementPartner = requirementParther;
            AboutMe = aboutMe;
            ImagePath = "accountIco.png";
        }

        public void AddYouLiked(User user)
        {
            string[] userData = new string[]{ user.FullName,user.Sex, user.Address, user.RequirementPartner, user.AboutMe };
            YouLiked.Add(userData);
        }

        public void AddYouLike(User user)
        {
            string[] userData = new string[] { user.FullName, user.Sex, user.Address, user.RequirementPartner, user.AboutMe };
            YouLike.Add(userData);
        }

        public void SetPasportData(string issuedBy, string dateIssued, string dateBirth, string series, string number)
        {
            PasportData.SetData(issuedBy,dateIssued,dateBirth,series,number);
        }
    }

    [Serializable]
    public class Pasport
    {
        [XmlElement("Pasport")]

        public string IssuedBy { get; set; }
        public string DateIssued { get; set; }
        public string DateBirth { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }

        public Pasport() { }

        public Pasport(string issuedBy, string dateIssued, string dateBirth, string series, string number)
        {
            IssuedBy = issuedBy;
            DateIssued = dateIssued;
            DateBirth = dateBirth;
            Series = series;
            Number = number;
        }

        public void SetData(string issuedBy, string dateIssued, string dateBirth, string series, string number)
        {
            IssuedBy = issuedBy;
            DateIssued = dateIssued;
            DateBirth = dateBirth;
            Series = series;
            Number = number;
        }
    }
}
