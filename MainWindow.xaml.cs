using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace tic_tac_toe_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public BinaryWriter glwriter;
        public BinaryReader glreader;
        public char playerContent;
        public bool turn;  // true= x turn, false= o turn
        int turn_count = 0;
        private int grid_count;
		// This constructor get the streams Write & Read as well as the player (x or O) 
        public MainWindow(BinaryWriter glwriter, BinaryReader glreader, char playerContent)
        {
            InitializeComponent();
            this.glwriter = glwriter;
            this.glreader = glreader;
            this.playerContent = playerContent;
            grid_count = grid.Children.Count;
            if (this.playerContent == 'X')
            {
                this.turn = true;
            }
            else
            {
                this.turn = false;
            }

            string buttonNamePush = "", buttonContentPush = "";
            //           if (playerContent == 'O')
            //           {
            Task t1 = new Task(() =>
            {

                if (playerContent == 'O')
                {
                    
                    buttonNamePush = glreader.ReadString();
                    buttonContentPush = glreader.ReadString();
                    if (buttonNamePush == "A1")
                    {
                        A1.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { A1.Content = "X"; A1.IsEnabled = false; }));
                    }
                    if (buttonNamePush == "A2")
                    {
                        A2.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { A2.Content = "X"; A2.IsEnabled = false; }));
                    }
                    if (buttonNamePush == "A3")
                    {
                        A3.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { A3.Content = "X"; A3.IsEnabled = false; }));
                    }
                    if (buttonNamePush == "B1")
                    {
                        B1.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { B1.Content = "X"; B1.IsEnabled = false; }));
                    }
                    if (buttonNamePush == "B2")
                    {
                        B2.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { B2.Content = "X"; B2.IsEnabled = false; }));
                    }
                    if (buttonNamePush == "B3")
                    {
                        B3.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { B3.Content = "X"; B3.IsEnabled = false; }));
                    }
                    if (buttonNamePush == "C1")
                    {
                        C1.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { C1.Content = "X"; C1.IsEnabled = false; }));
                    }
                    if (buttonNamePush == "C2")
                    {
                        C2.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { C2.Content = "X"; C2.IsEnabled = false; }));
                    }

                    if (buttonNamePush == "C3")
                    {
                        C3.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { C3.Content = "X"; C3.IsEnabled = false; }));
                    }                   
                }
            }
        );
            t1.Start();
#if false
            if (playerContent == 'O')
            {
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    Button btn = (Button)grid.Children[i];
                    if (btn != null)
                    {
                        if (btn.Name == buttonNamePush)
                        {
                            btn.Content = buttonContentPush;
                            btn.IsEnabled = false;
                            break;
                            //btn.Dispatcher.Invoke(DispatcherPriority.Normal,
                            //    new Action(() => { btn.Content = buttonContentPush; }));
                        }
                    }
                }
            }
