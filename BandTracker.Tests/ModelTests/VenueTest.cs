using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System.Collections.Generic;
using System;

namespace BandTracker.Tests
{

  [TestClass]
  public class VenueTests : IDisposable
  {
    public VenueTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=BandTracker_test;";
    }

    public void Dispose()
    {
      Venue.DeleteAll();
      Band.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Venue.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_OverrideTrueIfNamesAreTheSame_Venue()
    {
      // Arrange, Act
      Venue firstVenue = new Venue("Key Arena");
      Venue secondVenue = new Venue("Key Arena");

      // Assert
      Assert.AreEqual(firstVenue, secondVenue);
    }

    [TestMethod]
    public void Save_SavesToDatabase_VenueList()
    {
      //Arrange
      Venue testVenue = new Venue("Key Arena");

      //Act
      testVenue.Save();
      List<Venue> result = Venue.GetAll();
      List<Venue> testList = new List<Venue>{testVenue};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Venue testVenue = new Venue("Key Arena");

      //Act
      testVenue.Save();
      Venue savedVenue = Venue.GetAll()[0];

      int result = savedVenue.GetId();
      int testId = testVenue.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsVenueInDatabase_Venue()
    {
      //Arrange
      Venue testVenue = new Venue("Key Arena");
      testVenue.Save();

      //Act
      Venue foundVenue = Venue.Find(testVenue.GetId());

      //Assert
      Assert.AreEqual(testVenue, foundVenue);
    }

    [TestMethod]
    public void AddBand_AddsBandToVenue_BandList()
    {
      //Arrange
      Venue testVenue = new Venue("Key Arena");
      testVenue.Save();

      Band testBand = new Band("Green Day");
      testBand.Save();

      //Act
      testVenue.AddBand(testBand);

      List<Band> result = testVenue.GetBands();
      List<Band> testList = new List<Band>{testBand};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }



    [TestMethod]
    public void GetVenue_ViewVenueListFromDatabase_Venues()
    {
      //Arrange
      Band testBand = new Band("Green Day");
      testBand.Save();
      Venue testVenue = new Venue("Key Arena");
      testVenue.Save();
      testBand.AddVenue(testVenue);

      List<Venue> allVenues = Venue.GetAll();
      List<Venue> result = testBand.GetVenues();


      //Assert
      CollectionAssert.AreEqual(allVenues, result);
    }

    [TestMethod]
    public void Delete_DeletesVenueAssociationsFromDatabase_VenueList()
    {
      //Arrange
      Band testBand = new Band("Green Day");
      testBand.Save();

      string testName = "Key Arena";
      Venue testVenue = new Venue(testName);
      testVenue.Save();

      //Act
      testVenue.AddBand(testBand);
      testVenue.Delete();

      List<Venue> resultBandVenues = testBand.GetVenues();
      List<Venue> testBandVenues = new List<Venue> {};

      //Assert
      CollectionAssert.AreEqual(testBandVenues, resultBandVenues);
    }
  }
}
