using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.LocalDBModel
{
    public class accountsLoggedIn
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [MaxLength(250)]
        public string userid { get; set; }
        [MaxLength(250)]
        public string username { get; set; }
        [MaxLength(250)]
        public string userPassword { get; set; }
        [MaxLength(250)]
        public string fullName { get; set; }
        [MaxLength(250)]
        public string isLoggedIn { get; set; }
    }
}
