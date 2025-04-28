namespace RestaurantAppFullImp
{
    using RestaurantAppFullImp.Project.Controllers;
    using RestaurantAppFullImp.Project.Services;

    public partial class App : Application
    {
        public static MenuController Menu;
        public static CartController Cart;
        public static DatabaseService Database;
        
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Create a NavigationPage with MainMenuPage as the root
            var navPage = new NavigationPage(new Project.Views.MainMenuPage());
            return new Window(navPage);
        }

        static App()
        {
            // Initialize database service first
            Database = new DatabaseService();
            
            // Initialize controllers
            Menu = new MenuController();
            Cart = new CartController();
        }
    }
}