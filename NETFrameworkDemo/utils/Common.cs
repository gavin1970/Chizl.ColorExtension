using System.Drawing;
using System.Windows.Forms;

namespace NETFrameworkDemo.utils
{
    internal static class Common
    {
        internal static Color GetColor(IWin32Window owner, Color defColor)
        {
            var retVal = defColor;
            //load dialog
            var dlg = new ColorDialog();
            //set default to existing color
            dlg.Color = defColor;
            //show color picker
            dlg.ShowDialog(owner);
            //get color
            retVal = dlg.Color;
            //cleanup
            dlg.Dispose();

            return retVal;
        }
    }
}
