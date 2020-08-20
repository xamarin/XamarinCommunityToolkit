using System;
using ElmSharp;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using Xamarin.Forms.Platform.Tizen.Native;
using NativeBox = Xamarin.Forms.Platform.Tizen.Native.Box;

namespace Xamarin.CommunityToolkit.Tizen.UI.Views
{
    public class LayoutCanvas : WidgetLayout, IContainable<EvasObject>
    {
        readonly ObservableCollection<EvasObject> children = new ObservableCollection<EvasObject>();
		readonly NativeBox box;

        public LayoutCanvas(EvasObject parent) : base(parent)
        {
            box = new NativeBox(parent);
            SetContent(box);

            children.CollectionChanged += (o, e) =>
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
            };
        }

        public event EventHandler<LayoutEventArgs> LayoutUpdated
        {
            add { box.LayoutUpdated += value; }
            remove { box.LayoutUpdated -= value; }
        }

		public new IList<EvasObject> Children => children;

		protected override void OnUnrealize()
        {
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