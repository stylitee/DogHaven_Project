using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.LocalDBModel
{
    public class SettingsData
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [MaxLength(250)]
        public string breedingKilometers { get; set; }
        [MaxLength(250)]
        public string breedingEstablishments { get; set; }
    }
}
