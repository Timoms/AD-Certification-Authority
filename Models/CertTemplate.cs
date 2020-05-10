using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibADCA.Models
{
    public class CertTemplate
    {
        public string TemplatePropCommonName { get; set; }
        public string TemplatePropFriendlyName { get; set; }
        public List<TemplatePropEKU> TemplatePropEKUs { get; set; }
        public List<string> TemplatePropCryptoProviders { get; set; }
        public string TemplatePropMajorRevision { get; set; }
        public string TemplatePropDescription { get; set; }
        public int TemplatePropSchemaVersion { get; set; }
        public int TemplatePropMinorRevision { get; set; }
        public int TemplatePropRASignatureCount { get; set; }
        public int TemplatePropMinimumKeySize { get; set; }
        public int TemplatePropMinimumKeySizeAlt { get; set; }
        public string TemplatePropOID { get; set; }
        public List<string> TemplatePropEnrollmentFlags { get; set; }
        public List<string> TemplatePropSubjectNameFlags { get; set; }
        public List<string> TemplatePropPrivateKeyFlags { get; set; }
        public List<string> TemplatePropGeneralFlags { get; set; }
        public string TemplatePropSecurityDescriptor { get; set; }
        public List<string> TemplatePropSecurityGroups { get; set; }
        public List<CertTemplateExtension> TemplatePropExtensions { get; set; }
        public string TemplatePropValidityPeriod { get; set; }
        public string TemplatePropRenewalPeriod { get; set; }

    }
}
