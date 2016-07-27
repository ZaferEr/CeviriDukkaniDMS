using System.Linq;
using Tangent.CeviriDukkani.Domain.Common;

namespace DMS.Business.Extensions {
    public static class StringExtensions {
        public static bool IsDocumentExtension(this string fileName) {
            return fileName.GetExtensionOfFile() != "txt";
        }
    }
}