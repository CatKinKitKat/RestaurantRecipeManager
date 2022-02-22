using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRecipeManager.API.Models;
using RestaurantRecipeManager.Data;
using RestaurantRecipeManager.Models;
using RestaurantRecipeManager.ViewModels;
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

                    ViewBag.Recipe = recipe;
                    ViewBag.Ingredients = ingredientListNames.Select(x => (x.name, x.quantity)).ToList();

                    return View();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
        }

        [HttpGet("{id:int}")]
        public IActionResult EditName(byte id)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    Recipe recipe = _context.Recipes.Find(id);

                    if (recipe == null)
                    {
                        return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
                    }

                    ViewBag.Recipe = recipe;

                    return View();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
        }

        [HttpGet("{id:int}")]
        public IActionResult EditIngredients(byte id)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    List<RecIngListItem> ingredientList = _context.RecIngLists.Where(x => x.RId == id).Select(x => new RecIngListItem(x.RilId, x.RId, x.IId, x.Quantity)).ToList();

                    if (ingredientList == null || ingredientList.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Recipe with id = " + id + " is known it's ingredients unknown. Call support." });
                    }

                    var ingredientListNames = new Dictionary<byte, string>();

                    foreach (var i in ingredientList)
                    {
                        var j = _context.Ingredients.Find(i._IId);
                        ingredientListNames.Add(j.IId, j.Name);
                    }

                    ViewBag.Ingredients = ingredientList.Select(x => (x._RilId, x._RId, x._IId, x._Quantity)).ToList();
                    ViewBag.NameDict = ingredientListNames;

                    return View();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
        }

        [HttpGet("{id:int}")]
        public IActionResult EditIngredient(byte id)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    RecIngList item = _context.RecIngLists.Find(id);

                    if (item == null)
                    {
                        return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
                    }

                    ViewBag.Item = item;

                    return View();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
        }

        //[HttpPut] Should be a post but HTML form doesn't allow
        [HttpPost]
        public IActionResult ApplyNameEdit(RecipeVM form)
        {
            try
            {
                Recipe recipe = _context.Recipes.Find(form.RId);

                if (recipe == null)
                {
                    return NotFound(new { error = "Recipe with id = " + form.RId + " does not exist." });
                }

                recipe.Name = form.Name;

                _context.Entry(recipe).State = EntityState.Modified;

                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Recipe", action = "View", id = form.RId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
            }
        }

        //[HttpPut] Should be a post but HTML form doesn't allow
        [HttpPost]
        public IActionResult ApplyIngredientEdit(RecIngListsVM form)
        {
            try
            {
                RecIngList item = _context.RecIngLists.Find(form.RilId);

                if (item == null)
                {
                    return NotFound(new { error = "Item with id = " + form.RilId + " does not exist." });
                }

                item.Quantity = form.Quantity;

                _context.Entry(item).State = EntityState.Modified;

                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Recipe", action = "View", id = form.RId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var lastRIL = _context.RecIngLists.OrderByDescending(x => x.RilId).FirstOrDefault();
                byte newRilId = 1;

                if (lastRIL != null)
                {
                    newRilId += lastRIL.RilId;
                }

                var lastR = _context.Recipes.OrderByDescending(x => x.RId).FirstOrDefault();
                byte newRId = 1;

                if (lastR != null)
                {
                    newRId += lastR.RId;
                }

                RecipeAddVM recipeAddModel = new RecipeAddVM()
                {
                    RId = newRId,
                    RilId = newRilId,
                    Name = "Recipe",
                    IId = 1,
                    Quantity = 1
                };

                ViewBag.Item = recipeAddModel;

                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
            }
        }

        [HttpGet("id:int")]
        public IActionResult AddIngredient(byte id)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    var lastRIL = _context.RecIngLists.OrderByDescending(x => x.RilId).FirstOrDefault();
                    byte newRilId = 1;

                    if (lastRIL != null)
                    {
                        newRilId += lastRIL.RilId;
                    }

                    RecIngListsVM recIngListsVM = new RecIngListsVM()
                    {
                        RilId = newRilId,
                        RId = id,
                        IId = 1,
                        Quantity = 1
                    };

                    ViewBag.Item = recIngListsVM;

                    return View();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return RedirectToRoute(new { controller = "Recipe", action = "View", id = id });
        }

        [HttpPost]
        public IActionResult ApplyAdd(RecipeAddVM form)
        {
            if (form.RId >= 1 && form.RId <= 255)
            {
                try
                {
                    Recipe recipeModel = new Recipe()
                    {
                        RId = form.RId,
                        Name = form.Name
                    };

                    Ingredient IIdNav = _context.Ingredients.Find(form.IId);

                    if (IIdNav == null)
                    {
                        return BadRequest(new { error = "Ingredient with id = " + form.IId + " does not exist." });
                    }

                    RecIngList recIngListModel = new RecIngList()
                    {
                        RilId = form.RilId,
                        RId = form.RId,
                        IId = form.IId,
                        Quantity = form.Quantity,
                        IIdNavigation = IIdNav,
                        RIdNavigation = recipeModel
                    };

                    _context.Recipes.Add(recipeModel);
                    _context.RecIngLists.Add(recIngListModel);

                    _context.SaveChanges();

                    return RedirectToRoute(new { controller = "Recipe", action = "View", id = form.RId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return RedirectToRoute(new { controller = "Recipe", action = "View", id = form.RId });
        }

        [HttpPost]
        public IActionResult ApplyAddIngredient(RecIngListsVM form)
        {
            if (form.RId >= 1 && form.RId <= 255)
            {
                try
                {
                    var recipeModel = _context.Recipes.Find(form.RId);

                    if (recipeModel == null)
                    {
                        return BadRequest(new { error = "Recipe with id = " + form.RId + " does not exist." });
                    }

                    Ingredient IIdNav = _context.Ingredients.Find(form.IId);

                    if (IIdNav == null)
                    {
                        return BadRequest(new { error = "Ingredient with id = " + form.IId + " does not exist." });
                    }

                    RecIngList recIngListModel = new RecIngList()
                    {
                        RilId = form.RilId,
                        RId = form.RId,
                        IId = form.IId,
                        Quantity = form.Quantity,
                        IIdNavigation = IIdNav,
                        RIdNavigation = recipeModel
                    };

                    _context.RecIngLists.Add(recIngListModel);

                    _context.SaveChanges();

                    return RedirectToRoute(new { controller = "Recipe", action = "View", id = form.RId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return RedirectToRoute(new { controller = "Recipe", action = "View", id = form.RId });
        }

        //[HttpDelete("{id:int}")] Should be a delete but HTML href doesn't allow
        [HttpGet("{id:int}")]
        public IActionResult DeleteIngredient(byte id)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    var recIngList = _context.RecIngLists.Find(id);

                    if (recIngList == null)
                    {
                        return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
                    }

                    _context.RecIngLists.Remove(recIngList);

                    _context.SaveChanges();

                    return RedirectToRoute(new { controller = "Recipe", action = "All" });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Recipe with id = " + id + " does not exist." });
        }

        //[HttpDelete("{id:int}")] Should be a delete but HTML href doesn't allow
        [HttpGet("{id:int}")]
        public IActionResult Delete(byte id)
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

                    return RedirectToRoute(new { controller = "Recipe", action = "All" });
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