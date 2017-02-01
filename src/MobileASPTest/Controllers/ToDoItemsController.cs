using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MobileASPTest.Interfaces;
using MobileASPTest.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MobileASPTest.Controllers
{
    [Route("api/[controller]")]
    public class ToDoItemsController : Controller
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoItemsController(IToDoRepository toDoRespository)
        {
            _toDoRepository = toDoRespository;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_toDoRepository.All);
        }

        [HttpPost("{id}")]
        public IActionResult Create(string id, [FromBody]ToDoItem item)
        {
            try
            {
                if(item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                bool itemExists = _toDoRepository.DoesItemExist(item.ID);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.TodoItemIDInUse.ToString());
                }
                _toDoRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return Ok(item);
        }

        [HttpPost("{id}")]
        public IActionResult Edit(string id, [FromBody]ToDoItem item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                var existingItem = _toDoRepository.DoesItemExist(item.ID);
                if (existingItem)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _toDoRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var item = _toDoRepository.Find(id);
                if(item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _toDoRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }

        public enum ErrorCode
        {
            TodoItemNameAndNotesRequired,
            TodoItemIDInUse,
            RecordNotFound,
            CouldNotCreateItem,
            CouldNotUpdateItem,
            CouldNotDeleteItem
        }



        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
