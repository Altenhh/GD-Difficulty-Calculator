// Copyright (c) Alten. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;

namespace GD.Calculator.Online
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WebPropertyAttribute : Attribute
    {
        public int ID;

        public WebPropertyAttribute([NotNull] int webID = 0)
        {
            ID = webID;
        }
    }
}
