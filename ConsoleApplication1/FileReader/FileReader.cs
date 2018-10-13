using System.IO;

namespace ConsoleApplication1.FileReader
{
	public static class FileReader
	{
		public static string[] ReadFromFile(string path)
		{
			var text = File.ReadAllText(path);
			// char[] delimiterChars = { ' ', ',', '.', '\t', '\n', '\\', '\"' };
			text = text.Replace(" ", "");
			text = text.Replace("\"", "");
			text = text.Replace("\n", "");
			var words = text.Split(',');
			return words;
		}
	}
}
