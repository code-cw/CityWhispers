using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SQLite;

namespace CityWhispers
{
    public class WhisperDatabase
    {
        readonly SQLiteAsyncConnection database;

        public WhisperDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Whisper>().Wait();
        }

        public Task<List<Whisper>> GetWhispersAsync()
        {
            return database.Table<Whisper>().ToListAsync();
        }

        public Task<List<Whisper>> GetWhispersNotAnonymousAsync()
        {
            return database.QueryAsync<Whisper>("SELECT * FROM [Whisper] WHERE [Anonymous] = 0");
        }

        public Task<List<Whisper>> GetWhispersNearby(double latitude, double longitude)
        {
            return database.QueryAsync<Whisper>("SELECT * FROM [Whisper] WHERE [DISATANCE([WHISPER.LATITUDE]," +
                                                "[WHISPER.LONGITUDE], [LATITUDE], [LONGITUDE])] < 25");
        }

        public Task<Whisper> GetWhisperAsync(int id)
        {
            return database.Table<Whisper>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveWhisperAsync(Whisper item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteWhisperAsync(Whisper item)
        {
            return database.DeleteAsync(item);
        }

        public double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2deg(dist);
            dist = dist * 1609.344;

            return (dist);
        }

        public double Deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        public double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
