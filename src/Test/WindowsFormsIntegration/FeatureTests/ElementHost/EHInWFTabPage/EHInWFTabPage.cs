// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Forms;

using WFCTestLib.Util;
using WFCTestLib.Log;
using ReflectTools;
using MS.Internal.Mita.Foundation;
using MS.Internal.Mita.Foundation.Controls;
using System.Windows.Forms.Integration;


// Testcase:    EHInWFTabPage
// Description: We need to verify that EH and Avalon contols work in an WinForm TabPage control.
public class EHInWFTabPage : ReflectBase {

    public EHInWFTabPage(string[] args) : base(args) { }

    UIObject _uiApp = null;
    string _buttonClickResult;
    private System.Windows.Forms.TabControl _tabControl1;
    private System.Windows.Forms.TabPage _tabPage1;
    private System.Windows.Forms.TabPage _tabPage2;
    private System.Windows.Forms.Panel _panel1;
    private System.Windows.Forms.TabPage _tabPage3;
    private System.Windows.Forms.TabPage _tabPage4;
    private System.Windows.Forms.Panel _panel2;
    private System.Windows.Forms.Panel _panel3;
    private System.Windows.Forms.Panel _panel4;
    private System.Windows.Forms.Button _button1;
    private ElementHost _host1;
    private ElementHost _host2;
    private ElementHost _host3;
    private System.Windows.Controls.TextBox _av_tb1;
    private System.Windows.Controls.TextBox _av_tb2;
    private System.Windows.Controls.Button _av_btn;

    #region Testcase setup
    protected override void InitTest(TParams p)
    {
        base.InitTest(p);
        UseMita = true;
        this.Text = "EHInWFTabPageTest";
        InitializeForm();   //call the code generated by the WinForms designer
    }

