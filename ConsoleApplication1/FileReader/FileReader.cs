using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication1.FileReader
{
	public static class FileReader
	{
		public static string[][] ReadFromFile(string path)
		{
			var text = File.ReadAllText(path, Encoding.Default);
			// char[] delimiterChars = { ' ', ',', '.', '\t', '\n', '\\', '\"' };
			text = text.Replace("\r", "");
			var rows = text.Split('\n');
			var res = new string[rows.Length][];
			for (var i = 0; i < rows.Length; i++)
			{
				var words = rows[i].Split('\"');
				var filteredWords = words.Where(x => x != "").ToArray();
				res[i] = filteredWords;
			}

			return res;
		}
	}
}
