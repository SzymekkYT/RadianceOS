﻿using RadianceOS.System.Graphic;
using RadianceOS.System.Managment;
using RadianceOS.System.Programming;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Apps
{
	public static class Terminal
	{
		public static void Render( int X, int Y, int SizeX, int SizeY, int i, List<TextColor> texts)
		{
			int tempX = SizeX;
			if (SizeX + X > Explorer.screenSizeX)
			{
				tempX -= SizeX + X - (int)Explorer.screenSizeX;
			}
			Window.DrawTop(i, X, Y, SizeX, "Terminal", true, true, true, true, tempX);
			if(tempX == SizeX)
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X + 3, Y + 28, tempX, SizeY-25);
			else
			{
				Explorer.CanvasMain.DrawFilledRectangle(Kernel.middark, X, Y + 28, tempX, SizeY - 25);
			}
			Explorer.CanvasMain.DrawFilledRectangle(Kernel.main, X, Y+25, tempX, SizeY-25);
			Explorer.CanvasMain.DrawFilledRectangle(Apps.Process.Processes[i].color3, X+2, Y+27, tempX-4, SizeY-29);


			int start = 0;
			if(texts.Count >= SizeY/18)
			{
				start = texts.Count+1 - SizeY / 18;
			}
			for (int j = start; j < texts.Count; j++)
			{
				string tempText = texts[j].text;
                if (tempText.Length > SizeX / 8)
                {
					tempText = tempText.Substring(0, SizeX / 8 - 1);
                }
                if (j+1 < texts.Count)
				Explorer.CanvasMain.DrawString(tempText, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j-start)*18));
				else if(Apps.Process.Processes[i].selected)
				{
					string result = tempText.Substring(0, Apps.Process.Processes[i].CurrChar) + "_" + tempText.Substring(Apps.Process.Processes[i].CurrChar);
					Explorer.CanvasMain.DrawString(Apps.Process.Processes[i].metaData + ">" + result, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
				}
				else
					Explorer.CanvasMain.DrawString(Apps.Process.Processes[i].metaData + ">" + tempText, Kernel.font18, texts[j].color, X + 3, Y + 27 + ((j - start) * 18));
			}
			if (Apps.Process.Processes[i].selected)
			{
				
				InputSystem.Monitore(1, Apps.Process.Processes[i].CurrChar, i);
				InputSystem.SpecialCharracters = true;
				InputSystem.AllowArrows = true;
				InputSystem.AllowUpDown = false;
				Apps.Process.Processes[i].lines[Apps.Process.Processes[i].lines.Count - 1].text = InputSystem.CurrentString;
				
			}
		}
		public static void enter()
		{
			for (int i = 0; i < Apps.Process.Processes.Count; i++)
			{
				if (Apps.Process.Processes[i].ID == 1)
				{
					if (Apps.Process.Processes[i].selected)
					{
						int lineToAdd = Apps.Process.Processes[i].lines.Count - 1;
						string pathBefore = Apps.Process.Processes[i].metaData;
						Batch.RunCommand(Apps.Process.Processes[i].lines[Apps.Process.Processes[i].lines.Count -1].text, i);
						TextColor empty = new TextColor
						{
							text = "", color = Apps.Process.Processes[i].color1
						};
						TextColor empty2 = new TextColor
						{
							text = "",
							color = Apps.Process.Processes[i].color1
						};
						Apps.Process.Processes[i].lines[lineToAdd].text = (pathBefore + ">" + Apps.Process.Processes[i].lines[lineToAdd].text);
						Apps.Process.Processes[i].lines.Add(empty);
						Apps.Process.Processes[i].lines.Add(empty2);
						InputSystem.CurrentString = "";
						Apps.Process.Processes[i].CurrChar = 0;
					}
				}

			}
		}

		
	}
	public class TextColor
	{
		public string text = "";
		public Color color = Color.White;
	}
}
