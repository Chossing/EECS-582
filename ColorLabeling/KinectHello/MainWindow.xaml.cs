using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Drawing.Imaging;
using Microsoft.Kinect;

/*
 * Built off extremely useful code from: http://social.msdn.microsoft.com/Forums/en-US/kinectsdknuiapi/thread/c39bab30-a704-4de1-948d-307afd128dab
 */

namespace ColorLabeling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensor _sensor;
        private WriteableBitmap _bitmap;
        private byte[] _bitmapBits;
        private ColorImagePoint[] _mappedDepthLocations;
        private byte[] _colorPixels = new byte[0];
        private short[] _depthPixels = new short[0];
        private Dictionary<byte[], byte[]> nearest_cache = new Dictionary<byte[], byte[]>();

        private void SetSensor(KinectSensor newSensor)
        {
            if (_sensor != null) _sensor.Stop();
            _sensor = newSensor;
            if (_sensor != null)
            {
                Debug.Assert(_sensor.Status == KinectStatus.Connected, "This should only be called with Connected sensors.");
                _sensor.ColorStream.Enable(ColorImageFormat.YuvResolution640x480Fps15);
                _sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                _sensor.AllFramesReady += _sensor_AllFramesReady; // Register event
                _sensor.Start();
            }
        }

        void _sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    Debug.Assert(colorFrame.Width == 640 && colorFrame.Height == 480, "This app only uses 640x480.");

                    if (_colorPixels.Length != colorFrame.PixelDataLength)
                    {
                        _colorPixels = new byte[colorFrame.PixelDataLength];
                        _bitmap = new WriteableBitmap(640, 480, 96.0, 96.0, PixelFormats.Bgr32, null);
                        _bitmapBits = new byte[640 * 480 * 4];
                        this.image1.Source = _bitmap; // Assign the WPF element to _bitmap
                    }

                    colorFrame.CopyPixelDataTo(_colorPixels);
                }
            }

            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame != null)
                {
                    Debug.Assert(depthFrame.Width == 640 && depthFrame.Height == 480, "This app only uses 640x480.");

                    if (_depthPixels.Length != depthFrame.PixelDataLength)
                    {
                        _depthPixels = new short[depthFrame.PixelDataLength];
                        _mappedDepthLocations = new ColorImagePoint[depthFrame.PixelDataLength];
                    }

                    depthFrame.CopyPixelDataTo(_depthPixels);
                }
            }

            // Maximum short = 32767
            // Minimum short = 0 ...

            //for (int i = 0; i < _colorPixels.Length; i++) _bitmapBits[i] = 255;
            // Draw the un-mapped depth image
            /*
            for (int i = 0; i < _depthPixels.Length; i++)
            {
                //Console.WriteLine(_depthPixels[i]);
                int threshold = 10000;
                if (_depthPixels[i] < threshold)
                {
                    //_bitmapBits[4 * i] = _bitmapBits[4 * i + 1] = _bitmapBits[4 * i + 2] = _bitmapBits[4 * i + 3] = (byte)(255 * (threshold - _depthPixels[i]) / threshold);
                    _bitmapBits[4 * i] = _colorPixels[4 * i];
                    _bitmapBits[4 * i + 1] = _colorPixels[4 * i + 1];
                    _bitmapBits[4 * i + 2] = _colorPixels[4 * i + 2];
                    _bitmapBits[4 * i + 3] = _colorPixels[4 * i + 3];

                }
                else
                    _bitmapBits[4 * i] = _bitmapBits[4 * i + 1] = _bitmapBits[4 * i + 2] = _bitmapBits[4 * i + 3] = (byte)255;

            }
            */

            /*
            // Put the color image into _bitmapBits
            for (int i = 0; i < _colorPixels.Length; i += 4)
            {
                _bitmapBits[i + 3] = 255;
                _bitmapBits[i + 2] = _colorPixels[i + 2];
                _bitmapBits[i + 1] = _colorPixels[i + 1];
                _bitmapBits[i] = _colorPixels[i];
            }
            */

            
            this._sensor.MapDepthFrameToColorFrame(DepthImageFormat.Resolution640x480Fps30, _depthPixels, ColorImageFormat.YuvResolution640x480Fps15, _mappedDepthLocations);
            

            for (int i = 0; i < _depthPixels.Length; i++)
            {
                int depthVal = _depthPixels[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                ColorImagePoint point = _mappedDepthLocations[i];
                if ((point.X >= 0 && point.X < 640) && (point.Y >= 0 && point.Y < 480))
                {
                    int baseIndex = (point.Y * 640 + point.X) * 4;
                    //if ((depthVal <= 1000) && (depthVal > 400))
                    if ((depthVal <= 2000) && (depthVal > 800))
                    {
                        // Bucketing for speeding it up a little bit.
                        /*
                        byte[] color = new byte[3] { (byte)((int)(_colorPixels[baseIndex + 2]/10)*10), 
                                                    (byte)((int)(_colorPixels[baseIndex + 1]/10)*10), 
                                                    (byte)((int)(_colorPixels[baseIndex]/10)*10) };
                        byte[] colorMatch = nearest_color(color);
                        
                        _bitmapBits[baseIndex] = colorMatch[2];
                        _bitmapBits[baseIndex + 1] = colorMatch[1];
                        _bitmapBits[baseIndex + 2] = colorMatch[0];
                        */

                        _bitmapBits[baseIndex] = _colorPixels[baseIndex];
                        _bitmapBits[baseIndex + 1] = _colorPixels[baseIndex + 1];
                        _bitmapBits[baseIndex + 2] = _colorPixels[baseIndex + 2];

                        //_bitmapBits[baseIndex] = _bitmapBits[baseIndex + 1] = _bitmapBits[baseIndex + 2] = (byte)depthVal;
                    } else {
                        _bitmapBits[baseIndex] = _bitmapBits[baseIndex + 1] = _bitmapBits[baseIndex + 2] = (byte)255;
                    }
                }
            }

            _bitmap.WritePixels(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight), _bitmapBits, _bitmap.PixelWidth * sizeof(int), 0);
        
        }

        byte[] nearest_color(byte[] point)
        {
            if (nearest_cache.ContainsKey(point)) return nearest_cache[point];

            byte[,] colors = new byte[,] { 
            { 255, 0, 0 }, 
            { 0, 255, 0 }, 
            { 0, 0, 255 },
            { 0, 0, 0 },
            { 139, 10, 80 },                // Pink
            { 255, 255, 0 },                // Yellow
            { 205, 102, 0 }                 // Orange
            };
            //double [] distances = new double [colors.GetLength(0)];

            //int minIdx = 0;
            double minDistance = 1000000;
            int minColor = -1;

            for (int idx = 0; idx < colors.GetLength(0); idx++) {
                double distance = euc_distance(point,new byte[3]{colors[idx,0], colors[idx, 1], colors[idx, 2]});
                if (distance < minDistance)
                {
                    minColor = idx;
                    minDistance = distance;
                }
            }

            return new byte[] { colors[minColor, 0], colors[minColor, 1], colors[minColor, 2] };
        }

        double euc_distance(byte[] point, byte[] color)
        {
            return Math.Sqrt(Math.Pow(point[0] - color[0], 2) + Math.Pow(point[1] - color[1], 2) + Math.Pow(point[2] - color[2], 2));
        }

        public MainWindow()
        {
            InitializeComponent();

            KinectSensor.KinectSensors.StatusChanged += (object sender, StatusChangedEventArgs e) =>
            {
                if (e.Sensor == _sensor && e.Status != KinectStatus.Connected) SetSensor(null);
                else if ((_sensor == null) && (e.Status == KinectStatus.Connected)) SetSensor(e.Sensor);
            };

            foreach (var sensor in KinectSensor.KinectSensors) 
                if (sensor.Status == KinectStatus.Connected) SetSensor(sensor);    
        }
    }
}
