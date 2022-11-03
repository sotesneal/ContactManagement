using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContactsApplication.Services
{
    public interface IContactsService
    {
        IEnumerable<Contact> GetContacts();
        Contact GetContact(int id);
        void AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
        bool SaveChanges();
    }

    public class JsonFileContactsService : IContactsService
    {
        string fileName;
        string jsonString;
        Contact[] contacts;
        public JsonFileContactsService()
        {
             fileName = "contacts.json";
             jsonString = File.ReadAllText(fileName);
             contacts = JsonSerializer.Deserialize<Contact[]>(jsonString)!;
        }
        
        public void AddContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            foreach (var c in contacts)
            {
                if (c.Id == contact.Id)
                {
                    throw new ArgumentException("Contact with this id already exists");
                }
            }
            var newContacts = new Contact[contacts.Length + 1];
            Array.Copy(contacts, newContacts, contacts.Length);
            newContacts[newContacts.Length - 1] = contact;
            contacts = newContacts;
            jsonString = JsonSerializer.Serialize(contacts);
            File.WriteAllText(fileName, jsonString);
        }

        public void DeleteContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            foreach (var c in contacts)
            {
                if (c.Id == contact.Id)
                {
                    var newContacts = new Contact[contacts.Length - 1];
                    var i = 0;
                    foreach (var c1 in contacts)
                    {
                        if (c1.Id != contact.Id)
                        {
                            newContacts[i] = c1;
                            i++;
                        }
                    }
                    contacts = newContacts;
                    jsonString = JsonSerializer.Serialize(contacts);
                    File.WriteAllText(fileName, jsonString);
                    return;
                }
            }
            throw new ArgumentException("Contact with this id does not exist");
        }

        public Contact GetContact(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id)!;
        }

        public IEnumerable<Contact> GetContacts()
        {
            return contacts.ToList();
        }

        public bool SaveChanges()
        {
            return (contacts.Length > 0);
        }

        public void UpdateContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            foreach (var c in contacts)
            {
                if (c.Id == contact.Id)
                {
                    c.FirstName = contact.FirstName;
                    c.LastName = contact.LastName;
                    c.BillingAddress = contact.BillingAddress;
                    c.PhysicalAddress = contact.PhysicalAddress;
                    jsonString = JsonSerializer.Serialize(contacts);
                    File.WriteAllText(fileName, jsonString);
                    return;
                }
            }
            throw new ArgumentException("Contact with this id does not exist");

        }

      
    }




    public class Contact
    {
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BillingAddress { get; set; }
        public string PhysicalAddress { get; set; }

       
    }
}