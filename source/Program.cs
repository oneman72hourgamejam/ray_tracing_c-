using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ray_tracing_c_sharp
{
    class Vector3
    {
        public float[] e = new float[3];

        public Vector3() { }

        public Vector3(float e0, float e1, float e2)
        {
            e[0] = e0;
            e[1] = e1;
            e[2] = e2;
        }

        public float x()
        {
            return e[0];
        }

        public float y()
        {
            return e[1];
        }

        public float z()
        {
            return e[2];
        }

        public float r()
        {
            return e[0];
        }

        public float g()
        {
            return e[1];
        }

        public float b()
        {
            return e[2];
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.e[0] * v2.e[0], v1.e[1] * v2.e[1], v1.e[2] * v2.e[2]);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.e[0] + v2.e[0], v1.e[1] + v2.e[1], v1.e[2] + v2.e[2]);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.e[0] - v2.e[0], v1.e[1] - v2.e[1], v1.e[2] - v2.e[2]);
        }

        public static Vector3 operator *(float t, Vector3 v)
        {
            return new Vector3(t * v.e[0], t * v.e[1], t * v.e[2]);
        }

        public float length()
        {
            return (float)Math.Sqrt(e[0] * e[0] + e[1] * e[1] + e[2] * e[2]);
        }

        public static Vector3 operator /(Vector3 v, float t)
        {
            return new Vector3(v.e[0] / t, v.e[1] / t, v.e[2] / t);
        }

        public Vector3 unit_vector(Vector3 v)
        {
            return v / v.length();
        }

        public float dot(Vector3 v1, Vector3 v2)
        {
            return v1.e[0] * v2.e[0] + v1.e[1] * v2.e[1] + v1.e[2] * v2.e[2];
        }
    }

    class Ray
    {
        public Vector3 A;
        public Vector3 B;

        public Ray() { }
        public Ray(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
        }

        public Vector3 origin()
        {
            return A;
        }

        public Vector3 direction()
        {
            return B;
        }

        public Vector3 point_at_parameter(float t)
        {
            return A + t * B;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int nx = 200;
            int ny = 100;
            string file = @"C:\Users\User\Desktop\ex1.ppm";

            var writer = new StreamWriter(file);
            writer.WriteLine("P3");
            writer.WriteLine($"{nx}  {ny}");
            writer.WriteLine("255");
            writer.Close();

            var writerB = new BinaryWriter(new FileStream(file, FileMode.Append));

            Vector3 lower_left_corner = new Vector3(-2.0f, -1.0f, -1.0f);
            Vector3 horizontal = new Vector3(4.0f, 0.0f, 0.0f);
            Vector3 vertical = new Vector3(0.0f, 2.0f, 0.0f);
            Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);

            for (int j = ny-1; j >= 0; j--)
            {
                for (int i = 0; i < nx; i++)
                {
                    //Vector3 vec3 = new Vector3((float)i / (float)nx, (float)j / (float)ny, 0.2f);

                    float u = (float)i / (float)nx;
                    float v = (float)j / (float)ny;

                    Ray r = new Ray(origin, lower_left_corner + u * horizontal + v * vertical);
                    Vector3 vec3 = color(r);

                    int ir = (int)(255.99f * vec3.e[0]);
                    int ig = (int)(255.99f * vec3.e[1]);
                    int ib = (int)(255.99f * vec3.e[2]);

                    string s = ir + " " + ig + " " + ib;
                    writerB.Write(s);
                }
            }
            writerB.Close();
            Console.ReadLine();
        }

        public static float hit_sphere(Vector3 center, float radius, Ray r)
        {
            Vector3 oc = new Vector3();
            oc = r.origin() - center;
            float a = new Vector3().dot(r.direction(), r.direction());
            float b = 2.0f * new Vector3().dot(oc, r.direction());
            float c = new Vector3().dot(oc, oc) - radius * radius;
            float discriminant = b * b - 4 * a * c;
            if(discriminant < 0)
            {
                return -1.0f;
            }
            else
            {
                return (-b - (float)Math.Sqrt(discriminant)) / (2.0f * a);
            }
        }

        public static Vector3 color(Ray r)
        {
            float t = hit_sphere(new Vector3(0, 0, -1), 0.5f, r);
            if(t > 0.0f)
            {
                Vector3 N = new Vector3().unit_vector(r.point_at_parameter(t) - new Vector3(0, 0, -1));
                return 0.5f * new Vector3(N.x() + 1, N.y() + 1, N.z() + 1);
            }
            Vector3 unit_direction = new Vector3().unit_vector(r.direction());
            t = 0.5f * (unit_direction.y() + 1.0f);
            return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}
