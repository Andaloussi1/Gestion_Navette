using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace navette.Models
{
    public class UserSociete
    {
       
        public USER_APP USER { get; set;}
      
        public Societe societe { get; set; }
    }
}