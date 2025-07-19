using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_N6_ver3
{
    //tạo lớp dùng để tắt usercontrol, hiển thị lại trang chủ
    public class closeUsercontrol
    {
        public void closeUC(UserControl uc)
        {
            TableLayoutPanel trangchu = uc.Parent as TableLayoutPanel; //gán tablePanel chứa uc thành trangchu kiểu tablepanel

            if(trangchu != null)
            {
                trangchu.Controls.Remove(uc);  //xóa uc khỏi trangchu
                uc.Dispose(); //giải phóng tài nguyên

                trangchu.Visible = true;   //hiển thị lại trang chủ
                trangchu.BackgroundImage = Properties.Resources._287459670_10216722679978116_316651025411003491_n;
            }
        }
            //tắt usercontrol trong trang chủ, tạo chỗ cho usercontrol mới
       
        public void closeUC_call_UC(TableLayoutPanel trangchu)
        {
            List<Control> UCinPanel = new List<Control>(); // tạo list lưu trữ control

            // Duyệt qua tất cả các control trong panel
            foreach (Control ctrl in trangchu.Controls)
            {
                if (ctrl is UserControl)    //nếu control đó là UC
                {
                    UCinPanel.Add(ctrl);
                }
            }

            foreach (Control i in UCinPanel)
            {
                trangchu.Controls.Remove(i);
                i.Dispose();
            }
        }
    }
}
