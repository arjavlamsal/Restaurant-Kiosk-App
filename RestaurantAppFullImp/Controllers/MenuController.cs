using System.Collections.ObjectModel;
using RestaurantAppFullImp.Project.Models;
using RestaurantAppFullImp.Project.Services;
using MenuItem = RestaurantAppFullImp.Project.Models.MenuItem;

namespace RestaurantAppFullImp.Project.Controllers
{
    public class MenuController
    {
        private List<MenuItem> _menuItems;
        private readonly DatabaseService _databaseService;

        public MenuController()
        {
            // Initialize the database service
            _databaseService = new DatabaseService();
            
            // Initialize with a temporary list until async data is loaded
            _menuItems = new List<MenuItem>();
            
            // Load items from database
            LoadItemsFromDatabase();
        }

        // Asynchronously load items from database
        private async void LoadItemsFromDatabase()
        {
            // First, initialize the database with default items if it's empty
            await _databaseService.InitializeDefaultMenuItems(GetDefaultMenuItems());
            
            // Then load all items from the database
            _menuItems = await _databaseService.GetMenuItemsAsync();
        }

        // Get items with optional type filter (synchronous method for compatibility)
        public List<MenuItem> GetItems(MenuItemType? type = null)
        {
            if (type == null)
                return new List<MenuItem>(_menuItems);
            else
            {
                var result = from item in _menuItems
                             where item.Type == type
                             select item;

                return result.ToList();
            }
        }

        // Add a new menu item to the database
        public async Task<int> AddItemAsync(MenuItem item)
        {
            var result = await _databaseService.SaveMenuItemAsync(item);
            
            // Refresh the cached list
            _menuItems = await _databaseService.GetMenuItemsAsync();
            
            return result;
        }

        // Update an existing menu item
        public async Task<int> UpdateItemAsync(MenuItem item)
        {
            var result = await _databaseService.SaveMenuItemAsync(item);
            
            // Refresh the cached list
            _menuItems = await _databaseService.GetMenuItemsAsync();
            
            return result;
        }

        // Delete a menu item
        public async Task<int> DeleteItemAsync(MenuItem item)
        {
            var result = await _databaseService.DeleteMenuItemAsync(item);
            
            // Refresh the cached list
            _menuItems = await _databaseService.GetMenuItemsAsync();
            
            return result;
        }

        // Get a specific menu item by ID
        public async Task<MenuItem> GetItemAsync(int id)
        {
            return await _databaseService.GetMenuItemAsync(id);
        }

        // Get menu items filtered by name
        public async Task<List<MenuItem>> SearchItemsByNameAsync(string searchText)
        {
            return await _databaseService.SearchMenuItemsByNameAsync(searchText);
        }

        // Get menu items filtered by type
        public async Task<List<MenuItem>> GetItemsByTypeAsync(MenuItemType type)
        {
            return await _databaseService.GetMenuItemsByTypeAsync(type);
        }

        // Get menu items filtered by hasSize flag
        public async Task<List<MenuItem>> GetItemsByHasSizeAsync(bool hasSize)
        {
            return await _databaseService.GetMenuItemsByHasSizeAsync(hasSize);
        }

        // The default menu items to populate the database with initially
        private List<MenuItem> GetDefaultMenuItems()
        {
            return new List<MenuItem>()
            {
                // All your existing menu items from the setup_menu method
                new MenuItem { ItemName = "Seafood Alfredo", Type = MenuItemType.ENTREE, ItemPrice=15.95M},
                new MenuItem { ItemName = "Chicken Alfredo", Type = MenuItemType.ENTREE, ItemPrice=13.95M },
                new MenuItem { ItemName = "Chicken Picatta", Type = MenuItemType.ENTREE, ItemPrice=13.95M },
                new MenuItem { ItemName = "Turkey Club", Type = MenuItemType.ENTREE, ItemPrice=11.95M },
                new MenuItem { ItemName = "Lobster Pie", Type = MenuItemType.ENTREE, ItemPrice=19.95M },
                new MenuItem { ItemName = "Prime Rib", Type = MenuItemType.ENTREE, ItemPrice=20.95M },
                new MenuItem { ItemName = "Shrimp Scampi", Type = MenuItemType.ENTREE, ItemPrice=18.95M },
                new MenuItem { ItemName = "Turkey Dinner", Type = MenuItemType.ENTREE, ItemPrice=13.95M },
                new MenuItem { ItemName = "Stuffed Chicken", Type = MenuItemType.ENTREE, ItemPrice=14.95M },
                new MenuItem { ItemName = "Classic Fries", Type = MenuItemType.SIDE, ItemPrice=1.95M, HasSize=true },
                new MenuItem { ItemName = "Spicy Fries", Type = MenuItemType.SIDE, ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Mashed Potatoes", Type = MenuItemType.SIDE, ItemPrice=1.95M, HasSize=true },
                new MenuItem { ItemName = "Steamed Vegetables", Type = MenuItemType.SIDE, ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Garden Salad", Type = MenuItemType.SIDE, ItemPrice=2.95M },
                new MenuItem { ItemName = "Loaded Potatos", Type = MenuItemType.SIDE, ItemPrice=3.50M },
                new MenuItem { ItemName = "Mac and Cheese", Type = MenuItemType.SIDE , ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Soda", Type = MenuItemType.DRINK, ItemPrice=1.95M, HasSize=true },
                new MenuItem { ItemName = "Tea", Type = MenuItemType.DRINK, ItemPrice=1.50M, HasSize=true },
                new MenuItem { ItemName = "Coffee", Type = MenuItemType.DRINK, ItemPrice=1.25M, HasSize=true },
                new MenuItem { ItemName = "Mineral Water", Type = MenuItemType.DRINK, ItemPrice=2.95M, HasSize=true },
                new MenuItem { ItemName = "Juice", Type = MenuItemType.DRINK, ItemPrice=2.50M, HasSize=true },
                new MenuItem { ItemName = "Buffalo Wings", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Buffalo Fingers", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Potato Skins", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Nachos", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Mushrooms Caps", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Shrimp Cocktail", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Chips and Salsa", Type = MenuItemType.APPETIZER, ItemPrice=15.95M },
                new MenuItem { ItemName = "Apple Pie", Type = MenuItemType.DESSERT, ItemPrice=5.95M },
                new MenuItem { ItemName = "Sundae", Type = MenuItemType.DESSERT, ItemPrice=3.95M },
                new MenuItem { ItemName = "Carrot Cake", Type = MenuItemType.DESSERT, ItemPrice=5.95M },
                new MenuItem { ItemName = "Mud Pie", Type = MenuItemType.DESSERT , ItemPrice=4.95M },
                new MenuItem { ItemName = "Apple Crisp", Type = MenuItemType.DESSERT, ItemPrice=5.95M }
            };
        }
    }
}
