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
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace pedometerForm
{
    /// <summary>
    /// Interaction logic for monitor.xaml
    /// </summary>
    public partial class monitor : Window
    {
        byte[] data = new byte[1024];
        private Thread thread1;
        int recv;
    
        public monitor()
        {
            InitializeComponent();
            thread1 = new Thread(new ThreadStart(ReceiveMessage));
            thread1.Start();

        }
        private void ReceiveMessage()
        {

            TextBlock distanceTextBlock = distanceValuue as TextBlock;
            TextBlock paceTextBlock = paceValue as TextBlock;
            TextBlock speedTextBlock = speedValue as TextBlock;
            TextBlock caloriesTextBlock = caloriesValue as TextBlock;
            TextBlock stepTextBlock = stepValue as TextBlock;
            ProgressBar progressBar = progressBar1 as ProgressBar;        
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8002);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            newsock.Bind(ipep);
            //  Process.Start("C:/Program Files/Google/Google Earth/client/googleearth.exe");

  
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);

            while (true)
            {

                string welcome = "Welcome ! ";
                data = Encoding.ASCII.GetBytes(welcome);
                data = new byte[1024];
                recv = newsock.ReceiveFrom(data, ref Remote);
                string text = Encoding.ASCII.GetString(data, 0, recv);
             

                
                string[] sArray = text.Split(':');
                string command = sArray[0].ToLower();
                string content = sArray[1].ToLower();
                String content1 = "";
                content1 += "<" + DateTime.Now.ToShortTimeString() + ">";

                content1 += "command:" + command;
                content1 += "     content:" + content;
                if (command.Equals("d"))
                {
                   
                    string[] dArray = content.Split('|');
                    string distanceValueString = dArray[0].ToLower();
                    string paceValueString = dArray[1].ToLower();
                    string speedValueString = dArray[2].ToLower();
                    string caloriesValueString = dArray[3].ToLower();
                    string stepValueString = dArray[4].ToLower();
                    Int32 pace = Convert.ToInt32(paceValueString);
                    Int32 percentage = (pace * 100 / 200);
                  
                    distanceTextBlock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { distanceTextBlock.Text = distanceValueString; }));
                    paceTextBlock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { paceTextBlock.Text = paceValueString; }));
                    speedTextBlock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { speedTextBlock.Text = speedValueString; }));
                    caloriesTextBlock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { caloriesTextBlock.Text = caloriesValueString; }));
                    stepTextBlock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { stepTextBlock.Text = stepValueString; }));
                    progressBar.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { progressBar.Value = percentage; }));
                    if (percentage>80)
                    {
                        progressBar.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { progressBar.Foreground = new SolidColorBrush(Colors.Red); }));
                    }
                    else if (percentage <= 80 && percentage >= 30)
                    {
                        progressBar.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { progressBar.Foreground = new SolidColorBrush(Colors.Green); }));
                    }
                    else
                    {
                        progressBar.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { progressBar.Foreground = new SolidColorBrush(Colors.Blue); }));
                 
                    }
                }

               
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu textmenu = new ContextMenu();
            MenuItem OnTop = new MenuItem();
            OnTop.Header = "Top Most";
            OnTop.Tag = "OnTop";
            OnTop.IsChecked = Topmost;
            OnTop.Click += new RoutedEventHandler(btnOpenFile_Click);

            MenuItem hide = new MenuItem();
            hide.Header = "Hide monitor";
            hide.Tag = "Hide";
          
            hide.Click += new RoutedEventHandler(btnHideFile_Click);

            textmenu.Items.Add(OnTop);
            textmenu.Items.Add(hide);
          
            this.ContextMenu = textmenu;
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (Topmost)
            {
                Topmost = false;

            }
            else
            {
                Topmost = true;
            }
        }
        private void btnHideFile_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
