﻿using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Byz_Chess.Resources
{
    /// <summary>
    /// Interaction logic for PositionUC.xaml
    /// </summary>
    public partial class PositionUC : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            nameof(Stretch), typeof(Stretch),
            typeof(PositionUC)
        );

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            nameof(StrokeThickness), typeof(double),
            typeof(PositionUC)
        );

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register(
            nameof(StrokeMiterLimit), typeof(double),
            typeof(PositionUC)
        );

        public double StrokeMiterLimit
        {
            get => (double)GetValue(StrokeMiterLimitProperty);
            set => SetValue(StrokeMiterLimitProperty, value);
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            nameof(Stroke), typeof(Brush),
            typeof(PositionUC)
        );

        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data), typeof(Geometry),
            typeof(PositionUC)
        );
        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            nameof(Fill), typeof(Brush),
            typeof(PositionUC)
        );

        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty PieceProperty = DependencyProperty.Register(
            nameof(Piece), typeof(ImageSource),
            typeof(PositionUC)
        );

        public ImageSource? Piece
        {
            get => (ImageSource)GetValue(PieceProperty);
            set => SetValue(PieceProperty, value);
        }

        private bool _moveShadow = false;

        public bool MoveShadow
        {
            get => _moveShadow;
            set
            {
                _moveShadow = value;
                OnPropertyChanged();
            }
        }

        public PositionUC()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
