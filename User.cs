using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Bunifu
{
    public class User
    {
        private string lastname;
        private string username;
        private string password;
        private string email;
        private string description;
        private int type;
        private int id;

        public string Lastname { get => lastname; set => lastname = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public int Type { get => type; set => type = value; }
        public string Description { get => description; set => description = value; }
        public int Id { get => id; set => id = value; }

        public User(DataRow row)
        {
            this.id = (int)row["id"];
            this.Lastname = row["lastName"].ToString();
            this.Username = row["userName"].ToString();
            this.password = row["password"].ToString();
            this.email = row["email"].ToString();
            this.description = row["userDescription"].ToString();
            this.type = (int)row["type"];
        }
    }
}
