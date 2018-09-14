/**********************************************************
** 文件名： DrawPolygon.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools.DocToolkit
{
    using PointEnumerator = IEnumerator<Point>;
    using PointList = List<Point>;

    public class DrawPolygon : DrawLine
    {
        private PointList pointArray;

        private static Cursor handleCursor = new Cursor(typeof(DrawPolygon), "PolyHandle.cur");

        private const string entryLength = "Length";
        private const string entryPoint = "Point";

        public DrawPolygon() : base()
        {
            pointArray = new PointList();

            Initialize();
        }

        public DrawPolygon(int x1, int y1, int x2, int y2) : base()
        {
            pointArray = new PointList();
            pointArray.Add(new Point(x1, y1));
            pointArray.Add(new Point(x2, y2));

            Initialize();
        }

        public override DrawObject Clone()
        {
            DrawPolygon drawPolygon = new DrawPolygon();

            foreach (Point p in this.pointArray)
            {
                drawPolygon.pointArray.Add(p);
            }

            FillDrawObjectFields(drawPolygon);
            return drawPolygon;
        }

        public override void Draw(Graphics g)
        {
            int x1 = 0, y1 = 0;
            int x2, y2;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color, PenWidth);

            PointEnumerator enumerator = pointArray.GetEnumerator();

            if (enumerator.MoveNext())
            {
                x1 = ((Point)enumerator.Current).X;
                y1 = ((Point)enumerator.Current).Y;
            }

            while (enumerator.MoveNext())
            {
                x2 = ((Point)enumerator.Current).X;
                y2 = ((Point)enumerator.Current).Y;

                g.DrawLine(pen, x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            pen.Dispose();
        }

        public void AddPoint(Point point)
        {
            pointArray.Add(point);
        }

        public override int HandleCount
        {
            get
            {
                return pointArray.Count;
            }
        }

        public override Point GetHandle(int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > pointArray.Count)
                handleNumber = pointArray.Count;

            return ((Point)pointArray[handleNumber - 1]);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return handleCursor;
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > pointArray.Count)
                handleNumber = pointArray.Count;

            pointArray[handleNumber - 1] = point;

            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            int n = pointArray.Count;
            Point point;
            for (int i = 0; i < n; i++)
            {
                point = new Point(((Point)pointArray[i]).X + deltaX, ((Point)pointArray[i]).Y + deltaY);

                pointArray[i] = point;
            }
            Invalidate();
        }

        public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, orderNumber),
                pointArray.Count);

            int i = 0;
            foreach (Point p in pointArray)
            {
                info.AddValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, orderNumber, i++),
                    p);
            }

            base.SaveToStream(info, orderNumber);  // ??
        }

        public override void LoadFromStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            Point point;
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, orderNumber));

            for (int i = 0; i < n; i++)
            {
                point = (Point)info.GetValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, orderNumber, i),
                    typeof(Point));

                pointArray.Add(point);
            }

            base.LoadFromStream(info, orderNumber);
        }

        protected override void CreateObjects()
        {
            if (AreaPath != null)
                return;

            AreaPath = new GraphicsPath();

            int x1 = 0, y1 = 0;
            int x2, y2;

            PointEnumerator enumerator = pointArray.GetEnumerator();

            if (enumerator.MoveNext())
            {
                x1 = ((Point)enumerator.Current).X;
                y1 = ((Point)enumerator.Current).Y;
            }

            while (enumerator.MoveNext())
            {
                x2 = ((Point)enumerator.Current).X;
                y2 = ((Point)enumerator.Current).Y;

                AreaPath.AddLine(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            AreaPath.CloseFigure();

            AreaRegion = new Region(AreaPath);
        }
    }
}