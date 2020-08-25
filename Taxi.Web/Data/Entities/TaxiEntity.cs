using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taxi.Web.Data.Entities
{
    public class TaxiEntity
    {
        public int Id { get; set; }
        [StringLength(10, ErrorMessage = "{0} Max length is {1} and Minimun {2}", MinimumLength = 6)]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        public string Plaque { get; set; }

        public ICollection<TripEntity> Trips { get; set; }


    }
}
