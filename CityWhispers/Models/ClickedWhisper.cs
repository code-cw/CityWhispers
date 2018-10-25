using System;
using SQLite;

namespace CityWhispers
{
    public class ClickedWhisper
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ClickerUsername { get; set; }
        public int TimeStampInt { get; set; }
    }
}
