using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModels
{
    public class CreateCollectionViewModel
    {
        [Display(Name="Имя")]
        public string Name { get; set; }

        [Display(Name="Описание")]
        public string Description { get; set; }
        //public string AvatarLink { get; set; }
        
    }
}