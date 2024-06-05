using maps.Controllers;
using maps.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Xunit;

public class RouteControllerTests
{
    private DbContextOptions<context> _dbContextOptions;

    public RouteControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<context>()
          .UseNpgsql("Server=127.0.0.1;Port=5432;Database=testdatabase;User Id=postgres;Password=Vq2R8FJ;")
          .Options;
    }

    private context GetContext()
    {
        return new context(_dbContextOptions);
    }

    // Method for cleaning the database before each test
    private void ClearDatabase()
    {
        using (var context = GetContext())
        {
            context.Database.ExecuteSqlRaw("DELETE FROM routes");
        }
    }
    [Fact]
    public void Get_ReturnsAllRoutes()
    {
        // Arrange
        ClearDatabase();

        using (var context = GetContext())
        {
            context.routes.AddRange(
                new route { id = 1, name = "Route 1", length = 30, description = "Description 1", time = DateTime.Now },
                new route { id = 2, name = "Route 2", length = 50, description = "Description 2", time = DateTime.Now }
            );
            context.SaveChanges();
        }

        using (var context = GetContext())
        {
            var controller = new RouteController(context);

            // Act
            var result = controller.Get();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var table = Assert.IsType<DataTable>(jsonResult.Value);
            Assert.Equal(2, table.Rows.Count);

            // Verify data in the table (optional)
            // You can loop through the table rows and assert the values of specific columns
        }
    }

    [Fact]
    public void Post_AddsNewRoute_ReturnsId()
    {
        // Arrange
        ClearDatabase();
        using (var context = GetContext())
        {
            var controller = new RouteController(context);
            var newRoute = new route { name = "New Route", length = 70, description = "New Description", time = DateTime.Now };

            // Act
            var result = controller.Post(newRoute);

            // Assert
            Assert.Equal(1, context.routes.Count());
            Assert.Equal(context.routes.OrderBy(a => a.id).Last().id, result);  // Get the id from the database after saving
        }
    }

    [Fact]
    public void Put_UpdatesExistingRoute_ReturnsSuccessMessage()
    {
        // Arrange
        ClearDatabase();
        using (var context = GetContext())
        {
            var routeToUpdate = new route { id = 1, name = "Route To Update", length = 40, description = "Description To Update", time = DateTime.Now };
            context.routes.Add(routeToUpdate);
            context.SaveChanges();
        }

        using (var context = GetContext())
        {
            var controller = new RouteController(context);
            var updatedRoute = new route { id = 1, name = "Updated Route", length = 80, description = "Updated Description", time = DateTime.Now };

            // Act
            var result = controller.Put(updatedRoute);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("Updated Successfully", jsonResult.Value);
            var route = context.routes.First();
            Assert.Equal("Updated Route", route.name);
        }
    }

    [Fact]
    public void Put_RouteNotFound_ReturnsNotFoundMessage()
    {
        // Arrange
        ClearDatabase();
        using (var context = GetContext())
        {
            var controller = new RouteController(context);
            var updatedRoute = new route { id = 1, name = "Updated Route", length = 80, description = "Updated Description", time = DateTime.Now };

            // Act
            var result = controller.Put(updatedRoute);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("Route not found", jsonResult.Value);
        }
    }

    [Fact]
    public void Delete_RemovesRoute_ReturnsSuccessMessage()
    {
        // Arrange
        ClearDatabase();
        using (var context = GetContext())
        {
            var routeToDelete = new route { id = 1, name = "Route To Delete", length = 60, description = "Description To Delete", time = DateTime.Now };
            context.routes.Add(routeToDelete);
            context.SaveChanges();
        }

        using (var context = GetContext())
        {
            var controller = new RouteController(context);

            // Act
            var result = controller.Delete(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("Deleted Successfully", jsonResult.Value);
            Assert.Equal(0, context.routes.Count());
        }
    }

    [Fact]
    public void Delete_RouteNotFound_ReturnsNotFoundMessage()
    {
        // Arrange
        ClearDatabase();
        using (var context = GetContext())
        {
            var controller = new RouteController(context);

            // Act
            var result = controller.Delete(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("Route not found", jsonResult.Value);
        }
    }
}
