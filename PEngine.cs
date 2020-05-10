using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LibADCA;
using LibADCA.Models;

namespace LibADCA
{
    public class PEngine
    {
        /// <summary>
        /// Returns the parsed list of CertTemplates from a AD
        /// </summary>
        /// <param name="AdURL">Your Domain to crawl</param>
        /// <returns>List of parsed ADCA.Models.CertTemplate</returns>
        public List<CertTemplate> ParseTemplatesFromAD()
        {
            // Run command: certutil -v -template
            ProcessResult processResult = Command.ExecuteCmd("certutil -v -template");
            List<List<string>> rawTemplates = ParseTemplateNumbers(processResult.Output);

            List<CertTemplate> certTemplates = new List<CertTemplate>();
            rawTemplates.ForEach(e =>
            {
                certTemplates.Add(TemplateParse(e, false, null));
            });
            return certTemplates;
        }

        /// <summary>
        /// Parses all templates from a raw command output
        /// </summary>
        /// <param name="templates">command output</param>
        /// <returns>raw list of lines, each list represents one template</returns>
        public List<List<string>> ParseTemplateNumbers(List<string> templates)
        {
            List<TemplateRange> templateRanges = new List<TemplateRange>();
            List<List<string>> _retTemplates = new List<List<string>>();

            templates.RemoveRange(0, 5); // Remove header of command (Name, ID, URL)
            int templateId = 0;
            int _tempStart = 0;
            for (int i = 0; i < templates.Count; i++)
            {
                if (templates[i].Contains($"Template[{templateId}]"))
                {
                    _tempStart = i;
                }
                if (templates[i].Contains($"Template[{templateId + 1}]"))
                {
                    int _tempEnd = i - 1;
                    templateRanges.Add(new TemplateRange() { FromLine = _tempStart, TemplateId = templateId, ToLine = _tempEnd });
                    _tempStart = _tempEnd + 1;
                    templateId++;
                }
            }

            templateRanges.ForEach(e =>
            {
                _retTemplates.Add(templates.ToArray()[e.FromLine..e.ToLine].ToList());
            });

            return _retTemplates;
        }


