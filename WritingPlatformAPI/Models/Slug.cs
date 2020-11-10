using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingPlatformAPI.Models
{
    public static class Slug
    {
        public static string GenerateSlug(this string phrase)
        {
            IdnMapping idn = new IdnMapping();
            string punyCode = idn.GetAscii(phrase);
            return punyCode;
        }

        public static string RemoveDiacritics(this string text)
        {
            var s = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return s.Normalize(NormalizationForm.FormC);
        }
    }
}
