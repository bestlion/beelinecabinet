using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;

namespace tpscabinet
{
    public class helper
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);

        private enum ABM : uint
        {
            New = 0x00000000,
            Remove = 0x00000001,
            QueryPos = 0x00000002,
            SetPos = 0x00000003,
            GetState = 0x00000004,
            GetTaskbarPos = 0x00000005,
            Activate = 0x00000006,
            GetAutoHideBar = 0x00000007,
            SetAutoHideBar = 0x00000008,
            WindowPosChanged = 0x00000009,
            SetState = 0x0000000A,
        }
        private struct RECT { public int left, top, right, bottom; }
        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        public enum TaskbarPos
        {
            Unknown = -1,
            Left,
            Top,
            Right,
            Bottom,
        }
        public TaskbarPos TaskbarPosition
        {
            get;
            private set;
        }
        
        public Rectangle TaskbarBounds
        {
            get;
            private set;
        }
        
        public void GetTaskbarInfo()
        {
            APPBARDATA data = new APPBARDATA();
            data.cbSize = Marshal.SizeOf(typeof(APPBARDATA));
            IntPtr retval = SHAppBarMessage(ABM.GetTaskbarPos, ref data);
            if (retval == IntPtr.Zero) throw new Win32Exception("Please re-install Windows");
            this.TaskbarBounds = Rectangle.FromLTRB(data.rc.left, data.rc.top, data.rc.right, data.rc.bottom);
            this.TaskbarPosition = (TaskbarPos)data.uEdge;
        }


        public enum RectangleCorners
        {
            None = 0, TopLeft = 1, TopRight = 2, BottomLeft = 4, BottomRight = 8, All = TopLeft | TopRight | BottomLeft | BottomRight
        }
        public static GraphicsPath CreateRoundRect(Rectangle r, int radius, RectangleCorners corners)
        {
            Rectangle tlc = new Rectangle(r.Left, r.Top, Math.Min(2 * radius, r.Width), Math.Min(2 * radius, r.Height));
            Rectangle trc = tlc;
            trc.X = r.Right - 2 * radius;
            Rectangle blc = tlc;
            blc.Y = r.Bottom - 2 * radius;
            Rectangle brc = blc;
            brc.X = r.Right - 2 * radius;

            Point[] n = new Point[] {
                new Point(tlc.Left, tlc.Bottom), tlc.Location, 
                new Point(tlc.Right, tlc.Top), trc.Location, 
                new Point(trc.Right, trc.Top), 
                new Point(trc.Right, trc.Bottom),
                new Point(brc.Right, brc.Top), 
                new Point(brc.Right, brc.Bottom), 
                new Point(brc.Left, brc.Bottom), 
                new Point(blc.Right, blc.Bottom), 
                new Point(blc.Left, blc.Bottom), blc.Location
            };

            GraphicsPath p = new GraphicsPath();
            p.StartFigure();

            //Top Left Corner
            if ((RectangleCorners.TopLeft & corners)  == RectangleCorners.TopLeft)
                p.AddArc(tlc, 180, 90);
            else
                p.AddLines(new Point[] { n[0], n[1], n[2] });

            //Top Edge
            p.AddLine(n[2], n[3]);

            //Top Right Corner
            if ((RectangleCorners.TopRight & corners) == RectangleCorners.TopRight)
                p.AddArc(trc, 270, 90);
            else
                p.AddLines(new Point[] { n[3], n[4], n[5] });

            //Right Edge
            p.AddLine(n[5], n[6]);

            //Bottom Right Corner
            if ((RectangleCorners.BottomRight & corners) == RectangleCorners.BottomRight)
                p.AddArc(brc, 0, 90);
            else
                p.AddLines(new Point[] { n[6], n[7], n[8] });

            //Bottom Edge
            p.AddLine(n[8], n[9]);

            //Bottom Left Corner
            if ((RectangleCorners.BottomLeft & corners) == RectangleCorners.BottomLeft)
                p.AddArc(blc, 90, 90);
            else
                p.AddLines(new Point[] { n[9], n[10], n[11] });

            //Left Edge
            p.AddLine(n[11], n[0]);

            p.CloseFigure();
            return p;
        }
    }
}