        /// <summary>
        /// Parses the AD Template command into a class
        /// </summary>
        /// <param name="fromFile">Weather to parse the commandoutput from a file</param>
        /// <param name="filePath">Path of the file with the command result</param>
        /// <returns>A parsed CertTemplate List</returns>
        public CertTemplate TemplateParse(List<string> text = null, bool fromFile = false, string filePath = "")
        {
            CertTemplate certTemplate = new CertTemplate();
            var lines = new List<string>();
            if (fromFile)
            {
                lines = File.ReadAllLines(filePath).ToList();
            }
            else
            {
                lines = text;
            }

            if (lines.Count == 0)
            {
                return null;
            }

            certTemplate.TemplatePropCommonName = lines.Where(e => e.Contains("TemplatePropCommonName")).FirstOrDefault().EqualElement();
            certTemplate.TemplatePropFriendlyName = lines.Where(e => e.Contains("TemplatePropFriendlyName")).FirstOrDefault().EqualElement();
            var index = 0;
            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropEKUs")).FirstOrDefault());


            var tpeku = new List<TemplatePropEKU>();
            int curLine = index + 2;
            while (lines[curLine] != "")
            {
                lines[curLine] = lines[curLine].Replace("    ", "");
                TemplatePropEKU x = new TemplatePropEKU() { Id = lines[curLine].FirstWord(2), Name = lines[curLine].LastWord(2) };
                tpeku.Add(x);
                curLine++;
            }
            certTemplate.TemplatePropEKUs = tpeku;

            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropCryptoProviders")).FirstOrDefault());
            certTemplate.TemplatePropCryptoProviders = new List<string>();
            curLine = index + 1;
            while (lines[curLine] != "")
            {
                lines[curLine] = lines[curLine].Replace("    ", "");
                certTemplate.TemplatePropCryptoProviders.Add(lines[curLine].LastWord(2));
                curLine++;
            }

            certTemplate.TemplatePropMajorRevision = lines.Where(e => e.Contains("TemplatePropMajorRevision")).FirstOrDefault().EqualElement();
            certTemplate.TemplatePropDescription = lines.Where(e => e.Contains("TemplatePropDescription")).FirstOrDefault().EqualElement();
            certTemplate.TemplatePropSchemaVersion = int.Parse(lines.Where(e => e.Contains("TemplatePropSchemaVersion")).FirstOrDefault().EqualElement());
            certTemplate.TemplatePropMinorRevision = int.Parse(lines.Where(e => e.Contains("TemplatePropMinorRevision")).FirstOrDefault().EqualElement());
            certTemplate.TemplatePropRASignatureCount = int.Parse(lines.Where(e => e.Contains("TemplatePropRASignatureCount")).FirstOrDefault().EqualElement());
            certTemplate.TemplatePropMinimumKeySize = int.Parse(lines.Where(e => e.Contains("TemplatePropMinimumKeySize")).FirstOrDefault().EqualElement().Split(' ')[0]);
            certTemplate.TemplatePropMinimumKeySizeAlt = int.Parse(lines.Where(e => e.Contains("TemplatePropMinimumKeySize")).FirstOrDefault().EqualElement().Split(' ')[1].Trim('(', ')'));

            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropOID")).FirstOrDefault());
            certTemplate.TemplatePropOID = lines[index + 1].Trim();


            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropEnrollmentFlags")).FirstOrDefault());
            certTemplate.TemplatePropEnrollmentFlags = new List<string>();
            curLine = index + 1;
            while (lines[curLine] != "")
            {
                lines[curLine] = lines[curLine].Replace("    ", "");
                certTemplate.TemplatePropEnrollmentFlags.Add(lines[curLine]);
                curLine++;
            }

            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropSubjectNameFlags")).FirstOrDefault());
            certTemplate.TemplatePropSubjectNameFlags = new List<string>();
            curLine = index + 1;
            while (lines[curLine] != "")
            {
                lines[curLine] = lines[curLine].Replace("    ", "");
                certTemplate.TemplatePropSubjectNameFlags.Add(lines[curLine]);
                curLine++;
            }

            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropPrivateKeyFlags")).FirstOrDefault());
            certTemplate.TemplatePropPrivateKeyFlags = new List<string>();
            curLine = index + 1;
            while (lines[curLine] != "")
            {
                lines[curLine] = lines[curLine].Replace("    ", "");
                certTemplate.TemplatePropPrivateKeyFlags.Add(lines[curLine]);
                curLine++;
            }

            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropGeneralFlags")).FirstOrDefault());
            certTemplate.TemplatePropGeneralFlags = new List<string>();
            curLine = index + 1;
            while (lines[curLine] != "")
            {
                lines[curLine] = lines[curLine].Replace("    ", "");
                certTemplate.TemplatePropGeneralFlags.Add(lines[curLine]);
                curLine++;
            }

            certTemplate.TemplatePropSecurityDescriptor = lines.Where(e => e.Contains("TemplatePropSecurityDescriptor")).FirstOrDefault().EqualElement();

            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropSecurityDescriptor")).FirstOrDefault());
            certTemplate.TemplatePropSecurityGroups = new List<string>();
            curLine = index + 2;
            while (lines[curLine] != "")
            {
                lines[curLine] = lines[curLine].Replace("    ", "");
                certTemplate.TemplatePropSecurityGroups.Add(lines[curLine]);
                curLine++;
            }

            index = lines.IndexOf(lines.Where(e => e.Contains("TemplatePropExtensions")).FirstOrDefault());
            certTemplate.TemplatePropExtensions = new List<CertTemplateExtension>();
            int curExtension = 0;
            lines.ForEach(e =>
                        {
                            if (e.Contains($"Extension[{curExtension}]"))
                            {
                                ParseExtension(lines.IndexOf(e), lines, certTemplate);
                                curExtension++;
                                return;
                            }
                        });

            certTemplate.TemplatePropValidityPeriod = lines.Where(e => e.Contains("TemplatePropValidityPeriod")).FirstOrDefault().EqualElement();
            certTemplate.TemplatePropRenewalPeriod = lines.Where(e => e.Contains("TemplatePropRenewalPeriod")).FirstOrDefault().EqualElement();
            return certTemplate;
        }


        private void ParseExtension(int linepos, List<string> lines, CertTemplate template)
        {
            CertTemplateExtension c = new CertTemplateExtension();
            while (lines[linepos] != "")
            {
                // Check if we are in the description of the extension
                if (lines[linepos].Contains("Flags"))
                {
                    Regex regex = new Regex("(?<id>[^:]+):.*(?<flags>\\d+).*,.*(?<length>[a-z0-9]+)");
                    Match match = regex.Match(lines[linepos]);
                    c.Id = match.Groups[1].Value.Trim();
                    c.Flags = match.Groups[2].Value;
                    c.Length = match.Groups[3].Value;
                    template.TemplatePropExtensions.Add(c);
                }
                linepos++;
            }
        }

    }


}
