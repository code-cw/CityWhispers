using System;
using SQLite;

namespace CityWhispers
{
    public class WhisperAuthor
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string AuthorUsername { get; set; }
        public int TimeStampInt { get; set; }
    }
}