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

        //public Task<List<Whisper>> GetWhispersNotAnonymousAsync()
        //{
        //    return database.QueryAsync<Whisper>("SELECT * FROM [Whisper] WHERE [Anonymous] = 0");
        //}

        public async void DeleteExpiredWhispersAsync()
        {
            var whispers = await GetWhispersAsync();
            foreach(var whisper in whispers)
            {
                int now = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalHours;
                if (whisper.TimeStampInt + 120 <= now){
                    await DeleteWhisperAsync(whisper);
                }
            }
        }

        //public Task<List<Whisper>> GetWhispersNearby(double latitude, double longitude)
        //{
        //    return database.QueryAsync<Whisper>("SELECT * FROM [Whisper] WHERE [DISATANCE([WHISPER.LATITUDE]," +
        //                                        "[WHISPER.LONGITUDE], [LATITUDE], [LONGITUDE])] < 25");
        //}

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
    }
}
