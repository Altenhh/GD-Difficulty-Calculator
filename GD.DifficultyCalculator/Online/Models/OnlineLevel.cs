// Copyright (c) Alten. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GD.Calculator.Online.Requests;
using GDAPI.Objects.GeometryDash.General;

namespace GD.Calculator.Online.Models
{
    public class OnlineLevel
    {
        [WebProperty(2)]
        public string Title { get; set; }

        [WebProperty(1)]
        public int ID { get; set; }

        [WebProperty(3)]
        public string DescB64 { get; set; }

        public string Description { get; set; }

        [WebProperty(4)]
        public string LevelString { get; set; }

        public Level Level { get; set; }

        [WebProperty(5)]
        public int Version { get; set; }

        [WebProperty(6)]
        public int AuthorID { get; set; }

        [WebProperty(9)]
        public int Difficulty { get; set; }

        public string DemonDiffs { get; set; }

        [WebProperty(10)]
        public int Downloads { get; set; }

        [WebProperty(12)]
        public int OfficialSong { get; set; }

        [WebProperty(14)]
        public int Likes { get; set; }

        [WebProperty(17)]
        public string Demon { get; set; }

        [WebProperty(25)]
        public string Auto { get; set; }

        [WebProperty(18)]
        public int Stars { get; set; }

        [WebProperty(19)]
        public string Featured { get; set; }

        [WebProperty(42)]
        public string Epic { get; set; }

        [WebProperty(30)]
        public int CopiedID { get; set; }

        [WebProperty(28)]
        public string UploadedDate { get; set; }

        [WebProperty(29)]
        public string UpdatedDate { get; set; }

        [WebProperty(35)]
        public string CustomSongID { get; set; }

        [WebProperty(37)]
        public int Coins { get; set; }

        [WebProperty(38)]
        public string VerifiedCoins { get; set; }

        [WebProperty(39)]
        public int StarsRequested { get; set; }

        [WebProperty(40)]
        public string LdmAvailable { get; set; }

        [WebProperty(27)]
        public string PassHash { get; set; }

        public string Password { get; set; }

        private static string fromBase64(string encoded)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(encoded);

                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static OnlineLevel GetLevel(GetGjLevelsSettings settings)
        {
            var req = new GetGjLevels(settings);

            req.Perform();

            var result = req.Result.FirstOrDefault();

            if (result != null)
            {
                var req2 = new DownloadGjLevel(result.ID);
                req2.Perform();

                var level = req2.Result;

                level.Description = fromBase64(level.DescB64);

                var levelData = new Level(level.Title, level.Description, level.LevelString, "");
                level.Level = levelData;

                return level;
            }

            return null;
        }

        public static List<OnlineLevel> GetLevels(GetGjLevelsSettings settings)
        {
            var req = new GetGjLevels(settings);

            req.Perform();

            var               result = req.Result;
            List<OnlineLevel> levels = new List<OnlineLevel>();

            foreach (var level in result)
            {
                if (level != null)
                {
                    level.Description        = fromBase64(level.DescB64);

                    levels.Add(level);
                }
            }

            return levels;
        }

        public static OnlineLevel GetLevel(int id)
        {
            var settings = new GetGjLevelsSettings
            {
                String = id.ToString()
            };

            return GetLevel(settings);
        }

        public static OnlineLevel GetLevel(string title)
        {
            var settings = new GetGjLevelsSettings
            {
                String = title
            };

            return GetLevel(settings);
        }

        public static List<OnlineLevel> GetLevels(string title)
        {
            var settings = new GetGjLevelsSettings
            {
                String = title
            };

            return GetLevels(settings);
        }
    }
}
