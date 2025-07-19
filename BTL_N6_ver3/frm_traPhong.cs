using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BTL_N6_ver3
{
    public partial class frm_traPhong: Form
    {
        public frm_traPhong()
        {
            InitializeComponent();
        }

        string connectString = connectDb.connectString();
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();


        public void setTraphong(string phong, string maKH, string ten, string giaphong)
        {
            lbl_txt_phong.Text = phong;
            lbl_txt_maKH.Text = maKH;
            lbl_txt_SN.Text = ten;
            lbl_txt_giaphong.Text = giaphong;
            dateTimePicker_ngaytra.Text = DateTime.Today.ToString("yyy-MM-dd");
        }

        void reset_Text()
        {
            lbl_txt_phong.Text = "";
            lbl_txt_maKH.Text = "";
            lbl_txt_SN.Text = "";
            lbl_txt_giaphong.Text = "";
            dateTimePicker_ngaytra.Text = DateTime.Today.ToString("yyy-MM-dd");
        }

        private void btn_traphong_Click(object sender, EventArgs e)
        {
            try
            {
                string phong = lbl_txt_phong.Text.Trim();  // Danh sách phòng cần hủy, ví dụ: "101,102"
                string maKH = lbl_txt_maKH.Text.Trim(); // CCCD của khách hàng

                if (phong == "")
                {
                    MessageBox.Show("Hãy quay lại chọn 1 KHách hàng trước khi trả phòng?", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    DialogResult r = MessageBox.Show("Bạn muốn trả phòng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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


                        // Xóa phòng trong DatPhongDonLe
                        SqlCommand huyLe = connection.CreateCommand();
                        huyLe.CommandText = "DELETE FROM DatPhongDonLe WHERE Ma_Phong IN (" + string.Join(",", dsPhongCanXoa.Select(p => "'" + p + "'")) + ") AND CCCD = @cccd";
                        huyLe.Parameters.AddWithValue("@cccd", maKH);
                        huyLe.ExecuteNonQuery();

                        MessageBox.Show("Trả phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset_Text();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frm_traPhong_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectString);
            connection.Open();
        }

        private void frm_traPhong_FormClosing(object sender, FormClosingEventArgs e)
        {
          
            DialogResult r = MessageBox.Show("Quay lại trang chủ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r == DialogResult.No) 
            {
                e.Cancel = true;
            }
        }
    }
}
