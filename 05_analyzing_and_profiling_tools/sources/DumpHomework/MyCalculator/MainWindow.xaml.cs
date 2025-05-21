// Decompiled with JetBrains decompiler
// Type: MyCalculatorv1.MainWindow
// Assembly: MyCalculatorv1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E4247A5-25E4-47A6-84F4-A414933F7536
// Assembly location: C:\Users\Anastasiya_Zhukava1\source\Education\NET-MPI-2025\05_analyzing_and_profiling_tools\sources\DumpHomework\MyCalculator.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace MyCalculatorv1

{
    public partial class MainWindow : Window, IComponentConnector
    {
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //this.tb.Text += ((Button)sender).Content.ToString();

            var input = ((Button)sender).Content.ToString();

            if (string.IsNullOrEmpty(this.tb.Text))
            {
                if ("+-*/".Contains(input))
                {
                    return;
                }
            }
            else
            {
                var lastChar = this.tb.Text[this.tb.Text.Length - 1];

                if ("+-*/".Contains(lastChar) && "+-*/".Contains(input))
                {
                    return;
                }
            }

            this.tb.Text += input;
        }

        private void Result_click(object sender, RoutedEventArgs e) => this.result();

        private void result()
        {
            int num1 = 0;
            if (this.tb.Text.Contains("+"))
                num1 = this.tb.Text.IndexOf("+");
            else if (this.tb.Text.Contains("-"))
                num1 = this.tb.Text.IndexOf("-");
            else if (this.tb.Text.Contains("*"))
                num1 = this.tb.Text.IndexOf("*");
            else if (this.tb.Text.Contains("/"))
                num1 = this.tb.Text.IndexOf("/");
            string str = this.tb.Text.Substring(num1, 1);
            double num2 = Convert.ToDouble(this.tb.Text.Substring(0, num1));
            double num3 = Convert.ToDouble(this.tb.Text.Substring(num1 + 1, this.tb.Text.Length - num1 - 1));
            switch (str)
            {
                case "+":
                    TextBox tb1 = this.tb;
                    tb1.Text = $"{tb1.Text}={(object)(num2 + num3)}";
                    break;
                case "-":
                    TextBox tb2 = this.tb;
                    tb2.Text = $"{tb2.Text}={(object)(num2 - num3)}";
                    break;
                case "*":
                    TextBox tb3 = this.tb;
                    tb3.Text = $"{tb3.Text}={(object)(num2 * num3)}";
                    break;
                default:
                    TextBox tb4 = this.tb;
                    tb4.Text = $"{tb4.Text}={(object)(num2 / num3)}";
                    break;
            }
        }

        private void Off_Click_1(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Del_Click(object sender, RoutedEventArgs e) => this.tb.Text = "";

        private void R_Click(object sender, RoutedEventArgs e)
        {
            if (this.tb.Text.Length <= 0)
                return;
            this.tb.Text = this.tb.Text.Substring(0, this.tb.Text.Length - 1);
        }

        //[DebuggerNonUserCode]
        //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        //public void InitializeComponent()
        //{
        //    if (this._contentLoaded)
        //        return;
        //    this._contentLoaded = true;
        //    Application.LoadComponent((object)this, new Uri("/MyCalculatorv1;component/MainWindow.xaml", UriKind.Relative));
        //}

        //[DebuggerNonUserCode]
        //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //void IComponentConnector.Connect(int connectionId, object target)
        //{
        //    switch (connectionId)
        //    {
        //        case 1:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 2:
        //            this.tb = (TextBox)target;
        //            break;
        //        case 3:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 4:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 5:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 6:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 7:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 8:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 9:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 10:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 11:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 12:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 13:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 14:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 15:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Result_click);
        //            break;
        //        case 16 /*0x10*/:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Button_Click_1);
        //            break;
        //        case 17:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Off_Click_1);
        //            break;
        //        case 18:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.Del_Click);
        //            break;
        //        case 19:
        //            ((ButtonBase)target).Click += new RoutedEventHandler(this.R_Click);
        //            break;
        //        default:
        //            this._contentLoaded = true;
        //            break;
        //    }
        //}
    }
}
