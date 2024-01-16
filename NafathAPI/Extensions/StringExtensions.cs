using System.Text.RegularExpressions;

namespace NafathAPI.Extensions;
public static class StringExtensions
    {
    public static string RemoveFromEnd ( this string s , string suffix )
        {
        if ( s.EndsWith ( suffix ) )
            {
            return s.Substring ( 0 , s.Length - suffix.Length );
            }

        return s;
        }


    public static bool IsNullOrWhiteSpace ( this string s )
        {
        return s == null || string.IsNullOrWhiteSpace ( s );
        }


    public static bool IsNullOrEmpty ( this string s )
        {
        return s == null || string.IsNullOrEmpty ( s );
        }


    /// <summary>
    /// https://stackoverflow.com/questions/37301287/how-do-i-convert-pascalcase-to-kebab-case-with-c
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string PascalToKebabCase ( this string value )
        {
        //if (value == null) { return null; }

        //return Regex.Replace(value.ToString()!,
        //                     "([a-z])([A-Z])",
        //                     "$1-$2",
        //                     RegexOptions.CultureInvariant,
        //                     TimeSpan.FromMilliseconds(100)).ToLowerInvariant();

        if ( string.IsNullOrEmpty ( value ) )
            return value;

        return Regex.Replace (
            value ,
            "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])" ,
            "-$1" ,
            RegexOptions.Compiled )
            .Trim ( )
            .ToLower ( );
        }

    public static bool HasArabicCharacters ( this string text )
        {
        Regex regex = new Regex ( "[^\x00-\x7F]+" );
        return regex.IsMatch ( text );
        }
    }

