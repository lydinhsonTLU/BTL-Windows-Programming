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
    public partial class UserControl_huyphong: UserControl
    {
        string connectString = connectDb.connectString();
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public UserControl_huyphong()
        {
            InitializeComponent();
        }

        private void UserControl_huyphong_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectString);
            connection.Open();
            dateTimePicker_ngayden.Value = DateTime.Today;

            load_DGV();
            reset_Text();

            btn_quaylai.Cursor = Cursors.Hand;
            btn_huyphong.Cursor = Cursors.Hand;

            //DGV chỉ cho đọc, ko cho chỉnh sửa
            dataGridView_UC_huyPhong.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_UC_huyPhong.ReadOnly = true;
            dataGridView_UC_huyPhong.AllowUserToAddRows = false;
            dataGridView_UC_huyPhong.ClearSelection();

            //gán acceptbuton
            Form parentForm = this.FindForm(); // Lấy Form chứa UserControl
            if (parentForm != null)
            {
                parentForm.AcceptButton = btn_huyphong; // Gán AcceptButton cho nút btnXacNhan
            }
        }

        private void UserControl_huyphong_Click(object sender, EventArgs e)
        {
            dataGridView_UC_huyPhong.ClearSelection();
        }

        private void dataGridView_UC_huyPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {
                    // Bôi xanh (chọn) toàn bộ dòng khi nhấp vào ô
                    dataGridView_UC_huyPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Xóa tất cả các lựa chọn hiện tại
                    dataGridView_UC_huyPhong.ClearSelection();

                    // Chọn dòng được nhấp vào
                    dataGridView_UC_huyPhong.Rows[e.RowIndex].Selected = true;

                    lbl_txt_maKH.Text = dataGridView_UC_huyPhong.CurrentRow.Cells[0].Value.ToString();
                    txt_phong.Text = dataGridView_UC_huyPhong.CurrentRow.Cells[2].Value.ToString();

                    txt_so_nguoi.Text = dataGridView_UC_huyPhong.CurrentRow.Cells[3].Value.ToString();
                    lbl_txt_giaphong.Text = dataGridView_UC_huyPhong.CurrentRow.Cells[4].Value.ToString();
                    dateTimePicker_ngayden.Text = dataGridView_UC_huyPhong.CurrentRow.Cells[5].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //load DGV KH đã đặt phòng
        void load_DGV()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select k.CCCD, k.Ho_Ten, d.Ma_Phong, d.So_Nguoi, d.Gia_Phong, d.Ngay_Den FROM KhachHang k JOIN DatPhong d ON k.CCCD = d.CCCD";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);

            dataGridView_UC_huyPhong.DataSource = table;
        }

        //reset dữ liệu text
        void reset_Text()
        {
            lbl_txt_giaphong.Text = txt_phong.Text = txt_so_nguoi.Text = lbl_txt_maKH.Text = "";
            dateTimePicker_ngayden.Value = DateTime.Today;
        }

        private void btn_quaylai_Click(object sender, EventArgs e)
        {
            closeUsercontrol thoat = new closeUsercontrol();  //khai báo đối tượng tắt UC
            thoat.closeUC(this); //gọi phương thức tắt
        }

        private void btn_huyphong_Click(object sender, EventArgs e)
        {
            try
            {
                string phong = txt_phong.Text.Trim();  // Danh sách phòng cần hủy, ví dụ: "101,102"
                string maKH = lbl_txt_maKH.Text.Trim(); // CCCD của khách hàng

                if (phong == "" || maKH == "")
                {
                    MessageBox.Show("Chọn 1 thông tin bên dưới trước khi hủy phòng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult r = MessageBox.Show("Bạn muốn hủy phòng đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    // Lấy danh sách phòng hiện tại từ SQL
                    SqlCommand getPhongCmd = connection.CreateCommand();
                    getPhongCmd.CommandText = "SELECT Ma_Phong FROM DatPhong WHERE CCCD = @cccd";
                    getPhongCmd.Parameters.AddWithValue("@cccd", maKH);


                    string phongHienTai = getPhongCmd.ExecuteScalar()?.ToString(); // Ví dụ: "101,102,103"

                    if (string.IsNullOrEmpty(phongHienTai))
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu đặt phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tách danh sách phòng từ SQL & danh sách phòng cần xóa
                    List<string> dsPhongHienTai = phongHienTai.Split(',').Select(p => p.Trim()).ToList();
                    List<string> dsPhongCanXoa = phong.Split(',').Select(p => p.Trim()).ToList();

                    // Loại bỏ các phòng cần xóa khỏi danh sách hiện tại
                    dsPhongHienTai = dsPhongHienTai.Except(dsPhongCanXoa).ToList();

                    string phongMoi = string.Join(",", dsPhongHienTai); // Kết quả mong muốn: "103"

                    // Nếu danh sách trống -> xóa luôn bản ghi
                    if (dsPhongHienTai.Count == 0)
                    {
                        SqlCommand deleteDatPhong = connection.CreateCommand();
                        deleteDatPhong.CommandText = "DELETE FROM DatPhong WHERE CCCD = @cccd";
                        deleteDatPhong.Parameters.AddWithValue("@cccd", maKH);
                        deleteDatPhong.ExecuteNonQuery();

                        // Cập nhật trạng thái khách hàng
                        SqlCommand updateKH = connection.CreateCommand();
                        updateKH.CommandText = "UPDATE KhachHang SET Trang_Thai = N'Chưa đặt phòng' WHERE CCCD = @cccd";
                        updateKH.Parameters.AddWithValue("@cccd", maKH);
                        updateKH.ExecuteNonQuery();
                    }
                    else
                    {
                        // Tính lại giá phòng
                        int giaPhongMoi = 0;
                        foreach (string p in dsPhongHienTai)
                        {
                            if (p.StartsWith("S")) giaPhongMoi += 100000;
                            else if (p.StartsWith("D")) giaPhongMoi += 200000;
                            else if (p.StartsWith("T")) giaPhongMoi += 500000;
                        }

                        // Cập nhật lại DatPhong với danh sách mới
                        SqlCommand updateDatPhong = connection.CreateCommand();
                        updateDatPhong.CommandText = "UPDATE DatPhong SET Ma_Phong = @phongMoi, Gia_Phong = @giaMoi WHERE CCCD = @cccd";
                        updateDatPhong.Parameters.AddWithValue("@phongMoi", phongMoi);
                        updateDatPhong.Parameters.AddWithValue("@giaMoi", giaPhongMoi);
                        updateDatPhong.Parameters.AddWithValue("@cccd", maKH);
                        updateDatPhong.ExecuteNonQuery();
                    }

                    // Xóa phòng trong DatPhongDonLe
                    SqlCommand huyLe = connection.CreateCommand();
                    huyLe.CommandText = "DELETE FROM DatPhongDonLe WHERE Ma_Phong IN (" + string.Join(",", dsPhongCanXoa.Select(p => "'" + p + "'")) + ") AND CCCD = @cccd";
                    huyLe.Parameters.AddWithValue("@cccd", maKH);
                    huyLe.ExecuteNonQuery();

                    MessageBox.Show("Hủy phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset_Text();
                    load_DGV();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimkiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string timkiem = txtTimkiem.Text.Trim();
                if (timkiem == "")
                {
                    //text tìm kiếm rỗng thì trả về ban đầu
                    load_DGV();
                    reset_Text();
                    dataGridView_UC_huyPhong.ClearSelection();
                }
                else
                {
                    SqlCommand tim = connection.CreateCommand();
                    tim.CommandText = "Select k.CCCD, k.Ho_Ten, d.Ma_Phong, d.So_Nguoi, d.Gia_Phong, d.Ngay_Den FROM KhachHang k JOIN DatPhong d ON k.CCCD = d.CCCD WHERE k.CCCD LIKE '%" + timkiem + "%' " + "OR d.Ma_Phong LIKE N'%" + timkiem + "%' " + "OR d.Ngay_Den LIKE '%" + timkiem + "%'" + "OR k.Ho_Ten LIKE N'%"+timkiem+"%'" + "OR d.So_Nguoi LIKE N'%" + timkiem + "%'" + "OR d.Gia_Phong LIKE N'%" + timkiem + "%'";
                    adapter.SelectCommand = tim;
                    DataTable tabTim = new DataTable();
                    adapter.Fill(tabTim);
                    dataGridView_UC_huyPhong.DataSource = tabTim;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có vấn đề! Hãy ReLoad lại hệ thống!" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}

