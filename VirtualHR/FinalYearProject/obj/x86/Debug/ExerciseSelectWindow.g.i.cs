﻿#pragma checksum "..\..\..\ExerciseSelectWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7E7F2C732F46F2ABBF3A471A941E7FEE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FinalYearProject {
    
    
    /// <summary>
    /// ExerciseSelectWindow
    /// </summary>
    public partial class ExerciseSelectWindow : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\ExerciseSelectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border borderExerciseSelect;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\ExerciseSelectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtExerciseSelect;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\ExerciseSelectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBack;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\ExerciseSelectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFistClench;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\ExerciseSelectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnThumbTouch;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\ExerciseSelectWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClawStretch;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FinalYearProject;component/exerciseselectwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ExerciseSelectWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.borderExerciseSelect = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.txtExerciseSelect = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.btnBack = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\ExerciseSelectWindow.xaml"
            this.btnBack.Click += new System.Windows.RoutedEventHandler(this.btnBack_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnFistClench = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\ExerciseSelectWindow.xaml"
            this.btnFistClench.Click += new System.Windows.RoutedEventHandler(this.btnPerformExercise_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnThumbTouch = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\ExerciseSelectWindow.xaml"
            this.btnThumbTouch.Click += new System.Windows.RoutedEventHandler(this.btnPerformExercise_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnClawStretch = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\ExerciseSelectWindow.xaml"
            this.btnClawStretch.Click += new System.Windows.RoutedEventHandler(this.btnPerformExercise_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

