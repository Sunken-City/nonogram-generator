using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Nonogram_Generator
{
	class NonogramBitmapConverter
	{
		private Bitmap image;
		private byte[,] nonogram;
		public int width { get; set; }
		public int height { get; set; }
		private int maxWidth;
		private int maxHeight;
		public List<int>[] TopNumbers { get; set; }
		public List<int>[] SideNumbers { get; set; }

		// Constructor for a new bitmap to Nonogram converter
		public NonogramBitmapConverter(string filePath)
		{
			image = new Bitmap(filePath);
			width = image.Width;
			height = image.Height;
			maxWidth = (width / 2) + 1;
			maxHeight = (height / 2) + 1;
			nonogram = new byte[height, width];
			TopNumbers = new List<int>[width]; //The maximum number of problem numbers is half of the width + 1
			for (int i = 0; i < width; i++)
			{
				TopNumbers[i] = new List<int>();
			}
			SideNumbers = new List<int>[height]; //The maximum number of problem numbers is half of the height + 1
			for (int i = 0; i < width; i++)
			{
				SideNumbers[i] = new List<int>();
			}
			ConvertImage();
		}

		// Constructor for a new bitmap to Nonogram converter using a bitmap
		public NonogramBitmapConverter(Bitmap original)
		{
			image = original;
			width = image.Width;
			height = image.Height;
			maxWidth = (width / 2) + 1;
			maxHeight = (height / 2) + 1;
			nonogram = new byte[height, width];
			TopNumbers = new List<int>[width]; //The maximum number of problem numbers is half of the width + 1
			for (int i = 0; i < width; i++)
			{
				TopNumbers[i] = new List<int>();
			}
			SideNumbers = new List<int>[height]; //The maximum number of problem numbers is half of the height + 1
			for (int i = 0; i < width; i++)
			{
				SideNumbers[i] = new List<int>();
			}
			ConvertImage();
		}

		public byte[,] ToByteArray()
		{
			return nonogram;
		}
		
		private void ConvertImage()
		{
			int color;
			for (int currWidth = 0; currWidth < width; currWidth++)
			{
				for (int currHeight = 0; currHeight < height; currHeight++)
				{
					// Get the color of the pixel from the original image
					Color currColor = image.GetPixel(currWidth, currHeight);
					color = currColor.ToArgb();
					//If the color is white
					if (color == -1)
					{
						nonogram[currHeight, currWidth] = 0;
					}
					//If the color is black
					else if (color == -16777216)
					{
						nonogram[currHeight, currWidth] = 1;
					}
					else
					{
						nonogram[currHeight, currWidth] = 0;
					}
				}
			}
		}

		//A really ugly calculation, in nicer form.
		private int GetIndex(int height, int count)
		{
			return (-1 * ((height - 1) - (count - 1)));
		}

		public string[] GeneratePuzzle()
		{
			string[] puzzle = new string[height + maxHeight];
			for (int i = 0; i < height + maxHeight; i++)
			{
				puzzle[i] = string.Empty;
			}
			GenerateTop();
			GenerateSide();
			string margin = "";
			int currLine = 0;
			for (int i = 0; i < maxWidth; i++)
			{
				margin = margin + "  ";
			}

			// Create the top lines
			for (int currHeight = maxHeight; currHeight > 0; currHeight--)
			{
				puzzle[currLine] = margin + puzzle[currLine];
				for (int currWidth = 0; currWidth < width; currWidth++)
				{
					if (TopNumbers[currWidth].Count < currHeight)
					{
						puzzle[currLine] = puzzle[currLine] + "   ";
					}
					else if (TopNumbers[currWidth].ElementAt(GetIndex(currHeight, TopNumbers[currWidth].Count)) < 10)
					{
						puzzle[currLine] = puzzle[currLine] + " " + TopNumbers[currWidth].ElementAt(GetIndex(currHeight, TopNumbers[currWidth].Count)) + " ";
					}
					else
					{
						puzzle[currLine] = puzzle[currLine] + TopNumbers[currWidth].ElementAt(GetIndex(currHeight, TopNumbers[currWidth].Count)) + " ";
					}
				}

				puzzle[currLine] = puzzle[currLine] + "\n";
				currLine++;
			}

			// Create the side lines
			for (int currHeight = 0; currHeight < height; currHeight++)
			{
				for (int currWidth = 0; currWidth < SideNumbers[currHeight].Count; currWidth++)
				{
					if (SideNumbers[currHeight].ElementAt(currWidth) < 10)
					{
						puzzle[currLine] = puzzle[currLine] + " " + SideNumbers[currHeight].ElementAt(currWidth) + " ";
					}
					else
					{
						puzzle[currLine] = puzzle[currLine] + SideNumbers[currHeight].ElementAt(currWidth) + " ";
					}
				}
				while (puzzle[currLine].Length < margin.Length)
				{
					puzzle[currLine] = " " + puzzle[currLine];
				} 
				puzzle[currLine] = puzzle[currLine] + "\n";
				currLine++;
			}
			return puzzle;
		}

		private void GenerateTop()
		{
			for (int currWidth = 0; currWidth < width; currWidth++)
			{
				int blockCounter = 0;
				int topCounter = 0;
				//Console.Out.Write("[");

				for (int currHeight = 0; currHeight < height; currHeight++)
				{
					if ((nonogram[currHeight, currWidth] == 0)&&(blockCounter != 0))
					{
						TopNumbers[currWidth].Add(blockCounter);
						//Console.Out.Write(blockCounter + " ");
						topCounter++;
						blockCounter = 0;
					}
					else if (nonogram[currHeight, currWidth] == 1)
					{
						blockCounter++;
					}
				}
				if (topCounter == 0)
				{
					TopNumbers[currWidth].Add(blockCounter);
				}
				else if (blockCounter != 0)
				{
					TopNumbers[currWidth].Add(blockCounter);
				}
				//Console.Out.Write(blockCounter  + "]\n");
			}
		}

		private void GenerateSide()
		{
			for (int currHeight = 0; currHeight < height; currHeight++)
			{
				int blockCounter = 0;
				int topCounter = 0;
				//Console.Out.Write("[");
				for (int currWidth = 0; currWidth < width; currWidth++)
				{
					if ((nonogram[currHeight, currWidth] == 0) && (blockCounter != 0))
					{
						SideNumbers[currHeight].Add(blockCounter);
						//Console.Out.Write(blockCounter + " ");
						topCounter++;
						blockCounter = 0;
					}
					else if (nonogram[currHeight, currWidth] == 1)
					{
						blockCounter++;
					}
				}
				if (topCounter == 0)
				{
					SideNumbers[currHeight].Add(blockCounter);
				}
				else if (blockCounter != 0)
				{
					SideNumbers[currHeight].Add(blockCounter);
				}
				//Console.Out.Write(blockCounter  + "]\n");
			}
		}
	}
}
