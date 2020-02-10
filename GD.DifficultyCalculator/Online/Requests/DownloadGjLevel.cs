// Copyright (c) Alten. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using GD.Calculator.Online.Models;
using osu.Framework.IO.Network;

namespace GD.Calculator.Online.Requests
{
    public class DownloadGjLevel : APIRequest<OnlineLevel>
    {
        private int targetLevelID;

        public DownloadGjLevel(int targetLevelId)
        {
            targetLevelID = targetLevelId;
        }

        protected override string Target => "downloadGJLevel22";

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();

            req.AddParameter("levelID", targetLevelID.ToString());
            req.AddParameter("gameVersion", "20");
            req.AddParameter("binaryVersion", "35");

            return req;
        }
    }
}
