using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro_LEHANG
{
    public class Cls_Hang2
    {
        public Timer gameTimer;
        private frmHang1 frmHang1;
        private Timer blinkTimer;
        private Panel chessBoard;
        public Button btnkq1;
        public Button btnkq2;
        public string winName;

        public Panel Chessboard
        {
            get { return chessBoard; }
            set { chessBoard = value; }            
        }
        private List<Player> player;
        public List<Player> Player
        {
            get { return player; }
            set { player = value; }
        }
        private int currentPlayer;
        public int CurentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }
        private TextBox playerName;
        public TextBox PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        private PictureBox playerMark;
        public PictureBox PlayerMark
        {
            get { return playerMark; }
            set { playerMark = value; }
        }
        private List<List<Button>> matrix;
        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }
        private event EventHandler playerMarked;
        public event EventHandler PlayerMarked
        {
            add
            {
                playerMarked += value;
            }
            remove
            {
                playerMarked -= value;
            }
        }
        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;
            }
        }
        private Stack<Cls_Hang_PlayInfo> playTimeLine;
        public Stack<Cls_Hang_PlayInfo> PlayTimeLine
        {
            get { return playTimeLine; }
            set { playTimeLine = value; }
        }
        private bool daUndo = false;
       
        public Cls_Hang2(Panel chessBoard, TextBox playerName, PictureBox mark, frmHang1 frmHang1)
        {
            this.chessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = mark;
            this.frmHang1 = frmHang1;
            blinkTimer = new Timer();
            blinkTimer.Interval = 200; // Đặt khoảng thời gian nhấp nháy (miliseconds)
            blinkTimer.Tick += BlinkTimer_Tick;
            blinkTimer.Start();

            gameTimer = new Timer();
            gameTimer.Interval = 1000; // Cập nhật thời gian mỗi giây (1000ms)
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            gameStartTime = DateTime.Now;
            this.Player = new List<Player>() {
                new Player(frmHang1.playerName1,Image.FromFile(Application.StartupPath + "\\Resources\\X.jpg")),
                new Player(frmHang1.playerName2,Image.FromFile(Application.StartupPath + "\\Resources\\O.jpg"))
        };
        }
        
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            UpdateGameTimeLabel(); 
        }
        public DateTime gameStartTime;

        public void UpdateGameTimeLabel()
        {
            TimeSpan totalGameTime = DateTime.Now - gameStartTime;
            frmHang1.lblgt.Text = $"Thời gian trận chơi: {totalGameTime.Hours:D2}:{totalGameTime.Minutes:D2}:{totalGameTime.Seconds:D2}";
      
        }
        private bool isGameEnded = false;
        private bool isBlinking = false;
        private List<Button> winningButtons;
        public int CurrentPlayer
        {
            get => currentPlayer;
            set => currentPlayer = value;
        }
        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (isBlinking == false)
            {
                foreach (var row in Matrix)
                {
                    foreach (var btn in row)
                    {
                        if (winningButtons.Contains(btn))
                        {
                            if (CurrentPlayer == 0)
                            {
                                btn.BackgroundImage = (btn.BackgroundImage == null) ? Properties.Resources.O : null;
                            }
                            else
                            {
                                btn.BackgroundImage = (btn.BackgroundImage == null) ? Properties.Resources.X : null;
                            }
                        }
                    }
                }
            }
        }
        private List<Button> GetWinningButtons(Point point, Image backgroundImage)
        {
            List<Button> winningButtons = new List<Button>();

            if (isEndNgang(point, backgroundImage))
            {
                for (int i = point.X; i >= 0; i--)
                {
                    if (matrix[point.Y][i].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[point.Y][i]);
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = point.X + 1; i < Cls_Hang1.CHESS_BOARD_WIDTH; i++)
                {
                    if (matrix[point.Y][i].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[point.Y][i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (isEndDoc(point, backgroundImage))
            {
                for (int i = point.Y; i >= 0; i--)
                {
                    if (matrix[i][point.X].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[i][point.X]);
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = point.Y + 1; i < Cls_Hang1.CHESS_BOARD_HEIGHT; i++)
                {
                    if (matrix[i][point.X].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[i][point.X]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (isEndCheoChinh(point, backgroundImage))
            {
                for (int i = 0; i <= point.X; i++)
                {
                    if (point.X - i < 0 || point.Y - i < 0)
                        break;
                    if (matrix[point.Y - i][point.X - i].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[point.Y - i][point.X - i]);
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = 1; i <= Cls_Hang1.CHESS_BOARD_WIDTH - point.X; i++)
                {
                    if (point.Y + i >= Cls_Hang1.CHESS_BOARD_HEIGHT || point.X + i >= Cls_Hang1.CHESS_BOARD_WIDTH)
                        break;
                    if (matrix[point.Y + i][point.X + i].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[point.Y + i][point.X + i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (isEndCheoPhu(point, backgroundImage))
            {
                winningButtons.Add(matrix[point.Y][point.X]); 
                for (int i = 1; i <= Math.Min(point.X, Cls_Hang1.CHESS_BOARD_HEIGHT - 1 - point.Y); i++)
                {
                    if (matrix[point.Y + i][point.X - i].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[point.Y + i][point.X - i]);
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 1; i <= Math.Min(Cls_Hang1.CHESS_BOARD_WIDTH - 1 - point.X, point.Y); i++)
                {
                    if (matrix[point.Y - i][point.X + i].BackgroundImage == backgroundImage)
                    {
                        winningButtons.Add(matrix[point.Y - i][point.X + i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return winningButtons;
        }
        public void DrawChessBoard()
        {         
            Chessboard.Enabled = true;
            Chessboard.Controls.Clear();
            PlayTimeLine = new Stack<Cls_Hang_PlayInfo>();
            CurentPlayer = 0;
            player1UndoCount = 0;
            player2UndoCount = 0;
            ChangePlayer();
            Matrix = new List<List<Button>>();
            winningButtons = new List<Button>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i <Cls_Hang1.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cls_Hang1.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cls_Hang1.CHESS_WIDTH,
                        Height = Cls_Hang1.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        FlatStyle = FlatStyle.Flat, // Đặt kiểu nút thành Flat để loại bỏ viền                        
                        ForeColor = Color.Black, // Đặt màu chữ trên ô cờ (có thể thay đổi)
                        Font = new Font("Arial", 12), // Đặt font chữ (có thể thay đổi)
                        Text = "", // Đặt văn bản trong ô cờ (có thể thay đổi)
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };
                    btn.Click += btn_Click;
                    chessBoard.Controls.Add(btn);
                    Matrix[i].Add(btn);
                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Cls_Hang1.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
           if (btn.BackgroundImage != null)
            {
                MessageBox.Show("Ô này đã được đánh, vui lòng đánh ô khác.");
                return;
            }
            Mark(btn);
            PlayTimeLine.Push(new Cls_Hang_PlayInfo(GetChessPoint(btn),CurentPlayer));
            CurentPlayer = CurentPlayer == 1 ? 0 : 1;
            ChangePlayer();
           
            daUndo = false;
            if (playerMarked != null)
            {
                playerMarked(this, new EventArgs());
            }
            if (CurrentPlayer == 0)
            {
                frmHang1.btnUndo2.Enabled = true; // Bật nút Undo của người chơi 2 khi người chơi 1 đánh
                frmHang1.btnUndo1.Enabled = false; // Vô hiệu hóa nút Undo của người chơi 1
            }
            else
            {
                frmHang1.btnUndo1.Enabled = true; // Bật nút Undo của người chơi 1 khi người chơi 2 đánh
                frmHang1.btnUndo2.Enabled = false; // Vô hiệu hóa nút Undo của người chơi 2
            }
            if (isEndGame(btn))
            {
                EndGame();
            }           
        }
        public bool Undo()
        {
            if (PlayTimeLine.Count <= 0 || daUndo)
                return false;
            if (playedPs.Count > 0)
            {
                playedPs.RemoveAt(playedPs.Count - 1);
            }
            int currentPlayerUndoCount = (CurrentPlayer == 1) ? player1UndoCount : player2UndoCount;
            if (currentPlayerUndoCount >= 5)
            {
                MessageBox.Show("Không được Undo quá 5 lần trong 1 ván.");
                return false;
            }
            Cls_Hang_PlayInfo oldPoint = PlayTimeLine.Pop();
            Button btn = Matrix[oldPoint.Point.Y][oldPoint.Point.X];
            btn.BackgroundImage = null;
            if (CurrentPlayer == 1)
            {
                player1UndoCount++;
                frmHang1.undo1++;
                frmHang1.undoo1++;     
            }
            else
            {
                player2UndoCount++;
                frmHang1.undo2++;
                frmHang1.undoo2++; 
            }
            frmHang1.btnUndo1.Text = "UNDO: " + (frmHang1.undo1).ToString()+"/5";
            frmHang1.btnUndo2.Text = "UNDO: " + (frmHang1.undo2).ToString()+"/5";

            if (PlayTimeLine.Count <= 0)
            {
                CurrentPlayer = 0;
            }
            else
            {
                oldPoint = PlayTimeLine.Peek();
                CurrentPlayer = oldPoint.CurrentPlayer == 1 ? 0 : 1;
            }
            ChangePlayer();
            daUndo = true;
            return true;
        }
        public int player1UndoCount = 0;
        public int player2UndoCount = 0;
        public void EndGame()
        {
            gameTimer.Stop();
            
            
             winName = Player[CurrentPlayer == 1 ? 0 : 1].Name;
            if (endedGame != null)
                endedGame(this, new EventArgs());

            if (CurrentPlayer == 1)
            {
                frmHang1.win1++; 
            }
            else
            {
                frmHang1.win2++; 
            }
            frmHang1.kq1.Text ="WIN - "+ frmHang1.win1.ToString() + "/" + (frmHang1.win1 + frmHang1.win2).ToString();
            frmHang1.kq2.Text = "WIN - " + frmHang1.win2.ToString() + "/" + (frmHang1.win1 + frmHang1.win2).ToString();
            DialogResult result = MessageBox.Show($"{ winName}: WINNER! Bạn có muốn chơi tiếp", "Game Over", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                frmHang1.undo1 = 0;
                frmHang1.undo2 = 0;
                frmHang1.NewGame();
            }
            else if (result == DialogResult.No)
            {
                frmHang1.btnUndo1.Enabled = false;
                frmHang1.btnUndo2.Enabled = false;
            }
        }
        private bool isEndGame(Button btn)
        {
            Point point = GetChessPoint(btn);
            bool horizontalWin = isEndNgang(point, btn.BackgroundImage);
            bool verticalWin = isEndDoc(point, btn.BackgroundImage);
            bool primaryWin = isEndCheoChinh(point, btn.BackgroundImage);
            bool subWin = isEndCheoPhu(point, btn.BackgroundImage);
            if (horizontalWin || verticalWin || primaryWin || subWin)
            {
                winningButtons.AddRange(GetWinningButtons(point, btn.BackgroundImage));
                return true;
            }
            return false;
        }
        private Point GetChessPoint(Button btn)
        {           
            int doc = Convert.ToInt32(btn.Tag);
            int ngang = Matrix[doc].IndexOf(btn);
            Point point = new Point(ngang, doc);
            return point;
        }
        private bool isEndNgang(Point point,Image backgroundImage)
        {          
            int countLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if(Matrix[point.Y][i].BackgroundImage == backgroundImage)
                {
                    countLeft++;
                }
                else { break; }
            }
            int countRight = 0;
            for (int i = point.X+1; i < Cls_Hang1.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == backgroundImage)
                {
                    countRight++;
                }
                else { break; }
            }
            return countLeft+countRight >=5;
        }
        private bool isEndDoc(Point point, Image backgroundImage)
        {
            int countTop= 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == backgroundImage)
                {
                    countTop++;
                }
                else { break; }
            }
            int countBottom = 0;
            for (int i = point.Y + 1; i < Cls_Hang1.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == backgroundImage)
                {
                    countBottom++;
                }
                else { break; }
            }
            return countTop + countBottom >= 5;
        }
        private bool isEndCheoChinh(Point point, Image backgroundImage)
        {
            
            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                
                if(point.X -i<0 || point.Y - i < 0)
                {
                    break;
                }
                if (Matrix[point.Y-i][point.X-i].BackgroundImage == backgroundImage)
                {
                    countTop++;
                }
                else { break; }
            }
            int countBottom = 0;
            for (int i = 1; i <= Cls_Hang1.CHESS_BOARD_WIDTH- point.X; i++)
            {
                if (point.Y + i >= Cls_Hang1.CHESS_BOARD_HEIGHT || point.X + i >= Cls_Hang1.CHESS_BOARD_WIDTH)
                    break;
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == backgroundImage)
                {
                    countBottom++;
                }
                else { break; }
            }
            return countTop + countBottom >= 5;
        }
        private bool isEndCheoPhu(Point point, Image backgroundImage)
        {
            int countTop = 0;
            int countBottom = 0;
            for (int i = 0; i <= Math.Min(point.X, Cls_Hang1.CHESS_BOARD_HEIGHT - 1 - point.Y); i++)
            {
                if (matrix[point.Y + i][point.X - i].BackgroundImage == backgroundImage)
                {
                    countTop++;
                }
                else
                {
                    break;
                }
            }
            for (int i = 1; i <= Math.Min(Cls_Hang1.CHESS_BOARD_WIDTH - 1 - point.X, point.Y); i++)
            {
                if (matrix[point.Y - i][point.X + i].BackgroundImage == backgroundImage)
                {
                    countBottom++;
                }
                else
                {
                    break;
                }
            }
            return countTop + countBottom >= 5;
        }
        public void LoadGameHistory(List<Cls_Hang_PlayInfo> moves)
        {
            // Xóa tất cả nước đi hiện tại trên bàn cờ
            foreach (var row in Matrix)
            {
                foreach (var btn in row)
                {
                    btn.BackgroundImage = null;
                }
            }

            // Áp dụng lịch sử nước đi lên bàn cờ
            foreach (var move in moves)
            {
                Button btn = Matrix[move.Point.Y][move.Point.X];
                btn.BackgroundImage = Player[move.CurrentPlayer].Mark;

                // Cập nhật người chơi hiện tại
                CurentPlayer = move.CurrentPlayer;
                ChangePlayer();
            }
        }

        public List<Point> playedPs = new List<Point>();
        private void Mark(Button btn)
        {
            btn.BackgroundImage = Player[CurentPlayer].Mark;
          
        }
        private void ChangePlayer()
        {
            PlayerName.Text = Player[CurentPlayer].Name;
            PlayerMark.Image = Player[CurentPlayer].Mark;
        }
    }
}
