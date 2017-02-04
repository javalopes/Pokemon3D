using System;
using System.Windows.Forms;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.GameCore;

namespace Pokemon3D
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                using (var game = new GameController())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                GameLogger.Instance.Log(ex);
                MessageBox.Show("An unhandled exception occurred. Send the log file to the developers.",
                    "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}