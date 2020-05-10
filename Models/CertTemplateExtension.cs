using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibADCA.Models
{
    public class CertTemplateExtension
    {
        public string Id { get; set; }
        public string Flags { get; set; }
        public string Length { get; set; }
        public string CertificateTemplateNameCertificateType { get; set; }
        public List<string> EnhancedKeyUsage { get; set; }
        public List<string> KeyUsage { get; set; }
    }
}
