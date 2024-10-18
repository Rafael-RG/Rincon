using Mopups.Interfaces;
using Rincon.Models;
using System.Windows.Input;

namespace Rincon.CustomPopups;

public partial class MenuPage
{
    IPopupNavigation popupNavigation;

    ICommand logoutCommand;

    ICommand configurationCommand;

    ICommand managementOperatorsCommand;

    public User User { get; set; }

    public MenuPage(IPopupNavigation popupNavigation, User user, ICommand logoutCommand, ICommand configurationCommand, ICommand managementOperatorsCommand)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
        this.logoutCommand = logoutCommand;
        this.User = user;
        this.UserName.Text = user.Name;
        this.configurationCommand = configurationCommand;
        this.managementOperatorsCommand = managementOperatorsCommand;
    }

	private async void Logout_Clicked(object sender, EventArgs e)
	{
        this.logoutCommand.Execute(null);
        await this.popupNavigation.PopAllAsync();
    }

    private async void Configuration_Clicked(object sender, EventArgs e)
    {
        this.configurationCommand.Execute(null);
        await this.popupNavigation.PopAllAsync();
    }

    private async void ManagementOperators_Clicked(object sender, EventArgs e)
    {
        this.managementOperatorsCommand.Execute(null);
        await this.popupNavigation.PopAllAsync();
    }
}