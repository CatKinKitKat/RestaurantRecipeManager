using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRecipeManager.API.Models;
using RestaurantRecipeManager.Data;
using RestaurantRecipeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantRecipeManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RecipesController : ControllerBase
    {
        private RestaurantRecipeManagerContext _context;

        public RecipesController(RestaurantRecipeManagerContext context)
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
        public IActionResult List()
        {
            try
            {
                var recipes = _context.Recipes.ToList();

                List<RecipeModel> returnList = new List<RecipeModel>();

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

                    returnList.Add(recipeModel);
                }

                if (returnList == null || returnList.Count < 1)
                {
                    return NoContent();
                }

                return Ok(returnList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult View(byte id = 1)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    var recipe = _context.Recipes.Find(id);
                    var ingredientList = _context.RecIngLists.Where(x => x.RId == id).Select(x => new { x.IId, x.Quantity }).ToList();

                    if (recipe == null)
                    {
                        return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
                    }

                    if (ingredientList == null || ingredientList.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Recipe with id = " + id + " is known it's ingredients unknown. Call support." });
                    }

                    var ingredientListNames = new List<IngredientListItem>();

                    foreach (var i in ingredientList)
                    {
                        var j = _context.Ingredients.Find(i.IId);
                        IngredientListItem item = new IngredientListItem(i.IId, j.Name, i.Quantity);
                        ingredientListNames.Add(item);
                    }

                    return Ok(new { name = recipe.Name, ingredients = ingredientListNames });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
        }

        [HttpPost]
        public IActionResult Add([FromBody] RecipeModel body)
        {
            if (body.RId >= 1 && body.RId <= 255)
            {
                try
                {
                    var recipe = _context.Recipes.Find(body.RId);
                    List<RecipeModel.Ingredient> addedIngredients = body.Ingredients.ToList();

                    if (addedIngredients == null || addedIngredients.Count < 1)
                    {
                        return BadRequest(new { error = "Empty Ingredient List. 0" });
                    }

                    if (recipe != null)
                    {
                        return BadRequest(new { error = "Recipe with id = " + body.RId + " already exists." });
                    }

                    var RecIngListList = new List<RecIngListItem>();

                    int count = 1;

                    foreach (var i in addedIngredients)
                    {
                        byte j = Convert.ToByte(_context.RecIngLists.Count() + count);
                        RecIngListItem item = new RecIngListItem(j, body.RId, i.IId, i.Quantity);
                        RecIngListList.Add(item);
                        count++;
                    }

                    if (RecIngListList == null || RecIngListList.Count < 1)
                    {
                        return BadRequest(new { error = "Empty Ingredient List. 1" });
                    }

                    Recipe recipeModel = new Recipe()
                    {
                        RId = body.RId,
                        Name = body.Name
                    };

                    _context.Recipes.Add(recipeModel);

                    List<RecIngList> recIngListModelList = new List<RecIngList>();

                    foreach (var item in RecIngListList)
                    {
                        Ingredient IIdNav = _context.Ingredients.Find(item._IId);
                        RecIngList recIngListModel = new RecIngList()
                        {
                            RilId = item._RilId,
                            RId = item._RId,
                            IId = item._IId,
                            Quantity = item._Quantity,
                            IIdNavigation = IIdNav,
                            RIdNavigation = recipeModel
                        };
                        recIngListModelList.Add(recIngListModel);
                    }

                    if (recIngListModelList == null || recIngListModelList.Count < 1)
                    {
                        return BadRequest(new { error = "Empty Ingredient List. 2" });
                    }

                    _context.RecIngLists.AddRange(recIngListModelList);
                    _context.SaveChanges();

                    return Ok(new { status = "Added", id = body.RId, name = body.Name, ingredients = body.Ingredients });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return BadRequest(new { error = "Recipe with id = " + body.RId + " does not exist." });
        }

        [HttpPut]
        public IActionResult Edit([FromBody] RecipeModel body)
        {
            if (body.RId >= 1 && body.RId <= 255)
            {
                try
                {
                    var recipe = _context.Recipes.Find(body.RId);
                    List<RecipeModel.Ingredient> addedIngredients = body.Ingredients.ToList();

                    if (addedIngredients == null || addedIngredients.Count < 1)
                    {
                        return BadRequest(new { error = "Empty Ingredient List. 0" });
                    }

                    if (recipe == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Recipe with id = " + body.RId + " does not exist." });
                    }

                    var ingredientList = _context.RecIngLists.Where(x => x.RId == body.RId).Select(x => x).ToList();
                    List<byte> RilIds = (from ril in ingredientList select ril.RilId).ToList();

                    if (ingredientList == null || ingredientList.Count < 1)
                    {
                        return BadRequest(new { error = "Empty Ingredient List. 1" });
                    }

                    var RecIngListList = new List<RecIngListItem>();

                    int count = 1;
                    int rCount = 0;

                    foreach (var i in addedIngredients)
                    {
                        if (rCount < RilIds.Count)
                        {
                            byte j = RilIds[rCount];
                            var toUpdate = _context.RecIngLists.Find(j);
                            toUpdate.Quantity = i.Quantity;
                            _context.Entry(toUpdate).State = EntityState.Modified;
                            rCount++;
                            _context.SaveChanges();
                        }
                        else
                        {
                            byte j = Convert.ToByte(_context.RecIngLists.Count() + count);
                            RecIngListItem item = new RecIngListItem(j, body.RId, i.IId, i.Quantity);
                            RecIngListList.Add(item);
                            count++;
                        }
                    }

                    if (RecIngListList == null || RecIngListList.Count > 1)
                    {
                        List<RecIngList> recIngListModelList = new List<RecIngList>();

                        foreach (var item in RecIngListList)
                        {
                            Ingredient IIdNav = _context.Ingredients.Find(item._IId);
                            RecIngList recIngListModel = new RecIngList()
                            {
                                RilId = item._RilId,
                                RId = item._RId,
                                IId = item._IId,
                                Quantity = item._Quantity,
                                IIdNavigation = IIdNav,
                                RIdNavigation = recipe
                            };
                            recIngListModelList.Add(recIngListModel);
                        }

                        if (recIngListModelList == null || recIngListModelList.Count < 1)
                        {
                            return BadRequest(new { error = "Empty Ingredient List. 2" });
                        }

                        _context.RecIngLists.AddRange(recIngListModelList);

                        _context.SaveChanges();

                        return Ok(new { status = "Added", id = body.RId, name = body.Name, ingredients = body.Ingredients });
                    }

                    return Ok(new { status = "Added", id = body.RId, name = body.Name, ingredients = body.Ingredients });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return BadRequest(new { error = "Recipe with id = " + body.RId + " does not exist." });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(byte id = 0)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    var recipe = _context.Recipes.Find(id);
                    var ingredientList = _context.RecIngLists.Where(x => x.RId == id).Select(x => x).ToList();

                    if (recipe == null)
                    {
                        return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
                    }

                    _context.Recipes.Remove(recipe);

                    if (ingredientList != null || ingredientList.Count > 0)
                    {
                        _context.RecIngLists.RemoveRange(ingredientList);
                    }

                    _context.SaveChanges();

                    return Ok(new { status = "Removed", id = id, name = recipe.Name });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
        }
    }
}