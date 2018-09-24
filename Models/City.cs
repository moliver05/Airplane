using System.Collections.Generic;
using System;
using Airplanner;
using MySql.Data.MySqlClient;

namespace Airplanner
{
  public class City
  {
    private string _cityName;
    private int _cityId;

    public City (string cityName, int cityId = 0)
    {
      _cityName = cityName;
      _cityId = cityId;
    }

    public string GetCityName()
    {
      return _cityName;
    }

    public int GetCityId();
    {
      return _cityId;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool cityIdEquality = this.GetCityId() == newCity.GetCityId();
        bool cityNameEquality = this.GetCityName() == newCity.GetCityName();

        return (cityIdEquality && cityNameEquality);
      }
    }

    public override int GetHashCode()
    {
      string = this.GetCityId() + this.GetCityName();
      return allHash.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities (cityname) VALUE @name;";

      MySqlParameter cityName = new MySqlParameter();
      cityName.ParameterName = "@name";
      cityName.Value = this._cityName;
      cmd.Parameterds.Add(cityName);

      cmd.ExecuteNonQuery();
      cityId = (int) cmd.LastInsertedId;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<City> GetAll()
    {
      List<City> allCity = new List<City> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);

        City newCity = new City(cityName, cityId);
        allCity.Add(newCity);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCity;
    }
    public static City Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cityId = 0;
      string cityName = "";


      while(rdr.Read())
      {
        cityId = rdr.GetInt32(0);
        cityName = rdr.GetString(1);

      }


      City newCity = new City(cityName, cityId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return newCity;
    }
      
  }
}
