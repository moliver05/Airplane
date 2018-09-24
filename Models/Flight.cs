using System.Collections.Generic;
using System;
using Airplanner;
using MySql.Data.MySqlClient;

namespace Airplanner.Models
  {
  public class Flight
  {
    private int _flightId;
    private string _flightName;
    private DateTime _departTime;
    private string _flightStatus;
    private string _arrivalCity;
    private string _departCity;

  public Flight(string flightName, DateTime departTime, string flightStatus, string arrivalCity, string departCity, int flightId = 0)
  {
    _flightId = flightId;
    _flightName = flightName;
    _departTime = departTime;
    _flightStatus = flightStatus;
    _arrivalCity = arrivalCity;
    _departCity = departCity;
  }

  public int GetFlightId()
  {
    return _flightId;
  }

  public string GetFlightName()
  {
    return _flightName;
  }

  public DateTime GetDepartTime()
  {
    return _departTime;
  }

  public string GetFlightStatus()
  {
    return  _flightStatus;
  }

  public string GetArrivalCity()
  {
    return _arrivalCity;
  }

  public string GetDepartCity()
  {
    return _departCity;
  }

  public override bool Equals(System.Object otherFlight)
  {
    if (!(otherFlight is Flight))
    {
      return false;
    }
    else
    {
      Flight newFlight = (Flight) otherFlight;
      bool idEquality = this.GetFlightId() == newFlight.GetFlightId();
      bool flightNameEquality = this.GetFlightName() == newFlight.GetFlightName();
      bool flightStatusEquality = this.GetFlightStatus() == newFlight.GetFlightStatus();
      bool departTimeEquality = this.GetDepartTime() == newFlight.GetDepartTime();
      bool departCityEquality = this.GetDepartCity() == newFlight.GetDepartCity();
      bool arrivalCityEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
      return (idEquality && flightNameEquality && departTimeEquality && departCityEquality && flightStatusEquality && arrivalCityEquality);
    }
  }

    public override int GetHashCode()
    {
      string allHash = this.GetDepartCity() + this.GetArrivalCity() + this.GetFlightStatus();
      return allHash.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (flight_name, flight_date, departure_city, arrival_city, flight_status) VALUES (@flightName, @departTime, @departCity, @arrivalCity, @flightStatus);";

      MySqlParameter flightName = new MySqlParameter();
      flightName.ParameterName = "@flightName";
      flightName.Value = this._flightName;
      cmd.Parameters.Add(flightName);


      MySqlParameter departTime = new MySqlParameter();
      departTime.ParameterName = "@departTime";
      departTime.Value = this._departTime;
      cmd.Parameters.Add(departTime);

      MySqlParameter departCity = new MySqlParameter();
      departCity.ParameterName = "@departCity";
      departCity.Value = this._departCity;
      cmd.Parameters.Add(departCity);

      MySqlParameter arrivalCity = new MySqlParameter();
      arrivalCity.ParameterName = "@arrivalCity";
      arrivalCity.Value = this._arrivalCity;
      cmd.Parameters.Add(arrivalCity);

      MySqlParameter flightStatus = new MySqlParameter();
      flightStatus.ParameterName = "@flightStatus";
      flightStatus.Value = this._flightStatus;
      cmd.Parameters.Add(flightStatus);

      cmd.ExecuteNonQuery();
      _flightId = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
     }

     public static List<Flight> GetAll()
     {
       List<Flight> allFlights = new List<Flight> {};
       MySqlConnection conn = DB.Connection();
       conn.Open();

       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"SELECT * FROM flights;";

       var rdr = cmd.ExecuteReader() as MySqlDataReader;
       while (rdr.Read())
       {
         int flightId = rdr.GetInt32(0);
         string flightName = rdr.GetString(1);
         DateTime departTime = rdr.GetDateTime(2);
         string flightStatus = rdr.GetString(3);
         string arrivalCity = rdr.GetString(4);
         string departCity = rdr.GetString(5);

         Flight newFlight = new Flight(flightName, departTime, flightStatus, arrivalCity, departCity, flightId);
         allFlights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }

     public static Flight Find(int id)
     {
       MySqlConnection conn = DB.Connection();
       conn.Open();

       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchflightId);";

       MySqlParameter searchflightId = new MySqlParameter();
       searchflightId.ParameterName = "@searchflightId";
       searchflightId.Value = id;
       cmd.Parameters.Add(searchflightId);
//
       var rdr = cmd.ExecuteReader() as MySqlDataReader;
       int flightId = 0;
       string flightName = "";
       string flightStatus = "";
       string flightDepartureCity = "";
       string flightArrivalCity = "";
       DateTime flightDepartureTime = new DateTime(0000, 00, 00);

       while(rdr.Read())
       {
         flightId = rdr.GetInt32(0);
         flightName = rdr.GetString(1);
         flightDepartureTime = rdr.GetDateTime(2);
         flightStatus = rdr.GetString(3);
         flightArrivalCity = rdr.GetString(4);
         flightDepartureCity = rdr.GetString(5);
       }
       Flight newFlight = new Flight(flightName, flightDepartureTime, flightStatus, flightArrivalCity, flightDepartureCity, flightId);

       conn.Close();
       if(conn != null)
       {
         conn.Dispose();
       }
       return newFlight;
     }


    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM flights;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddCity(City newCity)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights_cities(flight_id, city_id) VALUES (@FlightId, @CityId);";

      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@FlightId";
      flight_id.Value = _flightId;
      cmd.Parameters.Add(flight_id);

      MySqlParameter city_id = new MySqlParameter();
      city_id.ParameterName = "@CityId";
      city_id.Value = newCity.GetCityId();//Getters in City.cs
      cmd.Parameters.Add(city_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public List<City> GetCity()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT cities.* FROM flights
      JOIN flights_cities ON (flights.id = flights_cities.flight_id)
      JOIN cities ON (flights_cities.city_id = cities.id)
      WHERE flights.id = @FlightId;";

      MySqlParameter flightIdParameter = new MySqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = _flightId;
      cmd.Parameters.Add(flightIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<City> cities = new List<City>{};

      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        cities.Add(newCity);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return cities;
    }

 }
}
