
namespace TimeReflector
{
    /// <summary>
    /// 1)
    /// Implement WhatIsTheTime method which will return clock in the mirror position.
    /// Examples
    ///     05:25 --> 06:35
    ///     01:50 --> 10:10
    ///     11:58 --> 12:02
    ///     12:01 --> 11:59
    /// Notes
    ///     Hours should be between 1 <= hour <= 12
    ///         0:20 should be 12:20
    ///         13:20 should be 01:20
    ///         
    /// 1a)
    /// Invalid input value the method will return error message instead.
    /// Examples
    ///     05:89 --> Invalid input! 
    /// 
    /// 1b)
    /// Let method accept $ symbol as 3
    /// Examples
    ///     0$:25 == 03:25
    /// 
    /// 2) 
    /// Add support for words ("one", "two", "three",...)
    /// Examples    
    ///     one twenty == 1:20
    /// 
    /// 3) 
    /// Add support for special english time names
    /// Example    
    ///     half past eight == 08:30
    ///     quarter to eight == 07:45
    /// 
    /// 4) 
    /// add support for mutiple inputs in string separated by ;; symbol. The return value will be always in the numeric form.
    /// Examples     
    ///     05:25;;one fifty;;11:58 --> 06:35;;10:10;;12:02
    /// 
    ///   
    /// 4a)
    /// If in there are two same times in the colletion the method will not include mirror values in the result 
    /// Examples
    ///     05:25;;five twentyfive --> 06:35
    /// 
    /// 
    /// Final words: 
    ///     - Dont forget to validate inputs!
    ///     - Feel free to add any additional test cases you want!
    ///     
    /// </summary>
    public static class TimeMirror
    {
        public static void Main()
        {
            string input = Console.ReadLine();
            string output = WhatIsTheTime(input);
            Console.WriteLine(output);
        }
        
        //Wraps the whole program to manage multiple inputs
        public static string WhatIsTheTime(string input)
        {
            if (input == "")
            {
                throw new ArgumentException("input cannot be an empty string");
            }
            string finalOutput = "";
            string[] times = input.Split(";;");

            //remove duplicates
            var uniqueTimes = times.Distinct().ToArray();
            for (int i = 0; i < uniqueTimes.Length; i++)
            {
                if (!finalOutput.Contains(Wrapper(uniqueTimes[i])))
                {
                    if (i > 0)
                    {
                        finalOutput += ";;";
                    }
                    finalOutput += Wrapper(uniqueTimes[i]);
                    continue;
                }
            }
            return finalOutput;
        }

        private static string Wrapper(string timeInMirror)
        {

            //handle dollar
            if (timeInMirror.Contains('$'))
            {
                timeInMirror = timeInMirror.Replace('$', '3');
            }

            //handle time as text
            if (timeInMirror.Any(x => char.IsLetter(x)))
            {
                timeInMirror = TextToTime(timeInMirror);
            }
  
            //split the input into an array and seperate hours and minutes into ints
            string[] strTime = timeInMirror.Split(':');
            int hour = int.Parse(strTime[0]);
            int min = int.Parse(strTime[1]);

            //check if hour is between 13 and 24 and returns corrected value (hour - 12)
            hour = ConvertHour(hour);

            //Checks input
            if (InputNotGood(hour, min))
            {
                return "Invalid input!";
            }

            return ComputeTime(hour, min);
        }

        static Dictionary<string, int> hourMap = new Dictionary<string, int>
        {
            {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6},
            {"seven", 7}, {"eight", 8}, {"nine", 9}, {"ten", 10}, {"eleven", 11}, {"twelve", 12}
        };

