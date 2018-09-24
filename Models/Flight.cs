using System.Collections.Generic;
using System;
using Airplanner;
using MySql.Data.MySqlClient;

namespace Airplanner
  {
  public class Flight
  {
    private int _flightId;
    private string _flightAirline;
    private DateTime _departTime;
    private string _flightStatus;
    private string _arrivalCity;
    private string _departCity;

  public Flight(int flightId = 0, string flightAirline, DateTime departTime, string flightStatus, string arrivalCity, string departCity)
  {
    _flightId = flightId;
    _flightAirline = flightAirline;
    _departTime = departTime;
    _flightStatus = flightStatus;
    _arrivalCity = arrivalCity;
    _departCity = departCity;
  }

  public string GetFlightId()
  {
    return _flightId;
  }

  public string GetFlightAirline()
  {
    return _flightAirline;
  }

  public string GetDepartTime()
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
      bool flightAirlineEquality = this.GetFlightAirline() == newFlight.GetFlightAirline();
      bool flightStatusEquality = this.GetFlightStatus() == newFlight.GetFlightStatus();
      bool departTimeEquality = this.GetDepartTime() == newFlight.GetDepartTime();
      bool departCityEquality = this.GetDepartCity() == newFlight.GetDepartCity();
      bool arrivalCityEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
      return (idEquality && flightAirlineEquality && departTimeEquality && departCityEquality && flightStatusEquality && arrivalCityEquality)
    }
  }

    public override int GetHashCode()
    {
      string allHash = this.GetDepartCity() + this.GetArrivalCity() + this.GetStatus();
      return allHash.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (flight_name, flight_date, departure_city, arrival_city, flight_status) VALUES (@flightAirline, @departTime, @departCity, @arrivalCity, @flightStatus);";

      MySqlParameter flightAirline = new MySqlParameter();
      flightAirline.ParameterName = "@flightAirline";
      flightAirline.ParameterValue = this._flightAirline;
      cmd.Parameters.Add(flightAirline);


      MySqlParameter departTime = new MySqlParameter();
      departTime.ParameterName = "@departTime";
      departTime.ParameterValue = this._departTime;
      cmd.Parameters.Add(departTime);

      MySqlParameter departCity = new MySqlParameter();
      departCity.ParameterName = "@departCity";
      departCity.ParameterValue = this._departCity;
      cmd.Parameters.Add(arrivalCity);

      MySqlParameter arrivalCity = new MySqlParameter();
      arrivalCity.ParameterName = "@arrivalCity";
      arrivalCity.ParameterValue = this._arrivalCity;
      cmd.Parameters.Add(arrivalCity);

      MySqlParameter flightStatus = new MySqlParameter();
      flightStatus.ParameterName = "@flightStatus";
      flightStatus.ParameterValue = this._flightStatus;
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

       var rdr = cmd.ExecuteNonQuery() as MySqlDataReader;
       while (rdr.Read())
       {
         int flightId = rdr.GetInt32(0);
         string flightAirline = rdr.GetString(1);
         string flightStatus = rdr.GetString(2);
         DateTime departTime = rdr.GetDepartTime(3);
         string departCity = rdr.GetString(4);
         string arrivalCity = rdr.GetString(5);
         Flight newFlight = new Flight(newflightId, newflightAirline, newflightStatus, newdepartTime, newarrivalCity)
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
       string flightAirline = "";
       string flightStatus = "";
       string flightDepartureCity = "";
       string flightArrivalCity = "";
       DateTime flightDepartureTime = new DateTime(0000, 00, 00);

       while(rdr.Read())
       {
         flightId = rdr.GetInt32(0);
         flightDepartureCity = rdr.GetString(1);
         flightArrivalCity = rdr.GetString(2);
         flightStatus = rdr.GetString(3);
         flightAirline = rdr.GetString(4);
         flightDepartureTime = rdr.GetDateTime(5);
       }
       Flight newFlight = new Flight(flightDepartureCity, flightArrivalCity, flightStatus, flightAirline, flightDepartureTime, flightId);

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
      flight_id.Value = _id;
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
      flightIdParameter.Value = _id;
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
