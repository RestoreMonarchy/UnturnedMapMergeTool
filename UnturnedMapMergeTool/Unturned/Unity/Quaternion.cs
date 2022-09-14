using System;

namespace UnturnedMapMergeTool.Unturned.Unity
{
    public class Quaternion
    {
        public Quaternion()
        {

        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }


        public Vector3 eulerAngles
            => ToEulerAngles(this);


        public static Quaternion Euler(float x, float y, float z)
        {
            return ToQuaternion(x, y, z);
        }


        // Copied from https://stackoverflow.com/a/70462919/11375275
        public static Quaternion ToQuaternion(double yaw, double pitch, double roll)
        {
            double cy = Math.Cos(yaw * 0.5);
            double sy = Math.Sin(yaw * 0.5);
            double cp = Math.Cos(pitch * 0.5);
            double sp = Math.Sin(pitch * 0.5);
            double cr = Math.Cos(roll * 0.5);
            double sr = Math.Sin(roll * 0.5);

            Quaternion q = new Quaternion();
            q.w = Convert.ToSingle(cr * cp * cy + sr * sp * sy);
            q.x = Convert.ToSingle(sr * cp * cy - cr * sp * sy);
            q.y = Convert.ToSingle(cr * sp * cy + sr * cp * sy);
            q.z = Convert.ToSingle(cr * cp * sy - sr * sp * cy);

            return q;
        }

        public static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles = new();

            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            angles.x = Convert.ToSingle(Math.Atan2(sinr_cosp, cosr_cosp));

            // pitch (y-axis rotation)
            double sinp = 2 * (q.w * q.y - q.z * q.x);
            if (Math.Abs(sinp) >= 1)
            {
                angles.y = Convert.ToSingle(Math.CopySign(Math.PI / 2, sinp));
            }
            else
            {
                angles.y = Convert.ToSingle(Math.Asin(sinp));
            }

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            angles.z = Convert.ToSingle(Math.Atan2(siny_cosp, cosy_cosp));

            return angles;
        }

    }
}
