using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame.Repository
{
    [Serializable]
    public static class HighScoreHandler
    {
        [Serializable]
        public struct Row : IComparable
        {
            public Row(string name, int score)
            {
                Name = name;
                Score = score;
            }

            public string Name { get; set; }
            public int Score { get; set; }

            public int CompareTo(Object obj)
            {
                return Score.CompareTo(((Row)obj).Score);
            }
        }
        public static bool AddRowToHighScoreFile(Row r, FileMode fm = FileMode.Append)
        {
            FileStream fileStream;
            string data = r.Name + ":" + r.Score.ToString() + Environment.NewLine;
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            try
            {
                fileStream = new FileStream(@"highscore.txt", fm, FileAccess.Write);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static List<Row> ReadHighScoreFile()
        {
            var list = new List<Row>();
            var fileStream = new FileStream(@"highscore.txt", FileMode.Open, FileAccess.Read);
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
