using NativeDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NativeDemo.Models
{
    public class UserData
    {
        public  List<User> Users { get; set; }

        public UserData()
        {
           
            var DBUsers = Repository.Find<User>(x => true);
       
           Users = DBUsers.OrderBy(i => i.FirstName).ToList();
        }

    }
}
