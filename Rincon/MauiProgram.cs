using CommunityToolkit.Maui;
using Rincon.Common.Extensions;
using Rincon.Common.Interfaces;
using Rincon.Common.Services;
using Rincon.DataAccess;

namespace Rincon;

/// <summary>
/// MAUI entry point 
/// </summary>
public static class MauiProgram
{
	/// <summary>
	/// Creates the app.
	/// Declares the font, depedenc
	/// y injection components
	/// </summary>
	/// <returns></returns>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()	
			.RegisterViewModelsAndServices()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddLocalization();
		builder.Services.AddDbContext<DatabaseContext>();
		builder.Services.AddSingleton<IDataService, DataService>();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        return builder.Build();
	}
}
