﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonUtils
{
    public static class IEnumerableExtension
    {
        public static bool Cycle<T>(this IEnumerable enumerable, SharedDelegate.ActionWithResult<T> action)
        {
            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!action((T)enumerator.Current)) return false;
            }
            return true;
        }
    }
}