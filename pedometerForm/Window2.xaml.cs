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
using Microsoft.Kinect;

namespace pedometerForm
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            KinectSensorCollection ksc = KinectSensor.KinectSensors;
            foreach (var item in ksc)
            {
                this.comboBox1.Items.Add(item.UniqueKinectId);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow f = (MainWindow)this.Owner;
            if (this.comboBox1.SelectedValue != null && !this.comboBox1.SelectedValue.Equals(""))
            {
                f.setKinect(this.comboBox1.SelectedIndex);
                this.Close();

            }
            else
            {
                MessageBox.Show("Please select a kinect first!");
            }
             }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
    }
}
