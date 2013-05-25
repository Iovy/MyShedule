//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  Program.cs
   Tool:      objectiF, CSharpSSvr V7.1.23
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Windows.Forms;

namespace MyShedule
{	
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
		    Application.EnableVisualStyles();
		    Application.SetCompatibleTextRenderingDefault(false);
		    Application.Run(new MainForm());
		}
	}
}