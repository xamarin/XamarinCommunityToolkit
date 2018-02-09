using System;
using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace XamarinCommunityToolkit.Controls.iOS.Views
{
    /// <summary>
    /// Copyright (c) 2017 fferegrino (Antonio Feregrino)
    /// https://github.com/messier16/Forms.Controls/blob/master/Forms.Controls.iOS/Native/Checkbox/Checkbox.cs
    /// </summary>
    [Register("CheckBoxView")]
    public class CheckBoxView : UIControl
    {
        private readonly CAShapeLayer _checkLayer = new CAShapeLayer();
        private readonly CAShapeLayer _containerLayer = new CAShapeLayer();
        private UIColor _checkboxBackgroundColor = UIColor.FromRGB(49, 95, 153);
        private UIColor _checkColor = UIColor.FromRGB(84, 142, 205);

        private bool _checked;
        private float _checkLineWidth = 2f;
        private float _containerLineWidth = 2f;

        private bool _fillsOnChecked;


        public CheckBoxView(NSCoder coder) : base(coder)
        {
        }

        public CheckBoxView(CGRect frame) : base(frame)
        {
            CustomInitialization();
        }

        public CheckBoxView(CGRect frame, bool on) : this(frame)
        {
            Checked = on;
        }


        [Export("CheckLineWidth")]
        [Browsable(true)]
        public float CheckLineWidth
        {
            get => _checkLineWidth;
            set
            {
                _checkLineWidth = value;
                LayoutLayers();
            }
        }

        [Export("CheckColor")]
        [Browsable(true)]
        public UIColor CheckColor
        {
            get => _checkColor;
            set
            {
                _checkColor = value;
                ColorLayers();
            }
        }

        [Export("ContainerLineWidth")]
        [Browsable(true)]
        public float ContainerLineWidth
        {
            get => _containerLineWidth;
            set
            {
                _containerLineWidth = value;
                LayoutLayers();
            }
        }

        [Export("CheckboxBackgroundColor")]
        [Browsable(true)]
        public UIColor CheckboxBackgroundColor
        {
            get => _checkboxBackgroundColor;
            set
            {
                _checkboxBackgroundColor = value;
                SetNeedsDisplay();
            }
        }

        [Export("FillsOnChecked")]
        [Browsable(true)]
        public bool FillsOnChecked
        {
            get => _fillsOnChecked;
            set
            {
                _fillsOnChecked = value;
                ColorLayers();
            }
        }

        [Export("Checked")]
        [Browsable(true)]
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                ColorLayers();
            }
        }

        private CGRect CheckFrame
        {
            get
            {
                var width = (float) Bounds.Width;
                var height = (float) Bounds.Height;

                float x;
                float y;

                float sideLength;
                if (width > height)
                {
                    sideLength = height;
                    x = (width - sideLength) / 2;
                    y = 0;
                }
                else
                {
                    sideLength = width;
                    x = 0;
                    y = (height - sideLength) / 2;
                }

                var halfLineWidth = ContainerLineWidth / 2;
                return new CGRect(
                    x + halfLineWidth,
                    y + halfLineWidth,
                    sideLength - ContainerLineWidth,
                    sideLength - ContainerLineWidth);
            }
        }

        private UIBezierPath ContainerPath => UIBezierPath.FromRoundedRect(
            new CGRect(CheckFrame.GetMinX() + NMath.Floor(CheckFrame.Width * 0.05000f) + 0.5f,
                CheckFrame.GetMinY() + NMath.Floor(CheckFrame.Height * 0.05000f) + 0.5f,
                NMath.Floor(CheckFrame.Width * 0.95000f) - NMath.Floor(CheckFrame.Width * 0.05000f),
                NMath.Floor(CheckFrame.Height * 0.95000f) - NMath.Floor(CheckFrame.Height * 0.05000f)), CheckFrame.Height * 0.1f);

        private UIBezierPath CheckPath
        {
            get
            {
                var checkPath = new UIBezierPath();
                checkPath.MoveTo(new CGPoint(CheckFrame.GetMinX() + 0.76208f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.26000f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.38805f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.62670f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.23230f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.47400f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.18000f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.52527f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.36190f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.70360f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.38805f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.72813f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.41420f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.70360f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.81437f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.31127f * CheckFrame.Height));
                checkPath.AddLineTo(new CGPoint(CheckFrame.GetMinX() + 0.76208f * CheckFrame.Width,
                    CheckFrame.GetMinY() + 0.26000f * CheckFrame.Height));
                checkPath.ClosePath();
                return checkPath;
            }
        }

        private void ColorLayers()
        {
            _containerLayer.StrokeColor = CheckboxBackgroundColor.CGColor;

            // Set colors based on 'on' property
            if (Checked)
            {
                _containerLayer.FillColor = FillsOnChecked ? CheckboxBackgroundColor.CGColor : UIColor.Clear.CGColor;
                _checkLayer.FillColor = CheckColor.CGColor;
                _checkLayer.StrokeColor = CheckColor.CGColor;
            }
            else
            {
                _containerLayer.FillColor = UIColor.Clear.CGColor;
                _checkLayer.FillColor = UIColor.Clear.CGColor;
                _checkLayer.StrokeColor = UIColor.Clear.CGColor;
            }
        }

        public override void PrepareForInterfaceBuilder()
        {
            base.PrepareForInterfaceBuilder();
            CustomInitialization();
        }

        public override void AwakeFromNib()
        {
            CustomInitialization();
        }

        private void CustomInitialization()
        {
            _checkLayer.FillColor = UIColor.Clear.CGColor;
            ColorLayers();
            LayoutLayers();
            Layer.AddSublayer(_containerLayer);
            Layer.AddSublayer(_checkLayer);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            LayoutLayers();
        }

        private void LayoutLayers()
        {
            // Set frames, line widths and paths for layers
            _containerLayer.Frame = Bounds;
            _containerLayer.LineWidth = ContainerLineWidth;
            _containerLayer.Path = ContainerPath.CGPath;

            _checkLayer.Frame = Bounds;
            _checkLayer.LineWidth = CheckLineWidth;
            _checkLayer.Path = CheckPath.CGPath;
        }

        public override bool BeginTracking(UITouch uitouch, UIEvent uievent)
        {
            base.BeginTracking(uitouch, uievent);
            return true;
        }

        public override bool ContinueTracking(UITouch uitouch, UIEvent uievent)
        {
            base.ContinueTracking(uitouch, uievent);
            return true;
        }

        public override void EndTracking(UITouch uitouch, UIEvent uievent)
        {
            base.EndTracking(uitouch, uievent);
            var touchLocationInView = uitouch?.LocationInView(this);
            if (touchLocationInView == null)
                return;

            var validBounds = Bounds;

            if (validBounds.Contains(touchLocationInView.Value))
            {
                Checked = !Checked;
                SendActionForControlEvents(UIControlEvent.ValueChanged);
            }
        }
    }
}