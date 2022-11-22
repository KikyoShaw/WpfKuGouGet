using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace WpfKuGouGet.VirtualizingWrapPanel
{
    /// <summary>
    /// A ItemsControl supporting virtualization.
    /// </summary>
    public class VirtualizingItemsControl : ItemsControl
    {
        public VirtualizingItemsControl()
        {
            ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));

            string template = @"<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                             xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                <Border
                    BorderThickness='{TemplateBinding Border.BorderThickness}'
                    Padding='{TemplateBinding Control.Padding}'
                    BorderBrush='{TemplateBinding Border.BorderBrush}'
                    Background='{TemplateBinding Panel.Background}'
                    SnapsToDevicePixels='True'>
                    <Border.Resources>
                        <Style x:Key='ScrollBarTrackButton' TargetType='{x:Type RepeatButton}'>
                            <Setter Property='OverridesDefaultStyle' Value='true'/>
                            <Setter Property='Background' Value='Transparent'/>
                            <Setter Property='Focusable' Value='false'/>
                            <Setter Property='IsTabStop' Value='false'/>
                            <Setter Property='MinHeight' Value='0'/>
                            <Setter Property='Template'>
                                <Setter.Value>
                                    <ControlTemplate TargetType='{x:Type RepeatButton}'>
                                        <Rectangle Fill='Transparent' Height='{TemplateBinding Height}' Width='{TemplateBinding Width}'/>
                                    </ControlTemplate>
                                </Setter.Value>
                            </Setter>
                        </Style>
                        
                        <ControlTemplate x:Key='ThumbTemplate' TargetType='{x:Type Thumb}'>
                            <Border CornerRadius='4' Margin='3,0,3,0' SnapsToDevicePixels='True' Background='{DynamicResource CHATVIEW_SCROLLVIEWER_BK}' Width='{TemplateBinding Width}' Height='{TemplateBinding Height}'/>
                        </ControlTemplate>
                        
                        <ControlTemplate x:Key='VerticalScrollBar' TargetType='{x:Type ScrollBar}'>
                            <Grid x:Name='VerticalRoot' Height='{TemplateBinding Height}' Background='Transparent'>
                                <Grid.RowDefinitions>
                                    <RowDefinition Height='Auto' />
                                    <RowDefinition Height='*' />
                                    <RowDefinition Height='Auto' />
                                </Grid.RowDefinitions>
                                <Track x:Name='PART_Track' IsDirectionReversed='True' Grid.Row='1'>
                                    <Track.DecreaseRepeatButton>
                                        <RepeatButton x:Name='HorizontalLargeDecrease' Command='ScrollBar.PageUpCommand' 
                                                          Style='{DynamicResource ScrollBarTrackButton}' />
                                    </Track.DecreaseRepeatButton>
                                    <Track.Thumb>
                                        <Thumb Template='{StaticResource ThumbTemplate}'/>
                                    </Track.Thumb>
                                    <Track.IncreaseRepeatButton>
                                        <RepeatButton x:Name='HorizontalLargeIncrease' Command='ScrollBar.PageDownCommand'
                                                          Style='{DynamicResource ScrollBarTrackButton}' />
                                    </Track.IncreaseRepeatButton>
                                </Track>
                            </Grid>
                        </ControlTemplate>
                        <ControlTemplate x:Key='HorizontalScrollBar' TargetType='{x:Type ScrollBar}'>
                            <Grid x:Name='HorizontalRoot' Height='{TemplateBinding Height}'>
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width='Auto' />
                                    <ColumnDefinition Width='*' />
                                    <ColumnDefinition Width='Auto' />
                                </Grid.ColumnDefinitions>
                                <Track x:Name='PART_Track' IsDirectionReversed='True' Grid.Column='1'>
                                    <Track.DecreaseRepeatButton>
                                        <RepeatButton x:Name='HorizontalLargeDecrease' Command='ScrollBar.PageLeftCommand' 
                                                          Style='{DynamicResource ScrollBarTrackButton}' />
                                    </Track.DecreaseRepeatButton>
                                    <Track.Thumb>
                                        <Thumb Template='{StaticResource ThumbTemplate}'/>
                                    </Track.Thumb>
                                    <Track.IncreaseRepeatButton>
                                        <RepeatButton x:Name='HorizontalLargeIncrease' Command='ScrollBar.PageRightCommand'
                                                          Style='{DynamicResource ScrollBarTrackButton}' />
                                    </Track.IncreaseRepeatButton>
                                </Track>
                            </Grid>
                        </ControlTemplate>
                        <Style x:Key='DefaultScrollBar' TargetType='{x:Type ScrollBar}'>
                            <Setter Property='SnapsToDevicePixels' Value='True' />
                            <Setter Property='OverridesDefaultStyle' Value='true' />
                            <Style.Triggers>
                                <Trigger Property='Orientation' Value='Vertical'>
                                    <Setter Property='Template' Value='{StaticResource VerticalScrollBar}' />
                                    <Setter Property='Width' Value='14' />
                                </Trigger>
                                <Trigger Property='Orientation' Value='Horizontal'>
                                    <Setter Property='Template' Value='{StaticResource HorizontalScrollBar}' />
                                    <Setter Property='Height' Value='0' />
                                </Trigger>
                            </Style.Triggers>
                        </Style>
                        <Style x:Key='ScrollViewerStyle' TargetType='{x:Type ScrollViewer}'>
                            <Setter Property='FocusVisualStyle' Value='{x:Null}'/>
                            <Setter Property='Template'>
                                <Setter.Value>
                                    <ControlTemplate TargetType='{x:Type ScrollViewer}'>
                                        <Grid Background='{TemplateBinding Background}'>
                                            <Grid.ColumnDefinitions>
                                                <ColumnDefinition Width='*' x:Name='leftColumn' />
                                                <ColumnDefinition Width='Auto' x:Name='rightColumn' />
                                            </Grid.ColumnDefinitions>
                                            <Grid.RowDefinitions>
                                                <RowDefinition Height='*' />
                                                <RowDefinition Height='Auto' />
                                            </Grid.RowDefinitions>
                                            <ScrollContentPresenter x:Name='PART_ScrollContentPresenter' CanContentScroll='{TemplateBinding CanContentScroll}'
                                                                CanHorizontallyScroll='False' CanVerticallyScroll='False' ContentTemplate='{TemplateBinding ContentTemplate}'
                                                                Content='{TemplateBinding Content}' Margin='{TemplateBinding Padding}'
                                                                Grid.Row='0' Grid.Column='0'/>
                                            <ScrollBar x:Name='PART_VerticalScrollBar' AutomationProperties.AutomationId='VerticalScrollBar'
                                                   ViewportSize='{TemplateBinding ViewportHeight}'
                                                   Cursor='Arrow' Grid.Column='1' Maximum='{TemplateBinding ScrollableHeight}'
                                                   Minimum='0' Grid.Row='0' Visibility='{TemplateBinding ComputedVerticalScrollBarVisibility}'
                                                   Value='{Binding VerticalOffset, Mode=OneWay, RelativeSource={RelativeSource TemplatedParent}}'
                                                   Orientation='Vertical' Style='{StaticResource DefaultScrollBar}'/>
                                            <ScrollBar x:Name='PART_HorizontalScrollBar' AutomationProperties.AutomationId='HorizontalScrollBar'
                                                   ViewportSize='{TemplateBinding ViewportWidth}'
                                                   Cursor='Arrow' Grid.Column='0' Maximum='{TemplateBinding ScrollableWidth}'
                                                   Minimum='0' Grid.Row='1' Visibility='{TemplateBinding ComputedHorizontalScrollBarVisibility}'
                                                   Value='{Binding HorizontalOffset, Mode=OneWay, RelativeSource={RelativeSource TemplatedParent}}'
                                                   Orientation='Horizontal' Style='{StaticResource DefaultScrollBar}'/>
                                        </Grid>
                                    </ControlTemplate>
                                </Setter.Value>
                            </Setter>
                        </Style>        
                    </Border.Resources>
                    <ScrollViewer Style='{StaticResource ScrollViewerStyle}'
                        x:Name='ScrollViewer'
                        Padding='{TemplateBinding Control.Padding}'
                        Focusable='False'>

                            <ItemsPresenter SnapsToDevicePixels='{TemplateBinding UIElement.SnapsToDevicePixels}'/>

                        
                    </ScrollViewer>
                </Border>
            </ControlTemplate>";
            Template = (ControlTemplate)XamlReader.Parse(template);

            ScrollViewer.SetCanContentScroll(this, true);
            ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);

            VirtualizingPanel.SetCacheLengthUnit(this, VirtualizationCacheLengthUnit.Page);
            VirtualizingPanel.SetCacheLength(this, new VirtualizationCacheLength(1));

            VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);
        }
    }
}