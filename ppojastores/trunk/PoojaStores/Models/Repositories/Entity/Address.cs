using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int? AddressTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string LocationStreet { get; set; }
        public string LandMark { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public bool? IsDeleted { get; set; }
        public string ZipCode { get; set; }
        public int? UserId { get; set; }
        public string IsDeliverAddress { get; set; }
        public string FullName { get; set; }
    }
}