        static Dictionary<string, int> minuteMap = new Dictionary<string, int>
        {
            {"oclock", 0}, {"o'clock", 0}, {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5},
            {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}, {"ten", 10}, {"eleven", 11}, {"twelve", 12},
            {"thirteen", 13}, {"fourteen", 14}, {"fifteen", 15}, {"sixteen", 16}, {"seventeen", 17},
            {"eighteen", 18}, {"nineteen", 19}, {"twenty", 20}, {"twentyone", 21}, {"twentytwo", 22},
            {"twentythree", 23}, {"twentyfour", 24}, {"twentyfive", 25}, {"twentysix", 26},
            {"twentyseven", 27}, {"twentyeight", 28}, {"twentynine", 29}, {"thirty", 30},
            {"thirtyone", 31}, {"thirtytwo", 32}, {"thirtythree", 33}, {"thirtyfour", 34},
            {"thirtyfive", 35}, {"thirtysix", 36}, {"thirtyseven", 37}, {"thirtyeight", 38},
            {"thirtynine", 39}, {"forty", 40}, {"fortyone", 41}, {"fortytwo", 42}, {"fortythree", 43},
            {"fortyfour", 44}, {"fortyfive", 45}, {"fortysix", 46}, {"fortyseven", 47},
            {"fortyeight", 48}, {"fortynine", 49}, {"fifty", 50}, {"fiftyone", 51}, {"fiftytwo", 52},
            {"fiftythree", 53}, {"fiftyfour", 54}, {"fiftyfive", 55}, {"fiftysix", 56}, {"fiftyseven", 57},
            {"fiftyeight", 58}, {"fiftynine", 59}
        };

        private static string TextToTime(string time)
        {
            //make the input lower case and trim any whitespaces
            time = time.ToLower().Trim();
            
            
            if (time.Contains("half past"))
            {
                return HandleSpecialCases(time, "half past", 30, 0);
            }
            if (time.Contains("quarter to"))
            {
                return HandleSpecialCases(time, "quarter to", 45, -1);
            }
            if (time.Contains("quarter past"))
            {
                return HandleSpecialCases(time, "quarter past", 15, 0);
            }

            string[] split = time.Split(' ');

            string hourTime = split[0];
            string minuteTime = split[1];

            if (!hourMap.ContainsKey(hourTime) || !minuteMap.ContainsKey(minuteTime))
            {
                return "Invalid input!";
            }

            int hour = hourMap[hourTime];
            int minute = minuteMap[minuteTime];

            return $"{hour}:{minute:D2}";

        }

        static string HandleSpecialCases(string time, string keyword, int minute, int hourOffset)
        {
            //get just the hour
            string hourTime = time.Replace(keyword, "").Trim();

            //hour not in dictionary
            if (!hourMap.ContainsKey(hourTime))
            {
                return "Invalid input!";
            }

            //compute the hour
            int hour = hourMap[hourTime] + hourOffset;
            if (hour <= 0)
            {
                hour += 12;
            }

            return $"{hour}:{minute:D2}";
        }

        private static int ConvertHour(int hour)
        {
            if (13 <= hour && hour <= 24)
            {
                hour -= 12;
            }
            return hour;
        }

        private static bool InputNotGood(int hour, int min)
        {   
            return !(1 <= hour && hour <= 12) || !( 0 <= min && min <= 60);
        }
        
        private static string ComputeTime(int hour, int min)
        {
            string strHour;
            string strMin;

            //Mirror Hours
            hour = 12 - hour;
            if (hour == 0)
            {
                hour = 12;
            }

            //Mirror Minutes
            min = 60 - min;
            if (min == 60)
            {
                min = 0;
            }
            else
            {
                //minute overflowed so we need to substract one more hour.
                hour--;
                if (hour == 0)
                {
                    hour = 12;
                }
            }

            //Add leading zeros
            if (hour < 10)
            {
                strHour = "0" + hour.ToString();
            }
            else
            {
                strHour = hour.ToString();
            }
            if (min < 10)
            {
                strMin = "0" + min.ToString();
            }
            else
            {
                strMin = min.ToString();
            }
            //return final string
            return (strHour + ":" + strMin);
        }

    }
}