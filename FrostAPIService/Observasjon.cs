using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostAPIService
{
    internal class Observasjon
    {
        public int Id { get; set; }
        public DateTime? Dato { get; set; } = DateTime.MinValue;
        public double? Temperatur { get; set; }
    }
}
