using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.CompilerServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Byz_Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string LocalIP = "127.0.0.1";
        private Thread threadClient;
        private TcpClient client;
        private string DatePrimite;
        private string IPConnect;
        private bool _playVsAI = false;

        public bool GameStopped
        {
            get => !ChessBoard.GameStarted;
            set
            {
                ChessBoard.GameStarted = !value;
                OnPropertyChanged();
            }
        }

        public bool PlayVsAI
        {
            get => _playVsAI;
            set
            {
                _playVsAI = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestingBtn_Click(object sender, RoutedEventArgs e)
        {
            GameStopped = false;
            ChessBoard.TestingBoard();
        }

        private void StandardBtn_Click(object sender, RoutedEventArgs e)
        {
            GameStopped = false;
            ChessBoard.ArrangeStandardBoard();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnlineConnectBtn_OnClick(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            OnlineConnectBtn.IsEnabled = false;
            OnlineConnectBtn.Content = "Waiting for player...";
            IPConnect = InputTextBox.Text;
            if (IPConnect != string.Empty) Connect();
            InputTextBox.Text = LocalIP;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            InputTextBox.Text = "LocalIP";
        }

        private void Connect()
        {
            ChessBoard.Online = true;
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(IPConnect);
            client = new TcpClient(IPConnect, 3000);
            Globals.DateClient = client.GetStream();
            Globals.ScriereServer = new StreamWriter(Globals.DateClient);
            threadClient = new Thread(DecodeInfoFromServer);
            Globals.ScriereServer.AutoFlush = true;
            Globals.ScriereServer.WriteLine("C");
            threadClient.Start();
        }

        void DecodeInfoFromServer()
        {
            StreamReader citire = new StreamReader(Globals.DateClient);
            while (true)
            {
                DatePrimite = citire.ReadLine();
                switch (DatePrimite[0])
                {
                    case 'S':
                        {
                            var playerNr = DatePrimite[1..];
                            Dispatcher.Invoke(() => PlayerNr.Text = playerNr);
                            GameStopped = false;
                            ChessBoard.PlayerNr = Convert.ToInt32(playerNr);
                            Dispatcher.Invoke(() => ChessBoard.ArrangeStandardBoard());
                            break;
                        }
                    case 'M':
                        {
                            DatePrimite = DatePrimite[1..];
                            var values = DatePrimite.Split('|');
                            var selectedRow = Convert.ToInt32(values[0][0].ToString());
                            var selectedColumn = Convert.ToInt32(values[0][1..]);
                            var newRow = Convert.ToInt32(values[1][0].ToString());
                            var newCol = Convert.ToInt32(values[1][1..]);
                            Dispatcher.Invoke(() => ChessBoard.DoOnlineMove(selectedRow, selectedColumn, newRow, newCol));
                            break;
                        }
                    case 'F':
                        {
                            Dispatcher.Invoke(() => ChessBoard.ResetGame());
                            break;
                        }
                }
            }
        }

        private void PlayAIBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayVsAI ^= true;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            GameStopped = false;
            ChessBoard.Board.PlayerToPlay = ColorCB.IsChecked == false ? 1 : 2;
            PlayVsAI ^= true;
            ChessBoard.VsAI = true;
            var result = 3;
            int.TryParse(DifficultyTxT.Text, out result);
            ChessBoard.AIdepth = result;
            ChessBoard.ArrangeStandardBoard();

        }
    }
}
