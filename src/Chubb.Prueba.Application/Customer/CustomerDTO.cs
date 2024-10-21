using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.DTOs.Customer
{
    public class CustomerDTO
    {
        public int? CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Cedula { get; set; }
        public string? Telephone { get; set; }
        public DateTime? DateBorn { get; set; }
        public string? Email { get; set; }
        public string? RepresentLegal { get; set; }
        public int? StatusId { get; set; }
        public int? UserCreateId { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? UserModificateId { get; set; }
        public DateTime? DateModificate { get; set; }
    }
}
