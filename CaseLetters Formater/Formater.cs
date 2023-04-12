using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseLetters_Formater
{
    internal class Formater
    {
        internal static string UpperFirstLetter(string text) 
        {

            string result = "";
            string[] words = text.Split(' ');

            if(!string.IsNullOrEmpty(text))
            {
                result = string.Join(" ", words.Select(word => char.ToUpper(word[0]) + word.Substring(1)));
            }

            return result;
        }
    }
}
