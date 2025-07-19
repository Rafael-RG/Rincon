using System;
using Rincon.Models;
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

    void TaskAssignedSelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var selectedTask = (TaskItem)this.TaskAssignedList.SelectedItem;
        this.ViewModel.SelectedTask = selectedTask;
        this.ViewModel.ShowTaskPopupCommand.Execute(selectedTask);
    }

    void TaskPendingSelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var selectedTask = (TaskItem)this.TaskPendingList.SelectedItem;
        this.ViewModel.SelectedTask = selectedTask;
        this.ViewModel.ShowTaskPopupCommand.Execute(selectedTask);
    }
}
