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
using gesture;
using System.Threading;
using Microsoft.Kinect;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using System.Media;
namespace pedometerForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] data = new byte[1024];
        private Thread thread1;
        int recv;
        KmlBuilder kml = new KmlBuilder();
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_KEYDOWN = 0x00;
        private const int KEY_A = 0x41;
        saveData sd = new saveData();
        bool control = false;

        private KinectSensor kinectDevice;
        private Skeleton[] frameSkeletons;
        private Pose[] poseLibrary;
        int counter = 0;
        Process gEarth;
        public string kinectstr = "";
        monitor form1;

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(
            byte bVk,
            byte bScan,
            int dwFlags,
            int dwExtraInfo
        );

        public MainWindow()
        {

            //  taskIcon = (TaskbarIcon)FindResource("MyNotifyIcon");
            InitializeComponent();
            //   Loaded += new RoutedEventHandler(Window1_Loaded);
            thread1 = new Thread(new ThreadStart(ReceiveMessage));
            thread1.Start();

        }


        private void ReceiveMessage()
        {
            Button stbn = startBtn as Button;
            TextBlock textBlock = textBlock1 as TextBlock;
            TextBlock androidLabel = androidDeviceStates as TextBlock;
            TextBlock kinectLabel = kinectDeviceStates as TextBlock;
            //得到本机IP，设置TCP端口号         
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8001);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //绑定网络地址
            newsock.Bind(ipep);
            //  Process.Start("C:/Program Files/Google/Google Earth/client/googleearth.exe");

            textBlock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { textBlock.Text = "This is a Server, host ip address is " + new IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address).ToString(); }));

            //得到客户机IP
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);

            while (true)
            {

                string welcome = "Welcome ! ";
                data = Encoding.ASCII.GetBytes(welcome);
                data = new byte[1024];
                //发送接受信息
                recv = newsock.ReceiveFrom(data, ref Remote);
                string text = Encoding.ASCII.GetString(data, 0, recv);

                string[] sArray = text.Split(':');
                string command = sArray[0].ToLower();
                string content = sArray[1].ToLower();
                String content1 = "";
                content1 += "<" + DateTime.Now.ToShortTimeString() + ">";

                content1 += "command:" + command;
                content1 += "     content:" + content;
                textBlock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { textBlock.Text = content1; }));
                if (command.Equals("android"))
                {
                    if (content.Equals("hello"))
                    {
                        androidLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { androidLabel.Text = "Connected"; }));
                        androidLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { androidLabel.Foreground = new SolidColorBrush(Colors.Green); }));

                    }
                    else
                    {
                        androidLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { androidLabel.Text = "Disconnected"; }));
                        androidLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { androidLabel.Foreground = new SolidColorBrush(Colors.Red); }));
                        
                    }

                }
                else if (command.Equals("kinect"))
                {
                    if (content.Equals("hello"))
                    {
                        kinectLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { kinectLabel.Text = "Connected"; }));
                        kinectLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { kinectLabel.Foreground = new SolidColorBrush(Colors.Green); }));

                    }
                    else
                    {
                        kinectLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { kinectLabel.Text = "Disconnected"; }));
                        kinectLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { kinectLabel.Foreground = new SolidColorBrush(Colors.Red); }));

                    }

                }
                string androidContent = "";
                string kinectContect = "";
                kinectLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { kinectContect = kinectLabel.Text; }));
                androidLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { androidContent = androidLabel.Text; }));

                if (androidContent.Equals("Disconnected") )
                {
                    stbn.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { stbn.IsEnabled = false; }));

                }
                else
                {
                    stbn.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { stbn.IsEnabled = true; }));
                }

                this.controlKeyboard(command, content);
              //  this.controlGoogleEarth(content);
            }

        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!sd.readGeData().Equals(""))
            {
                if (startBtn.Content.Equals("Start"))
                {
                gEarth =  Process.Start(sd.readGeData());
                    this.WindowState = WindowState.Minimized;
                    MyNotifyIcon.Visibility = Visibility.Visible;
                    MyNotifyIcon.ShowBalloonTip("Pedometer", "pedometer is start controling your computer....", BalloonIcon.Info);
                   
                    control = true;
                    startBtn.Content = "Stop";
                    form1 = new monitor();
                    form1.Show();
                }
                else
                {
                    MyNotifyIcon.ShowBalloonTip("Pedometer", "pedometer stop", BalloonIcon.Info);
                    MyNotifyIcon.Visibility = Visibility.Hidden;
                    control = false;
                    startBtn.Content = "Start";

                }
            }
            else
            {
                MessageBox.Show("Please select the google earth first!");
                SystemSounds.Beep.Play();
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window1 w1 = new Window1();
            w1.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            MyNotifyIcon.ShowBalloonTip("Pedometer", "pedometer stop", BalloonIcon.Info);
            MyNotifyIcon.Visibility = Visibility.Hidden;
            control = false;
            startBtn.Content = "Start";
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            w2.Owner = this;
            w2.ShowDialog();

        }
     
        public void setKinect(int kinect)
        {
            PopulatePoseLibrary();

            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;

            this.KinectDevice = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);



        }
        private void PopulatePoseLibrary()
        {
            this.poseLibrary = new Pose[2];

            this.poseLibrary[0] = new Pose();
            this.poseLibrary[0].Title = "l";
            this.poseLibrary[0].Angles = new PoseAngle[2];
            this.poseLibrary[0].Angles[0] = new PoseAngle(JointType.ShoulderLeft, JointType.ElbowLeft, 180, 20);
            this.poseLibrary[0].Angles[1] = new PoseAngle(JointType.ElbowLeft, JointType.WristLeft, 180, 20);

            this.poseLibrary[1] = new Pose();
            this.poseLibrary[1].Title = "r";
            this.poseLibrary[1].Angles = new PoseAngle[2];
            this.poseLibrary[1].Angles[0] = new PoseAngle(JointType.ShoulderRight, JointType.ElbowRight, 0, 20);
            this.poseLibrary[1].Angles[1] = new PoseAngle(JointType.ElbowRight, JointType.WristRight, 0, 20);

        }
        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Initializing:
                case KinectStatus.Connected:
                case KinectStatus.NotPowered:
                case KinectStatus.NotReady:
                case KinectStatus.DeviceNotGenuine:
                    this.kinectDevice = e.Sensor;
                    break;
                case KinectStatus.Disconnected:
                    //plugout kinect.......
                    break;
                default:
                    break;
            }
        }
        public KinectSensor KinectDevice
        {
            get { return this.kinectDevice; }
            set
            {
                if (this.kinectDevice != value)
                {
                    if (this.kinectDevice != null)
                    {
                        this.kinectDevice.Stop();
                        this.kinectDevice.SkeletonFrameReady -= KinectDevice_SkeletonFrameReady;
                        this.kinectDevice.SkeletonStream.Disable();
                        SkeletonViewerElement = null;
                        this.frameSkeletons = null;
                        this.textBlock1.Text = "Kinect disconnect!";
                        kinectDeviceStates.Text = "Disconnected";
                        kinectDeviceStates.Foreground = new SolidColorBrush(Colors.Red);

                    }

                    this.kinectDevice = value;

                    if (this.kinectDevice != null)
                    {
                        if (this.kinectDevice.Status == KinectStatus.Connected)
                        {
                            this.kinectDevice.SkeletonStream.Enable();
                            this.frameSkeletons = new Skeleton[this.kinectDevice.SkeletonStream.FrameSkeletonArrayLength];
                            this.kinectDevice.Start();
                            this.kinectDeviceStates.Text = "Connected";
                            this.textBlock1.Text = "Kinect start!";
                            SkeletonViewerElement.KinectDevice = this.KinectDevice;
                            this.kinectDeviceStates.Foreground = new SolidColorBrush(Colors.Green);
                            this.KinectDevice.SkeletonFrameReady += KinectDevice_SkeletonFrameReady;
                        }
                    }
                }
            }
        }

        private static Skeleton GetPrimarySkeleton(Skeleton[] skeletons)
        {
            Skeleton skeleton = null;

            if (skeletons != null)
            {
                for (int i = 0; i < skeletons.Length; i++)
                {
                    if (skeletons[i].TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (skeleton == null)
                        {
                            skeleton = skeletons[i];
                        }
                        else
                        {
                            if (skeleton.Position.Z > skeletons[i].Position.Z)
                            {
                                skeleton = skeletons[i];
                            }
                        }
                    }
                }
            }
            return skeleton;
        }


        private void KinectDevice_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    frame.CopySkeletonDataTo(this.frameSkeletons);
                    Skeleton skeleton = GetPrimarySkeleton(this.frameSkeletons);

                    if (skeleton == null)
                    {

                    }
                    else
                    {
                        ProcessPerforming(skeleton);
                    }
                }
            }
        }


        private void ProcessPerforming(Skeleton skeleton)
        {
            Joint head = skeleton.Joints[JointType.Head];
            Joint rightHand = skeleton.Joints[JointType.HandRight];
            Joint leftHand = skeleton.Joints[JointType.HandLeft];
            counter++;
            for (int i = 0; i < poseLibrary.Length; i++)
            {
                if (IsPose(skeleton, this.poseLibrary[i]))
                {

                    if (counter % 10 == 0)
                    {
                        switch (poseLibrary[i].Title)
                        {
                            case "l":

                                keybd_event(37, 0, KEYEVENTF_KEYDOWN, 0);
                                Thread.Sleep(50);
                                keybd_event(37, 0, KEYEVENTF_KEYUP, 0);
                                break;
                            case "r":
                                keybd_event(39, 0, KEYEVENTF_KEYDOWN, 0);
                                Thread.Sleep(50);
                                keybd_event(39, 0, KEYEVENTF_KEYUP, 0);
                                break;
                        }

                        this.controlKeyboard(poseLibrary[i].Title + ":", "");
                        // textBlock1.Text = poseLibrary[i].Title;
                    }
                }
            }
        }

        private bool IsPose(Skeleton skeleton, Pose pose)
        {
            bool isPose = true;
            double angle;
            double poseAngle;
            double poseThreshold;
            double loAngle;
            double hiAngle;

            for (int i = 0; i < pose.Angles.Length && isPose; i++)
            {
                poseAngle = pose.Angles[i].Angle;
                poseThreshold = pose.Angles[i].Threshold;
                angle = GetJointAngle(skeleton.Joints[pose.Angles[i].CenterJoint], skeleton.Joints[pose.Angles[i].AngleJoint]);

                hiAngle = poseAngle + poseThreshold;
                loAngle = poseAngle - poseThreshold;

                if (hiAngle >= 360 || loAngle < 0)
                {
                    loAngle = (loAngle < 0) ? 360 + loAngle : loAngle;
                    hiAngle = hiAngle % 360;

                    isPose = !(loAngle > angle && angle > hiAngle);
                }
                else
                {
                    isPose = (loAngle <= angle && hiAngle >= angle);
                }
            }

            return isPose;
        }

        private double GetJointAngle(Joint centerJoint, Joint angleJoint)
        {

            Point primaryPoint = GetJointPoint(this.KinectDevice, centerJoint, this.LayoutRoot.RenderSize, new Point());
            Point anglePoint = GetJointPoint(this.KinectDevice, angleJoint, this.LayoutRoot.RenderSize, new Point());
            Point x = new Point(primaryPoint.X + anglePoint.X, primaryPoint.Y);

            double a;
            double b;
            double c;

            a = Math.Sqrt(Math.Pow(primaryPoint.X - anglePoint.X, 2) + Math.Pow(primaryPoint.Y - anglePoint.Y, 2));
            b = anglePoint.X;
            c = Math.Sqrt(Math.Pow(anglePoint.X - x.X, 2) + Math.Pow(anglePoint.Y - x.Y, 2));

            double angleRad = Math.Acos((a * a + b * b - c * c) / (2 * a * b));
            double angleDeg = angleRad * 180 / Math.PI;

            if (primaryPoint.Y < anglePoint.Y)
            {
                angleDeg = 360 - angleDeg;
            }

            return angleDeg;
        }


        private static Point GetJointPoint(KinectSensor kinectDevice, Joint joint, Size containerSize, Point offset)
        {
            DepthImagePoint point = kinectDevice.MapSkeletonPointToDepth(joint.Position, kinectDevice.DepthStream.Format);
            point.X = (int)((point.X * containerSize.Width / kinectDevice.DepthStream.FrameWidth) - offset.X);
            point.Y = (int)((point.Y * containerSize.Height / kinectDevice.DepthStream.FrameHeight) - offset.Y);

            return new Point(point.X, point.Y);
        }

       


        private void controlKeyboard(string command, string content)
        {
            if (control)
            {
                switch (command)
                {
                    /* case "l":

                         keybd_event(37, 0, KEYEVENTF_KEYDOWN, 0);
                         Thread.Sleep(100);
                         keybd_event(37, 0, KEYEVENTF_KEYUP, 0);
                         break;
                     case "r":
                         keybd_event(39, 0, KEYEVENTF_KEYDOWN, 0);
                         Thread.Sleep(100);
                         keybd_event(39, 0, KEYEVENTF_KEYUP, 0);
                         break;
                     */

                    case "f":

                        keybd_event(38, 0, KEYEVENTF_KEYDOWN, 0);
                        Thread.Sleep(100);
                        keybd_event(38, 0, KEYEVENTF_KEYUP, 0);
                        break;
                    case "s":
                        string[] views = content.Split('|');
                        kml.KMLstreetview(views[0], views[1]);

                        break;
                    case "g":
                        string[] geos = content.Split('|');
                        kml.KML(geos[0], geos[1]);
                        break;
                    case "e":
                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.ProcessName == "googleearth")
                            {    
                                p.Kill();
                            }
                        }
                        break;
                }
            }
        }
    }
}
