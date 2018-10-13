using System;
using SQLite;

namespace CityWhispers
{
    public class Whisper
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Time { get; set; }
        public bool Anonymous { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}
