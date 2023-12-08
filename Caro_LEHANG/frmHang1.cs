using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WMPLib;


namespace Caro_LEHANG
{
    public partial class frmHang1 : Form
    {
        Cls_Hang2 ChessBoard;
        public string playerName1;
        public string playerName2;
        public static int win1;
        public static int win2;
        public Button kq1;
        public Button kq2;
        public static int undo1;
        public static int undo2;
        public static int undoo1;
        public static int undoo2;
        public Button btnUndo1;
        public Button btnUndo2;
        public Label lblgt;
        public TimeSpan tsp;
        public string winName1;
        public string winName2;
        


        bool nhac = true; 

        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public frmHang1()
        {
            InitializeComponent();         
            player.URL = "Nhacnen.mp3";

        }
        private void frmHang1_Load(object sender, EventArgs e)
        {          
            player.controls.play();
            panel2.Visible = false;
            panelcaro.Visible = false;
            menuToolStripMenuItem.Visible = false;
            kq1 = btnkq1;
            kq2 = btnkq2;
            btnUndo1 = button1;
            btnUndo2 = button2;
            lblgt = lblGameTime;
        }
        private void btnToggleSound_Click(object sender, EventArgs e)
        {
            if (nhac == true)
            {
                player.controls.pause();
                btnToggleSound.Text = "X";
                nhac = false;
            }else
            {
                player.controls.play();
                btnToggleSound.Text = " ";
                nhac = true;
            }    
        }
        private void buttonTron2_Click(object sender, EventArgs e)
        {
            panelcaro.Visible = false;
            panel2.Visible = true;
            txtName1.Focus();
        }
       
