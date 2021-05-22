using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm
{
    public partial class SquareControl : UserControl
    {
        public int Id { get; private set; }
        public SquareControl(int id)
        {
            InitializeComponent();

            this.Id = id;
        }
    }
}
