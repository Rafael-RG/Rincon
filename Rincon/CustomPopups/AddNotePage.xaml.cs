using Mopups.Interfaces;
using Rincon.Models;
using System.Windows.Input;

namespace Rincon.CustomPopups;

public partial class AddNotePage
{
    IPopupNavigation popupNavigation;

    public object[] Note { get; set; }

    public Note NoteItem { get; set; }

    ICommand okAddNoteCommand;

    public AddNotePage(IPopupNavigation popupNavigation, ICommand okAddNoteCommand, Note note= null)
	{
		InitializeComponent();
        this.NoteItem = note;

        if (this.NoteItem != null)
        {
            this.NoteTitle.Text = this.NoteItem.Title;
            this.NoteDescription.Text = this.NoteItem.Content;
        }

        this.popupNavigation = popupNavigation;
        this.okAddNoteCommand = okAddNoteCommand;
    }

	private async void CancelAddNote_Clicked(object sender, EventArgs e)
	{
        await this.popupNavigation.PopAllAsync();
    }

    private async void OkAddNote_Clicked(object sender, EventArgs e)
    {
        if(this.NoteTitle.Text == null || this.NoteDescription.Text == null)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Titulo y descripcion son requeridos", "Ok");
            return;
        }

        if (this.NoteItem != null) 
        {
            this.NoteItem.Title = this.NoteTitle.Text;
            this.NoteItem.Content = this.NoteDescription.Text;
        }

        this.Note = new object[] { this.NoteTitle.Text, this.NoteDescription.Text, this.NoteItem };
        this.okAddNoteCommand.Execute(this.Note);
        await this.popupNavigation.PopAllAsync();
    }
}