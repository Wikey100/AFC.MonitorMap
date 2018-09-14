/**********************************************************
** 文件名： Text.cs
** 文件作用:Text设备
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using DrawTools.DocToolkit;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrawTools.Device
{
    public class Text : DrawRectangle
    {
        private int objectID;
        private int flgID;
        private string textType;
        private string text;
        private Font font;
        private Color fontColor;

        public int ObjectID
        {
            get { return objectID; }
            set { objectID = value; }
        }

        public int FlgID
        {
            get { return flgID; }
            set { flgID = value; }
        }

        public Rectangle RectangleLs
        {
            get { return Rectangle; }
            set { Rectangle = value; }
        }

        public string TextType
        {
            get { return textType; }
            set { textType = value; }
        }

        public string Texttest
        {
            get { return text; }
            set { text = value; }
        }

        public Font TextFont
        {
            get { return font; }
            set { font = value; }
        }

        public Color FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }

        public Text() : this(0, 0, 1, 1)
        {
        }

        public Text(int x, int y, int width, int height)
            : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            objectID = objIdInc++;
            text = "Text";
            font = new Font("宋体", 9, FontStyle.Regular);
            fontColor = Color.Black;
            textType = "Text";
            setTextDisplay(x, y);
            Initialize();
        }

        public void setTextDisplay(int x, int y)
        {
            int d = font.Height;
            float f = font.Size;
            int a = System.Text.Encoding.Default.GetByteCount(text);
            int w = (int)(f * a) / 1 * 1;
            Rectangle = new Rectangle(x, y, w, d);
        }

        public override DrawObject Clone()
        {
            Text drawText = new Text();
            drawText.Rectangle = this.Rectangle;
            drawText.TextFont = this.TextFont;
            drawText.ObjectID = this.ObjectID;
            drawText.Texttest = this.Texttest;
            drawText.FontColor = this.FontColor;
            drawText.TextType = this.TextType;
            FillDrawObjectFields(drawText);
            return drawText;
        }

        public override void Draw(Graphics g)
        {
            ContentAlignment alignmentValue = ContentAlignment.BottomLeft;
            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Near;
            switch (alignmentValue)
            {
                case ContentAlignment.MiddleLeft:
                    style.Alignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleRight:
                    style.Alignment = StringAlignment.Far;
                    break;

                case ContentAlignment.MiddleCenter:
                    style.Alignment = StringAlignment.Center;
                    break;
            }
            g.DrawString(
                text,
                font,
                new SolidBrush(fontColor),
                Rectangle, style);
        }

        public override void OnDoubleClick(Object sender, EventArgs e)
        {
            AddTextForm dlg = new AddTextForm(text);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                text = dlg.Textvalues;
                setTextDisplay(this.RectangleLs.X, this.RectangleLs.Y);
                ((DrawArea)sender).SetDirty();
            }
        }

        public override void SetTextColor()
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = this.fontColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fontColor = dlg.Color;
            }
        }

        public override void SetTextSize()
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = this.font;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                font = dlg.Font;
                setTextDisplay(this.RectangleLs.X, this.RectangleLs.Y);
            }
        }

        public override void AntiClockWiseDirection()
        { }

        public override void ClockWiseDirection()
        { }

        public override Rectangle GetRectangle()
        {
            return Rectangle;
        }

        public void SetFontColor(string color)
        {
            int first = color.IndexOf(',');
            int second = color.LastIndexOf(',');
            int r = int.Parse(color.Substring(0, first).Trim());
            int g = int.Parse(color.Substring(first + 1, second - (first + 1)).Trim());
            int b = int.Parse(color.Substring(second + 1).Trim());
            this.fontColor = Color.FromArgb(r, g, b);
        }
    }
}