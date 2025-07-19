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

namespace BTL_N6_ver3
{
    public partial class UserControl_thanhtoan: UserControl
    {
        string connectString = connectDb.connectString();
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public UserControl_thanhtoan()
        {
            InitializeComponent();
        }

        private void UserControl_thanhtoan_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectString);
            connection.Open();

            btnTinhtien.Cursor = Cursors.Hand;
            btnThanhtoan.Cursor = Cursors.Hand;
            btnQuaylai.Cursor = Cursors.Hand;

            load_DGV();
            reset_Text();
            dateTimePicker_ngayTT.Value = DateTime.Today;

            //DGV chỉ cho đọc, ko cho chỉnh sửa
            dataGridView_KHchuaTT.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_KHchuaTT.ReadOnly = true;
            dataGridView_KHchuaTT.AllowUserToAddRows = false;
            dataGridView_KHchuaTT.ClearSelection();

            //gán acceptbutton
            Form parentForm = this.FindForm(); // Lấy Form chứa UserControl
            if (parentForm != null)
            {
                parentForm.AcceptButton = btnThanhtoan; // Gán AcceptButton cho nút btnXacNhan
            }
        }

        private void UserControl_thanhtoan_Click(object sender, EventArgs e)
        {
            dataGridView_KHchuaTT.ClearSelection();
        }

        //load DGV KH chưa thanh toán
        void load_DGV()
        {
            command = connection.CreateCommand();
            command.CommandText = "SELECT k.CCCD,k.Ho_Ten,d.Ma_Phong,d.Gia_Phong,k.So_Dien_Thoai,k.Trang_Thai,d.Ngay_Den FROM KhachHang k JOIN DatPhong d ON k.CCCD = d.CCCD WHERE k.Trang_Thai = N'Chưa thanh toán'";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);

            dataGridView_KHchuaTT.DataSource = table;
        }

        //reset dữ liệu text
        void reset_Text()
        {
            lblMaKH.Text = lblTenKH.Text = lblSoNgayO.Text = txtPhong.Text = "";
            dateTimePicker_ngayTT.Value = DateTime.Today;
            lblTongtien.Text = "Tổng Tiền Phải Thanh Toán:";
        }

        //hiển thị in4 KH khi chọn 1 dòng trong DGV
        private void dataGridView_KHchuaTT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {
                    // Bôi xanh (chọn) toàn bộ dòng khi nhấp vào ô
                    dataGridView_KHchuaTT.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Xóa tất cả các lựa chọn hiện tại
                    dataGridView_KHchuaTT.ClearSelection();

                    // Chọn dòng được nhấp vào
                    dataGridView_KHchuaTT.Rows[e.RowIndex].Selected = true;

                    if (dataGridView_KHchuaTT.CurrentRow.Cells[5].Value.ToString() == "Đã thanh toán")
                    {
                        MessageBox.Show("Khách hàng đã thanh toán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        lblMaKH.Text = dataGridView_KHchuaTT.CurrentRow.Cells[0].Value.ToString();
                        lblTenKH.Text = dataGridView_KHchuaTT.CurrentRow.Cells[1].Value.ToString();

                        dateTimePicker_ngayTT.Text = dataGridView_KHchuaTT.CurrentRow.Cells[6].Value.ToString();

                        DateTime ngayDen = Convert.ToDateTime(dataGridView_KHchuaTT.CurrentRow.Cells[6].Value);
                        DateTime ngayDi = DateTime.Today;
                        // Tính số ngày ở
                        int soNgayO = (ngayDi - ngayDen).Days;
                        if (soNgayO == 0)
                        {
                            lblSoNgayO.Text = "1";
                        }
                        else
                        {
                            lblSoNgayO.Text = soNgayO.ToString();
                        }
                        txtPhong.Text = dataGridView_KHchuaTT.CurrentRow.Cells[3].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi "+ ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        //tính tiền
        private void btnTinhtien_Click(object sender, EventArgs e)
        {
            if (lblMaKH.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn khách hàng thanh toán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }else
            {
                DateTime ngayDen = Convert.ToDateTime(dataGridView_KHchuaTT.CurrentRow.Cells[6].Value);
                DateTime ngayDi = DateTime.Today;
                // Tính số ngày ở
                int soNgayO = (ngayDi - ngayDen).Days;
                string tongtien;
                int giaphong = Convert.ToInt32(dataGridView_KHchuaTT.CurrentRow.Cells[3].Value);

                if (soNgayO == 0)
                {
                    lblSoNgayO.Text = "1";
                    lblTongtien.Text = "Tông Tiền Phải Thanh Toán: " + Convert.ToString(giaphong);
                }
                else
                {
                    tongtien = Convert.ToString(giaphong * soNgayO);
                    lblTongtien.Text = "Tông Tiền Phải Thanh Toán: " + tongtien;
                }
            }
            
        }

        //tìm kiếm
        private void txttimkiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string timkiem = txttimkiem.Text.Trim();
                if (timkiem == "")
                {
                    //text tìm kiếm rỗng thì trả về ban đầu
                    //UserControl_thanhtoan_Load(sender, e);
                    load_DGV();
                    reset_Text();
                    dataGridView_KHchuaTT.ClearSelection();
                    dateTimePicker_ngayTT.Value = DateTime.Today;
                }
                else
                {
                    DataTable tabTim = new DataTable();
                    tabTim.Clear();
                    SqlCommand tim = connection.CreateCommand();
                    tim.CommandText = "SELECT k.CCCD,k.Ho_Ten,d.Ma_Phong,d.Gia_Phong,k.So_Dien_Thoai,k.Trang_Thai,d.Ngay_Den FROM KhachHang k JOIN DatPhong d ON k.CCCD = d.CCCD WHERE ( k.CCCD LIKE '%" + timkiem + "%' " + "OR k.Ho_Ten LIKE N'%" + timkiem + "%' " + "OR d.Ma_Phong LIKE '%" + timkiem + "%'" + "OR k.So_Dien_Thoai LIKE '%"+timkiem+"%'" + "OR d.Gia_Phong LIKE N'%" + timkiem + "%'"+ "OR d.Ngay_Den LIKE N'%" + timkiem + "%'" +")" + "AND k.Trang_Thai = N'Chưa thanh toán'";
                    adapter.SelectCommand = tim;
                    
                    adapter.Fill(tabTim);
                    dataGridView_KHchuaTT.DataSource = tabTim;
                    //tabTim.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
                return;
            }
        }

        frm_trangchu tc = new frm_trangchu();

        //thanh toán
        private void btnThanhtoan_Click(object sender, EventArgs e)
        {
            if (lblTongtien.Text == "Tổng Tiền Phải Thanh Toán:")
            {
                MessageBox.Show("Cần tính tiền trước khi thanh toán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }else
            {  
                frm_hoadon hoadon = new frm_hoadon();
                frm_traPhong traphong = new frm_traPhong();
                string ngayTT = dateTimePicker_ngayTT.Value.ToString("yyyy-MM-dd");
                string cccd = lblMaKH.Text;
                string ten = lblTenKH.Text;
                string maP = dataGridView_KHchuaTT.CurrentRow.Cells[2].Value.ToString();
                string gia = txtPhong.Text;
                string soNgayO = lblSoNgayO.Text;
                string tenThuNgan = tc.gettenThuNgan();
                hoadon.SetHoadon(ngayTT,cccd,ten,maP,gia,soNgayO);
                traphong.setTraphong(maP, cccd, ten, gia);
                hoadon.ShowDialog();
                this.Show();
                
                //cập nhật trạng thái cho khách hàng
                string trangthaiupdate = "Đã thanh toán";
                command = connection.CreateCommand();
                command.CommandText = "UPDATE KhachHang SET Trang_Thai = N'" + trangthaiupdate + "' WHERE CCCD = '"+cccd+"'";
                int check = command.ExecuteNonQuery();

                if (check > 0)
                {
                    load_DGV();

                    //lưu vào bảng Hóa đơn
                    command = connection.CreateCommand();
                    command.CommandText = "insert into HoaDon values ('"+ngayTT+"', '"+cccd+"', N'"+ten+"', '"+maP+"', '"+gia+"', '"+soNgayO+"')";
                    command.ExecuteNonQuery();

                    DialogResult r = MessageBox.Show("Bạn muốn trả phòng ngay bây giờ không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (r == DialogResult.Yes)
                    {
                        traphong.ShowDialog();
                        this.Show();
                        MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView_KHchuaTT.ClearSelection();
                    }else
                    {
                        MessageBox.Show("Hãy quay lại mục HỦY PHÒNG để TRẢ PHÒNG trước khi rời đi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView_KHchuaTT.ClearSelection();
                    }
                }
                reset_Text();
            }
        }

        

        private void dataGridView_KHchuaTT_SelectionChanged(object sender, EventArgs e)
        {
            reset_Text();
        }

        private void dataGridView_KHchuaTT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn DataGridView di chuyển xuống dòng tiếp theo
                btnThanhtoan.PerformClick(); // Giả lập hành động nhấn vào nút
            }
        }

        private void btn_traphong_Click(object sender, EventArgs e)
        {
            frm_traphong2 traphong = new frm_traphong2();
            traphong.ShowDialog();

            this.Show();
        }

        private void btnQuaylai_Click_1(object sender, EventArgs e)
        {
            closeUsercontrol thoat = new closeUsercontrol();  //khai báo đối tượng tắt UC
            thoat.closeUC(this); //gọi phương thức tắt
        }
    }
}
