using System.Data.SqlClient;
using CarsAPI.Interfaces;
using ChallengeAPI.Models;
using Microsoft.Extensions.Configuration;

namespace ChallengeAPI.Services;

public  class CarsService: ICarService
{

    private SqlConnection _connection;
    private readonly  string connectionString;

    public CarsService (IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DBCars");
        _connection = new SqlConnection(connectionString);
    }

    public  List<CarsItemDTO> GetCars(int? id = null)
    {
        List<CarsItemDTO> Cars = new();

        using (_connection)
        {
                _connection.Open();
                using( SqlCommand cmd= new ("sp_Cars_Search", _connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    // the following line it's DRY pattern support
                    // this code can be use by GetCars and GetCars by ID.
                    // Also SQL SCript was replace, removing SP_CAR_LIST from the SP Creation Script
                    // and the SP_CAR_SEARCH was modified to be align with this DRY practice.
                    if (id.HasValue) {   cmd.Parameters.AddWithValue("@CarID",  id); }
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


    // After refactr this function must be private.
    public bool CheckIfCarExists (int id)
    {
        using (_connection)
        {
            _connection.Open();
            using( SqlCommand cmd= new ("sp_Cars_CheckIfExists", _connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CarID",  id);
                
                using (SqlDataReader reader= cmd.ExecuteReader())
                {
                    if ( reader.HasRows)
                    { 
                        while(reader.Read())
                        {
                            return Convert.ToBoolean( reader["Result"]);
                        };
                    }
                }
            }
        }
        return false;
    }

}