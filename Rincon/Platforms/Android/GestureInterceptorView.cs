using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

namespace Rincon.Platforms.Android;

public class GestureInterceptorView : AppCompatImageView
{
    public GestureInterceptorView(Context context) : base(context)
    {
        Initialize();
    }

    public GestureInterceptorView(Context context, IAttributeSet attrs) : base(context, attrs)
    {
        Initialize();
    }

    public GestureInterceptorView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
    {
        Initialize();
    }

    protected GestureInterceptorView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
        Initialize();
    }

    private void Initialize()
    {
        // Configurar la vista solo para interceptar gestos específicos
        Clickable = false;
        Focusable = false;
        FocusableInTouchMode = false;
        SetBackgroundColor(global::Android.Graphics.Color.Transparent);
    }

    private float startX;
    private float startY;
    private bool isEdgeSwipe = false;

    public override bool OnTouchEvent(MotionEvent e)
    {
        if (e != null)
        {
            var x = e.GetX();
            var y = e.GetY();
            var screenWidth = Resources?.DisplayMetrics?.WidthPixels ?? 0;
            var screenHeight = Resources?.DisplayMetrics?.HeightPixels ?? 0;

            // Definir zonas de los bordes (solo bordes izquierdo y derecho para navegación)
            const int edgeThreshold = 30;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    startX = x;
                    startY = y;
                    // Solo considerar bordes izquierdo y derecho para navegación hacia atrás
                    isEdgeSwipe = x <= edgeThreshold || x >= (screenWidth - edgeThreshold);
                    
                    if (isEdgeSwipe)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inicio de gesto en borde en X:{x}, Y:{y}");
                        return true; // Interceptar solo si es un gesto de borde
                    }
                    break;

                case MotionEventActions.Move:
                    if (isEdgeSwipe)
                    {
                        float deltaX = Math.Abs(x - startX);
                        float deltaY = Math.Abs(y - startY);
                        
                        // Solo bloquear si es un deslizamiento horizontal significativo desde el borde
                        if (deltaX > 50 && deltaX > deltaY)
                        {
                            System.Diagnostics.Debug.WriteLine($"Gesto de navegación bloqueado - deltaX:{deltaX}");
                            return true;
                        }
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    if (isEdgeSwipe)
                    {
                        isEdgeSwipe = false;
                        return true;
                    }
                    break;
            }
        }

        // Para toques normales en el centro, permitir el comportamiento normal
        return false; // No interceptar toques normales
    }

    public override bool DispatchTouchEvent(MotionEvent e)
    {
        // Solo interceptar si detectamos un gesto de navegación específico
        if (OnTouchEvent(e))
        {
            return true;
        }
        return base.DispatchTouchEvent(e);
    }
}