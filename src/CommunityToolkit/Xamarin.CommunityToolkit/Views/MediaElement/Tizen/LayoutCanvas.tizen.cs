using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms.Platform.Tizen.Native;
using ElmSharp;
using ELayout = ElmSharp.Layout;
using TBox = Xamarin.Forms.Platform.Tizen.Native.Box;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class LayoutCanvas : ELayout, IContainable<EvasObject>
	{
		readonly ObservableCollection<EvasObject> children = new ObservableCollection<EvasObject>();
		TBox box;

		public LayoutCanvas(EvasObject parent) : base(parent)
		{
			SetTheme("layout", "elm_widget", "default");
			box = new TBox(parent);
			SetContent(box);

			children.CollectionChanged += (o, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					foreach (var v in e.NewItems)
					{
						var view = v as EvasObject;
						if (view != null)
						{
							OnAdd(view);
						}
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					foreach (var v in e.OldItems)
					{
						if (v is EvasObject view)
						{
							OnRemove(view);
						}
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					OnRemoveAll();
				}
			};
		}

		public event EventHandler<LayoutEventArgs> LayoutUpdated
		{
			add { box.LayoutUpdated += value; }
			remove { box.LayoutUpdated -= value; }
		}

		public new IList<EvasObject> Children
		{
			get
			{
				return children;
			}
		}

		protected override void OnUnrealize()
		{
			foreach (var child in children)
			{
				child.Unrealize();
			}

			base.OnUnrealize();
		}

		void OnAdd(EvasObject view)
		{
			box.PackEnd(view);
		}

		void OnRemove(EvasObject view)
		{
			box.UnPack(view);
		}

		void OnRemoveAll()
		{
			box.UnPackAll();
		}
	}
}
