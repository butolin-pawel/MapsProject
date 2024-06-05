using System;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using maps.Controllers;
using maps.Models;

public class PlaceControllerTests
{
    private DbContextOptions<context> _dbContextOptions;

    public PlaceControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<context>()
            .UseNpgsql("Server=127.0.0.1;Port=5432;Database=testdatabase;User Id=postgres;Password=Vq2R8FJ;")
            .Options;
    }

    private context GetContext()
    {
        return new context(_dbContextOptions);
    }

    // Метод для очистки базы данных перед каждым тестом
    private void ClearDatabase()
    {
        using (var context = GetContext())
        {
            context.Database.ExecuteSqlRaw("DELETE FROM places");
        }
    }

    [Fact]
    public void Get_ReturnsAllPlaces()
    {
        // Arrange
        ClearDatabase(); // Очистка базы данных перед началом теста
        using (var context = GetContext())
        {
            context.places.AddRange(
                new place { id = 1, name = "Place 1", adress = "Address 1", longitude = 10.0, latitude = 20.0, description = "Description 1", dateofcreation = DateTime.Now },
                new place { id = 2, name = "Place 2", adress = "Address 2", longitude = 30.0, latitude = 40.0, description = "Description 2", dateofcreation = DateTime.Now }
            );
            context.SaveChanges();
        }

        using (var context = GetContext())
        {
            var controller = new PlaceController(context);

            // Act
            var result = controller.Get();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var table = Assert.IsType<DataTable>(jsonResult.Value);
            Assert.Equal(2, table.Rows.Count);
        }
    }

    [Fact]
    public void Post_AddsNewPlace_ReturnsId()
    {
        // Arrange
        ClearDatabase(); // Очистка базы данных перед началом теста
        using (var context = GetContext())
        {
            var controller = new PlaceController(context);
            var newPlace = new place { name = "Place 3", adress = "Address 3", longitude = 50.0, latitude = 60.0, description = "Description 3", dateofcreation = DateTime.Now };

            // Act
            var result = controller.Post(newPlace);

            // Assert
            Assert.Equal(1, context.places.Count());
        }
    }

    [Fact]
    public void Put_UpdatesExistingPlace_ReturnsSuccessMessage()
    {
        // Arrange
        ClearDatabase(); // Очистка базы данных перед началом теста
        using (var context = GetContext())
        {
            var placeToUpdate = new place { id = 1, name = "Place 4", adress = "Address 4", longitude = 70.0, latitude = 80.0, description = "Description 4", dateofcreation = DateTime.Now };
            context.places.Add(placeToUpdate);
            context.SaveChanges();
        }

        using (var context = GetContext())
        {
            var controller = new PlaceController(context);
            var updatedPlace = new place { id = 1, name = "Updated Place 4", adress = "Updated Address 4", longitude = 90.0, latitude = 100.0, description = "Updated Description 4", dateofcreation = DateTime.Now };

            // Act
            var result = controller.Put(updatedPlace);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("Updated Successfully", jsonResult.Value);
            var place = context.places.First();
            Assert.Equal("Updated Place 4", place.name);
        }
    }

    [Fact]
    public void Delete_RemovesPlace_ReturnsSuccessMessage()
    {
        // Arrange
        ClearDatabase(); // Очистка базы данных перед началом теста
        using (var context = GetContext())
        {
            var placeToDelete = new place { id = 1, name = "Place 5", adress = "Address 5", longitude = 110.0, latitude = 120.0, description = "Description 5", dateofcreation = DateTime.Now };
            context.places.Add(placeToDelete);
            context.SaveChanges();
        }

        using (var context = GetContext())
        {
            var controller = new PlaceController(context);

            // Act
            var result = controller.Delete(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("Deleted Successfully", jsonResult.Value);
            Assert.Equal(0, context.places.Count());
        }
    }
}
