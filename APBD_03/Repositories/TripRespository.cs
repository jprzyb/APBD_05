using APBD_03.Model;
using Microsoft.AspNetCore.Mvc;
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
    }

    // eof method
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
                RegisteredAt = (DateTime)dr["RegisteredAt"],
                PaymentDate = (DateTime)dr["PaymentDate"]
            };
            result.Add(grade);
        }
        return result;
    } // eof method
    public int DeleteClient(int id)
    {
        string deleteQuery = "DELETE FROM trip.Client WHERE IdClient = @IdClient";
        int affectedCount = 0;
        
        using var con = new SqlConnection(_connectionString);
        con.Open();
        if(!clientHasTripsAssigned(con, id)){}
        {
            using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = deleteQuery;
            cmd.Parameters.AddWithValue("@IdClient", id);
            affectedCount = cmd.ExecuteNonQuery();
        }
        return affectedCount;
    }

    private bool clientHasTripsAssigned(SqlConnection con, int id)
    {
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT COUNT(*) FROM trip.Client_Trip WHERE IdClient = @IdClient";
        cmd.Parameters.AddWithValue("@IdClient", id);
        return cmd.ExecuteNonQuery() > 0;

    } // eof method
    
    public int AssigneClientToTrip(AssigneClient assigneClient)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        var id = new Random().Next(0, 10001);
        if (!clientExist(con, assigneClient.Pesel))
        {
            createClient(con, assigneClient, id);
        }

        if (isClientAssignedToTrip(con, assigneClient)) return -1;
        if (doesTheTripExist(con, assigneClient)) return -1;
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO trip.Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate) VALUES (@IdClient, @IdTrip, @RegisteredAt, @PaymentDate)";
        cmd.Parameters.AddWithValue("@IdClient", getClientIdByPesel(con, assigneClient.Pesel));
        cmd.Parameters.AddWithValue("@IdTrip", assigneClient.IdTrip);
        cmd.Parameters.AddWithValue("@RegisteredAt", DateTime.Now.Date);
        cmd.Parameters.AddWithValue("@PaymentDate", assigneClient.PaymentDate);
        var affectedCount =  cmd.ExecuteNonQuery();
        return affectedCount;
    }

    private bool doesTheTripExist(SqlConnection con, AssigneClient assigneClient)
    {
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT COUNT(*) FROM trip.Trip WHERE IdTrip = @IdTrip";
        cmd.Parameters.AddWithValue("@IdTrip", assigneClient.IdTrip);
        return cmd.ExecuteNonQuery() > 0;
    }

    private bool isClientAssignedToTrip(SqlConnection con, AssigneClient assigneClient)
    {
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT COUNT(*) FROM trip.Client_Trip WHERE IdClient = @IdClient AND IdTrip = @IdTrip";
        cmd.Parameters.AddWithValue("@IdClient", getClientIdByPesel(con,assigneClient.Pesel));
        cmd.Parameters.AddWithValue("@IdTrip", assigneClient.IdTrip);
        return cmd.ExecuteNonQuery() > 0;
    }

    private int getClientIdByPesel(SqlConnection con, string Pesel)
    {
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdClient FROM trip.Client WHERE Pesel = @Pesel";
        cmd.Parameters.AddWithValue("@Pesel", Pesel);
        var dr = cmd.ExecuteReader();
        return (int)dr["IdClient"];
    }

    private void createClient(SqlConnection con, AssigneClient assigneClient, int id)
    {
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO trip.Client (IdClient, FirstName, LastName, Email, Telephone, Pesel) VALUES (@IdClient, @FirstName, @LastName, @Email, @Telephone, @Pesel)";
        cmd.Parameters.AddWithValue("@IdClient", id);
        cmd.Parameters.AddWithValue("@FirstName", assigneClient.FirstName);
        cmd.Parameters.AddWithValue("@LastName", assigneClient.LastName);
        cmd.Parameters.AddWithValue("@Email", assigneClient.Email);
        cmd.Parameters.AddWithValue("@Telephone", assigneClient.Telephone);
        cmd.Parameters.AddWithValue("@Pesel", assigneClient.Pesel);
        cmd.ExecuteNonQuery();
    }

    private bool clientExist(SqlConnection con, string clientPesel)
    {
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT COUNT(*) FROM trip.Client WHERE Pesel LIKE @Pesel";
        cmd.Parameters.AddWithValue("@Pesel", clientPesel);
        return cmd.ExecuteNonQuery() > 0;
    }
}