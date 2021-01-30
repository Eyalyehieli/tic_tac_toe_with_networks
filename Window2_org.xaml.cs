using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;

namespace tic_tac_toe_game
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Connection : Window
    {
        public BinaryWriter gWriter;
        public BinaryReader gReader;
        public char playerContent;

        public Connection()
        {
            InitializeComponent();
        }



        private void connectB_Click(object sender, RoutedEventArgs e)
        {
            string ipad = ip_connect.Text;
            int myport = Convert.ToInt32(port_connect.Text);
            Task t = new Task(() =>
            {

                try
                {
                    this.massage.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => { massage.Text = "try to connect " + Environment.NewLine; }));

                    System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

                    clientSocket.Connect(ipad, myport);
                    this.massage.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => { connectB.Background = Brushes.Green; }));


                    Stream streamS = clientSocket.GetStream();
                    BinaryWriter writer = new BinaryWriter(streamS);
                    gWriter = writer;
                    BinaryReader reader = new BinaryReader(streamS);
                    gReader = reader;



                    this.massage.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(
                            () => { massage.Text += "succesful connection to the server \n" + Environment.NewLine; }));
                    string str1, str2;
                    str1 = reader.ReadString();
                    if (str1.IndexOf("X")==21)
                    {
                        playerContent = 'X';
                    }
                    else
                    {
                        playerContent = 'O';
                    }
                    
                        
                    this.massage.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => { massage.Text = massage.Text + str1 + Environment.NewLine; }));
                    str2 = reader.ReadString();
                    this.massage.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => { massage.Text = massage.Text + str2 + Environment.NewLine; }));


                }
                catch (Exception ex)
                {
                    this.massage.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => { massage.Text = massage.Text + ex.Message + Environment.NewLine; }));
                }
            }
                );
            t.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (connectB.Background == Brushes.Green)
            {
                this.Hide();
                MainWindow win = new MainWindow(gWriter, gReader,playerContent);
                //MainWindow win = new MainWindow();
                win.ShowDialog();
            }
        }
    }
}
