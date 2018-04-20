using System;
using NativeDemo.Data;
using SQLite;



namespace NativeDemo.Models
{
    public class User :IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int DbId
        {
            get; set;
        }


        public DateTime UpdatedOn
        {
            get; set;
        }
        public string ImageSource { get; set; }
        public int ResourceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string About { get; set; }
        public DateTime Dob { get; set; }
        public string City { get; set; }
     

    
    }
}


