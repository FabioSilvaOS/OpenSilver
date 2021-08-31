﻿
/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*      
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/

using System;
using System.Collections.Generic;
using System.Globalization;
using OpenSilver.Internal;

#if MIGRATION
using System.Windows.Shapes;
#else
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
#endif

#if MIGRATION
namespace System.Windows.Media
#else
namespace Windows.UI.Xaml.Media
#endif
{
    public sealed partial class PointCollection : PresentationFrameworkCollection<Point>
    {
        private Path _parentPath;

        /// <summary>
        /// Initializes a new instance that is empty.
        /// </summary>
        public PointCollection() { }

        /// <summary>
        /// Initializes a new instance that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">int - The number of elements that the new list is initially capable of storing.</param>
        public PointCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Creates a PointCollection with all of the same elements as collection
        /// </summary>
        public PointCollection(IEnumerable<Point> points) : base(points) { }

        public static PointCollection Parse(string source)
        {
            var result = new PointCollection();

            if (source != null)
            {
                IFormatProvider formatProvider = CultureInfo.InvariantCulture;
                char[] separator = new char[2] { TokenizerHelper.GetNumericListSeparator(formatProvider), ' ' };
                string[] split = source.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                // Points count needs to be an even number
                if (split.Length % 2 == 1)
                {
                    throw new FormatException($"'{source}' is not an eligible value for a {typeof(PointCollection)}.");
                }

                for (int i = 0; i < split.Length; i += 2)
                {
                    result.Add(
                        new Point(
                            Convert.ToDouble(split[i], formatProvider),
                            Convert.ToDouble(split[i + 1], formatProvider)
                        )
                    );
                }
            }

            return result;
        }

        internal override void AddOverride(Point point)
        {
            this.AddInternal(point);
            this.NotifyCollectionChanged();
        }

        internal override void ClearOverride()
        {
            this.ClearInternal();
            this.NotifyCollectionChanged();
        }

        internal override void RemoveAtOverride(int index)
        {
            this.RemoveAtInternal(index);
            this.NotifyCollectionChanged();
        }

        internal override bool RemoveOverride(Point point)
        {
            if (this.RemoveInternal(point))
            {
                this.NotifyCollectionChanged();
                return true;
            }
            return false;
        }

        internal override void InsertOverride(int index, Point point)
        {
            this.InsertInternal(index, point);
            this.NotifyCollectionChanged();
        }

        internal override Point GetItemOverride(int index)
        {
            return this.GetItemInternal(index);
        }

        internal override void SetItemOverride(int index, Point point)
        {
            this.SetItemInternal(index, point);
            this.NotifyCollectionChanged();
        }

        internal void SetParentPath(Path path)
        {
            this._parentPath = path;
        }

        private void NotifyCollectionChanged()
        {
            if (this._parentPath != null)
            {
                this._parentPath.ScheduleRedraw();
            }
        }
    }
}