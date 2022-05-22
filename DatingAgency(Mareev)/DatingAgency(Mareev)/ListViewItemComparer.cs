using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatingAgency_Mareev_
{
    class ListViewItemComparer : IComparer
    {
        private int col;
        public static int SortColumn { get; set; }
        public static SortOrder Order { get; set; }
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public string FullCompareString(string s)
        {
            string Res = s;
            while (Res.Length < 10) Res = "0" + Res;
            return Res;
        }
        public int Compare(object x, object y)
        {

            string s1 = FullCompareString(((ListViewItem)x).SubItems[col].Text);
            string s2 = FullCompareString(((ListViewItem)y).SubItems[col].Text);
            if (Order == SortOrder.Ascending)
                return String.Compare(s1, s2);
            else
                return String.Compare(s2, s1);
        }
    }
}
