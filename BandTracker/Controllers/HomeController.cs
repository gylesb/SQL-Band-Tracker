using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;

namespace BandTracker.Controllers
{
  public class HomeController : Controller
  {
    // HOME AND MENU ROUTES
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/venues")]
    public ActionResult ViewVenue()
    {
      List<Venue> allVenues = Venue.GetAll();

      return View("VenueList", allVenues);
    }

    [HttpPost("/venues")]
    public ActionResult AddViewVenue()
    {
      Venue newVenue = new Venue(Request.Form["venue-name"]);
      newVenue.Save();
      List<Venue> allVenues = Venue.GetAll();

      return View("VenueList", allVenues);
    }

    [HttpGet("/bands")]
    public ActionResult ViewBand()
    {
      List<Band> allBands = Band.GetAll();

      return View("BandList", allBands);
    }

    [HttpPost("/bands")]
    public ActionResult AddViewBand()
    {
      Band newBand = new Band(Request.Form["band-name"]);
      newBand.Save();

      List<Band> allBands = Band.GetAll();

      return View("BandList", allBands);
    }

    [HttpGet("/venue/add")]
    public ActionResult AddVenue()
    {
      return View();
    }

    [HttpGet("/band/add")]
    public ActionResult AddBand()
    {
      return View();
    }

    [HttpGet("/band/{bandId}/venues")]
    public ActionResult ViewBand(int bandId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();
      Band thisBand = Band.Find(bandId);
      List<Venue> bandVenues = thisBand.GetVenues();
      List<Venue> allVenues = Venue.GetAll();

      model.Add("band", thisBand);
      model.Add("venues", bandVenues);
      model.Add("allVenues", allVenues);

      return View("BandProfile", model);
    }

    [HttpGet("/venue/{venueId}/bands")]
    public ActionResult ViewVenue(int venueId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();
      Venue thisVenue = Venue.Find(venueId);
      List<Band> venueBands = thisVenue.GetBands();
      List<Band> allBands = Band.GetAll();

      model.Add("venue", thisVenue);
      model.Add("bands", venueBands);
      model.Add("allBands", allBands);

      return View("VenueProfile",model);
    }

    [HttpPost("/venues/{venueId}/band/add")]
    public ActionResult AddBandToVenue(int venueId)
    {
      Venue thisVenue = Venue.Find(venueId);
      Band thisBand = Band.Find(Int32.Parse(Request.Form["band-id"]));
      thisVenue.AddBand(thisBand);

      return View("Index");
    }

    [HttpPost("/bands/{bandId}/venue/add")]
    public ActionResult AddVenueToBand(int bandId)
    {
      Band thisBand = Band.Find(bandId);
      Venue thisVenue = Venue.Find(Int32.Parse(Request.Form["venue-id"]));
      thisBand.AddVenue(thisVenue);

      return View("Index");
    }

  }

}
