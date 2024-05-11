using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeireMKit.Domain.Extensions
{
    public static class TextExtension
    {
        public static string ReduceTextSize(this string text, int maximumCharacters = 25)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (maximumCharacters <= 0)
            {
                throw new ArgumentException("Maximum characters should be greater than zero.", nameof(maximumCharacters));
            }

            return text.Length <= maximumCharacters ? text : $"{text.Substring(0, maximumCharacters)}...";
        }
    }
}
