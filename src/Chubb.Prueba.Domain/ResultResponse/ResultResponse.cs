using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.Entities.Result
{
    public class ResultResponse
    {
        public string? Code {  get; set; }
        public dynamic? Data {  get; set; }
        public string? Message{ get; set; }
    }
}
