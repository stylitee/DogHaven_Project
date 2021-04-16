package com.microsoft.windowsazure.mobileservices.authentication;


public class RedirectUrlActivity
	extends crc64a0e0a82d0db9a07d.WebAuthenticatorCallbackActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Microsoft.WindowsAzure.MobileServices.RedirectUrlActivity, Microsoft.Azure.Mobile.Client", RedirectUrlActivity.class, __md_methods);
	}


	public RedirectUrlActivity ()
	{
		super ();
		if (getClass () == RedirectUrlActivity.class)
			mono.android.TypeManager.Activate ("Microsoft.WindowsAzure.MobileServices.RedirectUrlActivity, Microsoft.Azure.Mobile.Client", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
