using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO.Ports;
using static System.Formats.Asn1.AsnWriter;

namespace teris_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort MijnPoort = new SerialPort("com3");
        private readonly ImageSource[] tileImage = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
        };

        private readonly ImageSource[] blockImage = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/block-Z.png", UriKind.Relative)),
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;
        int serialScore;

        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
            MijnPoort.BaudRate = 9600;
            MijnPoort.StopBits = StopBits.One;
            MijnPoort.DataBits = 8;
            MijnPoort.Parity = Parity.None;
            MijnPoort.Open();
        }
        public Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for(int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2 ) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }
        private void DrawGrid(GameGrid grid)
        {
            for(int r = 0; r < grid.Rows; r++)
            {
                for(int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r,c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImage[id];
                }
            }
        }
        private void DrawBlock(Block block)
        {
            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImage[block.Id];
            }
        }
        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImage[next.Id];
        }
        private void DrawHeldBlock(Block heldBlock)
        {
            if(heldBlock == null)
            {
                HoldImage.Source = blockImage[0];
            }
            else
            {
                HoldImage.Source = blockImage[heldBlock.Id];
            }
        }
        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImage[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.Blockqueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreTexst.Text = $"Score: {gameState.Score}";
        }
        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                //int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                //await Task.Delay(delay);
                //gameState.MoveBlockDown();
                //Draw(gameState);
                // Versnel de blokken elke keer als de score een veelvoud van 8 is
                if (gameState.Score % 8 >= 0)
                {
                    int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                    await Task.Delay(delay);
                }
                gameState.MoveBlockDown();
                Draw(gameState);

                // Zorg ervoor dat de score die naar de seriële poort wordt gestuurd, tussen 0 en 8 ligt
                SendNumberToSerialPort(gameState.Score % 9);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinaleScoreTexst.Text = $"Score: {gameState.Score}";
        }

        private void SendNumberToSerialPort(int number)
        {
            if (MijnPoort.IsOpen)
            {
                // Zet het getal om naar een string en stuur het naar de seriële poort
                MijnPoort.WriteLine(number.ToString());
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch(e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility=Visibility.Hidden;
            await GameLoop();
        }
    }
}
