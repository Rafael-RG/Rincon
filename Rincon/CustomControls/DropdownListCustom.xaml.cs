using CommunityToolkit.Maui.Views;
using Rincon.Models;

namespace Rincon.CustomControls;

public partial class DropdownListCustom : ContentView
{
    #region Properties
    public string SearchProduct { get; set; }

    public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(List<Product>), typeof(DropdownListCustom), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (DropdownListCustom)bindable;
        control.Products.ItemsSource = (List<Product>)newValue;
    });

    public static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(Product), typeof(DropdownListCustom), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (DropdownListCustom)bindable;
        control.Products.SelectedItem = (Product)newValue;
    });

    public static BindableProperty IsVisibleListProperty = BindableProperty.Create(nameof(IsVisibleList), typeof(bool), typeof(DropdownListCustom), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (DropdownListCustom)bindable;
        control.FrameList.IsVisible = (bool)newValue;
    });
    #endregion

    public DropdownListCustom()
	{
		InitializeComponent();
	}

    public List<Product> Items
    {
        get => (List<Product>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public Product SelectedItem
    {
        get => (Product)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public bool IsVisibleList
    {
        get => (bool)GetValue(IsVisibleListProperty);
        set => SetValue(IsVisibleListProperty, value);
    }

    void Expander_ExpandedChanged(System.Object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        var control = sender as Expander;

    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            Products.ItemsSource = Items;
        }
        else
        {
            Products.ItemsSource = Items.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Id.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }

    private void Collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedItem = (Product)Products.SelectedItem;
    }

    void SearchBar_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        FrameList.IsVisible = true;
    }

    void SearchBar_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        FrameList.IsVisible = false;
    }
}
