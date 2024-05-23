using APBD_03.Model;
using Microsoft.Data.SqlClient;

namespace APBD_03.Repositories;

public class TripRespository : ITripRepository
{
    private IConfiguration _configuration;
    private string _connectionString;

    public TripRespository(IConfiguration configuration)
    {
        _configuration = configuration;
        string strProject = "KUBUS"; // Wprowadź nazwę instancji serwera SQL
        string strDatabase = "apbd005"; // Wprowadź nazwę bazy danych
        string strUserID = "apbd05"; // Wprowadź nazwę użytkownika SQL Server
        string strPassword = "apbd05"; // Wprowadź hasło użytkownika SQL Server
        _connectionString = "data source=" + strProject +
                            ";Persist Security Info=false;database=" + strDatabase +
                            ";user id=" + strUserID + ";password=" +
                            strPassword +
                            ";Connection Timeout = 0;trustServerCertificate=true;";
    }

    public IEnumerable<TripCountryClient> GetTrips()
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = new SqlCommand();
        var result = new List<TripCountryClient>();
        cmd.Connection = con;
        String query =
            "SELECT * FROM trip.Trip ORDER BY DateFrom DESC;";
        cmd.CommandText = query;

        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var grade = new Trip()
            {
                IdTrip = (int)dr["IdTrip"],
                Name = (string)dr["Name"],
                Description = (string)dr["Description"],
                DateFrom = (DateTime)dr["DateFrom"],
                DateTo = (DateTime)dr["DateTo"],
                MaxPeople = (int)dr["MaxPeople"],
            };
            var countries = getCountriesByTripId(con, grade.IdTrip);
            var clients = getClientsByTripId(con, grade.IdTrip);
            var tripCountryClient = new TripCountryClient()
            {
                Name = (string)dr["Name"],
                Description = (string)dr["Description"],
                DateFrom = (DateTime)dr["DateFrom"],
                DateTo = (DateTime)dr["DateTo"],
                MaxPeople = (int)dr["MaxPeople"],
                Clients = clients,
                Countries = countries
            };
            result.Add(tripCountryClient);
        }
        return result;
    } // eof method
    private List<Country> getCountriesByTripId(SqlConnection con, int IdTrip)
    {
        var result = new List<Country>();
        var association = getCountryTripsById(con, IdTrip);
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdCountry, Name FROM trip.Country WHERE IdCountry = @IdCountry";
        foreach (var countryTrip in association)
        {
            var IdCountry = countryTrip.IdCountry;
            cmd.Parameters.AddWithValue("@IdCountry", IdCountry);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var grade = new Country()
                {
                    IdCountry = (int)dr["IdCountry"],
                    Name = (string)dr["Name"]
                };
                result.Add(grade);
            }
        }
        return result;
    } // eof method

    private List<CountryTrip> getCountryTripsById(SqlConnection con, int IdTrip)
    {
        var result = new List<CountryTrip>();
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdCountry, IdTrip FROM trip.CountryTrip WHERE IdTrip = @IdTrip";
        cmd.Parameters.AddWithValue("@IdTrip", IdTrip);
        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var grade = new CountryTrip()
            {
                IdCountry = (int)dr["IdCountry"],
                IdTrip = (int)dr["IdTrip"]
            };
            result.Add(grade);
        }
        return result;
    }
    private List<Client> getClientsByTripId(SqlConnection con, int IdTrip)
    {
        var result = new List<Client>();
        var association = getClientstripByTripId(con, IdTrip);
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdClient, FirstName, LastName, Email, Telephone, Pesel FROM trip.Client WHERE IdCLient = @IdClient";
        foreach (var countryTrip in association)
        {
            var IdClient = countryTrip.IdClient;
            cmd.Parameters.AddWithValue("@IdClient", IdClient);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var grade = new Client()
                {
                    IdClient = (int)dr["IdClient"],
                    FirstName = (string)dr["FirstName"],
                    LastName = (string)dr["LastName"],
                    Email = (string)dr["Email"],
                    Telephone = (string)dr["Telephone"],
                    Pesel = (string)dr["Pesel"]
                };
                result.Add(grade);
            }
        }
        return result;
    }

    private List<ClientTrip> getClientstripByTripId(SqlConnection con, int IdTrip)
    {
        var result = new List<ClientTrip>();
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdClient, IdTrip, RegisteredAt, PaymentDate FROM trip.CountryTrip WHERE IdTrip = @IdTrip";
        cmd.Parameters.AddWithValue("@IdTrip", IdTrip);
        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var grade = new ClientTrip()
            {
                IdClient = (int)dr["IdClient"],
                IdTrip = (int)dr["IdTrip"],
                RegisteredAt = (int)dr["RegisteredAt"],
                PaymentDate = (int)dr["PaymentDate"]
            };
            result.Add(grade);
        }
        return result;
    }
    // eof method
}