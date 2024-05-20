using APBD_05.Model;
using Microsoft.Data.SqlClient;

namespace APBD_05.Repositories;

public class TripRespository
{
    private IConfiguration _configuration;
    private string _connectionString;
    
    public TripRespository(IConfiguration configuration)
    {
        _configuration = configuration;
        string strProject = "KUBUS"; // Wprowadź nazwę instancji serwera SQL
        string strDatabase = "APBD05"; // Wprowadź nazwę bazy danych
        string strUserID = "user"; // Wprowadź nazwę użytkownika SQL Server
        string strPassword = "user"; // Wprowadź hasło użytkownika SQL Server
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
        var results = new List<TripCountryClient>();
        cmd.Connection = con;
        String tripQuery =
            "SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople FROM trip.Trip ORDER BY DateFrom DESC;";
        String CountryQuery =
            "SELECT IdCountry,IdTrip FROM trip.Country WHERE IdTrip = IdTrip;";
        String ClientQuery =
            "SELECT IdCountry,IdTrip FROM trip.Country WHERE IdTrip = IdTrip;";
        cmd.CommandText = tripQuery;
        
        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var trip = new Trip()
            {
                IdTrip = (int)dr["IdTrip"],
                Name = (string)dr["Name"],
                Description = (string)dr["Description"],
                DateFrom = (DateTime)dr["DateFrom"],
                DateTo = (DateTime)dr["DateTo"],
                MaxPeople = (int)dr["MaxPeople"],
            };
        }
        
        return results;
    }
}