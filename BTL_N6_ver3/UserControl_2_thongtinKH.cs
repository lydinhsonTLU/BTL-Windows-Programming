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
    public partial class UserControl_2_thongtinKH: UserControl
    {
        string connectString = connectDb.connectString();
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        public UserControl_2_thongtinKH()
        {
            InitializeComponent();
        }


        

        //load thông tin khi bắt đầu mở usercontrol
        private void UserControl_2_thongtinKH_Load(object sender, EventArgs e)
        {
            //kết nối SQL
            connection = new SqlConnection(connectString);
            connection.Open();

            //biểu tượng con trỏ thay đổi khi di chuột đến button
            btn_UC_xem_quaylai.Cursor = Cursors.Hand;
            btn_xoa.Cursor = Cursors.Hand;
            btn_UC_Xem_them.Cursor = Cursors.Hand;
            btn_sua.Cursor = Cursors.Hand;
            

            //DGV chỉ cho đọc, ko cho chỉnh sửa
            dataGridView_UC_xem.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_UC_xem.ReadOnly = true;
            dataGridView_UC_xem.AllowUserToAddRows = false;

            load_cb_que();
            load_DGV_UC_xem();
            dataGridView_UC_xem.ClearSelection();
            reset();

            //gán acceptbutton
            Form parentForm = this.FindForm(); // Lấy Form chứa UserControl
            if (parentForm != null)
            {
                parentForm.AcceptButton = btn_UC_Xem_them; // Gán AcceptButton cho nút btnXacNhan
            }

            dateTimePicker_UC_Xem_UC_xem_ngaysinh.Text = "01-01-2000";
        }

        //reset lại bảng nhập liệu
        void reset ()
        {
            txt_UC_xem_cccd.Enabled = true;
            txt_UC_xem_cccd.Text = txt_UC_xem_ten.Text = txt_UC_xem_sdt.Text = lbl_trangthai_text.Text = "";
            dateTimePicker_UC_Xem_UC_xem_ngaysinh.Text = "01-01-2000";
            radioButton_UC_Xem_nam.Checked = radioButton_UC_Xem_Nu.Checked = false;
            cb_UC_Xem_que.SelectedIndex = -1;
        }

        //làm mới dữ liệu DGV
        void load_DGV_UC_xem ()
        {
            dataGridView_UC_xem.AutoGenerateColumns = false;

            command = connection.CreateCommand();
            command.CommandText = "select * from KhachHang";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dataGridView_UC_xem.DataSource = table;
        }

        //load comboBox quê quán
        void load_cb_que ()
        {
            SqlCommand loadCb = connection.CreateCommand();
            DataTable quequan = new DataTable();
            loadCb.CommandText = "Select * from tinhthanhVN";
            adapter.SelectCommand = loadCb;
            adapter.Fill(quequan);

            cb_UC_Xem_que.DataSource = quequan;
            cb_UC_Xem_que.DisplayMember = "diadiem";
            cb_UC_Xem_que.ValueMember = "diadiem";
        }

        //loại bỏ các lựa chọn khi click ra ngoài DGV
        private void UserControl_2_thongtinKH_Click(object sender, EventArgs e)
        {
            dataGridView_UC_xem.ClearSelection();
        }


        //sự kiện chọn cả dòng trong DGV
        private void dataGridView_UC_xem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //cccd ko được sửa
                txt_UC_xem_cccd.Enabled = false;

                if (e.RowIndex >= 0)
                {
                    // Bôi xanh (chọn) toàn bộ dòng khi nhấp vào ô
                    dataGridView_UC_xem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Xóa tất cả các lựa chọn hiện tại
                    dataGridView_UC_xem.ClearSelection();

                    // Chọn dòng được nhấp vào
                    dataGridView_UC_xem.Rows[e.RowIndex].Selected = true;

                    txt_UC_xem_cccd.Text = dataGridView_UC_xem.CurrentRow.Cells[0].Value.ToString();
                    txt_UC_xem_ten.Text = dataGridView_UC_xem.CurrentRow.Cells[1].Value.ToString();
                    dateTimePicker_UC_Xem_UC_xem_ngaysinh.Text = dataGridView_UC_xem.CurrentRow.Cells[2].Value.ToString();

                    if (dataGridView_UC_xem.CurrentRow.Cells[3].Value.ToString() == "Nam") radioButton_UC_Xem_nam.Checked = true;
                    else radioButton_UC_Xem_Nu.Checked = true;

                    txt_UC_xem_sdt.Text = dataGridView_UC_xem.CurrentRow.Cells[4].Value.ToString();
                    cb_UC_Xem_que.Text = dataGridView_UC_xem.CurrentRow.Cells[5].Value.ToString();
                    lbl_trangthai_text.Text = dataGridView_UC_xem.CurrentRow.Cells[6].Value.ToString();

                }
                else
                {
                    dataGridView_UC_xem.ClearSelection();
                }
            }catch(Exception)
            {
                MessageBox.Show("Có vấn đề! Hãy ReLoad lại hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //thêm KH
        private void btn_UC_Xem_them_Click(object sender, EventArgs e)
        {
            try
            {
                string cccd = txt_UC_xem_cccd.Text.Trim();
                string ten = txt_UC_xem_ten.Text.Trim();
                string ngaysinh = dateTimePicker_UC_Xem_UC_xem_ngaysinh.Value.ToString("yyyy-MM-dd");
                string gioitinh;
                if (radioButton_UC_Xem_nam.Checked) gioitinh = "Nam";
                else gioitinh = "Nữ";
                string sdt = txt_UC_xem_sdt.Text.Trim();
                string que = cb_UC_Xem_que.Text.Trim();
                string trangthai = "Chưa đặt phòng";

                if (cccd == "" || ten == "" || (radioButton_UC_Xem_nam.Checked == false && radioButton_UC_Xem_Nu.Checked == false) || sdt == "" || cb_UC_Xem_que.Text == "")
                {
                    MessageBox.Show("Điền đầy đủ thông tin trước khi THÊM!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //ktra xem KH đã tồn tại chưa
                SqlCommand ktra = connection.CreateCommand();
                ktra.CommandText = "select * from KhachHang where CCCD = '" + cccd + "'";
                DataTable tablektra = new DataTable();
                adapter.SelectCommand = ktra;
                adapter.Fill(tablektra);

                if (tablektra.Rows.Count > 0)
                {
                    MessageBox.Show("Khách hàng đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    //chưa tồn tại thì thêm mới
                    SqlCommand themKH = connection.CreateCommand();
                    themKH.CommandText = "insert into KhachHang values " +
                        "('" + cccd + "', N'" + ten + "', '" + ngaysinh + "', N'" + gioitinh + "', '" + sdt + "', N'" + que + "', N'" + trangthai + "')";

                    int them = themKH.ExecuteNonQuery();
                    if (them > 0)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        load_DGV_UC_xem();
                        reset();
                        dataGridView_UC_xem.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("Thêm khách hàng thất bại!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }catch(Exception)
            {
                MessageBox.Show("Có vấn đề! Hãy ReLoad lại hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //sửa KH
        private void btn_sua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView_UC_xem.CurrentRow == null)
                {
                    MessageBox.Show("Chọn 1 khách hàng để SỬA!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string cccd = txt_UC_xem_cccd.Text.Trim();
                string ten = txt_UC_xem_ten.Text.Trim();
                string ngaysinh = dateTimePicker_UC_Xem_UC_xem_ngaysinh.Value.ToString("yyyy-MM-dd");
                string gioitinh;
                if (radioButton_UC_Xem_nam.Checked) gioitinh = "Nam";
                else gioitinh = "Nữ";
                string sdt = txt_UC_xem_sdt.Text.Trim();
                string que = cb_UC_Xem_que.Text.Trim();
                string trangthai = lbl_trangthai_text.Text.Trim();

                if (cccd == "" || ten == "" || (radioButton_UC_Xem_nam.Checked == false && radioButton_UC_Xem_Nu.Checked == false) || sdt == "")
                {
                    MessageBox.Show("Điền đầy đủ thông tin trước khi SỬA!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //update KH
                SqlCommand suaKH = connection.CreateCommand();
                suaKH.CommandText = "Update KhachHang set Ho_Ten = N'" + ten + "', Ngay_Sinh = N'" + ngaysinh + "', Gioi_Tinh = N'" + gioitinh + "'," +
                    " So_Dien_Thoai = '" + sdt + "', Que_Quan = N'" + que + "', Trang_Thai = N'" + trangthai + "' WHERE CCCD = '" + cccd + "'";

                int sua = suaKH.ExecuteNonQuery();
                if (sua > 0)
                {
                    MessageBox.Show("Sửa thông tin khách hàng thành công!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sửa thông tin khách hàng thất bại!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dataGridView_UC_xem.ClearSelection();
                load_DGV_UC_xem();
                reset();
                dataGridView_UC_xem.ClearSelection();
                txt_UC_xem_cccd.Enabled = true;
            }catch(Exception)
            {
                MessageBox.Show("Có vấn đề! Hãy ReLoad lại hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        //xóa KH
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                string checkcccd = txt_UC_xem_cccd.Text.Trim();
                string checkten = txt_UC_xem_ten.Text.Trim();
                if (checkcccd == "" || checkten == "")
                {
                    MessageBox.Show("Chọn 1 khách hàng để XÓA!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {

                    string cccd = dataGridView_UC_xem.CurrentRow.Cells["CCCD"].Value.ToString();
                    string ten = dataGridView_UC_xem.CurrentRow.Cells["ten"].Value.ToString();

                    DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{ten}' có CCCD '{cccd}'?",
                                                         "Xác nhận xóa",
                                                         MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SqlCommand xoaKH = connection.CreateCommand();

                        xoaKH.CommandText = "DELETE FROM KhachHang WHERE CCCD = '" + cccd + "'";

                        int xoa = xoaKH.ExecuteNonQuery();

                        if (xoa > 0)
                        {
                            MessageBox.Show("Xóa thông tin khách hàng thành công!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("Xóa thông tin khách hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        dataGridView_UC_xem.ClearSelection();
                        load_DGV_UC_xem();
                        reset();
                        dataGridView_UC_xem.ClearSelection();
                        txt_UC_xem_cccd.Enabled = true;
                    }else
                    {
                        reset();
                        dataGridView_UC_xem.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có vấn đề! Hãy ReLoad lại hệ thống!" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }


        //tìm kiếm KH
        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string timkiem = txt_timkiem.Text.Trim();
                if (timkiem == "")
                {
                    //text tìm kiếm rỗng thì trả về ban đầu
                    UserControl_2_thongtinKH_Load(sender, e);
                    reset();
                    txt_UC_xem_cccd.Enabled = true;
                    dataGridView_UC_xem.ClearSelection();
                }
                else
                {
                    SqlCommand tim = connection.CreateCommand();
                    tim.CommandText = "SELECT * FROM KhachHang WHERE CCCD LIKE '%" + timkiem + "%' " + "OR Ho_Ten LIKE N'%" + timkiem + "%' " + "OR Gioi_Tinh LIKE N'%" + timkiem + "%' " + "OR So_Dien_Thoai LIKE '%" + timkiem + "%' " + "OR Que_Quan LIKE N'%" + timkiem + "%' " + "OR Trang_Thai LIKE N'%" + timkiem + "%'" + "OR Ngay_Sinh LIKE '%"+timkiem+"%'";
                    adapter.SelectCommand = tim;
                    DataTable tabTim = new DataTable();
                    adapter.Fill(tabTim);
                    dataGridView_UC_xem.DataSource = tabTim;
                }
            }catch(Exception)
            {
                MessageBox.Show("Có vấn đề! Hãy ReLoad lại hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //sự kiện Quay lại
        private void btn_UC_xem_quaylai_Click(object sender, EventArgs e)
        {
            closeUsercontrol thoat = new closeUsercontrol();  //khai báo đối tượng tắt UC
            thoat.closeUC(this); //gọi phương thức tắt

        }


        //ko sử dụng
        private void dataGridView_UC_xem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
 
        }


        //sự kiện click button XEM hóa đơn trong DGV
        private void dataGridView_UC_xem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7 )
            {
                if (lbl_trangthai_text.Text == "Đã thanh toán")
                {
                    string cccd = txt_UC_xem_cccd.Text.Trim();
                    SqlCommand showBill = connection.CreateCommand();
                    showBill.CommandText = "SELECT * from HoaDon WHere CCCD = '" + cccd + "'";
                    DataTable tableShow = new DataTable();

                    adapter.SelectCommand = showBill;
                    adapter.Fill(tableShow);

                    if (tableShow.Rows.Count > 0)
                    {
                        string ngayTT = tableShow.Rows[0]["Ngay_Thanh_Toan"].ToString();  //truy cập hàng đầu tiên cột Ngay TT
                        string ten = tableShow.Rows[0]["Ho_Ten"].ToString();
                        string maP = tableShow.Rows[0]["Ma_Phong"].ToString();
                        string gia = tableShow.Rows[0]["Gia_Phong"].ToString();
                        string soNgayO = tableShow.Rows[0]["So_Ngay_O"].ToString();

                        
                        frm_hoadon hoadon = new frm_hoadon();
                        hoadon.SetHoadon(ngayTT, cccd, ten, maP, gia, soNgayO);   //gán giá trị vào hóa đơn

                        hoadon.ShowDialog();
                        this.Show();
                        dataGridView_UC_xem.ClearSelection();
                        reset();
                    }
                }else 
                {
                    DialogResult r = MessageBox.Show("Khách hàng này chưa có hóa đơn để xem!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (r == DialogResult.OK)
                    {
                        reset();
                        dataGridView_UC_xem.ClearSelection();
                    }
                    return;
                }
            }
        }


        //kiểm tra ngày sinh hợp lệ (>=18t) mới được đặt phòng
        private void dateTimePicker_UC_Xem_UC_xem_ngaysinh_Leave(object sender, EventArgs e)
        {
            if (dateTimePicker_UC_Xem_UC_xem_ngaysinh.Value.Year >= 2008)
            {
                MessageBox.Show("Không được chọn năm 2008 hoặc lớn hơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker_UC_Xem_UC_xem_ngaysinh.Text = "01-01-2000";
                return;
            }
        }
    }
}
