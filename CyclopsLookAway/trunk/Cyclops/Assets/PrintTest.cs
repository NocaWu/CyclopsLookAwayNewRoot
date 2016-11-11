using UnityEngine;
using System.Collections;
using System;
using System.Drawing;
using System.Drawing.Printing;


public class PrintTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PCPrint printer = new PCPrint ();
		printer.PrinterFont = new System.Drawing.Font ("Arial", 10);
		printer.TextToPrint = "<3 from Unity";
		printer.Print ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public class PCPrint : System.Drawing.Printing.PrintDocument{
		private System.Drawing.Font _font;
		private string _text;

		public string TextToPrint{
			get { return _text; }
			set { _text = value; }
		}
		public System.Drawing.Font PrinterFont{
			get { return _font; }
			set { _font = value; }
		}

		static int curChar;

		public PCPrint() : base(){
			_text = string.Empty;
		}

		public PCPrint(string str) : base(){
			_text = str;
		}


		protected override void OnBeginPrint(System.Drawing.Printing.PrintEventArgs e){
			base.OnBeginPrint (e);


			if (_font == null) {
				_font = new System.Drawing.Font ("Arial", 10);
			}
		}

		protected override void OnPrintPage(System.Drawing.Printing.PrintPageEventArgs e){
			base.OnPrintPage (e);

			int printHeight;
			int printWidth;
			int leftMargin;
			int rightMargin;
			Int32 lines;
			Int32 chars;

			{
				printHeight = base.DefaultPageSettings.PaperSize.Height - base.DefaultPageSettings.Margins.Top - base.DefaultPageSettings.Margins.Bottom;
				printWidth = base.DefaultPageSettings.PaperSize.Width - base.DefaultPageSettings.Margins.Left - base.DefaultPageSettings.Margins.Right;
				leftMargin = base.DefaultPageSettings.Margins.Left;  //X
				rightMargin = base.DefaultPageSettings.Margins.Top;  //Y
			}



			if (base.DefaultPageSettings.Landscape)
			{
				int tmp;

				tmp = printHeight;
				printHeight = printWidth;
				printWidth = tmp;
			}

			Int32 numLines = (int)printHeight / PrinterFont.Height;
			RectangleF printArea = new RectangleF(leftMargin, rightMargin, printWidth, printHeight);
			StringFormat format = new StringFormat(StringFormatFlags.LineLimit);
			e.Graphics.MeasureString(_text.Substring(RemoveZeros(curChar)), PrinterFont, new SizeF(printWidth, printHeight), format, out chars, out lines);
			e.Graphics.DrawString(_text.Substring(RemoveZeros(curChar)), PrinterFont, Brushes.Black, printArea, format);
			curChar += chars;
			if (curChar < _text.Length)
			{
				e.HasMorePages = true;
			}
			else
			{
				e.HasMorePages = false;
				curChar = 0;
			}

		}
			
		public int RemoveZeros(int value)
		{
			switch (value)
			{
			case 0:
				return 1;
			default:
				return value;
			}
		}


	}
}
