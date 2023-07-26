using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class ApplicationErrorLog
    {
        public int ID { get; set; }
        public string Error { get; set; }
        public string Stacktrace { get; set; }
        public string InnerException { get; set; }
        public string Source { get; set; }
        public DateTime? ExceptionDateTime { get; set; }
    }
}
