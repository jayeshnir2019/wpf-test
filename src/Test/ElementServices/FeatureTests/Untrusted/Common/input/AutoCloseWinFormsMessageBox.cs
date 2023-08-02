// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Avalon.Test.CoreUI.Trusted;
using Avalon.Test.CoreUI;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Avalon.Test.CoreUI.Common
{
    /// <summary>
    /// An automatically closing WinForms message box.
    /// </summary>
    /// <remarks>
    /// This message box is accessed through System.Windows.Forms (URT namespace).
    /// </remarks>
    public class AutoCloseWinFormsMessageBox : AutoCloseMessageBox
    {
        /// <summary>
        /// Construct an auto-closing message box.
        /// </summary>
        /// <param name="timeout">How long the message box appears, in milliseconds.</param>
        /// <param name="text">Message box text.</param>
        /// <param name="caption">Message box caption.</param>
        /// <remarks>
        /// After the timeout period expires, the message box is dismissed.
        /// </remarks>
        public AutoCloseWinFormsMessageBox(int timeout, string text, string caption): base()
        {
            this.Timeout = timeout;
            this.Text = text;
            this.Caption = caption;
        }

        /// <summary>
        /// Overrideable method to actually do the work to show the WinForms message box.
        /// </summary>
        /// <param name="text">Text to show in the message box.</param>
        /// <param name="caption">Caption for message box.</param>
        /// <returns>Message box result after the box is dismissed.</returns>
        protected override MessageBoxResult ShowCore(string text, string caption)
        {
            System.Windows.Forms.MessageBox.Show(text, caption);
            return MessageBoxResult.OK;
        }
    }

    /// <summary>
    /// An automatically closing WinForms message box that sends some input before closing.
    /// </summary>
    /// <remarks>
    /// Currently only keyboard input is accepted.
    /// </remarks>
    public class AutoCloseWinFormsSendInputMessageBox : AutoCloseWinFormsMessageBox
    {
        /// <summary>
        /// Construct an auto-closing message box.
        /// </summary>
        /// <param name="timeout">How long the message box appears, in milliseconds.</param>
        /// <param name="text">Message box text.</param>
        /// <param name="caption">Message box caption.</param>
        /// <param name="vk">Virtual key input.</param>
        /// <param name="pressed">Pressed or released?</param>
        /// <remarks>
        /// After the timeout period expires, the message box is dismissed. 
        /// Immediately before dismissal, input is sent to the system.
        /// </remarks>
        public AutoCloseWinFormsSendInputMessageBox(int timeout, string text, string caption, byte vk, bool pressed): base(timeout, text, caption)
        {
            _vk = vk;
            _pressed = pressed;
        }

        /// <summary>
        /// Safely kill a message box.
        /// </summary>
        /// <param name="hwnd">Window handle.</param>
        protected override void KillWindow(HandleRef hwnd)
        {
            // Just thunk some input into the system....
            Input.SendKeyboardInput(_vk, _pressed);

            // And continue with the window killing
            base.KillWindow(hwnd);
        }

        private byte _vk = 0;

        private bool _pressed = false;

    }

}
