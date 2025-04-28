using System.Collections.ObjectModel;
using RestaurantAppFullImp.Project.Models;
using MenuItem = RestaurantAppFullImp.Project.Models.MenuItem;

namespace RestaurantAppFullImp.Project.Views
{
    public partial class MenuDashboardPage : ContentPage
    {
        private ObservableCollection<MenuItem> _menuItems;
        private bool _isRefreshing;
        private MenuItemType? _selectedType = null;
        private bool? _selectedHasSize = null;
        private string _searchText = string.Empty;

        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public Command RefreshCommand { get; }
        public Command DeleteCommand { get; }
        public Command EditCommand { get; }
        public Command SearchCommand { get; }

        public MenuDashboardPage()
        {
            InitializeComponent();

            // Initialize collections
            MenuItems = new ObservableCollection<MenuItem>();

            // Initialize commands
            RefreshCommand = new Command(async () => await RefreshMenuItems());
            DeleteCommand = new Command<MenuItem>(async (item) => await DeleteMenuItem(item));
            EditCommand = new Command<MenuItem>(async (item) => await EditMenuItem(item));
            SearchCommand = new Command<string>(async (text) => await SearchMenuItems(text));

            // Set binding context
            BindingContext = this;
            menuItemsCollection.ItemsSource = MenuItems;

            // Load menu items
            LoadMenuItems();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadMenuItems();
        }

        private async void LoadMenuItems()
        {
            await RefreshMenuItems();
        }

        private async Task RefreshMenuItems()
        {
            IsRefreshing = true;

            try
            {
                MenuItems.Clear();

                List<MenuItem> items;

                // Apply filters if any
                if (!string.IsNullOrEmpty(_searchText))
                {
                    // Search by name
                    items = await App.Menu.SearchItemsByNameAsync(_searchText);
                }
                else if (_selectedType.HasValue)
                {
                    // Filter by type
                    items = await App.Menu.GetItemsByTypeAsync(_selectedType.Value);
                }
                else if (_selectedHasSize.HasValue)
                {
                    // Filter by size option
                    items = await App.Menu.GetItemsByHasSizeAsync(_selectedHasSize.Value);
                }
                else
                {
                    // Get all items
                    items = await App.Database.GetMenuItemsAsync();
                }

                foreach (var item in items)
                {
                    MenuItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load menu items: {ex.Message}", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task DeleteMenuItem(MenuItem item)
        {
            if (item == null)
                return;

            bool confirm = await DisplayAlert("Confirm Delete", 
                $"Are you sure you want to delete '{item.ItemName}'?", "Yes", "No");

            if (confirm)
            {
                try
                {
                    await App.Menu.DeleteItemAsync(item);
                    await RefreshMenuItems();
                    await DisplayAlert("Success", "Menu item deleted successfully.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete menu item: {ex.Message}", "OK");
                }
            }
        }

        private async Task EditMenuItem(MenuItem item)
        {
            if (item == null)
                return;

            // Navigate to edit page with the selected item
            await ShowItemEditor(item);
        }

        private async void OnAddItemClicked(object sender, EventArgs e)
        {
            // Navigate to add page with a new item
            await ShowItemEditor(new MenuItem());
        }

        private async Task ShowItemEditor(MenuItem item)
        {
            // Create a popup for editing the menu item
            string title = item.DatabaseID == 0 ? "Add New Menu Item" : "Edit Menu Item";

            string result = await DisplayPromptAsync(title, "Item Name:", initialValue: item.ItemName);
            if (string.IsNullOrEmpty(result))
                return;

            item.ItemName = result;

            string priceStr = await DisplayPromptAsync(title, "Price:", initialValue: item.ItemPrice.ToString("F2"));
            if (decimal.TryParse(priceStr, out decimal price))
            {
                item.ItemPrice = price;
            }

            string[] typeOptions = Enum.GetNames(typeof(MenuItemType));
            string typeResult = await DisplayActionSheet("Select Type", "Cancel", null, typeOptions);
            if (typeResult != "Cancel" && Enum.TryParse<MenuItemType>(typeResult, out MenuItemType type))
            {
                item.Type = type;
            }

            bool hasSizeResult = await DisplayAlert("Size Options", "Does this item have size options?", "Yes", "No");
            item.HasSize = hasSizeResult;

            if (hasSizeResult)
            {
                string[] sizeOptions = Enum.GetNames(typeof(MenuSizeType));
                string sizeResult = await DisplayActionSheet("Default Size", "Cancel", null, sizeOptions);
                if (sizeResult != "Cancel" && Enum.TryParse<MenuSizeType>(sizeResult, out MenuSizeType size))
                {
                    item.Size = size;
                }
            }

            try
            {
                if (item.DatabaseID == 0)
                {
                    await App.Menu.AddItemAsync(item);
                    await DisplayAlert("Success", "Menu item added successfully.", "OK");
                }
                else
                {
                    await App.Menu.UpdateItemAsync(item);
                    await DisplayAlert("Success", "Menu item updated successfully.", "OK");
                }

                await RefreshMenuItems();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save menu item: {ex.Message}", "OK");
            }
        }

        private async Task SearchMenuItems(string searchText)
        {
            _searchText = searchText;
            _selectedType = null;
            _selectedHasSize = null;
            
            typePicker.SelectedIndex = 0;
            sizePicker.SelectedIndex = 0;
            
            await RefreshMenuItems();
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                _searchText = string.Empty;
                await RefreshMenuItems();
            }
        }

        private async void OnTypeFilterChanged(object sender, EventArgs e)
        {
            int index = typePicker.SelectedIndex;
            
            if (index == 0)
            {
                // "All Types" selected
                _selectedType = null;
            }
            else
            {
                // Convert 1-based index to enum value (0-based)
                _selectedType = (MenuItemType)(index - 1);
            }
            
            _searchText = string.Empty;
            _selectedHasSize = null;
            
            searchBar.Text = string.Empty;
            sizePicker.SelectedIndex = 0;
            
            await RefreshMenuItems();
        }

        private async void OnSizeFilterChanged(object sender, EventArgs e)
        {
            int index = sizePicker.SelectedIndex;
            
            if (index == 0)
            {
                // "All Items" selected
                _selectedHasSize = null;
            }
            else if (index == 1)
            {
                // "Has Size Options" selected
                _selectedHasSize = true;
            }
            else
            {
                // "No Size Options" selected
                _selectedHasSize = false;
            }
            
            _searchText = string.Empty;
            _selectedType = null;
            
            searchBar.Text = string.Empty;
            typePicker.SelectedIndex = 0;
            
            await RefreshMenuItems();
        }

        private async void OnClearFiltersClicked(object sender, EventArgs e)
        {
            _searchText = string.Empty;
            _selectedType = null;
            _selectedHasSize = null;
            
            searchBar.Text = string.Empty;
            typePicker.SelectedIndex = 0;
            sizePicker.SelectedIndex = 0;
            
            await RefreshMenuItems();
        }

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            // Clear selection
            menuItemsCollection.SelectedItem = null;
        }

        private async void OnBackToMenuClicked(object sender, EventArgs e)
        {
            // Navigate back to the main menu
            await Navigation.PopAsync();
        }
    }
}