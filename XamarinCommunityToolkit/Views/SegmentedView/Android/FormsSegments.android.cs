using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using AColor = Android.Graphics.Color;
using ARes = Android.Resource;
using AContentRes = Android.Content.Res;
using AViews = Android.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions.Android;
using RadioButton = Android.Widget.RadioButton;

namespace Xamarin.CommunityToolkit.Android.UI.Views
{
	public class FormsSegments : RadioGroup, IDisposable
	{
		readonly Context context;
		readonly float defaultTextSize = 15.0f;
		readonly float defaultStrokeWidth = 2.5f;
		readonly int defaultButtonPadding = 16;

		SegmentMode mode;

		public SegmentMode DisplayMode
		{
			get => mode;
			set
			{
				mode = value;
				InitializeSegments();
			}
		}

		CornerRadius cornerRadius = 8.0f;

		public CornerRadius CornerRadius
		{
			get => cornerRadius;
			set
			{
				cornerRadius = value;
				InitializeSegments();
			}
		}

		AColor tintColor = AColor.Rgb(14, 98, 255);

		public AColor TintColor
		{
			get => tintColor;
			set
			{
				tintColor = value;
				InitializeSegments();
			}
		}

		AColor backgroundColor = AColor.Transparent;

		public AColor BackgroundColor
		{
			get => backgroundColor;
			set
			{
				backgroundColor = value;
				SetBackgroundColor(value);
			}
		}

		public ObservableCollection<string> Children { get; } = new ObservableCollection<string>();

		RadioButton currentSegment;

		public RadioButton CurrentSegment
		{
			get => currentSegment;
			set
			{
				currentSegment = value;
			}
		}

		public event EventHandler<SelectedPositionChangedEventArgs> SegmentSelected;

		int strokeWidth;
		bool disposed;

		public FormsSegments(Context context)
            : base(context)
		{
			this.context = context;
			Children.CollectionChanged += CollectionChanged;
			Build();
		}

		void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			InitializeSegments();
		}

		void Build()
		{
			strokeWidth = (int)context.ToPixels(defaultStrokeWidth);

			SetBackgroundColor(backgroundColor);

			Orientation = Orientation.Horizontal;
			LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
			CheckedChange += OnCheckChanged;
		}

		public void InitializeSegments()
		{
			if (ChildCount > 0)
				RemoveAllViews();

			for (var i = 0; i < Children.Count; i++)
			{
				var rb = InsertSegment(Children[i].ToString(), i);
				if (i == 0)
					rb.Checked = true;
			}
		}

		RadioButton InsertSegment(string title, int index)
		{
			var isLeft = index == 0;
			var position = isLeft ? Position.Left : index == Children.Count - 1 ? Position.Right : Position.Middle;
			var rb = GetRadioButton(title, position);
			AddView(rb);
			return rb;
		}

		AContentRes.ColorStateList TextColorSelector => new AContentRes.ColorStateList(
			new int[][] // states
			{
				new int[] { ARes.Attribute.StateChecked },
				new int[] { -ARes.Attribute.StateChecked }
			},
			new int[] // colors
			{
				AColor.White, TintColor
			});

		AContentRes.ColorStateList SegmentColorSelector => new AContentRes.ColorStateList(
			new int[][] // states
			{
				new int[] { ARes.Attribute.StateChecked },
				new int[] { -ARes.Attribute.StateChecked }
			},
			new int[] // colors
			{
				TintColor, BackgroundColor
			});

		void OnCheckChanged(object sender, CheckedChangeEventArgs e)
		{
			CurrentSegment = FindViewById<RadioButton>(e.CheckedId);
			SegmentSelected?.Invoke(this, new SelectedPositionChangedEventArgs(IndexOfChild(CurrentSegment)));
		}

		#region Drawable Resources

		RadioButton GetRadioButton(string title, Position position)
		{
			BitmapDrawable icon;
			var rb = new RadioButton(context);
			rb.SetPadding(defaultButtonPadding, defaultButtonPadding, defaultButtonPadding, defaultButtonPadding);
			rb.LayoutParameters = new RadioGroup.LayoutParams(0, LayoutParams.MatchParent, 1.0f);
			rb.Gravity = GravityFlags.Center;
			rb.SetButtonDrawable(null);

			if(DisplayMode == SegmentMode.Image)
			{
				icon = GetImage(title);
				rb.SetBackground(GetRadioButtonStateListDrawable(position, icon));
				return rb;
			}

			rb.SetBackground(GetRadioButtonStateListDrawable(position));
			rb.Text = title;
			rb.TextAlignment = AViews.TextAlignment.Center;
			rb.SetTextSize(ComplexUnitType.Sp, defaultTextSize);
			rb.SetAllCaps(true);
			rb.SetTypeface(null, TypefaceStyle.Bold);
			rb.SetTextColor(TextColorSelector);
			return rb;
		}

		BitmapDrawable GetImage(ImageSource filePath)
		{
			var tcs = new TaskCompletionSource<BitmapDrawable>();

			Task.Run(async () =>
			{
				try
				{
					var result = await context.GetFormsBitmapAsync(filePath);
					tcs.SetResult(new BitmapDrawable(Resources, result));
				}
				catch (Exception ex)
				{
					tcs.SetException(ex);
				}
			});

			return tcs.Task.Result;
		}

		StateListDrawable GetRadioButtonStateListDrawable(Position position, BitmapDrawable icon = null)
		{
			var drawable = new StateListDrawable();
			drawable.AddState(new int[] { ARes.Attribute.StateChecked }, GetSegmentDrawable(position, icon));
			drawable.AddState(new int[] { -ARes.Attribute.StateChecked }, GetSegmentDrawable(position, icon));
			return drawable;
		}

		Drawable GetSegmentDrawable(Position position, BitmapDrawable icon)
		{
			var rect = new GradientDrawable();
			InsetDrawable insetDrawable = null;

			rect.SetShape(ShapeType.Rectangle);
			rect.SetStroke(strokeWidth, TintColor);
			rect.SetColor(SegmentColorSelector);

			switch (position)
			{
				case Position.Left:
					rect.SetCornerRadii(new float[] { context.ToPixels(CornerRadius.TopLeft), context.ToPixels(CornerRadius.TopLeft), 0, 0, 0, 0, context.ToPixels(CornerRadius.BottomLeft), context.ToPixels(CornerRadius.BottomLeft) });
					insetDrawable = new InsetDrawable(rect, 0);
					break;
				case Position.Right:
					rect.SetCornerRadii(new float[] { 0, 0, context.ToPixels(CornerRadius.TopRight), context.ToPixels(CornerRadius.TopRight), context.ToPixels(CornerRadius.BottomRight), context.ToPixels(CornerRadius.BottomRight), 0, 0 });
					break;
				default:
					rect.SetCornerRadius(0);
					break;
			}

			insetDrawable = insetDrawable ?? new InsetDrawable(rect, -strokeWidth, 0, 0, 0);

			if (mode == SegmentMode.Image)
			{
				if (icon == null)
					throw new MissingMemberException("BitmapDrawable for icon is missing");

				icon.Gravity = GravityFlags.Center;

				var layers = new Drawable[2];
				layers[0] = insetDrawable;
				layers[1] = icon;

				return new LayerDrawable(layers);
			}

			return insetDrawable;
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;
			if (disposing)
			{
				Children.CollectionChanged -= CollectionChanged;
				CheckedChange -= OnCheckChanged;
			}
			base.Dispose(disposing);
		}
	}

	/// <summary>
	/// Position of the segment. Left, Middle, Right.
	/// </summary>
	internal enum Position
	{
		Middle,
		Left,
		Right
	}
}
