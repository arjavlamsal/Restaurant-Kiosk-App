# Restaurant-Kiosk-App

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

---

## 🌟 Overview

The **Restaurant-Kiosk-App** is a modern, cross-platform restaurant ordering system built with **.NET MAUI**. Designed specifically for kiosk-style deployment, it provides a seamless and intuitive touch-friendly interface for customers to browse menus, customize orders with a unique combo builder, manage their cart, and complete the checkout process. With built-in persistence using SQLite, this app is a robust solution for self-service ordering in various food service environments.

---

## ✨ Key Features

* **Comprehensive Menu Management:**
    * Define and categorize menu items (Entree, Side, Drink, Dessert, Appetizer) with customizable properties like name, price, size options, and associated icons.
    * Full **CRUD** (Create, Read, Update, Delete) operations are supported for menu items.
    * An administrative dashboard allows filtering and searching of menu items by type, size, and name.
    * Includes a default menu seeded on the first application run for quick setup.

* **Intuitive Combo Builder:**
    * Empower users to create custom meal combos by selecting an entree, side, and drink.
    * Dynamically update the combo price as selections are made.
    * Support size selection for eligible items within the combo.

* **Effortless Cart System:**
    * Easily add individual items or custom combos to the user's cart.
    * A persistently visible cart icon indicates the number of items, with a toggle to view cart details.
    * Allows users to modify quantities, remove items, or clear the entire cart.

* **Streamlined Checkout Process:**
    * Present a clear summary of the order, including subtotal, tax, and the option to add a tip.
    * Calculate and display the updated total cost including the tip.
    * Provide clear options to submit or cancel the order.

* **Seamless Navigation:**
    * Utilizes a clean navigation stack for smooth transitions between different sections: Kiosk Start, Main Menu, Combo Builder, Item Add/Edit, Menu Dashboard, and Checkout.

* **Reliable Persistence:**
    * Leverages **SQLite** for efficient local data storage.
    * All menu configurations and cart contents are persistently stored.
    * Database and necessary controllers are initialized automatically on application startup.

* **Cross-Platform Compatibility:**
    * Developed with **.NET MAUI**, enabling deployment on a wide range of platforms including Android, iOS, Windows, Mac Catalyst, and Tizen.
    * Includes platform-specific entry points and initialization routines.

---

## 🏗️ Main Components

The application is structured around the following key components:

* **Models:** Data structures defining `MenuItem`, `Combo`, `CartItem`, and relevant enums (`ItemType`, `ItemSize`).
* **Controllers:** Implement the core business logic for menu management and cart operations, including interactions with the database.
* **Views:** The user interface, built using XAML pages and their corresponding code-behind files (e.g., `MainMenuPage`, `ComboBuilderPage`, `CheckoutPage`).
* **Services:** Provides essential services like the `DatabaseService` for handling SQLite CRUD operations.
* **AppShell & App:** Handle the application's overall structure, navigation setup, and the initialization of global controllers and services.

---

## 🚶 Typical User Flow

1.  **Start Order:** The customer initiates the ordering process from the Kiosk Start page.
2.  **Browse Menu:** The customer navigates to the Main Menu to view available food and drink items. They can add individual items to their cart.
3.  **Build Combo (Optional):** The customer can choose to build a custom combo meal by selecting an entree, side, and drink.
4.  **Cart Management:** The customer reviews their selected items in the cart, making any necessary modifications (removing items, etc.).
5.  **Checkout:** The customer proceeds to the Checkout page, reviews the order summary, adds a tip if desired, and submits or cancels the order.

---

## 🛠️ Technical Stack

* **.NET MAUI:** For building the cross-platform native UI.
* **SQLite:** For local data persistence.
* **MVVM-like Structure:** Utilizing controllers and observable collections for data binding and separation of concerns.
* **Async/Await:** Employed for non-blocking database operations and UI updates.

---

## 🎯 Intended Use

This application is primarily designed for deployment as a self-service ordering kiosk in various food service establishments such as restaurants, cafes, and cafeterias. It empowers customers to place orders independently, reducing the burden on staff. The architecture is designed to be extensible, allowing for potential future integrations with Point of Sale (POS) systems, kitchen display systems, or remote ordering platforms.

---

**In Summary:** The Restaurant-Kiosk-App is a robust, user-friendly, and extensible self-service ordering system powered by modern .NET technologies, offering comprehensive menu management, intuitive combo building, and reliable data persistence for a seamless customer experience.
