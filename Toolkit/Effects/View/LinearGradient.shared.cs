using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Toolkit.Effects.Models;
using System.Collections.ObjectModel;

namespace Xamarin.Toolkit.Effects
{
    namespace Models
    {
        //From MVVM-helpers by James Montemagno
        /// <summary>
        /// Observable object with INotifyPropertyChanged implemented
        /// </summary>
        public class ObservableObject : INotifyPropertyChanged
        {
            /// <summary>
            /// Sets the property.
            /// </summary>
            /// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
            /// <param name="backingStore">Backing store.</param>
            /// <param name="value">Value.</param>
            /// <param name="validateValue">Validates value.</param>
            /// <param name="propertyName">Property name.</param>
            /// <param name="onChanged">On changed.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            protected virtual bool SetProperty<T>(
                ref T backingStore, T value,
                [CallerMemberName]string propertyName = "",
                Action onChanged = null,
                Func<T, T, bool> validateValue = null)
            {
                //if value didn't change
                if (EqualityComparer<T>.Default.Equals(backingStore, value))
                    return false;

                //if value changed but didn't validate
                if (validateValue != null && !validateValue(backingStore, value))
                    return false;

                backingStore = value;
                onChanged?.Invoke();
                OnPropertyChanged(propertyName);
                return true;
            }


            /// <summary>
            /// Occurs when property changed.
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Raises the property changed event.
            /// </summary>
            /// <param name="propertyName">Property name.</param>
            protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        /// <summary>
        /// The flow of the linear gradient and it's representation in degrees. eg. LeftToRight flow is 90 degrees
        /// </summary>
        public enum Flow : int
        {
            BottomToTop = 0,
            BottomLeftToTopRight = 45,
            LeftToRight = 90,
            TopLeftToBottomRight = 135,
            TopToBottom = 180,
            TopRightToBottomLeft = 225,
            RightToLeft = 270,
            BottomRightToTopLeft = 315,
            //360 is the same as 0
        }

        /// <summary>
        /// A linear gradient stop object. Used to visualize a linear gradient stop with a specific color and offset values.
        /// </summary>
        public class LinearGradientStop : ObservableObject
        {
            private double _offset;

            /// <summary>
            /// Where the stop begins. (from 0 to 1) along the gradient.
            /// </summary>
            public double Offset
            {
                get { return _offset; }
                set
                {
                    if (value != _offset)
                        SetProperty(ref _offset, value);
                }
            }

            private Color _color;

            /// <summary>
            /// Color of the stop.
            /// </summary>
            public Color Color
            {
                get { return _color; }
                set
                {
                    if (value != _color)
                        SetProperty(ref _color, value);
                }
            }

            /// <summary>
            /// Initialize a new object of LinearGradientStop with an offset and a color.
            /// </summary>
            /// <param name="offset">Offset to be added</param>
            /// <param name="color">Color to be added</param>
            public LinearGradientStop(double offset, Color color)
            {
                Offset = offset;
                Color = color;
            }
        }

        /// <summary>
        /// Acts as a container for the LinearGradientStops objects
        /// </summary>
        public class LinearGradientStopCollection : ObservableCollection<LinearGradientStop> { }

        /// <summary>
        /// Fills the VisualElement with a linear gradient background
        /// </summary>
        public class LinearGradient : ObservableObject
        {
            private Flow _flow;

            /// <summary>
            /// The flow of the gradient
            /// </summary>
            public Flow Flow
            {
                get { return _flow; }
                set
                {
                    if (value != _flow)
                    {
                        if ((int)value > 360)
                            SetProperty(ref _flow, value - 360);

                        SetProperty(ref _flow, value);
                    }
                }
            }

            private LinearGradientStopCollection _linearGradientStops;

            /// <summary>
            /// The stages of the gradient
            /// </summary>
            public LinearGradientStopCollection LinearGradientStops
            {
                get { return _linearGradientStops; }
                set
                {
                    if (value is null)
                        throw new ArgumentNullException("LinearGradientStops CANNOT be null.");

                    if (value != _linearGradientStops)
                        SetProperty(ref _linearGradientStops, value);
                }
            }
            /// <summary>
            /// Initializes a new instance of LinearGradient
            /// </summary>
            public LinearGradient() { }

            /// <summary>
            /// Initializes a new instance of LinearGradient with a Flow and a LinearGradientStopCollection
            /// </summary>
            /// <param name="flow">Flow to be added</param>
            /// <param name="linearGradientStopsCollection">LinearGradientStopCollection  to be added</param>
            public LinearGradient(Flow flow, LinearGradientStopCollection linearGradientStopsCollection)
            {
                Flow = flow;

                if (linearGradientStopsCollection is null)
                    throw new ArgumentNullException();

                LinearGradientStops = linearGradientStopsCollection;
            }
        }
    }
    public class LinearGradientEffect : RoutingEffect
    {
        public LinearGradientEffect()
            : base(EffectIds.LinearGradient)
        {
        }

        private LinearGradient _linearGradient;

        public LinearGradient LinearGradient
        {
            get { return _linearGradient; }
            set
            {
                if (value is null)
                    throw new ArgumentNullException();
                _linearGradient = value;
            }
        }
    }
}
