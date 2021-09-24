using System;
using Xamarin.Forms;

public class CustomButtonRenderer : ButtonRenderer
{
    public CustomButtonRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
    {
        base.OnElementChanged(e);

        if (Control != null && e.NewElement != null)
        {
            Control.Touch += OnTouch;
        }
    }

    private void OnTouch(object sender, TouchEventArgs args)
    {
        var buttonController = (IButtonController)Element;
        if (buttonController == null)
            return;

        var x = (int)args.Event.GetX();
        var y = (int)args.Event.GetY();

        if (!TouchInsideControl(x, y))
        {
            buttonController.SendReleased();
        }
        else if (args.Event.Action == MotionEventActions.Down)
        {
            buttonController.SendPressed();
        }
        else if (args.Event.Action == MotionEventActions.Up)
        {
            buttonController.SendReleased();
            buttonController.SendClicked();
        }
    }

    private bool TouchInsideControl(int x, int y)
    {
        return x <= Control.Right && x >= Control.Left && y <= Control.Bottom && y >= Control.Top;
    }
}
