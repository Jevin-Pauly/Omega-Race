using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzulNetworkBase
{
    class Tank
    {
        Azul.Sprite tankbase;
        public Tank(Azul.Sprite b, float x, float y)
        {
            tankbase = b;
            this.Set(x, y);
        }

        public void Set(float x, float y)
        {
            tankbase.x = x;
            tankbase.y = y;
            tankbase.Update();
        }

        public void Move(float xdelta, float ydelta)
        {
            Set(tankbase.x + xdelta, tankbase.y + ydelta);
        }

        public void render()
        {
            tankbase.Render();
        }
    }
}
