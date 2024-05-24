using maps.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using maps.Models;

[TestClass]
public class RouteControllerTests
{
    private RouteController _controller;

    [TestInitialize]
    public void Initialize()
    {
        _controller = new RouteController();
    }

    [TestMethod]
    public void TestGet()
    {
        var result = _controller.Get() as JsonResult;
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Value, typeof(DataTable));
    }

    [TestMethod]
    public void TestPost()
    {
        var newRoute = new route
        {
            name = "TestRoute",
            length = 50,
            description = "Test Description",
            time = DateTime.Now
        };

        var result = _controller.Post(newRoute) as JsonResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Added Successfully", result.Value);
    }

    [TestMethod]
    public void TestPut()
    {
        var updateRoute = new route
        {
            id = 1,
            name = "UpdatedRoute",
            length = 60,
            description = "Updated Description",
            time = DateTime.Now
        };
        var result = _controller.Put(updateRoute) as JsonResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Updated Successfully", result.Value);
    }

    [TestMethod]
    public void TestDelete()
    {
        int routeId = 3;
        var result = _controller.Delete(routeId) as JsonResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Deleted Successfully", result.Value);
    }
}
