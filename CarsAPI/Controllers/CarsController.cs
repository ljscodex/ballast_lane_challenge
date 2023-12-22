using Microsoft.AspNetCore.Mvc;
using ChallengeAPI.Models;
using ChallengeAPI.Services;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace ChallengeAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    //private readonly CarsService _carService;


    // after reactoring all the actions, this line will be removed.
    private readonly string connectionString;
    private readonly IConfiguration _configuration;

    public CarsController( IConfiguration configuration)
    {
        _configuration = configuration;
        // after reactoring all the actions, this line will be removed.
        connectionString = configuration.GetConnectionString("DBCars");

    }


  // GET: api/Cars
    [HttpGet("/AllCars")]
    public ActionResult<IEnumerable<CarsItemDTO>> GetAllCars()
    {

        List<CarsItemDTO> cars =   new List<CarsItemDTO>();
        CarsService _carService = new CarsService(_configuration);
        cars = _carService.GetCars();
        
        return  cars;
    }
    // The code bellow was Refactored
    // Imporved using ServiceLayer to handle all the BusinessLogic and Data Manipulation.


    /*
    // GET: api/Cars
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarsItemDTO>>> GetCars()
    {
        List<CarsItemDTO> Cars = new List<CarsItemDTO>();

        await using (SqlConnection connection=new(connectionString))
        {
                connection.Open();
                using( SqlCommand cmd= new ("sp_Cars_List", connection))
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
    }*/


    [Authorize]
    // GET: api/Cars/5
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<CarsItemDTO>>> GetCar(int id)
    {
        List<CarsItemDTO> cars =   new List<CarsItemDTO>();
        CarsService _carService = new CarsService(_configuration);
        cars = _carService.GetCars(id);
        
        return cars;
    }
    /*
    // This code is commented based on refactoring
    public async Task<ActionResult<CarsItemDTO>> GetCar(int id)
    {
        CarsItemDTO car= new CarsItemDTO();

       await using (SqlConnection connection=new(connectionString))
        {
                connection.Open();
                using( SqlCommand cmd= new ("sp_Cars_Search", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CarID",  id);
                    
                    using (SqlDataReader reader= cmd.ExecuteReader())
                    {
                      //  if ( reader.)
                      if ( reader.HasRows)
                      {
                        while(reader.Read())
                        {
                             car = new CarsItemDTO {
                                Id = Convert.ToInt32 ( reader["CarID"]),
                                Brand = reader["CarBrand"].ToString(),
                                Model = reader["CarModel"].ToString(),
                                isWorking = Convert.ToBoolean( reader["CarIsWorking"])
                            };
                        }
                     }
                    }
                    
                }
        }
        return car;
    }
    */

    // POST: api/Cars
    [Authorize]
    [HttpPost]
    public async void PostCarItem(CarsItemDTO carsItemDTO)
    {
      await using (SqlConnection connection=new(connectionString))
            {
                    connection.Open();
                    using( SqlCommand cmd= new ("sp_Cars_Create", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CarBrand",  carsItemDTO.Brand);
                        cmd.Parameters.AddWithValue("@CarModel",  carsItemDTO.Model);
                        cmd.Parameters.AddWithValue("@CarIsWorking",  carsItemDTO.isWorking);                        
                        cmd.Parameters.AddWithValue("@userID",  1);

                        cmd.ExecuteNonQuery();
                    }
            }

    }


    // PUT: api/Cars/5
    [Authorize]    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCarItem(int id, CarsItemDTO carDTO)
    {
        if (id != carDTO.Id)
        {
            return BadRequest();
        }

        CarsService _carService = new CarsService(_configuration);

        if (!_carService.CheckIfCarExists(id))
        {
            return NotFound();
        }

        try
        {
        await using (SqlConnection connection=new(connectionString))
            {
                    connection.Open();
                    using( SqlCommand cmd= new ("sp_Cars_Update", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CarID",  carDTO.Id); 
                        cmd.Parameters.AddWithValue("@CarBrand",  carDTO.Brand);
                        cmd.Parameters.AddWithValue("@CarModel",  carDTO.Model);
                        cmd.Parameters.AddWithValue("@CarIsWorking",  carDTO.isWorking);                        
                        cmd.Parameters.AddWithValue("@userID",  1);

                        cmd.ExecuteNonQuery();
                    }
            }
        }
        catch
        {
            return StatusCode(500);
        }

        return NoContent();
    }

    [Authorize]
    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        CarsService _carService = new CarsService(_configuration);

        if (!_carService.CheckIfCarExists(id))        {
            return NotFound();
        }
        try
        {
        await using (SqlConnection connection=new(connectionString))
            {
                    connection.Open();
                    using( SqlCommand cmd= new ("sp_Cars_Delete", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CarID",  id); 
                        cmd.ExecuteNonQuery();
                    }
            }
        }
        catch
        {
            return StatusCode(500);
        }

        return NoContent();
    }
    
}
