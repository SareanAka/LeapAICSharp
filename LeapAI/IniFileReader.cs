using System.Runtime.InteropServices;
using System.Text;

namespace LeapAI
{
    public class IniFileReader
    {
        private readonly string _path;

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Initialize a new instance of IniFileReader
        /// </summary>
        /// <param name="INIPath">The path to your Inifile </param>
        public IniFileReader(string INIPath)
        {
            _path = INIPath;
        }

        /// <summary>
        /// This function reads the Ini file 
        /// </summary>
        /// <param name="Section">Section under which the key is located</param>
        /// <param name="Key">Name associated with the key</param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            var temp = new StringBuilder(255);
            var ini = GetPrivateProfileString(Section, Key, "", temp, 255, _path);
            return temp.ToString();
        }
    }
}
