using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm.Helpers
{
    public static class ControlsFactory
    {
        public static ContextMenuStrip CreateComboPopupMenu(EventHandler handler)
        {
            var components = new Container();
            ContextMenuStrip popupMenu = new ContextMenuStrip(components);
            popupMenu.SuspendLayout();

            ToolStripMenuItem deleteToolStripMenuItem = new ToolStripMenuItem();

            // 
            // popupMenu
            // 
            popupMenu.Items.AddRange(new ToolStripItem[] {
            deleteToolStripMenuItem });
            popupMenu.Name = "popupMenu";
            popupMenu.Size = new System.Drawing.Size(181, 48);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += handler;

            popupMenu.ResumeLayout(false);

            return popupMenu;
        }
    }
}
