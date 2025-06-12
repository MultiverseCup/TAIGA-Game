using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameProject.Components
{
    public class MazeCollectibleComponent
    {
        public int Collected { get; private set; } = 0;
        public void Collect()
        {
            Collected++;
        }
    }

}
