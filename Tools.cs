﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibADCA
{
    static class Ext
    {
        public static string EqualElement(this string input)
        {
            return input.Substring(input.IndexOf("=") + 2);
        }
        public static string LastWord(this string input, int times = int.MaxValue)
        {
            return input.Split(new char[] { ' ' }, times).Last();
        }
        public static string FirstWord(this string input, int times = int.MaxValue)
        {
            return input.Split(new char[] { ' ' }, times).First();
        }
    }
}
