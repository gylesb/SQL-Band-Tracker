using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Band
  {
    private int _id;
    private string _name;

    public Band(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        return this.GetId().Equals(newBand.GetId());
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public string GetName()
    {
      return _name;
    }

    public int GetId()
    {
      return _id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO band (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Venue> GetVenues()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT venue.* FROM band
        JOIN band_venue ON (band.id = band_venue.band_id)
        JOIN venue ON (band_venue.venue_id = venue.id)
        WHERE band.id = @BandId;";

        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@BandId";
        bandIdParameter.Value = _id;
        cmd.Parameters.Add(bandIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Venue> venues = new List<Venue>{};

        while(rdr.Read())
        {
          int venueId = rdr.GetInt32(0);
          string venueName = rdr.GetString(1);
          Venue newVenue = new Venue(venueName, venueId);
          venues.Add(newVenue);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return venues;
      }

    public static Band Find(int id)
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();
    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"SELECT * FROM band WHERE id = (@searchId);";

    MySqlParameter searchId = new MySqlParameter();
    searchId.ParameterName = "@searchId";
    searchId.Value = id;
    cmd.Parameters.Add(searchId);

    var rdr = cmd.ExecuteReader() as MySqlDataReader;
    int BandId = 0;
    string BandName = "";

    while(rdr.Read())
    {
      BandId = rdr.GetInt32(0);
      BandName = rdr.GetString(1);
    }
    Band newBand = new Band(BandName, BandId);
    conn.Close();
    if (conn != null)
    {
      conn.Dispose();
    }
    return newBand;
  }


    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM band;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int BandId = rdr.GetInt32(0);
        string BandName = rdr.GetString(1);
        Band newBand = new Band(BandName, BandId);
        allBands.Add(newBand);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBands;
    }

      public void AddVenue(Venue newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO band_venue (band_id, venue_id) VALUES (@BandId, @VenueId);";

      MySqlParameter band_id = new MySqlParameter();
      band_id.ParameterName = "@BandId";
      band_id.Value = _id;
      cmd.Parameters.Add(band_id);

      MySqlParameter venue_id = new MySqlParameter();
      venue_id.ParameterName = "@VenueId";
      venue_id.Value = newVenue.GetId();
      cmd.Parameters.Add(venue_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM band; DELETE FROM band_venue;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    }
  }
