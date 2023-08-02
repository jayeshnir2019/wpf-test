// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Avalon.Test.CoreUI.Trusted;
using Avalon.Test.CoreUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using Avalon.Test.CoreUI.Common;
using Avalon.Test.CoreUI.CoreInput.Common;
using Avalon.Test.CoreUI.CoreInput.Common.Controls;
using Microsoft.Test.Win32;
using Microsoft.Test.Discovery;
using Microsoft.Test;
using Microsoft.Test.Logging;

namespace Avalon.Test.CoreUI.CoreInput
{
    /// <summary>
    /// Verify MouseDown and MouseUp events fire on a mouse click.
    /// </summary>
    /// <description>
    /// This is part of a collection of unit tests for input events.
    /// </description>
    /// <author>Microsoft</author>
 
    [CoreTestsLoader(CoreTestsTestType.MethodBase)]
    public class MouseDownApp: TestApp
    {
        /// <summary>
        /// Launch our test.
        /// </summary>
        [TestCase("2",@"CoreInput\Mouse","HwndSource", @"Compile and Verify MouseDown and MouseUp events fire on a mouse click in HwndSource.")]
        [TestCase("0",@"CoreInput\Mouse","Browser", @"Compile and Verify MouseDown and MouseUp events fire on a mouse click in Browser.")]
        [TestCase("1",@"CoreInput\Mouse","Window", @"Compile and Verify MouseDown and MouseUp events fire on a mouse click in window.")]        
        public static void LaunchTestCompile() 
        {
            HostType hostType = (HostType)Enum.Parse(typeof(HostType), DriverState.DriverParameters["TestParameters"]);

            GenericCompileHostedCase.RunCase(
                "Avalon.Test.CoreUI.CoreInput", 
                "MouseDownApp",
                "Run", 
                hostType);
            
        }

        /// <summary>
        /// Launch our test.
        /// </summary>
        [TestCase("0",@"CoreInput\Mouse","HwndSource",  @"Verify MouseDown and MouseUp events fire on a mouse click in HwndSource.")]
        [TestCase("0",@"CoreInput\Mouse","Window",  @"Verify MouseDown and MouseUp events fire on a mouse click in window.")]        
        public static void LaunchTest() 
        {
            HostType hostType = (HostType)Enum.Parse(typeof(HostType),DriverState.DriverParameters["TestParameters"]);

            ExeStubContainerFramework exe = new ExeStubContainerFramework(hostType);
            exe.Run(new MouseDownApp(),"Run");
            
        }

        /// <summary>
        /// Setup the test.
        /// </summary>
        /// <param name="sender">App sending the callback.</param>
        /// <returns>Null object.</returns>
        public override object DoSetup(object sender) 
        {
            CoreLogger.LogStatus("Constructing tree ....");
            
            // Construct test element, add event handling
            _rootElement = new InstrPanel();
            _rootElement.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseButton);
            _rootElement.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseButton);

            // Put the test element on the screen
            DisplayMe(_rootElement, 10, 10, 100, 100);

            return null;
        }

        /// <summary>
        /// Identify test operations to run.
        /// </summary>
        /// <param name="hwnd">Window handle.</param>
        /// <returns>Array of test operations.</returns>
        protected override InputCallback[] GetTestOps(HandleRef hwnd) 
        {
            InputCallback[] ops = new InputCallback[] 
            {
                delegate
                {
                    MouseHelper.Click(_rootElement);
                }                
            };
            return ops;
        }

        /// <summary>
        /// Validate the test.
        /// </summary>
        /// <param name="arg">Not used.</param>
        /// <returns>Null object.</returns>
        public override object DoValidate(object arg) 
        {
            CoreLogger.LogStatus("Validating...");

            // Note: for this test we are concerned about whether both events fire once.
            // 1 down + 1 up = 2 events
            
            CoreLogger.LogStatus("Events found: "+_eventLog.Count);
            
            // expect non-negative event count
            int actual = (_eventLog.Count);
            int expected = 2;
            bool eventFound = (actual == expected);

            CoreLogger.LogStatus("Setting log result to " + eventFound);
            this.TestPassed = eventFound;
            
            CoreLogger.LogStatus("Validation complete!");
            
            return null;
        }

        /// <summary>
        /// Standard mouse event handler.
        /// </summary>
        /// <param name="sender">Source sending the event.</param>
        /// <param name="e">Event-specific arguments.</param>
        private void OnMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Set test flag
            _eventLog.Add(e);

            // Log some debugging data
            CoreLogger.LogStatus(" ["+e.RoutedEvent.Name+"]");
            Point pt = e.GetPosition(null);
            CoreLogger.LogStatus("   Hello from: " + pt.X+","+pt.Y);
            CoreLogger.LogStatus("   Btn="+e.ChangedButton.ToString()+",State="+e.ButtonState.ToString()+",ClickCount="+e.ClickCount);

            // Don't route this event any more.
            e.Handled = true;
        }

        /// <summary>
        /// Store record of our fired events.
        /// </summary>
        private List<MouseButtonEventArgs> _eventLog = new List<MouseButtonEventArgs>();
    }
}

