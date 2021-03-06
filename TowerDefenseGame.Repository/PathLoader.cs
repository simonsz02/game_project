using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace TowerDefenseGame.Repository
{
    /// <summary>
    /// Handles the file containing the path
    /// </summary>
    public class PathLoader
    {
        /// <summary>
        /// Reads the file containing the path
        /// </summary>
        /// <returns>List of points representing the path</returns>
        public static List<Point> ReadPathFile()
        {
            var list = new List<Point>();
            var fileStream = new FileStream(@"Config\path.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(new Point(int.Parse(line.Split(',')[0]), int.Parse(line.Split(',')[1])));
                }
            }
            return list;
        }
    }
}
