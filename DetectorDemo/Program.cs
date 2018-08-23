using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DetectorDemo
{
    class Program
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct bbox_t
        {
            public uint x, y, w, h;     //(x,y) - top-left corner, (w, h) -width & height of bounded box
            public float prob;          // confidence - probability that the object was found correctly
            public uint obj_id;         // class of object - from range [0, classes-1]
            public uint track_id;       // tracking id for video (0 - untracked, 1 - inf - tracked object)
            public uint frames_counter; // counter of frames on which the object was detected

            public override string ToString()
            {
                if (prob != 0) return "obj id:" + obj_id + ", prob:" + prob;
                else return "";
            
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct bbox_t_container
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
            public bbox_t[] candidates;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct image_t
        {
            int h;          // height
            int w;          // width
            int c;          // number of chanels (3 - for RGB)
            float[] data;    // pointer to the image data
        }

        [DllImport("E:\\VSProjects\\darknet\\build\\darknet\\x64\\yolo_cpp_dll.dll")]
        public static extern int init(string configurationFilename, string weightsFilename, int gpu);

        [DllImport("E:\\VSProjects\\darknet\\build\\darknet\\x64\\yolo_cpp_dll.dll")]
        public static extern int detect_image(string filename, ref bbox_t_container container);

        [DllImport("E:\\VSProjects\\darknet\\build\\darknet\\x64\\yolo_cpp_dll.dll")]
        public static extern int detect_mat(IntPtr data, UIntPtr data_length, ref bbox_t_container container);

        [DllImport("E:\\VSProjects\\darknet\\build\\darknet\\x64\\yolo_cpp_dll.dll")]
        public static extern int dispose();

        [DllImport("E:\\VSProjects\\darknet\\build\\darknet\\x64\\yolo_cpp_dll.dll")]
        public static extern int get_device_count();

        [DllImport("E:\\VSProjects\\darknet\\build\\darknet\\x64\\yolo_cpp_dll.dll")]
        public static extern int get_device_name(int gpu, StringBuilder deviceName);


        static void Main(string[] args)
        {
            var prefix = "E:\\VSProjects\\darknet\\build\\darknet\\x64\\";
            init(prefix + "cfg\\yolov3-tiny.cfg", prefix + "yolov3-tiny.weights", 0);

            var btc = new bbox_t_container();

            detect_image("E:\\Pictures\\dog.jpg", ref btc);
            foreach(var box in btc.candidates)
            {
                Console.Write(box.ToString());
            }
            dispose();
        }
    }
}
