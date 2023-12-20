using System.Data.SqlClient;
using ChallengeAPI.Models;
using Microsoft.Extensions.Configuration;

namespace ChallengeAPI.Services;

public  class CarsService
{

    private SqlConnection _connection;
    private readonly  string connectionString;

    public CarsService (IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DBCars");
        _connection = new SqlConnection(connectionString);
    }

    public  List<CarsItemDTO> GetCars()
    {
        List<CarsItemDTO> Cars = new();

        using (_connection)
        {
                _connection.Open();
                using( SqlCommand cmd= new ("sp_Cars_List", _connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader= cmd.ExecuteReader())
                    {
                      if ( reader.HasRows)
                      {                        
                        while(reader.Read())
                        {
                            CarsItemDTO Car = new CarsItemDTO {
                                Id = Convert.ToInt32 ( reader["CarID"]),
                                Brand = reader["CarBrand"].ToString(),
                                Model = reader["CarModel"].ToString(),
                                isWorking = Convert.ToBoolean( reader["CarIsWorking"])
                            };
                            Cars.Add (Car);
                        }
                        }
                    }
                }
        }

        return Cars;
    }

    // TODO
    // AddCar
    // UpdateCar
    // DeleteCar

}