#endif
        }
        public MainWindow()
        {
            InitializeComponent();
            string buttonNamePush = "", buttonContentPush = "";
            if (playerContent == 'O')
            {
                Task t1 = new Task(() =>
                {
                    buttonNamePush = glreader.ReadString();
                    buttonContentPush = glreader.ReadString();
                }
            );
                t1.Start();
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    Button btn = (Button)grid.Children[i];
                    if (btn != null)
                    {
                        if (btn.Name == buttonNamePush)
                        {
                            //btn.Content = buttonContentPush;
                            btn.Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(() => { btn.Content = buttonContentPush; }));
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string buttonNamePush = "", buttonContentPush = "";
            Button b = (Button)sender;
            if (turn)
                b.Content = "X";
            else
                b.Content = "O";
            b.IsEnabled = false;
            turn_count++;
            string bName = b.Name;
            string bContent = (string)b.Content;

            DisableButtons();
            Task t = new Task(() =>
            {
                glwriter.Write(bName);
                glwriter.Write(bContent);
                CheckForWinner();
                DisableButtons();
                buttonNamePush = glreader.ReadString();
                buttonContentPush = glreader.ReadString();
                UpdateGrid(buttonNamePush, buttonContentPush);
            });
            t.Start();
        }

        private void UpdateGrid(string buttonNamePush, string buttonContentPush)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() => UpdateGrid(buttonNamePush, buttonContentPush)));
                return;
            }

            for (int i = 0; i < grid.Children.Count; i++)
            {
                Button btn = (Button)grid.Children[i];
                if (btn != null)
                {
                    if (btn.Name == buttonNamePush)
                    {
                        btn.Content = buttonContentPush;
                        btn.IsEnabled = false;
                        break;
                    }
                }
            }

            CheckForWinner();
            EnableButtons();
        }

        private bool CheckForWinner(Button[] buttons, out string winner)
        {
            string firstButtonContent = buttons[0].Content as string;

            if (String.IsNullOrEmpty(firstButtonContent))
            {
                winner = null;
                return false;
            }

            foreach (var button in new[] { buttons[1], buttons[2] })
            {
                if (button.Content as string != firstButtonContent)
                {
                    winner = null;
                    return false;
                }
            }

            winner = firstButtonContent;
            return true;
        }

        private void CheckForWinner()
        {
            // test if we can access the GUI thread if not call again using asynchronously BeginInvoke  
			if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(CheckForWinner));
                return;
            }

            bool there_is_a_winner = false;
            string winner;
			// prepare all the combinations for winner
            Button[][] checkButtonsArray = new[]
            {
                new[] {A1, A2, A3},
                new[] {B1, B2, B3},
                new[] {C1, C2, C3},
                new[] {A1, B1, C1},
                new[] {A2, B2, C2},
                new[] {A3, B3, C3},
                new[] {A1, B2, C3},
                new[] {A3, B2, C1}
            };
            // Running on all winner combination and check if there is a winner. 
            foreach (var checkButtons in checkButtonsArray)
            {
                if (CheckForWinner(checkButtons, out winner))
                {
                    if (winner == "O")
                    {
                        DeclareWinner(winner, o_win_count);
                    }
                    else
                    {
                        DeclareWinner(winner, x_win_count);
                    }

                    return;
                }
            }

            turn_count = 0;
            for (int i = 0; i < grid.Children.Count; i++)
            {
                Button btn = (Button)grid.Children[i];
                if ((string)btn.Content != "")
                {
                    turn_count++;
                }
            }

            if (turn_count == 9)
            {
                Declare("A Draw!!", draw_count);
            }

        }

        private void Declare(string content, Label winCount)
        {
            this.Hide();
            Window1 wo = new Window1(content);
            wo.ShowDialog();
            this.Show();
            winCount.Content = (Int32.Parse(winCount.Content.ToString()) + 1).ToString();

            foreach (var c in grid.Children)
            {
                var button = (Button) c;
                button.Content = "";
            }
        }


        private void DeclareWinner(string winner, Label winCount)
        {
            Declare("The Winner is " + winner + "!!", winCount);
        }

        private void EnableButtons()
        {
            foreach (var c in grid.Children)
            {
                Button b = (Button)c;
                if (String.IsNullOrEmpty(b.Content as string))
                {
                    b.IsEnabled = true;
                }
            }
        }
        
        private void DisableButtons()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(DisableButtons));
                return;
            }

            foreach (var c in grid.Children)
            {
                Button b = (Button)c;
                b.IsEnabled = false;
            }
        }


        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //turn = true;
            turn_count = 0;
            foreach (var c in grid.Children)
            {
                Button b = (Button)c;
                b.IsEnabled = true;
                b.Content = "";
            }
        }


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Enter(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            if (b.IsEnabled == true)
            {
                if (turn == true)
                    b.Content = "X";
                else
                    b.Content = "O";
            }
        }

        private void Button_Leave(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            if (b.IsEnabled == true)
            {
                b.Content = "";
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Help hlp = new Help();
            hlp.Visibility = Visibility.Visible;
        }




    }
}
