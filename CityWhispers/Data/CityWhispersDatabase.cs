using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SQLite;


namespace CityWhispers
{
    public class CityWhispersDatabase
    {
        readonly SQLiteAsyncConnection database;

        public CityWhispersDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Whisper>().Wait();
            database.CreateTableAsync<Profile>().Wait();
            database.CreateTableAsync<LoggedInProfile>().Wait();
            database.CreateTableAsync<WhisperAuthor>().Wait();
            database.CreateTableAsync<ClickedWhisper>().Wait();
        }

        public Task<List<Whisper>> GetWhispersAsync()
        {
            return database.Table<Whisper>().ToListAsync();
        }

        public async void DeleteExpiredWhispersAsync()
        {
            var whispers = await GetWhispersAsync();
            foreach(var whisper in whispers)
            {
                int now = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (whisper.TimeStampInt + 432e3 <= now){
                    await DeleteWhisperAsync(whisper);
                }
            }
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

        //public Task<int> DeleteWhisperAsyncID(int id)
        //{
        //    return database.DeleteAsync(id);
        //}

        //public Task<int> DeleteWhisperAsyncID(int id)
        //{
        //    return database.DeleteAsync(GetWhisperAsync(id));
        //}



        //Profile Functions
        public Task<List<Profile>> GetProfilesAsync()
        {
            return database.Table<Profile>().ToListAsync();
        }

        public Task<Profile> GetProfileAsync(int id)
        {
            return database.Table<Profile>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveProfileAsync(Profile item)
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

        public Task<int> DeleteProfileAsync(Profile item)
        {
            return database.DeleteAsync(item);
        }

        //public Task<int> DeleteProfileAsyncID(int id)
        //{
        //    return database.DeleteAsync(GetProfileAsync(id));
        //}



        //Logged In Profile Functions
        public Task<List<LoggedInProfile>> GetLoggedInAsync()
        {
            return database.Table<LoggedInProfile>().ToListAsync();
        }

        //public Task<LoggedInProfile> GetLoggedInAsync(int id)
        //{
        //    return database.Table<LoggedInProfile>().Where(i => i.ID == id).FirstOrDefaultAsync();
        //}

        public Task<int> SaveLoggedInAsync(LoggedInProfile item)
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

        public Task<int> DeleteLoggedInAsync(LoggedInProfile item)
        {
            return database.DeleteAsync(item);
        }



//******//WhisperAuthor Functions//***************************************************************//
        public Task<List<WhisperAuthor>> GetWhisperAuthorsAsync()
        {
            return database.Table<WhisperAuthor>().ToListAsync();
        }

        public Task<WhisperAuthor> GetWhisperAuthorAsync(int id)
        {
            return database.Table<WhisperAuthor>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        //public Task<WhisperAuthor> GetWhisperAuthorByAuthorIdAsync(int id)
        //{
        //    return database.Table<WhisperAuthor>().Where(i => i.ProfileID == id).ToListAsync();
        //}

        public Task<int> SaveWhisperAuthorAsync(WhisperAuthor item)
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

        public Task<int> DeleteWhisperAuthorAsync(WhisperAuthor item)
        {
            return database.DeleteAsync(item);
        }



//******//Clicked Whisper Functions//***************************************************************//
        public Task<List<ClickedWhisper>> GetClickedWhispersAsync()
        {
            return database.Table<ClickedWhisper>().ToListAsync();
        }

        public Task<ClickedWhisper> GetClickedWhisperAsync(int id)
        {
            return database.Table<ClickedWhisper>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveClickedWhisperAsync(ClickedWhisper item)
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

        public Task<int> DeleteClickedWhisperAsync(ClickedWhisper item)
        {
            return database.DeleteAsync(item);
        }
    }
}
