using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;
using Rincon.Platforms.Android;

namespace Rincon;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Landscape)]
public class MainActivity : MauiAppCompatActivity
{
    private GestureInterceptorView gestureInterceptor;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        // Configurar modo immersive para desactivar gestos de navegación
        SetImmersiveMode();
        
        // Configurar ventana para bloquear gestos del sistema
        ConfigureWindow();
        
        // Añadir interceptor de gestos
        SetupGestureInterceptor();
    }

    protected override void OnResume()
    {
        base.OnResume();
        
        // Reestablecer modo immersive cuando la app vuelve al primer plano
        SetImmersiveMode();
        ConfigureWindow();
    }

    public override void OnWindowFocusChanged(bool hasFocus)
    {
        base.OnWindowFocusChanged(hasFocus);
        
        if (hasFocus)
        {
            SetImmersiveMode();
        }
    }

    // Bloquear el botón de atrás
    public override void OnBackPressed()
    {
        // No hacer nada - bloquear navegación hacia atrás
        System.Diagnostics.Debug.WriteLine("Botón atrás bloqueado");
        // base.OnBackPressed(); - Comentado para deshabilitar
    }

    // Interceptar eventos de teclas del sistema
    public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
    {
        if (keyCode == Keycode.Back || keyCode == Keycode.Home || keyCode == Keycode.Menu)
        {
            System.Diagnostics.Debug.WriteLine($"Tecla del sistema bloqueada: {keyCode}");
            return true; // Bloquear estas teclas
        }
        return base.OnKeyDown(keyCode, e);
    }

    private void SetupGestureInterceptor()
    {
        try
        {
            // Crear el interceptor de gestos solo para bordes
            gestureInterceptor = new GestureInterceptorView(this);
            
            // Configurar para cubrir solo las áreas de los bordes
            var layoutParams = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);
            
            gestureInterceptor.LayoutParameters = layoutParams;
            
            // Hacer el interceptor completamente transparente y no clickeable por defecto
            gestureInterceptor.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            gestureInterceptor.Clickable = false;
            gestureInterceptor.Focusable = false;
            gestureInterceptor.FocusableInTouchMode = false;
            
            // Solo añadir si el decorView es accesible
            if (Window?.DecorView is ViewGroup rootView)
            {
                rootView.AddView(gestureInterceptor, 0); // Añadir al fondo, no al frente
                System.Diagnostics.Debug.WriteLine("Interceptor de gestos configurado exitosamente");
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error configurando interceptor de gestos: {ex.Message}");
        }
    }

    private void ConfigureWindow()
    {
        if (Window != null)
        {
            // Configuraciones adicionales para bloquear gestos
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                // Para Android 9+ - Configurar layout en notch
                var layoutParams = Window.Attributes;
                if (layoutParams != null)
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    layoutParams.LayoutInDisplayCutoutMode = LayoutInDisplayCutoutMode.ShortEdges;
#pragma warning restore CA1416 // Validate platform compatibility
                    Window.Attributes = layoutParams;
                }
            }
        }
    }

    private void SetImmersiveMode()
    {
        if (Window != null)
        {
            var decorView = Window.DecorView;
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                // Para Android 11+ (API 30+) - Bloqueo más agresivo
                var controller = WindowCompat.GetInsetsController(Window, decorView);
                if (controller != null)
                {
                    // Ocultar todas las barras del sistema
                    controller.Hide(WindowInsetsCompat.Type.SystemBars());
                    controller.Hide(WindowInsetsCompat.Type.NavigationBars());
                    controller.Hide(WindowInsetsCompat.Type.StatusBars());
                    
                    // Comportamiento más restrictivo
                    controller.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
                }
                
                // Configurar exclusión de gestos del sistema (Android 10+)
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    try
                    {
                        var exclusionRects = new List<Android.Graphics.Rect>();
                        var displayMetrics = Resources?.DisplayMetrics;
                        if (displayMetrics != null)
                        {
                            // Excluir toda la pantalla de los gestos del sistema
                            exclusionRects.Add(new Android.Graphics.Rect(0, 0, displayMetrics.WidthPixels, displayMetrics.HeightPixels));
#pragma warning disable CA1416 // Validate platform compatibility
                            decorView.SystemGestureExclusionRects = exclusionRects;
#pragma warning restore CA1416 // Validate platform compatibility
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error configurando exclusión de gestos: {ex.Message}");
                    }
                }
            }
            else
            {
                // Para versiones anteriores de Android - Configuración más agresiva
#pragma warning disable CA1422 // Validate platform compatibility
                var uiOptions = SystemUiFlags.LowProfile |
                               SystemUiFlags.Fullscreen |
                               SystemUiFlags.HideNavigation |
                               SystemUiFlags.ImmersiveSticky |
                               SystemUiFlags.LayoutStable |
                               SystemUiFlags.LayoutHideNavigation |
                               SystemUiFlags.LayoutFullscreen;
                
                decorView.SystemUiFlags = uiOptions;
#pragma warning restore CA1422 // Validate platform compatibility
            }
        }
    }
}
