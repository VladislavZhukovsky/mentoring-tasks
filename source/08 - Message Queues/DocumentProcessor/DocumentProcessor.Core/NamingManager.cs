using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocumentProcessor.Core
{
    public class NamingManager
    {
        private const string DOCUMENT_PREFIX = "DOC_";
        private const string DOCUMENT_NAME_PATTERN = "^DOC_[0-9][0-9][0-9][0-9]$";
        private const string DOCUMENT_NUMBER_PATTERN = "[0-9][0-9][0-9][0-9]";
        private const string DOCUMENT_NUMBER_FORMAT = "{0:0000}";
        private Regex documentNameRegex;
        private Regex documentNumberRegex;

        public NamingManager()
        {
            documentNameRegex = new Regex(DOCUMENT_NAME_PATTERN);
            documentNumberRegex = new Regex(DOCUMENT_NUMBER_PATTERN);
        }

        public string GetNextDocumentName(string folderPath, string extension)
        {
            var allFiles = Directory.EnumerateFiles(folderPath);
            var conventionDocNames = allFiles
                .Where(x => Path.GetExtension(x) == extension)
                .Select(x => Path.GetFileNameWithoutExtension(x))
                .Where(x => documentNameRegex.IsMatch(x));
            if (conventionDocNames.Count() == 0)
                return DOCUMENT_PREFIX + String.Format(DOCUMENT_NUMBER_FORMAT, 1);
            var numbers = conventionDocNames
                .Select(x => documentNumberRegex.Match(x).Value)
                .Select(x => int.Parse(x))
                .OrderByDescending(x => x);
            var maxNumber = numbers.First();
            var nextNumber = maxNumber + 1;
            var nextDocumentName = DOCUMENT_PREFIX + String.Format(DOCUMENT_NUMBER_FORMAT, nextNumber);
            return nextDocumentName;
        }
    }
}
