// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Automation;
using Microsoft.Windows.Test.Client.AppSec.BVT.ELCommon;
using Microsoft.Test.Win32;
using System.Windows.Controls;
using Microsoft.Test.Input;

namespace WindowTest
{
    /// <summary>
    /// 
    /// Test for SizeToContent.Width in code
    ///
    /// </summary>
    public partial class SizeToContent_Width_Code
    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        void OnClick(object sender, RoutedEventArgs e)
        {
            btn.Content = this.ActualWidth.ToString() + "x" + this.ActualHeight.ToString();
        }
        
        void OnContentRendered(object sender, EventArgs e)
        {
            // Validate Default Dimension
            SizeToContent expectedSTC = SizeToContent.Manual;               
            double expectedWindowWidth=200;
            double expectedWindowHeight=200;
            ValidateActualDimension(expectedSTC, expectedWindowWidth, expectedWindowHeight);

            double captionHeight = WindowValidator.GetCaptionHeight(this.Title);
            double borderLength = WindowValidator.GetBorderLength(this.Title);
            Logger.Status("borderLength=" + borderLength.ToString() + " CaptionHeight=" + captionHeight.ToString());
            
            expectedSTC = SizeToContent.Width;
            expectedWindowWidth=btn.Width+borderLength*2;
            expectedWindowHeight=this.Height;
            this.SizeToContent = expectedSTC;
            ValidateActualDimension(expectedSTC, expectedWindowWidth, expectedWindowHeight);
            Win32ValidateContentDimension(btn.Width, expectedWindowHeight-borderLength-captionHeight);

            expectedSTC = SizeToContent.WidthAndHeight;
            btn.Width = 300;
            btn.Height = 300;
            this.SizeToContent = expectedSTC;
            expectedWindowWidth = btn.Width + borderLength * 2;
            expectedWindowHeight = btn.Height + borderLength + captionHeight;
            ValidateActualDimension(expectedSTC, expectedWindowWidth, expectedWindowHeight);
            Win32ValidateContentDimension(btn.Width, btn.Height);
            
            TestHelper.Current.TestCleanup();

        }

        void ValidateActualDimension(SizeToContent expectedSTC, double expectedWidth, double expectedHeight)
        {
            Logger.Status("[EXPECTED] Window.SizeToContent = " + expectedSTC.ToString());

            // Validate Property Value
            if (this.SizeToContent != expectedSTC)
            {
                Logger.LogFail("[ACTUAL] Window.SizeToContent = " + this.SizeToContent.ToString());
            }
            else
            {
                Logger.Status("[VALIDATION PASSED] SizeToContent = " + expectedSTC.ToString());
            }

            Logger.Status("[EXPECTED] Window.ActualWidth = " + expectedWidth.ToString());
            if (!TestUtil.IsEqual(this.ActualWidth, expectedWidth))
            {
                Logger.LogFail("[ACTUAL] Window.ActualWidth = " + this.ActualWidth.ToString());
            }
            else
            {
                Logger.Status("[VALIDATION PASSED] Window.ActualWidth = " + this.ActualWidth.ToString());
            }

            Logger.Status("[EXPECTED] Window.ActualHeight = " + expectedHeight.ToString());
            if (!TestUtil.IsEqual(this.ActualHeight, expectedHeight))
            {
                Logger.LogFail("[ACTUAL] Window.ActualHeight = " + this.ActualHeight.ToString());
            }
            else
            {
                Logger.Status("[VALIDATION PASSED] Window.ActualHeight = " + this.ActualHeight.ToString());
            }                
        }
        
        void Win32ValidateContentDimension(double expectedContentWidth, double expectedContentHeight)
        {
            // win32 width validation
            Logger.Status("[WIN32 VALIDATION]");
            if (!WindowValidator.ValidateSizeToContent(this.Title, expectedContentWidth, expectedContentHeight))
            {
                Logger.LogFail("Win32 Validation failed!");
            }
            else
            {
                Logger.Status("[VALIDATION PASSED] Win32 Validation Passed");
            }                
        }
    }

}
