using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace BTL_N6_ver3
{
    public partial class frm_dangnhap: Form
    {
        string conn = connectDb.connectString();
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public frm_dangnhap()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void frm_dangnhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        void reset()
        {
            txtMk.Text = txtTk.Text = "";
        }


        //tạo form quên rồi gọi
        private void btnQuen_Click(object sender, EventArgs e)
        {
            frm_quen quen = new frm_quen();
            this.Hide();
            quen.ShowDialog();
            this.Show();
            reset();
        }

        //tạo form đăng ký rồi gọi
        private void btndangky_Click(object sender, EventArgs e)
        {
            this.Hide();
            frm_dangky dki = new frm_dangky();
            dki.ShowDialog();
            this.Show();
            reset();
        }

        //sự kiện button đăng nhập
        private void btndangnhap_Click(object sender, EventArgs e)
        {
            string tk = txtTk.Text.Trim();
            string mk = txtMk.Text.Trim();
            string tennv;
            if (tk == ""&& mk == "")
            {
                MessageBox.Show("Bạn chưa có tài khoản? Chọn ĐĂNG KÝ!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            if (tk==""||mk=="")
            {
                MessageBox.Show("Hãy nhập đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }else
            {
                command = connection.CreateCommand();
                string query = "Select * from DangNhap where TenTaiKhoan = '" + tk + "' AND MatKhau = '" + mk + "' ";
                command.CommandText = query;
                adapter.SelectCommand = command;
                table.Clear();
                adapter.Fill(table);

                if (table.Rows.Count == 1)
                {
                    tennv = table.Rows[0]["HoTen"].ToString();


                    MessageBox.Show("Đăng nhập thành công!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTk.Text = txtMk.Text = "";
                    this.Hide();
                    frm_trangchu trangchu = new frm_trangchu();
                    trangchu.settenNV(tennv);
                    trangchu.ShowDialog();
                    this.Show();
                }else
                {
                    MessageBox.Show("Tài khoản không tồn tại!. Đăng ký nếu chưa có tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        //hiện thỉ con trỏ bàn tay khi di chuột vào button
        private void frm_dangnhap_Load(object sender, EventArgs e)
        {
            //kết nối SQL
            connection = new SqlConnection(conn);
            connection.Open();


            //hiển thị bàn tay khi di chuột
            btndangnhap.Cursor = Cursors.Hand;
            btndangky.Cursor = Cursors.Hand;
            btnQuen.Cursor = Cursors.Hand;
            btnThoat.Cursor = Cursors.Hand;
        }

        //checkbox hiển thị mật khẩu
        private void checkBox_showPass_CheckedChanged(object sender, EventArgs e)
        {
            txtMk.UseSystemPasswordChar = checkBox_showPass.Checked;
        }
    }
}

