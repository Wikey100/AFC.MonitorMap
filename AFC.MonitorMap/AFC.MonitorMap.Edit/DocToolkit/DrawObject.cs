/**********************************************************
** 文件名： DrawObject.cs
** 文件作用:Base class for all draw objects
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace DrawTools.DocToolkit
{
    public abstract class DrawObject
    {
        #region Members

        private bool selected;

        private Color color;
        private int penWidth;
        protected int tagIDBase;

        protected string logicIDTail;

        public string LogicIDTail
        {
            get { return logicIDTail; }
            set { logicIDTail = value; }
        }

        protected string deviceIP;

        public string DeviceIP
        {
            get { return deviceIP; }
            set { deviceIP = value; }
        }

        public int TagIDBase
        {
            get { return tagIDBase; }
            set { tagIDBase = value; }
        }

        private int id;

        private static Color lastUsedColor = Color.Black;

        private static int lastUsedPenWidth = 1;

        private const string entryColor = "Color";

        private const string entryPenWidth = "PenWidth";

        #endregion Members

        public DrawObject()
        {
            id = this.GetHashCode();
        }

        #region Properties

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public int PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }

        public virtual int HandleCount
        {
            get
            {
                return 0;
            }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public static Color LastUsedColor
        {
            get
            {
                return lastUsedColor;
            }
            set
            {
                lastUsedColor = value;
            }
        }

        public static int LastUsedPenWidth
        {
            get
            {
                return lastUsedPenWidth;
            }
            set
            {
                lastUsedPenWidth = value;
            }
        }

        #endregion Properties

        #region Virtual Functions

        public abstract DrawObject Clone();

        public virtual DrawObject Clone(int n)
        {
            return null;
        }

        public virtual void Draw(Graphics g)
        {
        }

        public virtual Point GetHandle(int handleNumber)
        {
            return new Point(0, 0);
        }

        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);
            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }

        public virtual void DrawTracker(Graphics g)
        {
            if (!Selected)
                return;

            SolidBrush brush = new SolidBrush(Color.Black);

            for (int i = 1; i <= HandleCount; i++)
            {
                g.FillRectangle(brush, GetHandleRectangle(i));
            }

            brush.Dispose();
        }

        public virtual int HitTest(Point point)
        {
            return -1;
        }

        protected virtual bool PointInObject(Point point)
        {
            return false;
        }

        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return false;
        }

        public virtual void Move(int deltaX, int deltaY)
        {
        }

        public virtual void MoveHandleTo(Point point, int handleNumber)
        {
        }

        public virtual void Dump()
        {
            Trace.WriteLine(this.GetType().Name);
            Trace.WriteLine("Selected = " +
                selected.ToString(CultureInfo.InvariantCulture)
                + " ID = " + id.ToString(CultureInfo.InvariantCulture));
        }

        public virtual void Normalize()
        {
        }

        public virtual void SaveToStream(SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber),
                Color.ToArgb());

            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber),
                PenWidth);
        }

        public virtual void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber));

            Color = Color.FromArgb(n);

            PenWidth = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber));

            id = this.GetHashCode();
        }

        #endregion Virtual Functions

        #region Other functions

        protected void Initialize()
        {
            color = lastUsedColor;
            penWidth = LastUsedPenWidth;
        }

        protected void FillDrawObjectFields(DrawObject drawObject)
        {
            drawObject.selected = this.selected;
            drawObject.color = this.color;
            drawObject.penWidth = this.penWidth;
            drawObject.ID = this.ID;
        }

        #endregion Other functions

        public virtual void OnDoubleClick(object sender, EventArgs e)
        {
        }

        public static int objIdInc = 1;

        public string LogicIDAdd(string str, int count)
        {
            if (str != "")
            {
                var numStr = count.ToString("x2");
                return numStr;
            }
            else
            {
                return "";
            }
        }

        abstract public XmlElement ToXmlElement(XmlDocument doc);

        public virtual void AntiClockWiseDirection()
        {
        }

        public virtual void ClockWiseDirection()
        {
        }

        public virtual void SetTextColor()
        {
        }

        public virtual void SetTextSize()
        {
        }

        public virtual void ShowItemProperty(bool IsShow)
        {
        }

        public virtual Rectangle GetRectangle()
        {
            Rectangle dd = new Rectangle();
            return dd;
        }

        public virtual void setRectangleX(int X)
        {
        }

        public virtual void setRectangleY(int Y)
        {
        }

        public virtual void setRectangleWidth(int width)
        {
        }

        public virtual void setRectangleHeight(int height)
        {
        }
    }

    public enum EDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum HVDirection
    {
        Horizontal,
        Vertical
    }

    public enum ETextType
    {
        Text,
        Id
    }

    public enum LRDirection
    {
        Left,
        Right
    }
}