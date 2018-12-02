/*

Author: Todd Hubers (http://www.alivate.com.au)
 
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org/>
*/

/**
* PRMasterServer changes:
* Added a NotLike method, which is the same as Like, but not'd :p
*/

using System;

namespace Alivate
{
    public static class SQLMethods
    {
        /// <summary>
        /// EvaluateIsLike
        /// </summary>
        /// <param name="MatchValue">The value to evaluate</param>
        /// <param name="Pattern">The pattern to use</param>
        /// <returns>Whether the pattern matches the value to evaluate</returns>
        public static bool EvaluateIsLike(string MatchValue, string Pattern)
        {
            return EvaluateIsLike(MatchValue, Pattern, '%', '_', '!');
        }

        /// <summary>
        /// EvaluateIsNotLike
        /// </summary>
        /// <param name="MatchValue">The value to evaluate</param>
        /// <param name="Pattern">The pattern to use</param>
        /// <returns>Whether the pattern matches the value to evaluate</returns>
        public static bool EvaluateIsNotLike(string MatchValue, string Pattern)
        {
            return EvaluateIsNotLike(MatchValue, Pattern, '%', '_', '!');
        }

        /// <summary>
        /// EvaluateIsLike
        /// </summary>
        /// <param name="MatchValue">The value to evaluate</param>
        /// <param name="Pattern">The pattern to use</param>
        /// <returns>Whether the pattern matches the value to evaluate</returns>
        public static bool EvaluateIsLike(string MatchValue, string Pattern, char Wildcard = '%', char Placeholder = '_', char Escape = '!')
        {
            LikeParams l = new LikeParams()
            {
                MatchValue = MatchValue.ToLowerInvariant(),
                Pattern = Pattern.ToLowerInvariant(),
                Wildcard = Wildcard,
                PlaceHolder = Placeholder,
                Escape = Escape
            };

            return EvaluateIsLike(l, 0, 0);
        }

        /// <summary>
        /// EvaluateIsNotLike
        /// </summary>
        /// <param name="MatchValue">The value to evaluate</param>
        /// <param name="Pattern">The pattern to use</param>
        /// <returns>Whether the pattern matches the value to evaluate</returns>
        public static bool EvaluateIsNotLike(string MatchValue, string Pattern, char Wildcard = '%', char Placeholder = '_', char Escape = '!')
        {
            LikeParams l = new LikeParams()
            {
                MatchValue = MatchValue.ToLowerInvariant(),
                Pattern = Pattern.ToLowerInvariant(),
                Wildcard = Wildcard,
                PlaceHolder = Placeholder,
                Escape = Escape
            };

            return !EvaluateIsLike(l, 0, 0);
        }

        private static bool EvaluateIsLike(LikeParams l, int ValuePosition, int PatternPosition)
        {
            bool IsOnEscape = false;

            while (true)
            {
                IsOnEscape = false;
                if (l.Pattern[PatternPosition] == l.Escape)
                {
                    IsOnEscape = true;
                    PatternPosition++;
                    if (PatternPosition == l.Pattern.Length)
                    {
                        IsOnEscape = false;
                        PatternPosition--;
                        //throw new Exception("Escape character found at end of string - can't use that"); //Run out of characters
                    }
                }

                if (!IsOnEscape && l.Pattern[PatternPosition] == l.Wildcard)
                {
                    PatternPosition++; //Look at the next character from now on

                    if (PatternPosition == l.Pattern.Length) //Wildcard at the end of the pattern, requires no further processing
                        return true;

                    //Find the first case of a character we can match (escaped or otherwise, fast forwarding past placeholders and other wildcards along the way)
                    while (true)
                    {
                        IsOnEscape = false;

                        if (l.Pattern[PatternPosition] == l.Escape)
                        {
                            IsOnEscape = true;
                            PatternPosition++;
                            if (PatternPosition == l.Pattern.Length)
                                throw new Exception("Escape character found at end of string - can't use that"); //Run out of characters
                        }

                        if (!IsOnEscape && l.Pattern[PatternPosition] == l.PlaceHolder)
                        {
                            ValuePosition++; //Requires at least 1 character before search text
                            if (ValuePosition == l.MatchValue.Length)
                                return false; //Run out of characters
                        }
                        else if (!IsOnEscape && l.Pattern[PatternPosition] == l.Wildcard)
                        {
                            PatternPosition++;
                            if (PatternPosition == l.Pattern.Length)
                                return true; //Run out of characters
                        }
                        else
                        {
                            char SearchCharacter = l.Pattern[PatternPosition];
                            if (IsOnEscape && SearchCharacter != l.Wildcard && SearchCharacter != l.PlaceHolder && SearchCharacter != l.Escape)
                                throw new Exception("Invalid escape sequence (wildcard scan) - " + PatternPosition);

                            int start = ValuePosition;
                            while (true)
                            {
                                //Now we can find a starting position for continued Evaluation
                                start = l.MatchValue.IndexOf(SearchCharacter, start);
                                if (start == -1)
                                    return false; //Match could not be found

                                if (!IsOnEscape)
                                {
                                    if (EvaluateIsLike(l, start, PatternPosition))
                                        return true;  //Pop the true up the stack
                                }
                                else
                                {
                                    if (EvaluateIsLike(l, start, PatternPosition - 1)) //Substract 1 so the recursed function can re-evaluate
                                        return true;  //Pop the true up the stack
                                }

                                start++; //Try to find another match
                            }
                        }
                    }

                }
                else if (!IsOnEscape && l.Pattern[PatternPosition] == l.PlaceHolder)
                {
                    if (ValuePosition == l.MatchValue.Length) //MatchValue is too short - we've reached the end
                        return false;
                }
                else
                {
                    if (ValuePosition == l.MatchValue.Length)
                        return false; //Characters left over in value, without wildcard in pattern on last character

                    char ValueCharacter = l.MatchValue[ValuePosition];
                    char PatternCharacter = l.Pattern[PatternPosition];
                    if (IsOnEscape && PatternCharacter != l.Wildcard && PatternCharacter != l.PlaceHolder && PatternCharacter != l.Escape)
                        throw new Exception(String.Format("Invalid escape sequence - {0} - {1}", PatternPosition, ValueCharacter));

                    if (ValueCharacter != PatternCharacter) //Characters don't match - fail
                        return false;
                }

                ValuePosition++;
                PatternPosition++;

                if (PatternPosition == l.Pattern.Length)
                {
                    if (ValuePosition == l.MatchValue.Length)
                        return true; //Run out of characters
                    else
                        return false; //Left over characters in Value without % in pattern
                }
            }
        }

        //Shared data across recursion
        class LikeParams
        {
            public string MatchValue;
            public string Pattern;
            public char Wildcard;
            public char PlaceHolder;
            public char Escape;
        }
    }
}