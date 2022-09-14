using System;

namespace UnturnedMapMergeTool.Unturned.Unity
{
    public class EulerAngles
    {
        public EulerAngles()
        {

        }

        public EulerAngles(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

    }
}
