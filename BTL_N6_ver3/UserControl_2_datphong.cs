using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Globalization;

namespace BTL_N6_ver3
{
    public partial class UserControl_2_datphong: UserControl
    {
        string connectString = connectDb.connectString();
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();


        public UserControl_2_datphong()
        {
            InitializeComponent();
        }

        private void UserControl_2_datphong_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectString);
            connection.Open();

            btnDatPhong.Cursor = Cursors.Hand;
            btnQuaylai.Cursor = Cursors.Hand;

            //DGV chỉ cho đọc, ko cho chỉnh sửa
            dataGridView_loaiPhong.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_loaiPhong.ReadOnly = true;
            dataGridView_loaiPhong.AllowUserToAddRows = false;

            load_DGV_loaiPhong();
            load_cb_KH();
            resetTicket();
            loadPhongDaDat();
            dataGridView_loaiPhong.ClearSelection();
            cb_UC_dat_maKH.SelectedIndex = -1;
            lbl_ten_txt.Text = "";

            foreach (Control i in tableLayoutPanel_phongDon.Controls)
            {
                if (i is Button btn)
                {
                    btn.Cursor = Cursors.Hand;
                }
            }
            foreach (Control i in tableLayoutPanel_phongDoi.Controls)
            {
                if (i is Button btn)
                {
                    btn.Cursor = Cursors.Hand;
                }
            }
            foreach (Control i in tableLayoutPanel_phongNhom.Controls)
            {
                if (i is Button btn)
                {
                    btn.Cursor = Cursors.Hand;
                }
            }

