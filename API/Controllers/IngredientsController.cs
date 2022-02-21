using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRecipeManager.Data;
using RestaurantRecipeManager.Models;
using RestaurantRecipeManager.Models.Compound;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantRecipeManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class IngredientsController : ControllerBase
    {
        private RestaurantRecipeManagerContext _context;

        public IngredientsController(RestaurantRecipeManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult List()
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

                    return Ok(new { name = ingredient.Name, quantity = stock.Quantity });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Ingredient with id = " + id + " does not exist." });
        }

        [HttpPost]
        public IActionResult Add([FromBody] IngredientModel body)
        {
            if (body.IId >= 1 && body.IId <= 255)
            {
                try
                {
                    var ingredient = _context.Ingredients.Find(body.IId);
                    var stock = _context.Stock.Where(x => x.IId == body.IId).FirstOrDefault();

                    if (ingredient != null)
                    {
                        return BadRequest(new { error = "Ingredient with id = " + body.IId + " already exists." });
                    }

                    if (stock != null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ingredient with id = " + body.IId + " is unknown but it's in stock. Call support." });
                    }

                    Ingredient ingredientModel = new Ingredient()
                    {
                        IId = body.IId,
                        Name = body.Name
                    };

                    Stock stockModel = new Stock()
                    {
                        IId = body.IId,
                        Quantity = body.Quantity
                    };

                    _context.Ingredients.Add(ingredientModel);
                    _context.Stock.Add(stockModel);

                    _context.SaveChanges();

                    return Ok(new { status = "Added", id = body.IId, name = body.Name, quantity = body.Quantity });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return BadRequest(new { error = "Object with id = " + body.IId + " already exists." });
        }

        [HttpPut]
        public IActionResult Edit([FromBody] IngredientModel body)
        {
            if (body.IId >= 1 && body.IId <= 255)
            {
                try
                {
                    var ingredient = _context.Ingredients.Find(body.IId);
                    var stock = _context.Stock.Find(body.IId);

                    if (ingredient == null)
                    {
                        return NotFound(new { error = "Ingredient with id = " + body.IId + " does not exist." });
                    }

                    if (stock == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ingredient with id = " + body.IId + " is known but does not exist. Call support." });
                    }

                    ingredient.IId = body.IId;
                    ingredient.Name = body.Name;

                    stock.IId = body.IId;
                    stock.Quantity = body.Quantity;

                    _context.Entry(ingredient).State = EntityState.Modified;
                    _context.Entry(stock).State = EntityState.Modified;

                    _context.SaveChanges();

                    return Ok(new { status = "Updated", id = body.IId, name = body.Name, quantity = body.Quantity });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Ingredient with id = " + body.IId + " does not exist." });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(byte id = 0)
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

                    return Ok(new { status = "Removed", id = id, name = ingredient.Name });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { error = "There was an error processing your request." });
                }
            }

            return NotFound(new { error = "Ingredient with id = " + id + " does not exist." });
        }
    }
}