        private void buttonTron3_Click(object sender, EventArgs e)
        {          
            panelcaro.Visible = true;
            panel2.Visible = false;
            win1 = 0;
            win2 = 0;
            undo1 = 0;
            undo2 = 0;
            kq1.Text = "WIN - " + win1.ToString() + "/" + (win1 + win2).ToString();
            kq2.Text = "WIN - " + win2.ToString() + "/" + (win1 + win2).ToString();
            btnUndo1.Text ="UNDO - "+ undo1.ToString()+"/5";
            btnUndo2.Text = "UNDO - " + undo2.ToString()+"/5";
            playerName1 = txtName1.Text.Trim(); // Lấy tên từ TextBox và loại bỏ khoảng trắng ở đầu và cuối.
            playerName2 = txtName2.Text.Trim();
            ChessBoard = new Cls_Hang2(pnlChessBoard, txtPlayerName, pctbMark,this);
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;
            prcbCoolDown.Step = Cls_Hang1.COOL_DOWN_STEP;
            prcbCoolDown.Maximum = Cls_Hang1.COOL_DOWN_TIME;
            prcbCoolDown.Value = 0;
            tmCoolDown.Interval = Cls_Hang1.COOL_DOWN_INTERVAL;
            NewGame();
            if (string.IsNullOrEmpty(playerName1) || string.IsNullOrEmpty(playerName2))
            {
                panel2.Visible = true;
                panelcaro.Visible = false;
                MessageBox.Show("Vui lòng nhập tên trước khi bắt đầu chơi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                lblPlayerName1.Text =  playerName1;
                lblPlayerName2.Text =  playerName2;
              
                panel2.Visible = false;
                panelcaro.Visible = true;
                menuToolStripMenuItem.Visible = true;              
            }
        }
        public void NewGame()
        {
            
            undo1 = 0;
            undo2 = 0;
            ChessBoard.gameTimer.Start();
            ChessBoard.gameStartTime = DateTime.Now;
            btnUndo1.Text = "UNDO - " + undo1.ToString() + "/5";
            btnUndo2.Text = "UNDO - " + undo2.ToString() + "/5";
            ChessBoard.DrawChessBoard();
            prcbCoolDown.Value = 0;
            tmCoolDown.Start();
            undoToolStripMenuItem.Enabled = true;
            pnlChessBoard.BackColor = Color.White;
        }
        void Exit()
        {
            Application.Exit();  
        }
        void Undo()
        {
            ChessBoard.Undo();
        }
        void EndGame()
        {
            tmCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            btnUndo1.Enabled = false;
            btnUndo2.Enabled = false;
            pnlChessBoard.BackColor = Color.Gray;          
        }

        private void ChessBoard_PlayerMarked(object sender, EventArgs e)
        {

            tmCoolDown.Start();
            prcbCoolDown.Value = 0;
        }
        private List<ChessMove> moveHistory = new List<ChessMove>();

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
           

            // Lấy thông tin lịch sử của trận đấu
            string player1 = playerName1;
            string player2 = playerName2;
            string thang = ChessBoard.winName;
            int undoc1 = ChessBoard.player1UndoCount;
            int undoc2 = ChessBoard.player2UndoCount;
            List<ChessMove> moves = new List<ChessMove>(moveHistory);

            // Thêm thông tin lịch sử vào danh sách gameHistories
            gameHistories.Add(new GameHistory(player1, player2,thang,undoc1,undoc2));

            UpdateHistoryMenu();

            // Xử lý kết thúc trận đấu (hiển thị kết quả và thực hiện các thao tác khác)
            EndGame();
        }
        private void iconButton3_Click(object sender, EventArgs e)
        {
            if (nhac == true)
            {
                player.controls.pause();
                iconButton3.Text = "X";
                nhac = false;
            }
            else
            {
                player.controls.play();
                iconButton3.Text = " ";
                nhac = true;
            }           
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            ClearGameHistory();
            panel2.Visible = false;
            panelcaro.Visible = false;
            txtName1.Text = "";
            txtName2.Text = "";
            tmCoolDown.Stop();
            menuToolStripMenuItem.Visible = false;
        }
        private void iconButton2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panelcaro.Visible = false;
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            if (nhac == true)
            {
                player.controls.pause();
                iconButton3.Text = "X";
                nhac = false;
            }
            else
            {
                player.controls.play();
                iconButton3.Text = " ";
                nhac = true;
            }
        }
        private void iconButton4_Click(object sender, EventArgs e)
        {
            ClearGameHistory();
            panel2.Visible = false;
            panelcaro.Visible = false;
            txtName1.Text = "";
            txtName2.Text = "";            
        }
        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();
            if(prcbCoolDown.Value>= prcbCoolDown.Maximum)
            {
                ChessBoard.gameTimer.Stop();
                tmCoolDown.Stop();
                pnlChessBoard.Enabled = false;
                undoToolStripMenuItem.Enabled = false;
                pnlChessBoard.BackColor = Color.Gray;
                DialogResult result = MessageBox.Show("Kết thúc! Chơi tiếp?", "Kết thúc trò chơi", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    NewGame();
                }
                else if (result == DialogResult.No)
                {
                    btnUndo1.Enabled = false;
                    btnUndo2.Enabled = false;                   
                }
            }
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearGameHistory();
            panel2.Visible = true;
            panelcaro.Visible = false;
            txtName1.Focus();
            txtName1.Text = "";
            txtName2.Text = "";
            tmCoolDown.Stop();
            menuToolStripMenuItem.Visible = false;
            ChessBoard.gameTimer.Stop();
            ChessBoard.gameStartTime = DateTime.Now;
        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }
        private void frmHang1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát Game?", "Game Caro",MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)            
            e.Cancel = true;
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Undo();
        }
        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Tệp tin văn bản (.txt)|.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                SaveResultsToFile(filePath, win1.ToString(), win2.ToString());
            }
        }
        private void SaveResultsToFile(string filePath, string result1, string result2)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Tổng số trận: " + Convert.ToInt32(win1 + win2) + " trận");                   
                    writer.WriteLine( playerName1 + "(X) thắng: " + win1 +" trận");
                    writer.WriteLine( playerName2 + "(O) thắng: " + win2 + " trận");
                    writer.WriteLine("Tổng số lần undo:");
                    writer.WriteLine(playerName1 + " undo: " + undoo1 + " lần");
                    writer.WriteLine(playerName2 + " undo: " + undoo2 + " lần");            
                }
                MessageBox.Show("Lưu kết quả thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Tệp tin văn bản (.txt)|.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    SaveResultsToFile(filePath, win1.ToString(), win2.ToString());
                }
            }
        }
        public class ChessMove
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public char PlayerMark { get; set; }

            public ChessMove(int row, int column, char playerMark)
            {
                Row = row;
                Column = column;
                PlayerMark = playerMark;
            }
        }

        public class GameHistory
        {
            public string PlayerName1 { get; set; }
            public string PlayerName2 { get; set; }
            public string NguoiThang { get; set; }
            public int UndoName1 { get; set; }
            public int UndoName2 { get; set; }
            public GameHistory(string player1, string player2, string thang,int undoc1,int undoc2)
            {
                PlayerName1 = player1;
                PlayerName2 = player2;
                NguoiThang = thang;
                UndoName1 = undoc1;
                UndoName2 = undoc2;
              
            }
        }
        private List<GameHistory> gameHistories = new List<GameHistory>();
    
        private void UpdateHistoryMenu()
        {
            historyToolStripMenuItem.DropDownItems.Clear();

            for (int i = 0; i < gameHistories.Count; i++)
            {
                var gameHistory = gameHistories[i];
                var menuItem = new ToolStripMenuItem($"Trận {i + 1  }");
                menuItem.Tag = i; // Đánh dấu trận đấu

                winName1 = ChessBoard.winName;
                // Khi người chơi chọn một trận đấu, gán sự kiện Click để hiển thị thông tin trận đấu
                menuItem.Click += (sender, e) =>
                {
                    int index = (int)((ToolStripMenuItem)sender).Tag;
                    ShowGameHistory(index);
                };

                historyToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }
      

        private void ShowGameHistory(int index)
        {
           
            if (index >= 0 && index < gameHistories.Count)
            {
                var gameHistory = gameHistories[index];

                MessageBox.Show($"Trận {index + 1}:" +"\n"+"\n"+
                   // $"Người chơi 1: {gameHistory.PlayerName1}\n" +
                   // $"Người chơi 2: {gameHistory.PlayerName2}\n"
                     $"Người chơi thắng: {gameHistory.NguoiThang}\n"+
                     $"{gameHistory.PlayerName1} undo {gameHistory.UndoName1} lần \n"+
                     $"{gameHistory.PlayerName2} undo {gameHistory.UndoName2} lần \n" 
                     , "Thông tin trận đấu"
                    );
            }   
        }
        private void ClearGameHistory()
        {
            gameHistories.Clear();
            UpdateHistoryMenu();
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
        }
    }
}

