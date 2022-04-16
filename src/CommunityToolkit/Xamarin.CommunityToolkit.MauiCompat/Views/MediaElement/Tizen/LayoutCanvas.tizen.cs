using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ElmSharp;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen.Native;
using ELayout = ElmSharp.Layout;
using TBox = Microsoft.Maui.Controls.Compatibility.Platform.Tizen.Native.Box;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class LayoutCanvas : ELayout, IContainable<EvasObject>
	{
		readonly ObservableCollection<EvasObject> children = new ObservableCollection<EvasObject>();
		readonly WeakEventManager<LayoutEventArgs> weakEventManager = new WeakEventManager<LayoutEventArgs>();
		readonly TBox box;

		public LayoutCanvas(EvasObject parent)
			: base(parent)
		{
			SetTheme("layout", "elm_widget", "default");
			box = new TBox(parent);
			SetContent(box);
			children.CollectionChanged += OnChildrenCollectionChanged;
			box.LayoutUpdated += OnBoxLayoutUpdated;
		}

		void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var v in e.NewItems)
				{
					if (v is EvasObject view)
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
		}

		void OnBoxLayoutUpdated(object sender, LayoutEventArgs e) => weakEventManager.RaiseEvent(this, e, nameof(LayoutUpdated));

		public event EventHandler<LayoutEventArgs> LayoutUpdated
		{
			add => weakEventManager.AddEventHandler(value);
			remove => weakEventManager.RemoveEventHandler(value);
		}

		public new IList<EvasObject> Children => children;

		protected override void OnUnrealize()
		{
			children.CollectionChanged -= OnChildrenCollectionChanged;
			box.LayoutUpdated -= OnBoxLayoutUpdated;

			foreach (var child in children)
			{
				child.Unrealize();
			}

			base.OnUnrealize();
		}

		void OnAdd(EvasObject view) => box.PackEnd(view);

		void OnRemove(EvasObject view) => box.UnPack(view);

		void OnRemoveAll() => box.UnPackAll();
	}
}