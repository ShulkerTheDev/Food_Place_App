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
        public static List<FoodPlace> MapFromRangeData(IList<IList<Object>> values)
        {
            var foodPlaces = new List<FoodPlace>();

            foreach (var value in values)
            {
                int distInt = Convert.ToInt32(value[1].ToString());

                int priceInt = Convert.ToInt32(value[2].ToString());

                int healthInt = Convert.ToInt32(value[3].ToString());

                double avgDouble = Convert.ToDouble(value[4].ToString());
                
                FoodPlace foodPlace = new()
                {
                  
                    Name = value[0].ToString(),
                    DistanceRating = distInt,
                    PriceRating = priceInt,
                    HealthRating = healthInt,
                    AvgRating = avgDouble
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
        public IActionResult Get(int distanceRating, int priceRating, int healthRating, int avgRating)
        {
            var range = $"{sheet}!A4:E997";
            var request = foodPlaceSheetValues.Get(spreadSheetId, range);
            var response = request.Execute();
            var values = response.Values;

            List<FoodPlace> foodPlaceResult = FoodPlacesMapper.MapFromRangeData(values);
            foodPlaceResult.RemoveAll(r=> r.DistanceRating < distanceRating);
            foodPlaceResult.RemoveAll(r=> r.PriceRating < priceRating);
            foodPlaceResult.RemoveAll(r=> r.HealthRating < healthRating);
            foodPlaceResult.RemoveAll(r=> r.AvgRating < avgRating);
            

            if (response.Values != null && response.Values.Count > 0)
            {
              return Ok(foodPlaceResult);
            }
            else
            {
              return NotFound();
            }
            
        }
    }
}
