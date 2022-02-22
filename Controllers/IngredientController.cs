using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRecipeManager.API.Models;
using RestaurantRecipeManager.Data;
using RestaurantRecipeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantRecipeManager.Controllers
{
    [Route("[controller]/[action]")]
    public class IngredientController : Controller
    {
        private RestaurantRecipeManagerContext _context;

        public IngredientController(RestaurantRecipeManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult All()
        {
            try
            {
                var ingredients = _context.Ingredients.ToList();

                List<IngredientModel> returnList = new List<IngredientModel>();

                foreach (var ingredient in ingredients)
                {
                    var shelf = _context.Stock.Where(x => x.IId == ingredient.IId).FirstOrDefault();
                    byte stock = 0;
                    if (shelf != null)
                    {
                        stock = shelf.Quantity;
                    }

                    IngredientModel ingredientModel = new IngredientModel()
                    {
                        IId = ingredient.IId,
                        Name = ingredient.Name,
                        Quantity = stock
                    };

                    returnList.Add(ingredientModel);
                }

                if (returnList == null || returnList.Count < 1)
                {
                    return NoContent();
                }

                ViewBag.AllIngredients = returnList;

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
                var ingredient = _context.Ingredients.Find(id);
                var stock = _context.Stock.Where(x => x.IId == id).FirstOrDefault();

                if (ingredient == null)
                {
                    return NotFound(new { error = "Ingredient with id = " + id + " does not exist." });
                }

                if (stock == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ingredient with id = " + id + " is known but does not exist. Call support." });
                }

                IngredientModel ingredientModel = new IngredientModel()
                {
                    IId = ingredient.IId,
                    Name = ingredient.Name,
                    Quantity = stock.Quantity
                };

                ViewBag.Ingredient = ingredientModel;

                return View();
            }

            return NotFound(new { error = "Ingredient with id = " + id + " does not exist." });
        }

        [HttpGet("{id:int}")]
        public IActionResult Edit(byte id)
        {
            if (id >= 1 && id <= 255)
            {
                var ingredient = _context.Ingredients.Find(id);
                var stock = _context.Stock.Where(x => x.IId == id).FirstOrDefault();

                if (ingredient == null)
                {
                    return NotFound(new { error = "Ingredient with id = " + id + " does not exist." });
                }

                if (stock == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ingredient with id = " + id + " is known but does not exist. Call support." });
                }

                IngredientModel ingredientModel = new IngredientModel()
                {
                    IId = ingredient.IId,
                    Name = ingredient.Name,
                    Quantity = stock.Quantity
                };

                ViewBag.Ingredient = ingredientModel;

                return View();
            }

            return NotFound(new { error = "Ingredient with id = " + id + " does not exist." });
        }

        //[HttpPut] Should be a post but HTML form doesn't allow
        [HttpPost]
        public IActionResult ApplyEdit(IngredientModel form)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "Ingredient", action = "Edit", id = form.IId });
            }

            if (form.IId >= 1 && form.IId <= 255)
            {
                try
                {
                    var ingredient = _context.Ingredients.Find(form.IId);
                    var stock = _context.Stock.Find(form.IId);

                    if (ingredient == null)
                    {
                        return NotFound(new { error = "Ingredient with id = " + form.IId + " does not exist." });
                    }

                    if (stock == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ingredient with id = " + form.IId + " is known but does not exist. Call support." });
                    }

                    ingredient.IId = form.IId;
                    ingredient.Name = form.Name;

                    stock.IId = form.IId;
                    stock.Quantity = form.Quantity;

                    _context.Entry(ingredient).State = EntityState.Modified;
                    _context.Entry(stock).State = EntityState.Modified;

                    _context.SaveChanges();

                    return RedirectToRoute(new { controller = "Ingredient", action = "View", id = form.IId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return RedirectToRoute(new { controller = "Ingredient", action = "Edit", id = form.IId });
        }

        [HttpGet]
        public IActionResult Add()
        {

            var last = _context.Ingredients.OrderByDescending(x => x.IId).FirstOrDefault();
            byte newIId = 1;

            if (last != null)
            {
                newIId += last.IId;
            }

            ViewBag.NewIId = newIId;

            return View();
        }

        [HttpPost]
        public IActionResult ApplyAdd(IngredientModel form)
        {
            if (form.IId >= 1 && form.IId <= 255)
            {
                try
                {
                    var ingredient = _context.Ingredients.Find(form.IId);
                    var stock = _context.Stock.Where(x => x.IId == form.IId).FirstOrDefault();

                    if (ingredient != null)
                    {
                        return BadRequest(new { error = "Ingredient with id = " + form.IId + " already exists." });
                    }

                    if (stock != null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ingredient with id = " + form.IId + " is unknown but it's in stock. Call support." });
                    }

                    Ingredient ingredientModel = new Ingredient()
                    {
                        IId = form.IId,
                        Name = form.Name
                    };

                    Stock stockModel = new Stock()
                    {
                        IId = form.IId,
                        Quantity = form.Quantity
                    };

                    _context.Ingredients.Add(ingredientModel);
                    _context.Stock.Add(stockModel);

                    _context.SaveChanges();

                    return RedirectToRoute(new { controller = "Ingredient", action = "View", id = form.IId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return RedirectToRoute(new { controller = "Ingredient", action = "All" });
        }

        //[HttpDelete("{id:int}")] Should be a delete but HTML href doesn't allow
        [HttpGet("{id:int}")]
        public IActionResult Delete(byte id)
        {
            if (id >= 1 && id <= 255)
            {
                try
                {
                    var ingredient = _context.Ingredients.Find(id);
                    var stock = _context.Stock.Where(x => x.IId == id).FirstOrDefault();

                    if (ingredient == null)
                    {
                        return NotFound(new { error = "Ingredient with id = " + id + " does not exist." });
                    }

                    if (stock == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ingredient with id = " + id + " is known but does not exist. Call support." });
                    }

                    _context.Ingredients.Remove(ingredient);
                    _context.Stock.Remove(stock);

                    _context.SaveChanges();

                    return RedirectToRoute(new { controller = "Ingredient", action = "All" });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return RedirectToRoute(new { controller = "Ingredient", action = "View", id = id });
        }
    }
}