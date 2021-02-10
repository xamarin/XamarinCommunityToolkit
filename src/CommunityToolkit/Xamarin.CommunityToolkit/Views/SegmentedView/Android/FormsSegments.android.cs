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
		const string TAG = "FormsSegments";

		readonly Context context;
		readonly float defaultTextSize = 15.0f;
		readonly float defaultStrokeWidth = 2.5f;
		readonly int defaultButtonPadding = 16;

		int strokeWidth;
		bool isDisposed;

		public SegmentMode DisplayMode { get; set; }

		public CornerRadius CornerRadius { get; set; } = 8.0f;

		public AColor TintColor { get; set; } = AColor.Rgb(14, 98, 255);

		public AColor NormalTextColor { get; set; }

		public AColor SelectedTextColor { get; set; }

		public RadioButton CurrentSegment { get; set; }

		public ObservableCollection<string> Children { get; } = new ObservableCollection<string>();

		public event EventHandler<SelectedPositionChangedEventArgs> SegmentSelected;

		public AColor BackgroundColor { get; set; }

		public bool ShouldReactToCollectionChanges { get; set; }

		public FormsSegments(Context context)
			: base(context)
		{
			this.context = context;
			Build();
		}

		async void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!ShouldReactToCollectionChanges)
				return;

			try
			{
				await Initialize().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				Log.Error(TAG, ex.Message);
			}
		}

		void Build()
		{
			NormalTextColor = TintColor;
			SelectedTextColor = AColor.White;

			Background = null;

			strokeWidth = (int)context.ToPixels(defaultStrokeWidth);
			Orientation = Orientation.Horizontal;
			LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
			CheckedChange += OnCheckChanged;
		}

		public async Task Initialize()
		{
			if (ChildCount > 0)
			{
				if (Children != null)
					Children.CollectionChanged -= CollectionChanged;

				RemoveAllViews();
			}

			for (var i = 0; i < Children.Count; i++)
			{
				var rb = await InsertSegment(Children[i].ToString(), i);
				if (i == 0)
					rb.Checked = true;
			}

			if (Children != null)
				Children.CollectionChanged += CollectionChanged;
		}

		async Task<RadioButton> InsertSegment(string title, int index)
		{
			var isLeft = index == 0;
			var position = isLeft ? Position.Left : index == Children.Count - 1 ? Position.Right : Position.Middle;
			var rb = await GetRadioButton(title, position);
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
				SelectedTextColor,
				NormalTextColor
			});

		AContentRes.ColorStateList SegmentColorSelector => new AContentRes.ColorStateList(
			new int[][] // states
			{
				new int[] { ARes.Attribute.StateChecked },
				new int[] { -ARes.Attribute.StateChecked }
			},
			new int[] // colors
			{
				TintColor,
				BackgroundColor
			});

		void OnCheckChanged(object sender, CheckedChangeEventArgs e)
		{
			CurrentSegment = FindViewById<RadioButton>(e.CheckedId);
			SegmentSelected?.Invoke(this, new SelectedPositionChangedEventArgs(IndexOfChild(CurrentSegment)));
		}

		async Task<RadioButton> GetRadioButton(string title, Position position)
		{
			BitmapDrawable icon;
			var rb = new RadioButton(context);
			rb.SetPadding(defaultButtonPadding, defaultButtonPadding, defaultButtonPadding, defaultButtonPadding);
			rb.LayoutParameters = new RadioGroup.LayoutParams(0, LayoutParams.MatchParent, 1.0f);
			rb.Gravity = GravityFlags.Center;
			rb.SetButtonDrawable(null);

			if (DisplayMode == SegmentMode.Image)
			{
				icon = await GetImage(title);
				rb.SetBackground(GetRadioButtonStateListDrawable(position, icon));
			}
			else
			{
				rb.SetBackground(GetRadioButtonStateListDrawable(position));
				rb.Text = title;
				rb.TextAlignment = AViews.TextAlignment.Center;
				rb.SetTextSize(ComplexUnitType.Sp, defaultTextSize);
				rb.SetAllCaps(true);
				rb.SetTypeface(null, TypefaceStyle.Bold);
				rb.SetTextColor(TextColorSelector);
			}

			return rb;
		}

		async Task<BitmapDrawable> GetImage(ImageSource filePath)
		{
			return new BitmapDrawable(Resources, await context.GetFormsBitmapAsync(filePath));
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
			InsetDrawable insetDrawable = null;
			var rect = new GradientDrawable();
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

			insetDrawable ??= new InsetDrawable(rect, -strokeWidth, 0, 0, 0);

			if (DisplayMode == SegmentMode.Image)
			{
				if (icon == null)
					throw new MissingMemberException("BitmapDrawable for icon is missing");

				icon.Gravity = GravityFlags.Center;

				var layers = new Drawable[2];
				layers[0] = insetDrawable;
				layers[1] = icon;

				return new InsetDrawable(new LayerDrawable(layers), 0, 0, 0, 0);
			}

			return insetDrawable;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;
			if (disposing)
			{
				if (Children != null)
					Children.CollectionChanged -= CollectionChanged;
				CheckedChange -= OnCheckChanged;
			}
			base.Dispose(disposing);
		}
	}

	/// <summary>
	/// Position of the segment. Left, Middle, Right.
	/// </summary>
	enum Position
	{
		Middle,
		Left,
		Right
	}
}
