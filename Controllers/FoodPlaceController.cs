using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;

namespace Food_Place_App
{

    public static class FoodPlacesMapper
    {
        public static List<FoodPlace> MapFromRangeData(IList<IList<object>> values)
        {
            var foodPlaces = new List<FoodPlace>();

            foreach (var value in values)
            {
                FoodPlace foodPlace = new()
                {
                    Name = value[0].ToString(),
                    DistanceRating = value[1].ToString(),
                    PriceRating = value[2].ToString(),
                    HealthRating = value[3].ToString(),
                    AvgRating = value[4].ToString()
                };

                foodPlaces.Add(foodPlace);
            }

            return foodPlaces;
        }

        public static IList<IList<object>> MapToRangeData(FoodPlace foodPlace)
        {
            var objectList = new List<object>() { foodPlace.Name, foodPlace.DistanceRating, foodPlace.PriceRating, foodPlace.HealthRating, foodPlace.AvgRating};
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
    [Route("[controller]")]
    [ApiController]
    public class FoodPlaceController : ControllerBase
    {

        public static readonly string spreadSheetId = "18jdL_LoJIg73CR9T2x0nSNZqwwYyZceiDEKKp6SjZuE";
        public static readonly string sheet = "Food places";

        SpreadsheetsResource.ValuesResource foodPlaceSheetValues;

        public FoodPlaceController(GoogleSheetsHelper googleSheetsHelper)
        {
            foodPlaceSheetValues = googleSheetsHelper.service.Spreadsheets.Values;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var range = $"{sheet}!A4:E997";
            var request = foodPlaceSheetValues.Get(spreadSheetId, range);
            var response = request.Execute();
            var values = response.Values;

            string foodValues = JsonConvert.SerializeObject(values);
            FoodPlace foodPlaceResult = JsonConvert.DeserializeObject<FoodPlace>(foodValues);
            

            if (response.Values != null && response.Values.Count > 0)
            {
              return Ok(FoodPlacesMapper.MapFromRangeData(values));
            }
            else
            {
              return NotFound();
            }
            
        }
    }
}
