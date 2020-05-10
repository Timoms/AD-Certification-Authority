using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibADCA.Models
{
    public class ProcessResult
    {
        public List<string> Output { get; set; }
        public string OutputText { get; set; }
        public bool EncounteredError { get; set; }
        public int Duration { get; set; }
    }
}
