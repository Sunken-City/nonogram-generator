using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nonogram_Generator
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Out.WriteLine("Please enter the filename of the bitmap.");
			string fileName = @"C:\Users\acloudy\Desktop\Cards\smiley.bmp"; ////Console.In.ReadLine();
			//try
			{
				NonogramBitmapConverter convert = new NonogramBitmapConverter(fileName);
				byte [,] pico = convert.ToByteArray();
				for (int currWidth = 0; currWidth < convert.width; currWidth++)
				{
					for (int currHeight = 0; currHeight < convert.height; currHeight++)
					{
						Console.Out.Write(pico[currWidth,currHeight] + " ");
					}
					Console.Out.Write("\n");
				}
				string[] file = convert.GeneratePuzzle();
				foreach (string item in file)
				{
					Console.Out.WriteLine(item);
				}
			}
			//catch (Exception e)
			{
			//	Console.Out.WriteLine(e.Message + "\n Press enter");
			}
			Console.In.ReadLine();
			
		}
	}
}