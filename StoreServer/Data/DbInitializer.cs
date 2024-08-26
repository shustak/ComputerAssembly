using StoreServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreServer.Data;
using StoreServer.Models;
using System;
using System.Linq;

namespace StoreServer.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ComputerContext(
                serviceProvider.GetRequiredService<DbContextOptions<ComputerContext>>()))
            {
                // Удаление и создание базы данных с нуля при каждом запуске (для целей разработки)
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Проверка наличия процессоров
                if (context.Processors.Any())
                {
                    return; // База данных уже инициализирована
                }

                // Добавление процессоров
                context.Processors.AddRange(
                    new Processor { Model = "Intel Core i5", Socket = "LGA1200", Frequency = 2.9, Cores = 6, Price = 200 },
                    new Processor { Model = "AMD Ryzen 5", Socket = "AM4", Frequency = 3.6, Cores = 6, Price = 250 }
                );

                // Добавление RAM
                context.RAM.AddRange(
                    new RAM { Type = "DDR4", Size = 16, Frequency = 3200, Model = "Corsair Vengeance", Price = 80 },
                    new RAM { Type = "DDR4", Size = 32, Frequency = 3600, Model = "G.Skill Trident Z", Price = 150 }
                );

                // Добавление материнских плат
                context.Motherboards.AddRange(
                    new Motherboard { Model = "ASUS ROG Strix", Socket = "LGA1200", FormFactor = "ATX", RAMType = "DDR4", Price = 180 },
                    new Motherboard { Model = "MSI B450", Socket = "AM4", FormFactor = "Micro-ATX", RAMType = "DDR4", Price = 120 }
                );

                // Добавление корпусов
                context.Frames.AddRange(
                    new Frame { Model = "NZXT H510", FormFactor = "ATX", Price = 70 },
                    new Frame { Model = "Cooler Master Q300L", FormFactor = "Micro-ATX", Price = 50 }
                );

                // Добавление блоков питания
                context.PowerUnits.AddRange(
                    new PowerUnit { Model = "Corsair RM750x", Wattage = 750, FormFactor = "ATX", Price = 120 },
                    new PowerUnit { Model = "EVGA 600 W1", Wattage = 600, FormFactor = "ATX", Price = 50 }
                );

                // Добавление HDD/SSD
                context.HDD_SSD.AddRange(
                    new HDD_SSD { Model = "Samsung 970 EVO", Type = "SSD", Capacity = 1000, Interface = "NVMe", Price = 150 },
                    new HDD_SSD { Model = "Seagate Barracuda", Type = "HDD", Capacity = 2000, Interface = "SATA", Price = 80 }
                );

                // Сохранение всех изменений в базе данных
                context.SaveChanges();
            }
        }
    }
}
