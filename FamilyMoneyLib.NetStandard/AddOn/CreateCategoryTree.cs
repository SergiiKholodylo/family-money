using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages.SQLite;

namespace FamilyMoneyLib.NetStandard.AddOn
{
    public class CreateCategoryTree
    {
        public static void CreateDefaultCategoryTree()
        {
            var storage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            storage.DeleteAllData();
            var car = storage.CreateCategory("🚗 Car", "Car", 1, null);
            storage.CreateCategory("🅿 Parking", "Parking", 2, car);
            storage.CreateCategory("💷 Insurance", "Insurance",3,car);
            storage.CreateCategory("🛒 Accessories", "Accessories",4,car);
            storage.CreateCategory("🚘 Other", "Other",5,car);
            storage.CreateCategory("🛠 Service","Service",6,car);
            storage.CreateCategory("⛽ Fuel","Fuel",7,car);

            var relaxation = storage.CreateCategory("🎈 Relaxation", "Relaxation", 8,null);
            storage.CreateCategory("🏅 Sport","sport",9,relaxation);
            storage.CreateCategory("🎠 Entertainment","Entertainment",10,relaxation);
            storage.CreateCategory("🏆 Sport/Fitness","Sport/Fitness",11,relaxation);

            var home = storage.CreateCategory("🏡 Home", "home", 12, null);
            storage.CreateCategory("📆 Rent","Rent",13,home);
            storage.CreateCategory("🪑🪑 Furniture","Furniture",14,home);

            var health = storage.CreateCategory("👨‍⚕ Healthcare", "Healthcare", 15, null);
            storage.CreateCategory("💊 Medicine","Medicine",16,health);
            storage.CreateCategory("🏥 Hospital","Hospital",17,health);
            storage.CreateCategory("🦷 Dentist","Dentist",18,health);
            storage.CreateCategory("⚕ Medical Insurance","Medical Insurance",19,health);
            storage.CreateCategory("💉 Analysis","Analysis",20,health);

            var books = storage.CreateCategory("📚 Books", "Books", 21, null);

            var bills = storage.CreateCategory("🗃 Bills", "Bills", 22, null);
            storage.CreateCategory("🚿 Water", "Water",23,bills);
            storage.CreateCategory("🔥 Gas","Gas",24,bills);
            storage.CreateCategory("🔌 Electricity","Electricity",25,bills);
            storage.CreateCategory("📡 Internet","Internet",26,bills);
            storage.CreateCategory("📶 Mobile","Mobile",27,bills);
            storage.CreateCategory("🏚 Rental fee","Rental fee",28,bills);
            storage.CreateCategory("🥵 Central Heating", "Central Heating", 29, bills);

            var clothes = storage.CreateCategory("👕 Clothes", "Clothes", 30, null);
            var c00per = storage.CreateCategory("👔 C00per clothes","c00per clothes",31,clothes);
            var gerda = storage.CreateCategory("🥻 Gerda clothes", "Gerda clothes", 32, clothes);
            var sam = storage.CreateCategory("👗 Sam clothes", "Sam clothes", 33, clothes);


            var food = storage.CreateCategory("🍝 Food", "Food", 34, null);
            var meat = storage.CreateCategory("🥩 Meat","Meat",35,food);
            var fish = storage.CreateCategory("🦈 Fish", "Fish", 36, food);
            var vegetables = storage.CreateCategory("🍅 Vegetables", "Vegetables", 37, food);
            var fruit = storage.CreateCategory("🍎 Fruit", "Fruit", 38, food);
            var cereals = storage.CreateCategory("🥣 Cereals", "Cereals", 39, food);

            var transport = storage.CreateCategory("🚇 Transport", "Transport", 40, null);
            storage.CreateCategory("🚇 Metro","Metro",41,transport);
            storage.CreateCategory("🚌 Bus","Bus",42,transport);
            storage.CreateCategory("🚋 Tram","Tram",43,transport);
            storage.CreateCategory("🚎 Trolleybus","Trolleybus",44,transport);
            storage.CreateCategory("🚐 Minibus","Minibus",45,transport);

            var cigarettes = storage.CreateCategory("🚬 Cigarettes", "Cigarettes", 46, null);

            var journey = storage.CreateCategory("🌏 Journey", "Journey", 47, null);
            storage.CreateCategory("🛌 Hotels","Hotels",48,journey);
            storage.CreateCategory("🚙 Car","Car",49,journey);
            storage.CreateCategory("🍜 Food", "Food", 50, journey);
            storage.CreateCategory("☕ Coffee","Coffee",51,journey);
            storage.CreateCategory("🏛 Museum", "Museum",52,journey);
            storage.CreateCategory("🎌 Souvenirs","Souvenirs",53,journey);
            storage.CreateCategory("✈ Tickets","Tickets",54,journey);
            storage.CreateCategory("🏺 Drinks","Drinks",55,journey);
            storage.CreateCategory("📔 Books and Guides", "Books and Guides", 56, journey);

        }
    }
}
