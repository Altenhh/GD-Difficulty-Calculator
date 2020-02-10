// Copyright (c) Alten. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using GD.Calculator.Online.Models;
using osu.Framework.IO.Network;

namespace GD.Calculator.Online.Requests
{
    public class GetGjLevels : APIRequestList<OnlineLevel>
    {
        private GetGjLevelsSettings settings;

        public GetGjLevels(GetGjLevelsSettings settings)
        {
            this.settings = settings;
        }

        protected override string Target => "getGJLevels21";

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();

            var gdw = settings.Gdw.BoolToNumber();
            var difficulties = settings.Difficulties.ParseList();
            var length = settings.Lengths.ParseList();

            var uncompleted = settings.Uncompleted.BoolToNumber();
            var onlyCompleted = settings.OnlyCompleted.BoolToNumber();
            var featured = settings.Featured.BoolToNumber();
            var original = settings.Original.BoolToNumber();
            var twoPlayers = settings.TwoPlayers.BoolToNumber();
            var coins = settings.Coins.BoolToNumber();
            var epic = settings.Epic.BoolToNumber();
            var star = settings.Star.BoolToNumber();
            var customSong = settings.CustomSong.BoolToNumber();

            req.AddParameter("gdw", gdw);
            req.AddParameter("diff", difficulties);
            req.AddParameter("length", length);

            req.AddParameter("uncompleted", uncompleted);
            req.AddParameter("onlyCompleted", onlyCompleted);
            req.AddParameter("featured", featured);
            req.AddParameter("original", original);
            req.AddParameter("twoPlayers", twoPlayers);
            req.AddParameter("coins", coins);
            req.AddParameter("epic", epic);
            req.AddParameter("star", star);
            req.AddParameter("customSong", customSong);
            
            //todo: make login thing
            req.AddParameter("accountID", settings.AccountID.ToString());
            req.AddParameter("gjp", settings.Gjp);
            
            req.AddParameter("type", ((int)settings.Type).ToString());
            req.AddParameter("str", settings.String);
            req.AddParameter("page", settings.Page.ToString());
            req.AddParameter("song", settings.Song.ToString());

            return req;
        }
    }

    public static class GetLevelsExtensions
    {
        public static string BoolToNumber(this bool @bool)
            => @bool ? "1" : "0";

        public static string ParseList<TEnum>(this List<TEnum> list)
            where TEnum : struct, Enum
        {
            if (list.Count == 0)
                return "-";

            string str = "";

            list.ForEach(e =>
            {
                var val = Convert.ToInt32(e);
                str += val + ",";
            });

            str = str.Substring(0, str.Length - 1);

            return str;
        }
    }

    public class GetGjLevelsSettings
    {
        /// <summary>
        /// If this request should be handled on GD World side.
        /// </summary>
        public bool Gdw { get; set; } = false;

        public int AccountID { get; set; } = 0;

        public string Gjp { get; set; } = "";

        /// <summary>
        /// Search filter.
        /// </summary>
        public SearchType Type { get; set; } = SearchType.Search;

        /// <summary>
        /// String to search for.
        /// </summary>
        public string String { get; set; } = "";

        public List<Difficulties> Difficulties { get; set; } = new List<Difficulties>();

        public List<Length> Lengths { get; set; } = new List<Length>();

        public int Page { get; set; } = 0;

        public bool Uncompleted { get; set; } = false;

        public bool OnlyCompleted { get; set; } = false;

        public bool Featured { get; set; } = false;

        public bool Original { get; set; } = false;

        public bool TwoPlayers { get; set; } = false;

        public bool Coins { get; set; } = false;

        public bool Epic { get; set; } = false;

        public bool Star { get; set; } = false;

        public bool CustomSong { get; set; } = false;

        public int Song { get; set; } = 0;
    }

    public enum SearchType : int
    {
        Search = 0,
        Downloaded = 1,
        Likes = 2,
        Trends = 3,
        Recent = 4,
        Magic = 7,
        Award = 11,
        Followed = 12,
        Friends = 13
    }

    public enum Length : int
    {
        Tiny,
        Short,
        Medium,
        Long,
        XL,
    }

    public enum Difficulties : int
    {
        Auto = -3,
        Demon = -2,
        NA = -1,
        Easy = 1,
        Normal = 2,
        Hard = 3,
        Harder = 4,
        Insane = 5,
    }

    public enum DemonDiffs : int
    {
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Insane = 4,
        Extreme = 5
    }
}
