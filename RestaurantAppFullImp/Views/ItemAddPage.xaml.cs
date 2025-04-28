/*
I acknowledge the following statements:
1. That the code I provide below is my own work and NOT copied from any outside resource, which includes, but not limited to, an artificial intelligence program unless given explicit permission by the instructor.
2. That the code I provide below is my own work and NOT the work of my peers, tutors, or any other individual unless given explicit permission by the instructor.
3. That if the code below is in violation of statements 1 and 2 above, I may be reported to the Academic Integrity office and subject to penalties as described in the Academic Integrity Policy.
Your Name: Arjav Lamsal
Your Student ID: w10195547
*/

namespace RestaurantAppFullImp.Project.Views;
using Project.Models;
using System.Collections.ObjectModel;

/*
    Hint: A class called ItemSelectView is defined that "wraps" around the data model and provides properties 
    that are referenced in the collection view's data template.  The collection view should be an observable
    list of ItemSelectView objects that bind to the menu items being shown.
*/


public partial class ItemAddPage : ContentPage
{
	// Page attributes here
	private MenuItemType _itemType;
	private ObservableCollection<ItemSelectView> _itemViews;
	private ObservableCollection<SizeTypeView> _sizeViews;
	private MenuItem? _selectedItem;
	private MenuSizeType _selectedSize = MenuSizeType.SMALL;

    public ItemAddPage(MenuItemType type)
	{
		InitializeComponent();
		
        // Store the item type
        _itemType = type;
        
        // Set page title based on item type
        switch (_itemType)
        {
            case MenuItemType.ENTREE:
                Title = "Add an Entrée";
                break;
            case MenuItemType.SIDE:
                Title = "Add a Side";
                break;
            case MenuItemType.DRINK:
                Title = "Add a Drink";
                break;
            case MenuItemType.DESSERT:
                Title = "Add a Dessert";
                break;
            default:
                Title = "Add an Item";
                break;
        }
        
        // Filter items by type from menu controller
        var menuItems = App.Menu.GetItems(_itemType);
        
        // Create ItemSelectView collection for the items
        _itemViews = new ObservableCollection<ItemSelectView>();
        foreach (var item in menuItems)
        {
            // Make a copy of the item to avoid modifying the original
            MenuItem menuItem = item.DeepCopy();
            _itemViews.Add(new ItemSelectView { Item = menuItem });
        }
        
        // Set the items source for the CollectionView
        collItemSelection.ItemsSource = _itemViews;
        
        // Create size options collection
        _sizeViews = new ObservableCollection<SizeTypeView>
        {
            new SizeTypeView { Text = "Small", Rate = "+0%" },
            new SizeTypeView { Text = "Medium", Rate = $"+{MenuItem.MEDIUM_RATE * 100}%" },
            new SizeTypeView { Text = "Large", Rate = $"+{MenuItem.LARGE_RATE * 100}%" }
        };
        
        // Set the size options source
        collSizeSelection.ItemsSource = _sizeViews;
    }

    private void SelectItem(object sender, SelectionChangedEventArgs e)
    {
        // Check if an item was selected
        if (e.CurrentSelection.Count > 0)
        {
            // Get the selected item view
            ItemSelectView selectedView = e.CurrentSelection[0] as ItemSelectView;
            
            if (selectedView != null)
            {
                // Store the selected item
                _selectedItem = selectedView.Item;
                
                // Show size options only if the item has size options
                layoutSizeSelectView.IsVisible = _selectedItem.HasSize;
                
                // Reset size to small when a new item is selected
                _selectedSize = MenuSizeType.SMALL;
                collSizeSelection.SelectedItem = null;
            }
        }
    }

    private void SelectSize(object sender, SelectionChangedEventArgs e)
    {
        // Check if a size was selected and we have a selected item
        if (e.CurrentSelection.Count > 0 && _selectedItem != null)
        {
            // Get the selected size view
            SizeTypeView selectedSizeView = e.CurrentSelection[0] as SizeTypeView;
            
            if (selectedSizeView != null)
            {
                // Set the size based on selection
                if (selectedSizeView.Text == "Small")
                {
                    _selectedSize = MenuSizeType.SMALL;
                }
                else if (selectedSizeView.Text == "Medium")
                {
                    _selectedSize = MenuSizeType.MEDIUM;
                }
                else if (selectedSizeView.Text == "Large")
                {
                    _selectedSize = MenuSizeType.LARGE;
                }
            }
        }
    }

    private async void AddItemClicked(object sender, EventArgs e)
    {
        // Check if an item has been selected
        if (_selectedItem == null)
        {
            // Show an alert if no item is selected
            await DisplayAlert("Selection Required", "Please select an item to add to your cart.", "OK");
            return;
        }
        
        // Set the size if the item has size options
        if (_selectedItem.HasSize)
        {
            _selectedItem.Size = _selectedSize;
        }
        
        // Use the Cart controller to add the item to the cart
        App.Cart.AddItem(_selectedItem);
        
        // Show confirmation message
        await DisplayAlert("Item Added", $"{_selectedItem.ItemName} has been added to your cart.", "OK");
        
        // Navigate directly to MainMenuPage
        Application.Current.MainPage = new MainMenuPage();
    }
}