using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using ChallengeAPI.Controllers;
using ChallengeAPI.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Sdk;
using System.Text;


namespace CarsAPI.test
{
    public class CarsAPI
    {
    
        private readonly WebApplicationFactory<CarsController> _application;

        public CarsAPI()
        {
            _application = new WebApplicationFactory<CarsController>();
        }

        [Fact]
        public async Task CheckGetAllCars()
        {
            var httpClient = _application.CreateClient();

            var response = await httpClient.GetAsync( $"/AllCars");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]

        public async Task CheckGetCarsByID(int id)
        {
            var httpClient = _application.CreateClient();

            var response = await httpClient.GetAsync( $"/AllCars/{id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]

        public async Task CheckGetCarsByBadID(int id)
        {
            var httpClient = _application.CreateClient();

            var response = await httpClient.GetAsync( $"/AllCars/{id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Theory]
        [InlineData("Toyota", "Yaris" , false)]
        [InlineData("Chevrolet", "Cruze" , false)]
        [InlineData("Ferrari", "PuraSangre" , false)]
        [InlineData("Porsche", "911" , false)]
        public async Task CheckCreateCar(string Brand, string Model, bool IsWorking)
        {
            var httpClient = _application.CreateClient();

            var response = await httpClient.PostAsJsonAsync($"/API/Cars",
                new CarsItemDTO()
                {
                    Brand = Brand,
                    Model = Model,
                    isWorking = IsWorking
                });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }



        [Theory]
        [InlineData(30192,"Toyota", "Yaris", true )]
        [InlineData(99999, "Chevrolet", "Cruze" , false)]

        public async Task CheckUpdateBadCar(int id, string Brand, string Model, bool IsWorking)
        {
            var httpClient = _application.CreateClient();

            CarsItemDTO car = new CarsItemDTO()
            {
                Id = id,
                Brand = Brand,
                Model = Model,
                isWorking = IsWorking
            };

            var httpContent = new StringContent(JsonSerializer.Serialize<CarsItemDTO>(car) , Encoding.UTF8, "application/json");            

            var response = await httpClient.PutAsync ( $"/API/Cars/{id}",  httpContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Theory]
        [InlineData(30192,"Toyota", "Yaris", true )]
        [InlineData(99999, "Chevrolet", "Cruze" , false)]

        public async Task CheckUpdateBadCarbyBadID(int id, string Brand, string Model, bool IsWorking)
        {
            var httpClient = _application.CreateClient();

            CarsItemDTO car = new CarsItemDTO()
            {
                Id = id +1 ,
                Brand = Brand,
                Model = Model,
                isWorking = IsWorking
            };

            var httpContent = new StringContent(JsonSerializer.Serialize<CarsItemDTO>(car) , Encoding.UTF8, "application/json");            

            var response = await httpClient.PutAsync ( $"/API/Cars/{id}",  httpContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Theory]
        [InlineData(-1 )]

        public async Task CheckBadDeleteCar(int id)
        {
            var httpClient = _application.CreateClient();
            var response = await httpClient.DeleteAsync($"/API/Cars/{id}");   
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}