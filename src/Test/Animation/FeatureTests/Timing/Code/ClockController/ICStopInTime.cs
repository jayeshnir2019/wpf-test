// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Avalon Timing Verification Test Automation 
//   

/* 
 * Description: Verify the ClockController's Stop method, invoked via CurrentTimeInvalidated
 */

//Instructions:
//  1. Create a TimeManager
//  2. Create a Timeline with begin=0 and duration=10 and attach to Manager
//  3. Start the TimeManager at timeline = 0
//  4. Stop the Timeline in the CurrentTimeInvalidated event handler
//  5. Check the CurrentProgress, CurrentGlobalSpeed and CurrentState properties after each Tick

//Pass Verification:
//   The output of this test should match the expected output in ICStopInTimeExpect.txt

//Warnings:
//  Any changes made in the output should be reflected ICStopInTimeExpect.txt file

//Dependencies:
//  TestRuntime.dll, Timing\Common\GlobalClasses.cs

using System;
using System.Windows;
using System.Windows.Media.Animation;

using Microsoft.Test.Logging;
using Microsoft.Test.TestTypes;
using Microsoft.Test.Discovery;

namespace Microsoft.Test.Animation
{
    class ICStopInTime :ITimeBVT
    {
        ParallelTimeline    _parent;
        Clock               _tClock;
        int                 _currentTimeCount = 0;
        
        /*
         *  Function:   Test
         *  Arguments:  Framework
         */
        public override string Test()
        {
            TimeManagerInternal tManager = EstablishTimeManager(this);
            DEBUG.ASSERT(tManager != null, "Cannot create TimeManager" , " Created TimeManager ");
            
            // Create a TimeNode
            _parent = new ParallelTimeline();
            DEBUG.ASSERT(_parent != null, "Cannot create Timeline" , " Created Timeline " );
            _parent.BeginTime   = TimeSpan.FromMilliseconds(0);
            _parent.Duration    = new Duration(TimeSpan.FromMilliseconds(10));
            _parent.Name        = "Timeline";           
            
            // Create a Clock, connected to the Timeline.
            _tClock = _parent.CreateClock();     
            DEBUG.ASSERT(_tClock != null, "Cannot create Clock" , " Created Clock " );
            
            AttachCurrentTimeInvalidatedTC(_tClock);

            //Intialize output String
            outString = "";
            
            //Run the Timer         
            TimeGenericWrappers.EXECUTE( this, _tClock, tManager, 0, 12, 1, ref outString);
            
            return outString;
        }
        
        public override void OnProgress( Clock subject )
        {
            outString += "   CurrentProgress    = " + subject.CurrentProgress + "\n";
            outString += "   CurrentTime        = " + subject.CurrentTime + "\n";
            outString += "   CurrentState       = " + subject.CurrentState + "\n";
            outString += "   CurrentGlobalSpeed = " + subject.CurrentGlobalSpeed + "\n";
        }
        
        public override void OnCurrentTimeInvalidated(object subject, EventArgs args)
        {
            _currentTimeCount++;
            if (_currentTimeCount == 5)
            {
                _tClock.Controller.Stop();
            }
        }
    }
}