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
        [DefaultValue(100)]
        public int block_lenght { get; set; }

        [DefaultValue(10)]
        public int street_width { get; set; }

        public int map_width { get; set; }
        public int map_height { get; set; }

        public PositionTracking(Drawable3D draw){
            map_width = 3 * block_lenght + street_width * 2;
            map_height = map_width;
        }
        /*
        public Point BlockCoordinate(Vector4 pos)
        {
            // Asumes block is larger than street
            
            int x = 0
        }*/
    }
}
