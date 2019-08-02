using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;

namespace FamilyMoneyLib.NetStandard.AddOn
{
    public class CreateCategoryTree
    {
        public static void CreateDefaultCategoryTree()
        {
            var storage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            storage.DeleteAllData();
            var car = storage.CreateCategory("🚗Car", "Car", 0, null);
            storage.CreateCategory("🅿Parking", "Parking", 0, car);
            storage.CreateCategory("💷Insurance", "Insurance",0,car);
            storage.CreateCategory("🛒Accessories", "Accessories",0,car);
            storage.CreateCategory("🚘Other", "Other",0,null);
            storage.CreateCategory("🛠Service","Service",0,car);
            storage.CreateCategory("⛽Fuel","Fuel",0,car);

            var relaxation = storage.CreateCategory("🎈Relaxation", "Relaxation", 0,null);
            storage.CreateCategory("🏅Sport","sport",0,relaxation);
            storage.CreateCategory("🎠Entertainment","Entertainment",0,relaxation);
            storage.CreateCategory("🏆Sport/Fitness","Sport/Fitness",0,relaxation);

            var home = storage.CreateCategory("🏡Home", "home", 0, null);
            storage.CreateCategory("📆Rent","Rent",0,home);
            storage.CreateCategory("🪑🪑 Furniture","Furniture",0,home);

            var health = storage.CreateCategory("👨‍⚕Healthcare", "Healthcare", 0, null);
            storage.CreateCategory("💊Medicine","Medicine",0,health);
            storage.CreateCategory("🏥Hospital","Hospital",0,health);
            storage.CreateCategory("🦷Dentist","Dentist",0,health);
            storage.CreateCategory("⚕Medical Insurance","Medical Insurance",0,health);
            storage.CreateCategory("💉Analysis","Analysis",0,health);

            var books = storage.CreateCategory("📚Books", "Books", 0, null);

            var bills = storage.CreateCategory("🗃Bills", "Bills", 0, null);
            storage.CreateCategory("🚿Water", "Water",0,bills);
            storage.CreateCategory("🔥Gas","Gas",0,bills);
            storage.CreateCategory("🔌Electricity","Electricity",0,bills);
            storage.CreateCategory("📡Internet","Internet",0,bills);
            storage.CreateCategory("📶Mobile","Mobile",0,bills);
            storage.CreateCategory("🏚Rental fee","Rental fee",0,bills);
            storage.CreateCategory("🥵Central Heating", "Central Heating", 0, bills);

            var clothes = storage.CreateCategory("👕Clothes", "Clothes", 0, null);
            var c00per = storage.CreateCategory("👔C00per clothes","c00per clothes",0,clothes);
            var gerda = storage.CreateCategory("🥻Gerda clothes", "Gerda clothes", 0, clothes);
            var sam = storage.CreateCategory("👗Sam clothes", "Sam clothes", 0, clothes);


            var food = storage.CreateCategory("🍝Food", "Food", 0, null);
            var meat = storage.CreateCategory("🥩Meat","Meat",0,food);
            var fish = storage.CreateCategory("🦈Fish", "Fish", 0, food);
            var vegetables = storage.CreateCategory("🍅Vegetables", "Vegetables", 0, food);
            var fruit = storage.CreateCategory("🍎Fruit", "Fruit", 0, food);
            var cereals = storage.CreateCategory("🥣Cereals", "Cereals", 0, food);

            var transport = storage.CreateCategory("🚇Transport", "Transport", 0, null);
            storage.CreateCategory("🚇Metro","Metro",0,transport);
            storage.CreateCategory("🚌Bus","Bus",0,transport);
            storage.CreateCategory("🚋Tram","Tram",0,transport);
            storage.CreateCategory("🚎Trolleybus","Trolleybus",0,transport);
            storage.CreateCategory("🚐Minibus","Minibus",0,transport);

            var cigarettes = storage.CreateCategory("🚬Cigarettes", "Cigarettes", 0, null);

            var journey = storage.CreateCategory("🌏Journey", "Journey", 0, null);
            storage.CreateCategory("🛌Hotels","Hotels",0,journey);
            storage.CreateCategory("🚙Car","Car",0,journey);
            storage.CreateCategory("🍜Food", "Food", 0, journey);
            storage.CreateCategory("☕Coffee","Coffee",0,journey);
            storage.CreateCategory("🏛Museum", "Museum",0,journey);
            storage.CreateCategory("🎌Souvenirs","Souvenirs",0,journey);
            storage.CreateCategory("✈Tickets","Tickets",0,journey);
            storage.CreateCategory("🏺Drinks","Drinks",0,journey);
            storage.CreateCategory("📔Books and Guides", "Books and Guides", 0, journey);

        }
    }
}
