using Android.App;
using Android.Runtime;
using doghavenCapstone.API_Keys;
using Plugin.CurrentActivity;
using System;

[Application(Debuggable = true)]

[MetaData("com.google.android.maps.v2.API_KEY", Value = GoogleMapsApiKey.Key)]

public class MainAppication : Application
{
    public MainAppication(IntPtr handel, JniHandleOwnership transer) : base(handel, transer)
    {

    }

    public override void OnCreate()
    {
        base.OnCreate();
        CrossCurrentActivity.Current.Init(this);
    }
}