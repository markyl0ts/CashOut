using CashOut.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata;
using CashOut.Helpers;
using CashOut.Models;

namespace CashOut.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService) 
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var contacts = _contactService.GetAll();
            if(!contacts.Any())
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.RecordNotFound, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = contacts, message = Constants.Success });
        }

        [HttpGet]
        [Route("{contactId}")]
        public IActionResult GetById(int contactId)
        {
            var contact = _contactService.GetById(contactId);
            if(contact == null)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.RecordNotFound, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = contact, message = Constants.Success });
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(Contact contact)
        {
            var obj = _contactService.Add(contact);
            if (contact == null)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.FailedToCreate, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = obj, message = Constants.Success });
        }

        [HttpDelete]
        [Route("delete/{contactId}")]
        public IActionResult DeleteById(int contactId)
        {
            int res = _contactService.Delete(contactId);
            if(res == 0)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.FailedToDelete, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = res, message = Constants.Success });
        }

        [HttpPut]
        [Route("update")]
        public IActionResult Update(Contact contact)
        {
            int res = _contactService.Update(contact);
            if (res == 0)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.FailedToUpdate, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = res, message = Constants.Success });
        }
    }
}
