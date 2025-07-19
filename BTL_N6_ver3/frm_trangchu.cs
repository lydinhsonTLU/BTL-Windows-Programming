using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_N6_ver3
{
    public partial class frm_trangchu: Form
    {
        public frm_trangchu()
        {
            InitializeComponent();
            
        }   

        //skien đăng xuất bằng form
        private void frm_trangchu_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                this.Show();
            }
        }

        public void settenNV (string ten)
        {
            lbl_nv.Text += ten;
        }
       
        //lấy tên nhân viên khi đăng nhập thành công
        public string gettenThuNgan ()
        {
            return lbl_nv.Text;
        }

        //Không sử dụng
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //hiển thị con trỏ bàn tay
        private void frm_trangchu_Load(object sender, EventArgs e)
        {
            lbl_tongquan.Cursor = Cursors.Hand;
            lbl_thongtinKH.Cursor = Cursors.Hand;
            lbl_datphong.Cursor = Cursors.Hand;
            lbl_huyphong.Cursor = Cursors.Hand;
            lbl_thanhtoan.Cursor = Cursors.Hand;
            lbl_dangxuat.Cursor = Cursors.Hand;


        }


        //gán 6 label về sự kiện di chuột vào
        private void lbl_tongquan_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl.Text == "Tổng quan")
            {
                lbl.BackColor = Color.FromArgb(224, 224, 224);
                lbl.ForeColor = Color.Black;
            } else if (lbl.Text == "Khách hàng")
            {
                lbl.BackColor = Color.FromArgb(224, 224, 224);
                lbl.ForeColor = Color.Black;
            }else if (lbl.Text == "Đặt phòng")
            {
                lbl.BackColor = Color.FromArgb(224, 224, 224);
                lbl.ForeColor = Color.Black;
            }else if (lbl.Text == "Hủy phòng")
            {
                lbl.BackColor = Color.FromArgb(224, 224, 224);
                lbl.ForeColor = Color.Black;
            }else if (lbl.Text == "Thanh toán")
            {
                lbl.BackColor = Color.FromArgb(224, 224, 224);
                lbl.ForeColor = Color.Black;
            }else if (lbl.Text == "Đăng xuất")
            {
                lbl.BackColor = Color.FromArgb(224, 224, 224);
                lbl.ForeColor = Color.Black;
            }
        }

        //gán 6 label về sự kiện di chuột ra
        private void lbl_tongquan_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl.Text == "Tổng quan")
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
            }
            else if (lbl.Text == "Khách hàng")
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
            }
            else if (lbl.Text == "Đặt phòng")
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
            }
            else if (lbl.Text == "Hủy phòng")
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
            }
            else if (lbl.Text == "Thanh toán")
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
            }
            else if (lbl.Text == "Đăng xuất")
            {
                lbl.BackColor = Color.Black;
                lbl.ForeColor = Color.White;
            }
        }

        //skien mở UC tổng quan
        private void lbl_tongquan_Click(object sender, EventArgs e)
        {
            closeUsercontrol Uc_call_uc = new closeUsercontrol();
            Uc_call_uc.closeUC_call_UC(tableLayoutPanel_mainscree_userControl);

            UserControl_tongquan UC_view = new UserControl_tongquan();
            UC_view.Dock = DockStyle.Fill;

            //lấp đầy panel
            tableLayoutPanel_mainscree_userControl.Controls.Add(UC_view, 0, 0);
            tableLayoutPanel_mainscree_userControl.BackgroundImage = null;
        }

        //Skien mở UC thông tin Khách hàng
        private void lbl_thongtinKH_Click(object sender, EventArgs e)
        {
            closeUsercontrol Uc_call_uc = new closeUsercontrol();
            Uc_call_uc.closeUC_call_UC(tableLayoutPanel_mainscree_userControl);

            UserControl_2_thongtinKH uc_in4 = new UserControl_2_thongtinKH();
            uc_in4.Dock = DockStyle.Fill;

            //lấp đầy panel
            tableLayoutPanel_mainscree_userControl.Controls.Add(uc_in4, 0, 0);
            tableLayoutPanel_mainscree_userControl.BackgroundImage = null;
        }


        //skien mở UC đặt phòng
        private void lbl_datphong_Click(object sender, EventArgs e)
        {
            closeUsercontrol Uc_call_uc = new closeUsercontrol();
            Uc_call_uc.closeUC_call_UC(tableLayoutPanel_mainscree_userControl);

            UserControl_2_datphong uc_book = new UserControl_2_datphong();
            uc_book.Dock = DockStyle.Fill;

            //lấp đầy panel
            tableLayoutPanel_mainscree_userControl.Controls.Add(uc_book, 0, 0);
            tableLayoutPanel_mainscree_userControl.BackgroundImage = null;
        }

        //skien mở UC hủy phòng
        private void lbl_huyphong_Click(object sender, EventArgs e)
        {
            closeUsercontrol Uc_call_uc = new closeUsercontrol();
            Uc_call_uc.closeUC_call_UC(tableLayoutPanel_mainscree_userControl);

            UserControl_huyphong UC_huy = new UserControl_huyphong();
            UC_huy.Dock = DockStyle.Fill;

            //lấp đầy panel
            tableLayoutPanel_mainscree_userControl.Controls.Add(UC_huy, 0, 0);
            tableLayoutPanel_mainscree_userControl.BackgroundImage = null;
        }

        //skien mở UC thanh toán
        private void lbl_thanhtoan_Click(object sender, EventArgs e)
        {
            closeUsercontrol Uc_call_uc = new closeUsercontrol();
            Uc_call_uc.closeUC_call_UC(tableLayoutPanel_mainscree_userControl);

            UserControl_thanhtoan UC_pay = new UserControl_thanhtoan();
            UC_pay.Dock = DockStyle.Fill;

            //lấp đầy panel
            tableLayoutPanel_mainscree_userControl.Controls.Add(UC_pay, 0, 0);
            tableLayoutPanel_mainscree_userControl.BackgroundImage = null;
        }

        //skien đăng xuất
        private void lbl_dangxuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}
