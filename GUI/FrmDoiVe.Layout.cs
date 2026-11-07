using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmDoiVe
    {
        private bool layingOut;
        private int Px(int value)
        {
            float scale=Math.Max(DeviceDpi/96f,Font.SizeInPoints/9.5F);
            return (int)Math.Round(value*scale);
        }
        private Label MakeLabel(string text,float size=9.5F,bool bold=false)
        {
            return new Label { Text=text, Font=new Font("Segoe UI",size,bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor=Color.FromArgb(15,23,42), AutoSize=false, UseMnemonic=false };
        }
        private Guna2Button MakeButton(string text,bool primary)
        {
            var button=new Guna2Button { Text=text,Font=new Font("Segoe UI",9.5F,FontStyle.Bold),BorderRadius=6,
                FillColor=primary ? Color.FromArgb(37,99,235) : Color.White,
                ForeColor=primary ? Color.White : Color.FromArgb(51,65,85),Cursor=Cursors.Hand,
                BorderColor=Color.FromArgb(203,213,225),BorderThickness=primary ? 0 : 1,AccessibleName=text };
            button.HoverState.FillColor=primary ? Color.FromArgb(29,78,216) : Color.FromArgb(241,245,249);
            button.DisabledState.FillColor=Color.FromArgb(226,232,240);
            button.DisabledState.ForeColor=Color.FromArgb(71,85,105);
            button.FocusedColor=Color.FromArgb(13,148,136);
            return button;
        }
        private Guna2ComboBox MakeCombo(string name)
        {
            return new Guna2ComboBox { Font=new Font("Segoe UI",9.5F),BorderRadius=6,
                ForeColor=Color.FromArgb(15,23,42),BorderColor=Color.FromArgb(203,213,225),
                DropDownStyle=ComboBoxStyle.DropDownList,AccessibleName=name };
        }
        private int LabelAt(Label label,int x,int y,int width,int gap)
        {
            int height=TextRenderer.MeasureText(label.Text,label.Font,new Size(Math.Max(1,width),int.MaxValue),
                TextFormatFlags.WordBreak|TextFormatFlags.NoPrefix).Height+Px(4);
            label.SetBounds(x,y,Math.Max(1,width),Math.Max(label.Font.Height,height));
            return label.Bottom+gap;
        }
        private void LayoutExchange()
        {
            if(layingOut || IsDisposed || Disposing || header==null) return;
            layingOut=true;
            try {
                int pad=Px(20),gap=Px(16),width=Math.Max(1,ClientSize.Width-pad*2);
                header.SetBounds(pad,pad,width,100);
                title.SetBounds(0,0,width,title.Font.Height+Px(8));
                customerDetails.SetBounds(Math.Max(0,width-Px(175)),0,Px(175),Px(36));
                int y=LabelAt(customer,0,title.Bottom+Px(8),width,Px(6));
                y=LabelAt(eventName,0,y,width,Px(12));
                header.Height=y;
                actions.SetBounds(pad,ClientSize.Height-pad-Px(48),width,Px(48));
                btnXacNhan.SetBounds(Math.Max(0,width-Px(180)),0,Px(180),Px(44));
                btnDong.SetBounds(Math.Max(0,btnXacNhan.Left-Px(112)),0,Px(100),Px(44));
                body.SetBounds(pad,header.Bottom,width,Math.Max(Px(40),actions.Top-header.Bottom-gap));
                bool stacked=width<Px(880);
                int summaryWidth=stacked ? Math.Max(1,body.ClientSize.Width-SystemInformation.VerticalScrollBarWidth) : Px(320);
                int seatWidth=stacked ? summaryWidth : width-summaryWidth-gap;
                int seatHeight=stacked ? Px(450) : body.ClientSize.Height;
                seatCard.SetBounds(0,0,seatWidth,seatHeight);
                summaryCard.SetBounds(stacked ? 0 : seatCard.Right+gap,stacked ? seatCard.Bottom+gap : 0,
                    summaryWidth,stacked ? Px(470) : body.ClientSize.Height);
                LayoutSeatCard();
                if (stacked) summaryCard.Top=seatCard.Bottom+gap;
                LayoutSummary();
            } finally { layingOut=false; }
        }
        private void LayoutSeatCard()
        {
            int pad=Px(20),gap=Px(12),width=Math.Max(1,seatCard.Width-pad*2),height=Px(40);
            int y=LabelAt(showLabel,pad,pad,width,Px(8));
            cboSuatMoi.ItemHeight=Math.Max(1,height-6); cboKhuVucMoi.ItemHeight=Math.Max(1,height-6);
            cboSuatMoi.SetBounds(pad,y,width,height);
            y=LabelAt(location,pad,cboSuatMoi.Bottom+Px(8),width,Px(16));
            int zoneWidth=Math.Min(Px(195),width/2);
            searchLabel.SetBounds(pad,y,width-zoneWidth-gap,Px(24));
            zoneLabel.SetBounds(pad+width-zoneWidth,y,zoneWidth,Px(24));
            y+=Px(28);
            txtTimGhe.SetBounds(pad,y,Math.Max(1,width-zoneWidth-gap),height);
            cboKhuVucMoi.SetBounds(pad+width-zoneWidth,y,zoneWidth,height);
            y+=height+Px(16);
            // Keep the grid and its footer apart when DPI or long text reduces the viewport.
            seatCard.Height=Math.Max(seatCard.Height,y+Px(150));
            count.SetBounds(pad,seatCard.Height-pad-Px(24),width,Px(24));
            dgvGheTrong.SetBounds(pad,y,width,Math.Max(Px(60),count.Top-y-Px(8)));
            dgvGheTrong.ColumnHeadersHeight=Px(38);
            int rowHeight=Px(52);
            if(dgvGheTrong.RowTemplate.Height!=rowHeight) {
                dgvGheTrong.RowTemplate.Height=rowHeight;
                foreach(DataGridViewRow row in dgvGheTrong.Rows) row.Height=rowHeight;
            }
            stateLabel.SetBounds(pad,y+Px(20),width,Math.Max(Px(50),dgvGheTrong.Height-Px(80)));
            stateLabel.TextAlign=ContentAlignment.MiddleCenter;
            retry.SetBounds(pad+(width-Px(110))/2,stateLabel.Bottom,Px(110),Px(40));
        }
        private void LayoutSummary()
        {
            if(summaryCard==null || IsDisposed || Disposing) return;
            int pad=Px(20);
            // Reserve the scrollbar gutter so wrapping cannot introduce horizontal scrolling.
            summaryContent.Width=Math.Max(1,summaryCard.ClientSize.Width-SystemInformation.VerticalScrollBarWidth);
            int width=Math.Max(1,summaryContent.Width-pad*2);
            int y=LabelAt(oldHeading,pad,pad,width,Px(12));
            y=LabelAt(oldTicket,pad,y,width,Px(24));
            y=LabelAt(newHeading,pad,y,width,Px(12));
            int newTop=y;
            y=LabelAt(newTicket,pad,y,width,Px(24));
            y=Math.Max(y,newTop+Px(90));
            y=LabelAt(moneyHeading,pad,y,width,Px(12));
            y=LabelAt(difference,pad,y,width,Px(16));
            y=LabelAt(methodLabel,pad,y,width,Px(8));
            cboPhuongThuc.ItemHeight=Math.Max(1,Px(40)-6);
            cboPhuongThuc.SetBounds(pad,y,width,Px(40));
            summaryContent.Height=cboPhuongThuc.Bottom+pad;
        }
    }
}
