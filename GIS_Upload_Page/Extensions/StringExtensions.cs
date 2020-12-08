using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GIS_Upload_Page.Extensions
{
    public static class StringExtensions
    {
        public static string TrimSafe(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Trim();
        }

        public static string TransformTextForDb(this string value)
        {
            /*if (!string.IsNullOrEmpty(value))
            {
                value = Regex.Replace(Regex.Replace(value.Trim(), "[“”]", "\""), "[‘’]", "'");
            }(*/
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ReplaceProblemCharacters();
            return value;
        }

        public static bool IsValidEmail(this string value)
        {
            return CheckIsValidEmail(value);
        }

        private static bool CheckIsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string ReplaceProblemCharacters(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var inputArray = value.ToCharArray();
                for (int i = 0; i < inputArray.Length; i++)
                {
                    switch (inputArray[i])
                    {
                        // en dash
                        case '\u2013':
                            inputArray[i] = '-';
                            break;
                        // em dash
                        case '\u2014':
                            inputArray[i] = '-';
                            break;
                        // horizontal bar
                        case '\u2015':
                            inputArray[i] = '-';
                            break;
                        // double low line
                        case '\u2017':
                            inputArray[i] = '_';
                            break;
                        // left single quotation mark
                        case '\u2018':
                            inputArray[i] = '\'';
                            break;
                        // right single quotation mark
                        case '\u2019':
                            inputArray[i] = '\'';
                            break;
                        // single low-9 quotation mark
                        case '\u201a':
                            inputArray[i] = ',';
                            break;
                        // single high-reversed-9 quotation mark
                        case '\u201b':
                            inputArray[i] = '\'';
                            break;
                        // left double quotation mark
                        case '\u201c':
                            inputArray[i] = '\"';
                            break;
                        // right double quotation mark
                        case '\u201d':
                            inputArray[i] = '\"';
                            break;
                        // double low-9 quotation mark
                        case '\u201e':
                            inputArray[i] = '\"';
                            break;
                        // horizontal ellipsis
                        case '\u2026':
                            inputArray[i] = '.';
                            break;
                        // prime
                        case '\u2032':
                            inputArray[i] = '\'';
                            break;
                        // double prime
                        case '\u2033':
                            inputArray[i] = '\"';
                            break;
                    }
                }
                return new string(inputArray);
            }
            return value;
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
            }
        }
    }
}