            //gán acceptbutton 
            Form parentForm = this.FindForm(); // Lấy Form chứa UserControl
            if (parentForm != null)
            {
                parentForm.AcceptButton = btnDatPhong; // Gán AcceptButton cho nút btnXacNhan
            }
        }

        //hơi thừa nhưng thôi không xóa
        private void UserControl_2_datphong_Click(object sender, EventArgs e)
        {

        }

        //load phòng đã đặt khi mở usercontrol đặt phòng
        void loadPhongDaDat ()
        {
            SqlCommand load = connection.CreateCommand();
            load.CommandText = "select Ma_Phong from DatPhongDonLe";
            adapter.SelectCommand = load;
            DataTable loadRoom = new DataTable();
            adapter.Fill(loadRoom);

            //phòng đơn
            foreach (DataRow i in loadRoom.Rows)        //duyệt qua từng dòng trong bảng loadRoom
            {
                string tenPhong = i["Ma_Phong"].ToString();

                foreach (Control don in tableLayoutPanel_phongDon.Controls) // duyệt các control trong panel
                {
                    if (don is Button && don.Text == tenPhong)  // nếu control đó là button và text giống tên phòng
                    {
                        don.BackColor = Color.FromArgb(192, 255, 255);
                        don.Enabled = false;
                    }
                }
            }

            //phòng đôi
            foreach (DataRow i in loadRoom.Rows)
            {
                string tenPhong = i["Ma_Phong"].ToString();

                foreach (Control doi in tableLayoutPanel_phongDoi.Controls)
                {
                    if (doi is Button && doi.Text == tenPhong)
                    {
                        doi.BackColor = Color.FromArgb(255, 255, 192);
                        doi.Enabled = false;
                    }
                }
            }

            //phòng nhóm
            foreach (DataRow i in loadRoom.Rows)
            {
                string tenPhong = i["Ma_Phong"].ToString();

                foreach (Control nhom in tableLayoutPanel_phongNhom.Controls)
                {
                    if (nhom is Button && nhom.Text == tenPhong)
                    {
                        nhom.BackColor = Color.FromArgb(192, 192, 255);
                        nhom.Enabled = false;
                    }
                }
            }
        }

        //load dữ liệu DGV loại phòng
        void load_DGV_loaiPhong ()
        {
            DataTable loaiPhong = new DataTable();
            command = connection.CreateCommand();
            command.CommandText = "Select * from loaiPhong Order by Gia_Phong_Theo_Ngay ASC";
            adapter.SelectCommand = command;
            adapter.Fill(loaiPhong);

            dataGridView_loaiPhong.DataSource = loaiPhong;
        }


        //load mã KH
        void load_cb_KH()
        {
            DataTable cbKH = new DataTable();
            command = connection.CreateCommand();
            command.CommandText = "Select CCCD from KhachHang";
            adapter.SelectCommand = command;
            adapter.Fill(cbKH);

            cb_UC_dat_maKH.DataSource = cbKH;
            cb_UC_dat_maKH.DisplayMember = "CCCD";
            cb_UC_dat_maKH.ValueMember = "CCCD";
        }

        //reset phiếu đặt phòng
        void resetTicket()
        {
            lbl_maPhong_txt.Text = txtSoNguoi.Text = "";
            lbl_gia_txt.Text = "0";
            cb_UC_dat_maKH.SelectedIndex = -1;
            dateTimePicker_den.Value = DateTime.Today;
            lbl_ten_txt.Text = "";
        }


        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            closeUsercontrol thoat = new closeUsercontrol();  //khai báo đối tượng tắt UC
            thoat.closeUC(this); //gọi phương thức tắt
        }



        //gán các btn từ 1 - 8 về 1 skien 
        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string maPhong = lbl_maPhong_txt.Text;
            int giaPhong = Convert.ToInt32(lbl_gia_txt.Text);

            if (maPhong.Length > 0)
            {
                if (btn.BackColor == Color.White)    //cập nhật chọn phòng
                {
                    lbl_maPhong_txt.Text = lbl_maPhong_txt.Text + "," + btn.Text;   //thêm tên dòng đó
                    lbl_gia_txt.Text = Convert.ToString(giaPhong + 100000);

                    btn.BackColor = Color.FromArgb(192, 255, 255);
                }else if (btn.BackColor == Color.FromArgb(192, 255, 255))     //cập nhật hủy phòng
                {
                    if (lbl_maPhong_txt.Text.Length - 5 < 0)
                    {
                        lbl_maPhong_txt.Text = "";
                    }else
                    {
                        lbl_maPhong_txt.Text = maPhong.Remove(maPhong.Length - 5);   //xóa tên phòng đó
                    }
                        lbl_gia_txt.Text = Convert.ToString(giaPhong - 100000);

                    btn.BackColor = Color.White;
                }
            }else
            {
                if (maPhong.Length == 0)
                {
                    if (btn.BackColor == Color.White)    //cập nhật chọn phòng
                    {
                        lbl_maPhong_txt.Text = btn.Text;   //thêm tên dòng đó
                        lbl_gia_txt.Text = "100000";

                        btn.BackColor = Color.FromArgb(192, 255, 255);
                    }
                    else if (btn.BackColor == Color.FromArgb(192, 255, 255))     //cập nhật hủy phòng
                    {
                        lbl_maPhong_txt.Text = "";   //xóa tên phòng đó
                        lbl_gia_txt.Text = "0";

                        btn.BackColor = Color.White;
                    }
                }
            }
        }

        //gán btn 9 - 16 về 1 skien
        private void button9_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string maPhong = lbl_maPhong_txt.Text;
            int giaPhong = Convert.ToInt32(lbl_gia_txt.Text);

            if (maPhong.Length > 0)
            {
                if (btn.BackColor == Color.White)    //cập nhật chọn phòng
                {
                    lbl_maPhong_txt.Text = lbl_maPhong_txt.Text + "," + btn.Text;   //thêm tên dòng đó
                    lbl_gia_txt.Text = Convert.ToString(giaPhong + 200000);

                    btn.BackColor = Color.FromArgb(255, 255, 192);
                }
                else if (btn.BackColor == Color.FromArgb(255, 255, 192))     //cập nhật hủy phòng
                {
                    if (lbl_maPhong_txt.Text.Length - 5 < 0)
                    {
                        lbl_maPhong_txt.Text = "";
                    }
                    else
                    {
                        lbl_maPhong_txt.Text = maPhong.Remove(maPhong.Length - 5);   //xóa tên phòng đó
                    }
                    lbl_gia_txt.Text = Convert.ToString(giaPhong - 200000);

                    btn.BackColor = Color.White;
                }
            }
            else
            {
                if (maPhong.Length == 0)
                {
                    if (btn.BackColor == Color.White)    //cập nhật chọn phòng
                    {
                        lbl_maPhong_txt.Text = btn.Text;   //thêm tên dòng đó
                        lbl_gia_txt.Text = "200000";

                        btn.BackColor = Color.FromArgb(255, 255, 192);
                    }
                    else if (btn.BackColor == Color.FromArgb(255, 255, 192))     //cập nhật hủy phòng
                    {
                        lbl_maPhong_txt.Text = "";   //xóa tên phòng đó
                        lbl_gia_txt.Text = "0";

                        btn.BackColor = Color.White;
                    }
                }
            }
        }

        //gán btn 17-24
        private void button17_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string maPhong = lbl_maPhong_txt.Text;
            int giaPhong = Convert.ToInt32(lbl_gia_txt.Text);

            if (maPhong.Length > 0)
            {
                if (btn.BackColor == Color.White)    //cập nhật chọn phòng
                {
                    lbl_maPhong_txt.Text = lbl_maPhong_txt.Text + "," + btn.Text;   //thêm tên dòng đó
                    lbl_gia_txt.Text = Convert.ToString(giaPhong + 500000);

                    btn.BackColor = Color.FromArgb(192, 192, 255);
                }
                else if (btn.BackColor == Color.FromArgb(192, 192, 255))     //cập nhật hủy phòng
                {
                    if (lbl_maPhong_txt.Text.Length - 5 < 0)
                    {
                        lbl_maPhong_txt.Text = "";
                    }
                    else
                    {
                        lbl_maPhong_txt.Text = maPhong.Remove(maPhong.Length - 5);   //xóa tên phòng đó
                    }
                    lbl_gia_txt.Text = Convert.ToString(giaPhong - 500000);

                    btn.BackColor = Color.White;
                }
            }
            else
            {
                if (maPhong.Length == 0)
                {
                    if (btn.BackColor == Color.White)    //cập nhật chọn phòng
                    {
                        lbl_maPhong_txt.Text = btn.Text;   //thêm tên dòng đó
                        lbl_gia_txt.Text = "500000";

                        btn.BackColor = Color.FromArgb(192, 192, 255);
                    }
                    else if (btn.BackColor == Color.FromArgb(192, 192, 255))     //cập nhật hủy phòng
                    {
                        lbl_maPhong_txt.Text = "";   //xóa tên phòng đó
                        lbl_gia_txt.Text = "0";

                        btn.BackColor = Color.White;
                    }
                }
            }
        }

        //skien đặt phòng
        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            try
            {
                string cccd = cb_UC_dat_maKH.Text.Trim();
                string maPhong = lbl_maPhong_txt.Text.Trim();
                string soNguoi = txtSoNguoi.Text.Trim();
                string gia = lbl_gia_txt.Text.Trim();
                string ngayden = dateTimePicker_den.Value.ToString("yyyy-MM-dd");
                //DateTime ngaydi = dateTimePicker_di.Value;

                if (cccd == "" || soNguoi == "")
                {
                    MessageBox.Show("Điền đầy đủ thông tin trước khi đặt phòng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
 
                    if (maPhong.Length > 4)
                    {
                        SqlCommand bookDonle = connection.CreateCommand();
                        SqlCommand bookTongQuan = connection.CreateCommand();

                        List<string> dsPhong = new List<string>(maPhong.Split(','));     //nếu length của Phòng > 4 tức là KH đặt nhiều hơn 1P

                        int dem = 0;    //tạo biến check xem đủ phòng chưa
                        foreach (string i in dsPhong)
                        {
                            bookDonle.CommandText = "insert into DatPhongDonLe values ('" + cccd + "', '" + i + "', '" + soNguoi + "', '" + gia + "', '" + ngayden + "')";
                            bookDonle.ExecuteNonQuery();
                            dem++;

                            //ko cho thao tác với các phòng đã đặt
                            foreach (Control j in tableLayoutPanel_phongDon.Controls)
                            {
                                if (j is Button && j.Text == i) j.Enabled = false;
                            }
                            foreach (Control j in tableLayoutPanel_phongDoi.Controls)
                            {
                                if (j is Button && j.Text == i) j.Enabled = false;
                            }
                            foreach (Control j in tableLayoutPanel_phongNhom.Controls)
                            {
                                if (j is Button && j.Text == i) j.Enabled = false;
                            }
                        }

                        //đồng thời thêm tất cả vào bảng chung
                        bookTongQuan.CommandText = "insert into DatPhong values ('" + cccd + "', '" + maPhong + "', '" + soNguoi + "', '" + gia + "', '" + ngayden + "')";
                        int tong = bookTongQuan.ExecuteNonQuery();

                        if (dem == dsPhong.Count && tong > 0)
                        {
                            //update trang thai
                            SqlCommand update_trangthai = connection.CreateCommand();
                            string tt = "Chưa thanh toán";
                            update_trangthai.CommandText = "Update KhachHang set Trang_Thai = N'" + tt + "' WHERE CCCD = '" + cccd + "'";
                            int check = update_trangthai.ExecuteNonQuery();
                            if (check > 0)
                            {
                                MessageBox.Show("Đặt phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                resetTicket();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Có vấn đề! Hãy ReLoad lại hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        int checkgia = Convert.ToInt32(txtSoNguoi.Text.Trim());

                        if (maPhong.StartsWith("S"))
                        {
                            if (checkgia > 2)
                            {
                                MessageBox.Show("Vượt quá số người tối đa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }else if (maPhong.StartsWith("D"))
                        {
                            if (checkgia > 5)
                            {
                                MessageBox.Show("Vượt quá số người tối đa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }else if (maPhong.StartsWith("T"))
                        {
                            if (checkgia > 10)
                            {
                                MessageBox.Show("Vượt quá số người tối đa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                            Button btn = sender as Button;

                        //insert vào bảng chung
                        SqlCommand book = connection.CreateCommand();
                        book.CommandText = "insert into DatPhong values ('" + cccd + "', '" + maPhong + "', '" + soNguoi + "', '" + gia + "', '" + ngayden + "')";

                        //insert vào bảng đơn
                        SqlCommand RiengLe = connection.CreateCommand();
                        RiengLe.CommandText = "insert into DatPhongDonLe values ('" + cccd + "', '" + maPhong + "', '" + soNguoi + "', '" + gia + "', '" + ngayden + "')";

                        //ktra xem đặt phòng thành công hay chưa
                        int check1 = book.ExecuteNonQuery();
                        int check2 = RiengLe.ExecuteNonQuery();

                        if (check1 > 0 && check2 > 0)
                        {
                            SqlCommand update_trangthai = connection.CreateCommand();
                            string tt = "Chưa thanh toán";
                            update_trangthai.CommandText = "Update KhachHang set Trang_Thai = N'" + tt + "' WHere CCCD = '" + cccd + "'";
                            int check = update_trangthai.ExecuteNonQuery();
                            if (check > 0)
                            {
                                MessageBox.Show("Đặt phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                resetTicket();

                                foreach (Control j in tableLayoutPanel_phongDon.Controls)
                                {
                                    if (j is Button && j.Text == maPhong) j.Enabled = false;
                                }
                                foreach (Control j in tableLayoutPanel_phongDoi.Controls)
                                {
                                    if (j is Button && j.Text == maPhong) j.Enabled = false;
                                }
                                foreach (Control j in tableLayoutPanel_phongNhom.Controls)
                                {
                                    if (j is Button && j.Text == maPhong) j.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Đặt phòng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Hãy nhập đúng dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }


        //chọn CCCD thì hiển thị tên tương ứng
        private void cb_UC_dat_maKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_UC_dat_maKH.SelectedValue != null)
            {
                Load_ten();
            }
            
        }

        //hàm lấy tên với cccd tương ứng
        void Load_ten()
        {
            string cccd = cb_UC_dat_maKH.Text;
            DataTable loadten = new DataTable();
            command = connection.CreateCommand();
            command.CommandText = "Select Ho_Ten FROM KhachHang Where CCCD = '"+cccd+"'";
            adapter.SelectCommand = command;

            adapter.Fill(loadten);

            if (loadten.Rows.Count > 0) // Kiểm tra nếu có dữ liệu
            {
                lbl_ten_txt.Text = loadten.Rows[0]["Ho_Ten"].ToString(); // Lấy dữ liệu từ dòng đầu tiên
            }
        }

        //ko sử dụng
        private void cb_UC_dat_maKH_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_den_Leave(object sender, EventArgs e)
        {
            if (dateTimePicker_den.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Không được chọn ngày nhỏ hơn hôm nay!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker_den.Value = DateTime.Today;
            }
        }
    }
}
