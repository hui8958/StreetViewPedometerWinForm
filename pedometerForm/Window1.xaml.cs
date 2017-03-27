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
using Microsoft.Win32;


namespace pedometerForm
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        saveData sd = new saveData();
        public Window1()
        {
            InitializeComponent();
            textBox1.Text = sd.readGeData();
        
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();//定义打开文本框实体
            open.Title = "Choose google earth";//对话框标题
            open.Filter = "*.exe|";//文件扩展名
            try
            {
                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text = open.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Please select the location of the google earth.");
            }
            else
            {
                sd.saveGEData(textBox1.Text);
                this.Close();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
