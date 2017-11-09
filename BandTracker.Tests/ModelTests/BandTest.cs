using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;

namespace BandTracker.Tests
{
  [TestClass]
  public class BandTests : IDisposable
  {
    public BandTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889 ;database=BandTracker_test;";
    }

    [TestMethod]
    public void GetAll_BandsEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Band.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Band()
    {
      //Arrange, Act
      Band firstBand = new Band("Green Day");
      Band secondBand = new Band("Green Day");

      //Assert
      Assert.AreEqual(firstBand, secondBand);
    }

    [TestMethod]
    public void Save_SavesBandToDatabase_BandList()
    {
      //Arrange
      Band testBand = new Band("Green Day");
      testBand.Save();

      //Act
      List<Band> result = Band.GetAll();
      List<Band> testList = new List<Band>{testBand};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_DatabaseAssignsIdToBand_Id()
    {
      //Arrange
      Band testBand = new Band("Green Day");
      testBand.Save();

      //Act
      Band savedBand = Band.GetAll()[0];

      int result = savedBand.GetId();
      int testId = testBand.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void GetBand_ViewBandListFromDatabase_Band()
    {
      //Arrange
      Band testBand = new Band("Green Day");
      testBand.Save();
      Venue testVenue = new Venue("Key Arena");
      testVenue.Save();
      testVenue.AddBand(testBand);

      List<Band> allBands = Band.GetAll();
      List<Band> result = testVenue.GetBands();


      //Assert
      CollectionAssert.AreEqual(allBands, result);
    }

    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }
  }


}
