﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdobeScriptMaker.UI.Views.Timeline
{
    public class TimelineRuler : Control
    {
        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(double), typeof(TimelineRuler), new PropertyMetadata(0.0));

        static TimelineRuler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineRuler), new FrameworkPropertyMetadata(typeof(TimelineRuler)));
        }
    }
}
