﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace MiniCommandLineHelper
{
    public enum StressPipelineOperation
    {
        Start, 
        Creation, 
        Publish, 
        Load, 
        Deletion,
        FundamentalReport
    }

    public static class Utility
    {
        /// <summary>
        /// Consolidate user parameters and default parameters
        /// </summary>
        /// <param name="userArgs"></param>
        /// <param name="methodParameters"></param>
        /// <returns></returns>
        public static object[] CombineParameters(string[] userArgs, ParameterInfo[] methodParameters)
        {
            var joinedArgs = new List<object>();
            var requiredArgs = new List<object>();
            var tempUserArgs = new Dictionary<string, object>();
            try
            {
                if (userArgs.Length > 0)
                {
                    foreach (var tmp in userArgs)
                    {
                        if (tmp.StartsWith("-"))
                        {
                            var paramAndValue = new[] {tmp.Substring(1, tmp.IndexOf(":", StringComparison.Ordinal) - 1), tmp.Substring(tmp.IndexOf(":", StringComparison.Ordinal) + 1)};
                            tempUserArgs.Add(paramAndValue[0].ToLower(), paramAndValue[1]);
                        }
                        else
                        {
                            requiredArgs.Add(tmp);
                        }
                    }
                }
                joinedArgs.AddRange(requiredArgs);
                foreach (var parameter in methodParameters)
                {
                    if (parameter.HasDefaultValue)
                    {
                        var val = parameter.DefaultValue;
                        var key = parameter.Name.ToLower();
                        if (tempUserArgs.ContainsKey(key))
                        {
                            val = tempUserArgs[key];
                        }
                        Type paramType = parameter.ParameterType;

                        try
                        {
                            val = Convert.ChangeType(val, paramType);
                        }
                        catch (InvalidCastException)
                        {
                            
                        }

                        joinedArgs.Add(val);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Not enough arguments");
            }
            return joinedArgs.ToArray();
        }
    }
}
