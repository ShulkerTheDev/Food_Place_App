async function getFoodPlace(DisRate, PriceRate, HealthRate, AvgRate){
  let resultData;
  let varData;
  await fetch(
    'https://localhost:5001/FoodPlace?' + new URLSearchParams(
      { distanceRating: DisRate, 
        priceRating: PriceRate,
        healthRating: HealthRate,
        avgRating: AvgRate
      }
    )
  ).then(response =>{
      return response.json();
    }
  ).then(data => {
      resultData = JSON.stringify(data);
    }
  );

  console.log(resultData);
  
  return resultData;
}
