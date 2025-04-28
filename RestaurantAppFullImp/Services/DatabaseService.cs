using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RestaurantAppFullImp.Project.Models;
using MenuItem = RestaurantAppFullImp.Project.Models.MenuItem;

namespace RestaurantAppFullImp.Project.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        private static readonly string _databasePath = Path.Combine(FileSystem.AppDataDirectory, "restaurant.db");
        
        public DatabaseService()
        {
            _database = new SQLiteAsyncConnection(_databasePath);
            _database.CreateTableAsync<MenuItem>().Wait();
        }

        // Get all menu items
        public async Task<List<MenuItem>> GetMenuItemsAsync()
        {
            return await _database.Table<MenuItem>().ToListAsync();
        }

        // Get menu items by type
        public async Task<List<MenuItem>> GetMenuItemsByTypeAsync(MenuItemType type)
        {
            return await _database.Table<MenuItem>()
                .Where(i => i.Type == type)
                .ToListAsync();
        }

        // Get menu items by HasSize
        public async Task<List<MenuItem>> GetMenuItemsByHasSizeAsync(bool hasSize)
        {
            return await _database.Table<MenuItem>()
                .Where(i => i.HasSize == hasSize)
                .ToListAsync();
        }

        // Get menu items by name (search functionality)
        public async Task<List<MenuItem>> SearchMenuItemsByNameAsync(string searchText)
        {
            return await _database.Table<MenuItem>()
                .Where(i => i.ItemName.ToLower().Contains(searchText.ToLower()))
                .ToListAsync();
        }

        // Get a specific menu item by ID
        public async Task<MenuItem> GetMenuItemAsync(int id)
        {
            return await _database.Table<MenuItem>()
                .Where(i => i.DatabaseID == id)
                .FirstOrDefaultAsync();
        }

        // Save a new or existing menu item
        public async Task<int> SaveMenuItemAsync(MenuItem item)
        {
            if (item.DatabaseID != 0)
            {
                return await _database.UpdateAsync(item);
            }
            else
            {
                return await _database.InsertAsync(item);
            }
        }

        // Delete a menu item
        public async Task<int> DeleteMenuItemAsync(MenuItem item)
        {
            return await _database.DeleteAsync(item);
        }

        // Initialize database with default menu items if it's empty
        public async Task InitializeDefaultMenuItems(List<MenuItem> defaultItems)
        {
            var count = await _database.Table<MenuItem>().CountAsync();
            
            if (count == 0)
            {
                foreach (var item in defaultItems)
                {
                    await _database.InsertAsync(item);
                }
            }
        }
    }
}