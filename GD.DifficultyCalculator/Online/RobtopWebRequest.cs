// Copyright (c) Alten. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.IO.Network;

namespace GD.Calculator.Online
{
    /// <summary>
    /// Handles RobTop's weird parsing method.
    /// </summary>
    /// <typeparam name="T">the response format.</typeparam>
    public class RobtopWebRequest<T> : WebRequest
        where T : new()
    {
        private readonly char charToSplitBy;

        public RobtopWebRequest(string url = null, char charToSplitBy = ':', params object[] args)
            : base(url, args)
        {
            this.charToSplitBy = charToSplitBy;
        }

        protected override string Accept => "application/x-www-form-urlencoded";
        public T ResponseObject { get; private set; }

        protected override void ProcessResponse() =>
            ResponseObject = RobtopAnalyzer.DeserializeObject<T>(GetResponseString(), charToSplitBy);
    }

    /// <summary>
    /// Handles RobTop's weird parsing method.
    /// </summary>
    /// <typeparam name="T">the response format.</typeparam>
    public class RobtopWebRequestList<T> : WebRequest
        where T : new()
    {
        private readonly char charToSplitBy;
        private readonly char charToAddToList;

        public RobtopWebRequestList(string url = null, char charToSplitBy = ':', char charToAddToList = '|', params object[] args)
            : base(url, args)
        {
            this.charToSplitBy   = charToSplitBy;
            this.charToAddToList = charToAddToList;
        }

        protected override string Accept => "application/x-www-form-urlencoded";
        public List<T> ResponseObject { get; private set; }

        protected override void ProcessResponse() =>
            ResponseObject = RobtopAnalyzer.DeserializeObjectList<T>(GetResponseString(), charToSplitBy, charToAddToList);
    }
}