    void av_btn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _buttonClickResult = "AV Button clicked";
    }

    void button1_Click(object sender, EventArgs e)
    {
        _buttonClickResult = "WF Button clicked";
    }
    protected override bool BeforeScenario(TParams p, System.Reflection.MethodInfo scenario)
    {
        _uiApp = UIObject.Root.Children.Find(UICondition.CreateFromName("EHInWFTabPageTest"));
        _buttonClickResult = "";
        return base.BeforeScenario(p, scenario);
    }

    #endregion

    //==========================================
    // Scenarios
    //==========================================
    #region Scenarios
    [Scenario("2 tabs, 1 with a EH & Avalon Button, 1 with WF button.  Verify that switching tabs show the correct button (switch back and forth more than once).")]
    public ScenarioResult Scenario1(TParams p) {
        Tab TabControl1;
        TabItem Tab3;
        TabItem Tab4;
        ScenarioResult sr = new ScenarioResult();

        TabControl1 = new Tab(_uiApp.Descendants.Find(UICondition.CreateFromId("tabControl1")));
        Tab3 = TabControl1.Items[2];
        Tab4 = TabControl1.Items[3];

        //try to find the buttons on the 3rd and 4th tabs

        ClickTabWithButton(Tab3, "button1");
        //ensure the click event fired - it will set the 'buttonClickResult' text
        sr.IncCounters(_buttonClickResult == "WF Button clicked", "Could not find WF button on Tab Control Page", p.log);

        ClickTabWithButton(Tab4, "AVButton");
        //ensure the click event fired - it will set the 'buttonClickResult' text
        sr.IncCounters(_buttonClickResult == "AV Button clicked", "Could not find AV button on Tab Control Page", p.log);

        ClickTabWithButton(Tab3, "button1");
        //ensure the click event fired - it will set the 'buttonClickResult' text
        sr.IncCounters(_buttonClickResult == "WF Button clicked", "Could not find WF button on Tab Control Page", p.log);

        ClickTabWithButton(Tab4, "AVButton");
        //ensure the click event fired - it will set the 'buttonClickResult' text
        sr.IncCounters(_buttonClickResult == "AV Button clicked", "Could not find AV button on Tab Control Page", p.log);

        ClickTabWithButton(Tab3, "button1");
        //ensure the click event fired - it will set the 'buttonClickResult' text
        sr.IncCounters(_buttonClickResult == "WF Button clicked", "Could not find WF button on Tab Control Page", p.log);

        ClickTabWithButton(Tab4, "AVButton");
        //ensure the click event fired - it will set the 'buttonClickResult' text
        sr.IncCounters(_buttonClickResult == "AV Button clicked", "Could not find AV button on Tab Control Page", p.log);

        return sr;
    }

    void ClickTabWithButton(TabItem tabToClick, string buttonName)
    {
        tabToClick.Click();
        Utilities.SleepDoEvents(10);
        UIObject myButton = tabToClick.Descendants.Find(UICondition.CreateFromId(buttonName));
        Mouse.Instance.MovePointerToClickPoint(myButton);
        Mouse.Instance.Click(MS.Internal.Mita.Foundation.MouseButtons.Primary);
        Utilities.SleepDoEvents(10);
    }

    [Scenario("2 tabs, both with a EH & EH Textbox.  Verify that text is preserved between tabs. (switch back and forth more than once).")]
    public ScenarioResult Scenario2(TParams p) {
        Tab TabControl1;
        TabItem Tab1;
        TabItem Tab2;
        string txt;
        ScenarioResult sr = new ScenarioResult();

        TabControl1 = new Tab(_uiApp.Descendants.Find(UICondition.CreateFromId("tabControl1")));
        Tab1 = TabControl1.Items[0];
        Tab2 = TabControl1.Items[1];

        //try to find the text on the 1st and 2nd tabs            
        txt = ClickTabWithTextBox(Tab1, "avTextbox1");
        sr.IncCounters(txt == "AV TextBox inside EH on first tab.", "Did not find correct text on Tab 1", p.log);

        txt = ClickTabWithTextBox(Tab2, "avTextbox2");
        sr.IncCounters(txt == "AV TextBox inside EH on second tab.", "Did not find correct text on Tab 2", p.log);

        //we found the text ok - now do it again
        txt = ClickTabWithTextBox(Tab1, "avTextbox1");
        sr.IncCounters(txt == "AV TextBox inside EH on first tab.", "Did not find correct text on Tab 1", p.log);

        txt = ClickTabWithTextBox(Tab2, "avTextbox2");
        sr.IncCounters(txt == "AV TextBox inside EH on second tab.", "Did not find correct text on Tab 2", p.log);

        //we found the text ok - now do it one last time
        txt = ClickTabWithTextBox(Tab1, "avTextbox1");
        sr.IncCounters(txt == "AV TextBox inside EH on first tab.", "Did not find correct text on Tab 1", p.log);

        txt = ClickTabWithTextBox(Tab2, "avTextbox2");
        sr.IncCounters(txt == "AV TextBox inside EH on second tab.", "Did not find correct text on Tab 2", p.log);

        return sr;
    }

    string ClickTabWithTextBox(TabItem tabToClick, string textboxName)
    {
        tabToClick.Click();
        Utilities.SleepDoEvents(10);
        UIObject myTextbox = tabToClick.Descendants.Find(UICondition.CreateFromId(textboxName));
        Edit myEdit = new Edit(myTextbox);
        return myEdit.DocumentRange.GetText(-1);
    }


    #endregion

    void InitializeForm()
    {
        //note: this code was borrowed from the designer code in a standalone WinForms app
        this.ClientSize = new System.Drawing.Size(410, 423);
        _tabControl1 = new System.Windows.Forms.TabControl();

        _tabPage1 = new System.Windows.Forms.TabPage();
        _tabPage2 = new System.Windows.Forms.TabPage();
        _tabPage3 = new System.Windows.Forms.TabPage();
        _tabPage4 = new System.Windows.Forms.TabPage();
        
        _panel1 = new System.Windows.Forms.Panel();
        _panel2 = new System.Windows.Forms.Panel();
        _panel3 = new System.Windows.Forms.Panel();
        _panel4 = new System.Windows.Forms.Panel();
        
        _button1 = new System.Windows.Forms.Button();
        
        _tabControl1.Controls.Add(_tabPage1);
        _tabControl1.Controls.Add(_tabPage2);
        _tabControl1.Controls.Add(_tabPage3);
        _tabControl1.Controls.Add(_tabPage4);

        _tabControl1.Location = new System.Drawing.Point(26, 13);
        _tabControl1.Name = "tabControl1";
        _tabControl1.SelectedIndex = 0;
        _tabControl1.Size = new System.Drawing.Size(355, 389);
        _tabControl1.TabIndex = 0;
        
        _tabPage1.Controls.Add(_panel1);
        _tabPage1.Location = new System.Drawing.Point(4, 22);
        _tabPage1.Name = "tabPage1";
        _tabPage1.Padding = new System.Windows.Forms.Padding(3);
        _tabPage1.Size = new System.Drawing.Size(347, 363);
        _tabPage1.TabIndex = 0;
        _tabPage1.Text = "tabPage1";
        _tabPage1.UseVisualStyleBackColor = true;
        
        _tabPage2.Controls.Add(_panel2);
        _tabPage2.Location = new System.Drawing.Point(4, 22);
        _tabPage2.Name = "tabPage2";
        _tabPage2.Padding = new System.Windows.Forms.Padding(3);
        _tabPage2.Size = new System.Drawing.Size(347, 363);
        _tabPage2.TabIndex = 1;
        _tabPage2.Text = "tabPage2";
        _tabPage2.UseVisualStyleBackColor = true;
        
        _tabPage3.Controls.Add(_panel3);
        _tabPage3.Location = new System.Drawing.Point(4, 22);
        _tabPage3.Name = "tabPage3";
        _tabPage3.Size = new System.Drawing.Size(347, 363);
        _tabPage3.TabIndex = 2;
        _tabPage3.Text = "tabPage3";
        _tabPage3.UseVisualStyleBackColor = true;
        
        _tabPage4.Controls.Add(_panel4);
        _tabPage4.Location = new System.Drawing.Point(4, 22);
        _tabPage4.Name = "tabPage4";
        _tabPage4.Size = new System.Drawing.Size(347, 363);
        _tabPage4.TabIndex = 3;
        _tabPage4.Text = "tabPage4";
        _tabPage4.UseVisualStyleBackColor = true;

        _panel1.Location = new System.Drawing.Point(24, 25);
        _panel1.Name = "panel1";
        _panel1.Size = new System.Drawing.Size(297, 311);
        _panel1.TabIndex = 0;

        _panel2.Location = new System.Drawing.Point(25, 26);
        _panel2.Name = "panel2";
        _panel2.Size = new System.Drawing.Size(297, 311);
        _panel2.TabIndex = 1;
        
        _panel3.Controls.Add(_button1);
        _panel3.Location = new System.Drawing.Point(25, 26);
        _panel3.Name = "panel3";
        _panel3.Size = new System.Drawing.Size(297, 311);
        _panel3.TabIndex = 1;
        
        _panel4.Location = new System.Drawing.Point(25, 26);
        _panel4.Name = "panel4";
        _panel4.Size = new System.Drawing.Size(297, 311);
        _panel4.TabIndex = 1;
        
        _button1.Dock = System.Windows.Forms.DockStyle.Fill;
        _button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        _button1.Location = new System.Drawing.Point(0, 0);
        _button1.Name = "button1";
        _button1.Size = new System.Drawing.Size(297, 311);
        _button1.TabIndex = 0;
        _button1.Text = "WF Button";
        _button1.UseVisualStyleBackColor = true;
        _button1.Click += new EventHandler(button1_Click);
        
        _av_tb1 = new System.Windows.Controls.TextBox();
        _av_tb1.Text = string.Empty;
        _av_tb1.Width = 300;
        _av_tb1.Height = 200;
        _av_tb1.TextWrapping = System.Windows.TextWrapping.Wrap;
        _av_tb1.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
        _av_tb1.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
        _av_tb1.Text = "AV TextBox inside EH on first tab.";
        _av_tb1.Name = "avTextbox1";
        _av_tb1.AcceptsReturn = true;
        _av_tb1.MaxLength = 32760;

        _host1 = new ElementHost();
        _host1.Child = _av_tb1;
        _host1.Dock = DockStyle.Fill;

        _av_tb2 = new System.Windows.Controls.TextBox();
        _av_tb2.Text = string.Empty;
        _av_tb2.Width = 300;
        _av_tb2.Height = 200;
        _av_tb2.TextWrapping = System.Windows.TextWrapping.Wrap;
        _av_tb2.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
        _av_tb2.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
        _av_tb2.Text = "AV TextBox inside EH on second tab.";
        _av_tb2.Name = "avTextbox2";
        _av_tb2.AcceptsReturn = true;
        _av_tb2.MaxLength = 32760;

        _host2 = new ElementHost();
        _host2.Child = _av_tb2;
        _host2.Dock = DockStyle.Fill;

        _av_btn = new System.Windows.Controls.Button();
        _av_btn.Content = "AV Button";
        _av_btn.Name = "AVButton";
        _av_btn.Click += new System.Windows.RoutedEventHandler(av_btn_Click);

        _host3 = new ElementHost();
        _host3.Child = _av_btn;
        _host3.Dock = DockStyle.Fill;

        _panel1.Controls.Add(_host1);
        _panel2.Controls.Add(_host2);
        _panel4.Controls.Add(_host3);

        this.Controls.Add(this._tabControl1);
    }
}

// Keep these in sync by running the testcase locally through the driver whenever
// you add, remove, or rename scenarios.
//
// [Scenarios]
//@ 2 tabs, 1 with a EH &amp; Avalon Button, 1 with WF button.  Verify that switching tabs show the correct button (switch back and forth more than once).
//@ 2 tabs, both with a EH &amp; EH Textbox.  Verify that text is preserved between tabs. (switch back and forth more than once).
