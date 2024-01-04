using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderFoodApplication.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

// ... (your existing using statements)

namespace OrderFoodApplication.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private readonly string apiURL = "https://forkify-api.herokuapp.com/api/v2/recipes";
        private readonly string apiKey = "64092a23-9828-47cb-ba25-f0427a05e9d7";

        public async Task<IActionResult> Index()
        {
            // Get recipes for each category
            List<Recipe> pizzaRecepie = await GetRecipesAsync("pizza", false, "Pizza");
            List<Recipe> cakeRecepie = await GetRecipesAsync("cake", false, "Cake");
            List<Recipe> chickenRecepie = await GetRecipesAsync("chicken", false, "Chicken");
            List<Recipe> chocolateRecepie = await GetRecipesAsync("chocolate", false, "Chocolate");
            // Add similar lines for other categories

            // Combine recipes for different categories if needed
            List<Recipe> allRecipes = new List<Recipe>();
            allRecipes.AddRange(pizzaRecepie);
            allRecipes.AddRange(cakeRecepie);
            allRecipes.AddRange(chickenRecepie);
            allRecipes.AddRange(chocolateRecepie);
            // Add similar lines for other categories

            return View(allRecipes);
        }

        [HttpPost]
        public IActionResult GetRecipeCard([FromBody] List<Recipe> recipes)
        {
            return PartialView("_RecipeCard", recipes);
        }

        private async Task<List<Recipe>> GetRecipesAsync(string recipeCategory, bool isAllShow, string categoryForModel)
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                string apiUrl = $"{apiURL}?search={recipeCategory}&key={apiKey}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ForkifyApiResponse>(jsonResult);

                    // Assign the category to each recipe
                    List<Recipe> recipes = isAllShow
                        ? result.data.recipes.Select(r => { r.Category = categoryForModel; return r; }).ToList()
                        : result.data.recipes.Take(8).Select(r => { r.Category = categoryForModel; return r; }).ToList();

                    return recipes;
                }
                else
                {
                    throw new HttpRequestException($"API request failed with status code {response.StatusCode}");
                }
            }
        }
    }
}

