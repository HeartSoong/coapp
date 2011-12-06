﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Original Copyright (c) 2009 Microsoft Corporation. All rights reserved.
//     Changes Copyright (c) 2010  Garrett Serack. All rights reserved.
// </copyright>
// <license>
//     The software is licensed under the Apache 2.0 License (the "License")
//     You may not use the software except in compliance with the License. 
// </license>
//-----------------------------------------------------------------------

// -----------------------------------------------------------------------
// Original Code: 
// (c) 2009 Microsoft Corporation -- All rights reserved
// This code is licensed under the MS-PL
// http://www.opensource.org/licenses/ms-pl.html
// Courtesy of the Open Source Techology Center: http://port25.technet.com
// -----------------------------------------------------------------------

namespace CoApp.Toolkit.Extensions {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
#if !COAPP_ENGINE_CORE
    using Collections;
#endif 
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public static class CollectionExtensions {
        /// <summary>
        /// Splits a string into a List of strings.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> SplitToList(this string str, params char[] separator) {
            var result = new List<string>();
            if(!string.IsNullOrEmpty(str)) {
                result.AddRange(str.Split(separator));
            }

            return result;
        }

        /// <summary>
        /// Removes duplicate strings from a list.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> Uniq(this IEnumerable<string> collection, StringComparison stringComparison) {
            return Uniq((collection is List<string>) ? (collection as List<string>) : collection.ToList(), stringComparison);
        }

        /// <summary>
        /// Removes duplicate strings from a list.
        /// Assumes Case Sensitivity.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> Uniq(this IEnumerable<string> collection) {
            return Uniq((collection is List<string>) ? (collection as List<string>) : collection.ToList());
        }

        /// <summary>
        /// Removes duplicate strings from a list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> Uniq(this List<string> list, StringComparison stringComparison) {
            for(var i = 0; i < list.Count; i++) {
                for(var j = list.Count - 1; j > i; j--) {
                    if(list[i].Equals(list[j], stringComparison)) {
                        list.RemoveAt(j);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Removes duplicate strings from a list.
        /// Assumes Case Sensitivity.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> Uniq(this List<string> list) {
            for(var i = 0; i < list.Count; i++) {
                for(var j = list.Count - 1; j > i; j--) {
                    if(list[i].Equals(list[j])) {
                        list.RemoveAt(j);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Combines a list of strings into a single string seperated by seperator
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Combine(this List<string> list, char separator) {
            var sb = new StringBuilder();
            foreach(string s in list) {
                if(sb.Length > 0) {
                    sb.Append(separator);
                }
                sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Combines a list of strings into a single string seperated by seperator
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Combine(this List<string> list, string separator) {
            var sb = new StringBuilder();
            foreach(string s in list) {
                if(sb.Length > 0) {
                    sb.Append(separator);
                }
                sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Adds the contents of one collection to another.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination">The destination.</param>
        /// <param name="collection">The collection.</param>
        /// <remarks></remarks>
        public static void AddRange<T>(this Collection<T> destination, IEnumerable<T> collection) {
            foreach (var i in collection)
                destination.Add(i);
        }

        /// <summary>
        /// Determines whether the collection object is either null or an  empty collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns><c>true</c> if [is null or empty] [the specified collection]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection ) {
            return collection == null ? true : !collection.Any();
        }

#if !COAPP_ENGINE_CORE
         public static Dictionary<string, IEnumerable<string>>Merge(this Dictionary<string, IEnumerable<string>> result, IDictionary<string, IEnumerable<string>> more ) {
            foreach( var k in more.Keys) {
                if( result.ContainsKey(k)) {
                    result[k] = result[k].Union(more[k]).Distinct();
                }
                else {
                    result.Add(k, more[k]);
                }
            }
            return result;
        }

         public static TValue GetOrDefault<TKey,TValue>(this Dictionary<TKey,TValue> dictionary, TKey key) {
            TValue value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static IEnumerable<T> ToLazyEnumerable<T>(this IEnumerable<T> collection) {
            return new LazyEnumerable<T>(collection);
        }


        /// <summary>
        /// A way to use the Add method more like a Concat. Very useful for Aggregate. Instead of returning void,
        /// this returns the list itself.
        /// </summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="list"></param>
        /// <param name="item">The item to add to the list</param>
        /// <returns>The list after the new <paramref name="item"/> has been added.</returns>
        public static IList<T> LAdd<T>(this IList<T> list, T item)
        {
            list.Add(item);

            return list;
        }

#endif

    }
}
