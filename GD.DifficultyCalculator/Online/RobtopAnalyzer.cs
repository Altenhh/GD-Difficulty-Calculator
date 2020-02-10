// Copyright (c) Alten. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Extensions.IEnumerableExtensions;

namespace GD.Calculator.Online
{
    public class RobtopAnalyzer
    {
        public static T DeserializeObject<T>(string value, char charToSplit = ':')
            where T : new()
        {
            var result = new Dictionary<int, object>();

            if (value == null)
                return new T();

            var seperated = value.Replace('~', ' ').Split(charToSplit);

            for (var i = 0; i < seperated.Length - 1;) // we want to skip by 2 in order to skip the value.
            {
                int.TryParse(seperated[i], out var key);
                i++;
                var val = seperated[i];

                result.Add(key, val);

                i++;
            }

            // Now let's try to set the value.
            var t = new T();

            // Get all properties
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(false);

                foreach (var attr in attrs)
                {
                    if (attr is WebPropertyAttribute webAttr)
                    {
                        result.TryGetValue(webAttr.ID, out var toSet);
                        var    type = prop.PropertyType;
                        object changedType;

                        try
                        {
                            changedType = toSet != null ? Convert.ChangeType(toSet, type) : null;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);

                            throw;
                        }

                        // Finally set them
                        prop.SetValue(t, changedType);
                    }
                }
            }

            return t;
        }

        public static List<T> DeserializeObjectList<T>(string value, char charToSplit = ':', char charToAddToList = '|')
            where T : new()
        {
            var list = new List<Dictionary<int, object>>();

            var listValues = value.Split(charToAddToList);

            listValues.ForEach(v =>
            {
                var dictionary = new Dictionary<int, object>();

                var seperated = v.Split(charToSplit);

                for (var i = 0; i < seperated.Length - 1;) // we want to skip by 2 in order to skip the value.
                {
                    int.TryParse(seperated[i], out var key);
                    i++;
                    var val = seperated[i];

                    dictionary.Add(key, val);

                    i++;
                }

                list.Add(dictionary);
            });

            // Now let's try to set the value.
            var listType = new List<T>();

            // Get all properties
            list.ForEach(dictionary =>
            {
                var type = new T();

                var props = type.GetType().GetProperties();

                foreach (var prop in props)
                {
                    var attrs = prop.GetCustomAttributes(false);

                    foreach (var attr in attrs)
                    {
                        if (attr is WebPropertyAttribute webAttr)
                        {
                            dictionary.TryGetValue(webAttr.ID, out var toSet);
                            var propType = prop.PropertyType;
                            object changedType;

                            try
                            {
                                changedType = toSet != null ? Convert.ChangeType(toSet, propType) : null;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);

                                throw;
                            }

                            // Finally set them
                            prop.SetValue(type, changedType);
                        }
                    }
                }

                listType.Add(type);
            });

            return listType;
        }
    }
}
