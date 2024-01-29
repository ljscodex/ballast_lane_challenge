using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeAPI.Models;

namespace CarsAPI.Interfaces
{
    public interface ICarService
    {
        
            
            public  List<CarsItemDTO> GetCars(int? id);
            public bool CheckIfCarExists (int id);



    }
}