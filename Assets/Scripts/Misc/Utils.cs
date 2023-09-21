using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class GenUtils
    {
        public static bool HasComponent <T>(GameObject obj)
        {
            return obj.GetComponent<T>() != null;
        }

        public static IEnumerator WaitForTrueSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }

        public static string StringToLowerCase(string inputString)
        {
        string lowerString = "";
        for (var i = 0; i < inputString.Length; i++)
        {
            if (char.IsLetter(inputString[i]))
            {
                lowerString += char.ToLower(inputString[i]);
            }
            else
            {
                lowerString += inputString[i];
            }
        }
        return lowerString;
        }

        public static string PeriodToLineBreak(string inputString)
        {
        string literallyJustAPeriod = ".";
        char[] aMerePeriod = literallyJustAPeriod.ToCharArray();

        string newString = "";
        for (var i = 0; i < inputString.Length; i++)
        {
            if (inputString[i] == aMerePeriod[0])
            {
                newString += ".\n\n";
            }
            else
            {
                newString += inputString[i];
            }
        }

        return newString;
        }
    }
}