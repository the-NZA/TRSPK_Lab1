using System;

namespace UILayerConsole
{
	public static class HelperFuncs
	{
		public static DateTime ReadDate(string text)
		{
			Console.Write($"{text} (формат 01.22.2021)\n> ");

			string tmp = Console.ReadLine();
			if (String.IsNullOrWhiteSpace(tmp))
			{
				throw new Exception("Введена пустаная строка");
			}

			DateTime date;
			if (!DateTime.TryParse(tmp, out date))
			{
				throw new Exception("Введен недопустимый формат даты");
			}

			return date;
		}

		public static uint ReadUint(string text, uint upperBound)
		{
			Console.Write($"{text}\n> ");

			string tmp = Console.ReadLine();
			if (String.IsNullOrWhiteSpace(tmp))
			{
				throw new Exception("Введена пустаная строка");
			}

			uint i = Convert.ToUInt32(tmp);
			if (i >= upperBound)
			{
				throw new Exception("Введено недопустимое значение");
			}

			return i;
		}

		public static int ReadInt(string text)
		{
			Console.Write($"{text}\n> ");

			string tmp = Console.ReadLine();
			if (String.IsNullOrWhiteSpace(tmp))
			{
				throw new Exception("Введена пустаная строка");
			}

			int i = Convert.ToInt32(tmp);

			return i;
		}

		public static string ReadString(string text)
		{
			Console.Write($"{text}\n> ");

			string tmp = Console.ReadLine();
			if (String.IsNullOrWhiteSpace(tmp))
			{
				throw new Exception("Введена пустаная строка");
			}

			return tmp;
		}

		public static decimal ReadDecimal(string text)
		{
			Console.Write($"{text}\n> ");

			string tmp = Console.ReadLine();
			if (String.IsNullOrWhiteSpace(tmp))
			{
				throw new Exception("Введена пустаная строка");
			}

			decimal i = Convert.ToDecimal(tmp);

			return i;
		}
	}
}