using System;
using Rincon.ViewModels;

namespace Rincon.Pages;

public partial class OperatorsPage
{
    /// <summary>
    /// Receives the dependencies by DI
    /// </summary>
    public OperatorsPage(OperatorsViewModel viewModel) : base(viewModel, "Operators")
    {
        InitializeComponent();
    }
}
