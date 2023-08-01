// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows;

using Avalon.Test.CoreUI;
using Avalon.Test.CoreUI.Common;
using Avalon.Test.CoreUI.Trusted;

using Microsoft.Test.TestTypes;

namespace Avalon.Test.CoreUI.Resources
{
    /// <summary>Base class of all *Helper classes.</summary>
    /// <remarks>
    /// The Helper classes are formed in a derivation chain since 
    /// multiple inheritance isn't possible in C#.  The hierarchy 
    /// looks like this:
    /// <para>
    /// + TestHelper
    ///  + GenericTestCase
    ///   
    /// </para> 
    /// </remarks>
    public class TestHelper : AvalonTest
    {
        /// <summary>
        /// Checks the condition, and throws a <see cref="Microsoft.Test.TestValidationException">Microsoft.Test.TestValidationException</see>
        /// if the condition == false.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="comment">comment to add if the test fails</param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public void CheckResults(bool condition, string comment, string expected, string actual)
        {
            using(CoreLogger.AutoStatus(comment))
            {
                if(condition == false)
                {
                    throw new Microsoft.Test.TestValidationException(comment + "; EXPECTED:'" + expected + "'; ACTUAL:'" + actual + "'");
                }
                else
                {
                    CoreLogger.LogStatus("Result is true");
                }
            }
        }
               

        /// <summary>
        /// Checks the condition, and throws a <see cref="Microsoft.Test.TestValidationException">Microsoft.Test.TestValidationException</see>
        /// if the condition == false.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="comment">comment to add if the test fails</param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public void CheckResults(bool condition, string comment, int expected, int actual)
        {
            CheckResults(condition, comment, expected.ToString(), actual.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// Display a Element on a Window.
        /// </summary>
        public void DisplayObject(object o)
        {
            if (_surface == null)
            {
                _surface = new SurfaceFramework("Window", 0,0,400,400);
            }   
            _surface.DisplayObject(o);
        }

        /// <summary>
        /// ShutDown the App.
        /// </summary>
        public void AppShutDown(Application app)
        {
            app.Shutdown();
        }

        SurfaceFramework _surface = null;
    }
    
}
