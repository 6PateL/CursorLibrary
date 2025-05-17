using CursorLibrary.SD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursorLibrary.Models
{
    public class MouseInfoModel
    {
        public int PositionX { get; set; } = 0;
        public int PositionY { get; set; } = 0; 
        public MouseType MouseType { get; set; } = MouseType.UNSELECTED;
    }
}
