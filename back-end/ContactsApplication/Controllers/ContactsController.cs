using ContactsApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService _contactsService;

        public ContactsController(IContactsService contactsService)
        {
            _contactsService = contactsService;
        }
        // GET: api/Contacts
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> Get()
        {
            var contacts = _contactsService.GetContacts();
            return Ok(contacts);
        }

        // GET: api/Contacts/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Contacts
        [HttpPost]
        public ActionResult <Contact> CreateContact([FromBody] Contact contact)
        {
            _contactsService.AddContact(contact);
            _contactsService.SaveChanges();
            return CreatedAtAction(nameof(CreateContact), new { id = contact.Id }, contact);
        }
        //DELETE: api/Contacts/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            var contact = _contactsService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }
            _contactsService.DeleteContact(contact);
            _contactsService.SaveChanges();
            return NoContent();
        }
        // PUT: api/Contacts/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateContact(int id, [FromBody] Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }
            var existingContact = _contactsService.GetContact(id);
            if (existingContact == null)
            {
                return NotFound();
            }
            _contactsService.UpdateContact(contact);
            _contactsService.SaveChanges();
            return NoContent();
        }

    }
}
