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

using System.Collections;
using System.Windows.Markup;
using System.Windows.Data;
using OpenSilver.Internal.Controls;
using OpenSilver.Internal.Xaml.Context;

namespace System.Windows.Controls
{
    /// <summary>
    /// Represents a control with a single piece of content. Controls such as Button,
    /// CheckBox, and ScrollViewer directly or indirectly inherit from this class.
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class ContentControl : Control
    {
#region Constructor

        public ContentControl()
        {
            this.DefaultStyleKey = typeof(ContentControl);
        }

#endregion Constructor

#region Dependency Properties

        /// <summary>
        /// Gets or sets the content of a ContentControl.
        /// </summary>
        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValueInternal(ContentProperty, value); }
        }

        /// <summary>
        /// Identifies the Content dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content),
                typeof(object),
                typeof(ContentControl),
                new FrameworkPropertyMetadata(null, OnContentChanged));

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ContentControl)d).OnContentChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Gets or sets the data template that is used to display the content of the
        /// ContentControl.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValueInternal(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the ContentTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                nameof(ContentTemplate),
                typeof(DataTemplate),
                typeof(ContentControl),
                new PropertyMetadata((object)null));

#endregion Dependency Properties

#region Protected Methods

        protected virtual void OnContentChanged(object oldContent, object newContent)
        {
            // Remove the old content child
            this.RemoveLogicalChild(oldContent);

            if (this.ContentIsNotLogical)
            {
                return;
            }

            // We want to update the logical parent only if we don't have one already.
            FrameworkElement fe = newContent as FrameworkElement;
            if (fe != null)
            {
                DependencyObject logicalParent = fe.Parent;
                if (logicalParent != null)
                {
                    return;
                }
            }

            this.AddLogicalChild(newContent);
        }

#endregion Protected Methods

#region Internal Properties

        /// <summary>
        ///    Indicates whether Content should be a logical child or not.
        /// </summary>
        internal bool ContentIsNotLogical
        {
            get;
            set;
        }

        /// <summary>
        /// Returns enumerator to logical children
        /// </summary>
        /*protected*/ internal override IEnumerator LogicalChildren
        {
            get
            {
                object content = Content;
                
                if (ContentIsNotLogical || content == null)
                {
                    return EmptyEnumerator.Instance;
                }

                // If the current ContentControl is in a Template.VisualTree and is meant to host
                // the content for the container then that content shows up as the logical child
                // for the container and not for the current ContentControl.
                FrameworkElement fe = content as FrameworkElement;
                if (fe != null)
                {
                    DependencyObject logicalParent = fe.Parent;
                    if (logicalParent != null && logicalParent != this)
                    {
                        return EmptyEnumerator.Instance;
                    }
                }

                return new ContentModelTreeEnumerator(this, content);
            }
        }

        internal override FrameworkTemplate TemplateInternal => base.TemplateInternal ?? DefaultTemplate;

        private static FrameworkTemplate DefaultTemplate { get; } = new UseContentTemplate();

        #endregion Internal Properties

        #region Internal Methods

        /// <summary>
        /// Prepare to display the item.
        /// </summary>
        internal void PrepareContentControl(object item, DataTemplate template)
        {
            if (item != this)
            {
                // don't treat Content as a logical child
                this.ContentIsNotLogical = true;

                this.ContentTemplate = template;
                this.Content = item;
            }
            else
            {
                this.ContentIsNotLogical = false;
            }
        }

        internal void ClearContentControl(object item)
        {
            if (this != item)
            {
                this.ClearValue(ContentProperty);
            }
        }

        internal override string GetPlainText() => ContentObjectToString(Content);

        internal static string ContentObjectToString(object content)
        {
            if (content != null)
            {
                if (content is FrameworkElement feContent)
                {
                    return feContent.GetPlainText();
                }

                return content.ToString();
            }

            return string.Empty;
        }

        #endregion Internal Methods

        private sealed class UseContentTemplate : FrameworkTemplate
        {
            private readonly TemplateContent _defaultTemplate =
                new TemplateContent(
                    new XamlContext(),
                    static (owner, context) =>
                    {
                        var grid = new Grid { TemplatedParent = owner.AsDependencyObject() };
                        var tb = new TextBlock { TemplatedParent = owner.AsDependencyObject() };
                        tb.SetBinding(TextBlock.TextProperty, new Binding());
                        grid.Children.Add(tb);
                        return grid;
                    });

            public UseContentTemplate()
            {
                Seal();
            }

            internal override bool BuildVisualTree(IInternalFrameworkElement container)
            {
                var cc = (ContentControl)container;
                object content = cc.Content;
                if (content is FrameworkElement fe)
                {
                    container.TemplateChild = fe;
                    return true;
                }
                else if (content is not null && (content is not string s || s.Length > 0))
                {
                    container.TemplateChild = _defaultTemplate.LoadContent(cc);
                    return true;
                }

                return false;
            }
        }
    }
}
