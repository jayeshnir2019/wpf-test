// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

using Avalon.Test.CoreUI;
using Avalon.Test.CoreUI.Events;
using Avalon.Test.CoreUI.Trusted;

using Microsoft.Test;
using Microsoft.Test.Discovery;
using Microsoft.Test.Logging;
using Microsoft.Test.TestTypes;

namespace Avalon.Test.CoreUI.Events
{
    /// <summary>
    /// Tests Attaching Bubble EventHandler to Configuration Fig 46 to test Bubbling
    /// <para />
    /// This is a BVT scenario for attaching bubble event to the follow tree
    /// star=visual
    /// line=model
    /// cc1   cc2
    ///  \     *
    ///   \   *
    ///    cc3
    /// </summary>
    /// <remarks>
    /// <para />
    /// Area: Events\Bubble
    /// <para />
    /// <para />
    /// <para />
    /// FileName:  BubblingEventRoutingFrameworkAvalonFig46.cs
    /// <para />
    /// <ol>Scenarios covered:
    /// <li>Fetch RoutedEvent for bubble event</li>
    /// <li>Add event handlers for bubble event</li>
    /// <li>Raise the bubble event</li>
    /// <li>Handlers are called in the correct order</li>
    /// </ol>
    /// </remarks>
    /// <seealso cref="TestCaseType"/>
    [Test(0, "Events.Bubble", "BubblingEventRoutingFrameworkAvalonFig46")]
    [Test(0, @"Events\Bubble")]
    public class BubblingEventRoutingFrameworkAvalonFig46 : EventHelper
    {
        #region Constructor
        public BubblingEventRoutingFrameworkAvalonFig46()
        {
            RunSteps += new TestStep(StartTest);
        }
        #endregion


        #region Test Steps
        /// <summary>
        /// Entry Method for the test case
        /// </summary>
        TestResult StartTest()
        {
            CoreLogger.LogStatus ("This is a BVT scenario for attaching bubble event to an CustomControl with VI parent of CustomControl and Model parent of ce");
            CoreLogger.LogStatus ("Tests attach bubbling event");

            // Local Varaibles
            // Create three CustomAvalonControl to build a tree later and test Events
            CustomControl cc1 = null;
            CustomControl cc2 = null;
            CustomControl cc3 = null;

            // Create a routedEvent to get EventManager.GetRoutedEventFromName
            RoutedEvent routedEvent;

            // Create size of three object array to contain three CustomAvalonControl
            RouteTarget[] targets = new RouteTarget[3];

            // Create a CustomRoutedEventArgs for later RaiseEvent use
            CustomRoutedEventArgs args;

            // Create Dispatcher        
            Dispatcher context = MainDispatcher;

            // Enter Dispatcher
            
            using (CoreLogger.AutoStatus ("Creating custom controls"))
            {
                cc1 = new CustomControl();
                cc2 = new CustomControl();
                cc3 = new CustomControl();
            }

            using (CoreLogger.AutoStatus ("Construct tree"))
            {
                cc1.AppendChild (cc3);
                cc2.AppendModelChild (cc3);
            }

            using (CoreLogger.AutoStatus ("Fetch RoutedEvent for bubble event"))
            {
                routedEvent = BubblingEventRoutingFrameworkAvalonFig42.RoutedEvent1;
            }

            using (CoreLogger.AutoStatus ("Add event handlers for bubble event"))
            {
                cc1.AddHandler (routedEvent, new CustomRoutedEventHandler (OnRoutedEvent2));
                cc2.AddHandler (routedEvent, new CustomRoutedEventHandler (OnRoutedEvent1));
                cc3.AddHandler (routedEvent, new CustomRoutedEventHandler (OnRoutedEvent3));
            }

            using (CoreLogger.AutoStatus ("Raise the bubble event on control3"))
            {
                targets[0].Source = cc3;
                targets[0].Sender = cc3;
                targets[1].Source = cc1;
                targets[1].Sender = cc1;
                targets[2].Source = cc2;
                targets[2].Sender = cc2;
                args = new CustomRoutedEventArgs (routedEvent, targets);
                cc3.RaiseEvent (args);
            }

            using (CoreLogger.AutoStatus ("Validation for event"))
            {
                if (args.HandlersCalledCount != 2)
                {
                    throw new Microsoft.Test.TestValidationException ("Incorrect HandlersCalledCount");
                }
            }

            //Any test failures will be caught by throwing an Exception during verification.
            return TestResult.Pass;

            // Exit Dispatcher
        }
        #endregion


        #region Public Members
        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent1 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent1");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 2);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent2 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent2");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 1);
        }

        /// <summary>
        /// Handler called
        /// </summary>
        /// <param name="sender">Pass the object to it</param>
        /// <param name="args">Pass the CustomRoutedEventArgs to it</param>
        public void OnRoutedEvent3 (object sender, CustomRoutedEventArgs args)
        {
            CoreLogger.LogStatus ("OnRoutedEvent3");

            // Verify sender, Source, and handler count.
            this.VerifyRoutedEvent (sender, args, 0);
        }
        #endregion
    }
}


