using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using System.ComponentModel;

namespace Utilities
{
    public class PositionTracking
    {
        Vector4 lastPos;
        Vector4 curPos;

        Point lastCell = new Point();
        Point curCell = new Point();

        public int block_lenght = 100;
        public float street_width = 13.6f;
        public int map_width { get; set; }
        public int map_height { get; set; }

        int corner_pos;
        Point center;

        public PositionTracking(){
            block_lenght = 100;
            street_width = 10;

            map_width = (int) (3 * block_lenght + street_width * 2);
            map_height = map_width;
            
            corner_pos = (int) (block_lenght / street_width + 1);

            center.X = (int) ((map_width / 2) / street_width);
            center.Y = center.X;
        }

        public void setLastPos(Vector4 val)
        {
            lastPos = val;
            lastCell = BlockCoordinate(val);
        }

        public void setCurPos(Vector4 val)
        {
            curPos = val;
            curCell = BlockCoordinate(val);
        }

        public Point BlockCoordinate(Vector4 pos)
        {
            int x = (int) (pos.X / street_width);
            int y = (int) (pos.Y / street_width);

            return new Point(x, y);
        }

        public void Teletransport(Vector4 new_pos)
        {
            setLastPos(new Vector4(curPos));
            setCurPos(new Vector4(new_pos));

            bool last_cell_was_corner = lastCell.X % corner_pos == 0 && lastCell.Y % corner_pos == 0;

            Point last_dist = DistFromCenter(lastCell);
            Point cur_dist = DistFromCenter(curCell);
            bool going_to_border = last_dist.X <= cur_dist.X && last_dist.Y <= cur_dist.Y;

            if (last_cell_was_corner && going_to_border)
            {
                Point dir = new Point(curCell.X - lastCell.X, curCell.Y - lastCell.Y);

                setLastPos(new Vector4(curPos));

                setCurPos(new Vector4((curPos.X + block_lenght * 2 * dir.X) % map_width,
                    (curPos.Y + block_lenght * 2 * dir.Y) % map_height, curPos.Z, curPos.W));
            }          
                
        }

        private Point DistFromCenter(Point cell)
        {
            return new Point(Math.Abs(center.X - cell.X), Math.Abs(center.Y - cell.Y));
        }


    }
}
