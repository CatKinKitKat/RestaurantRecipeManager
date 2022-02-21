using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantRecipeManager.Data;
using RestaurantRecipeManager.Models.Compound;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantRecipeManager.Controllers
{
    [Route("[controller]/[action]")]
    public class RecipeController : Controller
    {
        private RestaurantRecipeManagerContext _context;

        public RecipeController(RestaurantRecipeManagerContext context)
        {
            _context = context;
        }

        private class IngredientListItem
        {
            public byte id { get; set; }
            public string name { get; set; }
            public byte quantity { get; set; }

            public IngredientListItem(byte IId, string Name, byte Quantity)
            {
                id = IId;
                name = Name;
                quantity = Quantity;
            }
        }

        private class RecIngListItem
        {
            public byte _RilId { get; set; }
            public byte _RId { get; set; }
            public byte _IId { get; set; }
            public byte _Quantity { get; set; }

            public RecIngListItem(byte RilId, byte RId, byte IId, byte Quantity)
            {
                _RilId = RilId;
                _RId = RId;
                _IId = IId;
                _Quantity = Quantity;
            }
        }

        [HttpGet]
        public IActionResult All()
        {
            try
            {
                var recipes = _context.Recipes.ToList();

                List<RecipeModel> returnList = new List<RecipeModel>();
                Dictionary<byte, string> keyValuePairs = new Dictionary<byte, string>();

                foreach (var recipe in recipes)
                {
                    var ingredientList = _context.RecIngLists.Where(x => x.RId == recipe.RId)
                        .Select(x => new RecipeModel.Ingredient(x.IId, x.Quantity))
                        .ToList()
                        .AsEnumerable();

                    RecipeModel recipeModel = new RecipeModel()
                    {
                        RId = recipe.RId,
                        Name = recipe.Name,
                        Ingredients = ingredientList
                    };

                    foreach (var toName in ingredientList)
                    {
                        if (!keyValuePairs.ContainsKey(toName.IId))
                        {
                            var found = _context.Ingredients.Find(toName.IId);
                            keyValuePairs.Add(found.IId, found.Name);
                        }
                    }

                    returnList.Add(recipeModel);
                }

                if (returnList == null || returnList.Count < 1)
                {
                    return NoContent();
                }

                ViewBag.AllRecipes = returnList;
                ViewBag.LazyTranslate = keyValuePairs;

                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult View(byte id)
        {
            return View();
        }

        [HttpPut]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPut]
        public IActionResult Add()
        {
            return View();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(byte id)
        {
            return View();
        }
    }
}