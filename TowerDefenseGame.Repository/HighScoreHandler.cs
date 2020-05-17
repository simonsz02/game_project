using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame.Repository
{
    /// <summary>
    /// Class for the Highscore
    /// </summary>
    [Serializable]
    public static class HighScoreHandler
    {
        /// <summary>
        /// Row class
        /// </summary>
        [Serializable]
        public struct Row : IComparable
        {
            /// <summary>
            /// Constructor of the row class
            /// </summary>
            /// <param name="name">name of the player</param>
            /// <param name="score">score the player gets</param>
            public Row(string name, int score)
            {
                Name = name;
                Score = score;
            }

            /// <summary>
            /// Name property of the player
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Score property of the player
            /// </summary>
            public int Score { get; set; }

            /// <summary>
            /// CompareTo method
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(Object obj)
            {
                return Score.CompareTo(((Row)obj).Score);
            }
        }

        /// <summary>
        /// Add row to Highscore file
        /// </summary>
        /// <param name="r"></param>
        /// <param name="fm"></param>
        /// <returns></returns>
        public static bool AddRowToHighScoreFile(Row r, FileMode fm = FileMode.Append)
        {
            FileStream fileStream;
            string data = r.Name + ":" + r.Score.ToString() + Environment.NewLine;
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            try
            {
                fileStream = new FileStream(@"Config\highscore.txt", fm, FileAccess.Write);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Read the Highscore file
        /// </summary>
        /// <returns></returns>
        public static List<Row> ReadHighScoreFile()
        {
            var list = new List<Row>();
            var fileStream = new FileStream(@"Config\highscore.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {

                    list.Add(new Row(line.Split(':')[0], int.Parse(line.Split(':')[1])));
                }
            }
            return list;
        }
    }
}
