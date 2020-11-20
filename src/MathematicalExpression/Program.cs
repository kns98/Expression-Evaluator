//
// CODE MADE AVAILABLE UNDER GPL LICENSE
// COPYRIGHT 2019-2010
// CloudAda
//
//  Date    : Sep 2019
//  Comment : New App Created
//  Author  : Xuezhe Li
//
//  Date    : 
//  Comment : 
//  Author  : 
//

using System;
using System.Windows.Forms;

namespace MathematicalExpression
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SimpleDemo());
        }
    }
}