using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModels
{
    public class CreateItemViewModel
    {
        [Display(Name = "Имя айтема")]
        public string Name { get; set; }

        [Display(Name = "Описание айтема")]
        public string Description { get; set; }
        public int collectionId { get; set; }
    }
}