using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfKuGouGet.ViewModel
{
    /// <summary>
    /// UI虚拟化管理
    /// </summary>
    public partial class KuGouHelper
    {
        private VirtualizationCacheLengthUnit _cacheUnit = VirtualizationCacheLengthUnit.Page;
        public VirtualizationCacheLengthUnit CacheUnit
        {
            get => _cacheUnit;
            set => Set("CacheUnit", ref _cacheUnit, value);
        }

        private VirtualizationCacheLength _cacheLength = new VirtualizationCacheLength(1);
        public VirtualizationCacheLength CacheLength
        {
            get => _cacheLength;
            set => Set("CacheLength", ref _cacheLength, value);
        }
        private ScrollUnit _scrollUnit = ScrollUnit.Pixel;
        public ScrollUnit ScrollUnit
        {
            get => _scrollUnit;
            set => Set("ScrollUnit", ref _scrollUnit, value);
        }
        private ScrollBarVisibility _horizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => _horizontalScrollBarVisibility;
            set => Set("HorizontalScrollBarVisibility", ref _horizontalScrollBarVisibility, value);
        }
        private ScrollBarVisibility _verticalScrollBarVisibility = ScrollBarVisibility.Auto;
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => _verticalScrollBarVisibility;
            set => Set("VerticalScrollBarVisibility", ref _verticalScrollBarVisibility, value);
        }
        private Orientation _orientation = Orientation.Vertical;
        public Orientation Orientation
        {
            get => _orientation;
            set => Set("Orientation", ref _orientation, value);
        }
        private VirtualizingWrapPanel.SpacingMode _spacingMode = VirtualizingWrapPanel.SpacingMode.Uniform;
        public VirtualizingWrapPanel.SpacingMode SpacingMode
        {
            get => _spacingMode;
            set => Set("SpacingMode", ref _spacingMode, value);
        }
        private bool _stretchItems = true;
        public bool StretchItems
        {
            get => _stretchItems;
            set => Set("StretchItems", ref _stretchItems, value);
        }
        private double _scrollLineDelta = 16.0;
        public double ScrollLineDelta
        {
            get => _scrollLineDelta;
            set => Set("ScrollLineDelta", ref _scrollLineDelta, value);
        }
        private double _mouseWheelDelta = 48.0;
        public double MouseWheelDelta
        {
            get => _mouseWheelDelta;
            set => Set("MouseWheelDelta", ref _mouseWheelDelta, value);
        }
        private int _scrollLineDeltaItem = 1;
        public int ScrollLineDeltaItem
        {
            get => _scrollLineDeltaItem;
            set => Set("ScrollLineDeltaItem", ref _scrollLineDeltaItem, value);
        }
        private int _mouseWheelDeltaItem = 3;
        public int MouseWheelDeltaItem
        {
            get => _mouseWheelDeltaItem;
            set => Set("MouseWheelDeltaItem", ref _mouseWheelDeltaItem, value);
        }
    }
}
