using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace POC
{
	public class Message : ContentPage
	{
        public class AlertMessage
        {
            public static readonly string ID = typeof(AlertMessage).Name;

            public string Title;
            public string Message;
        }
    }
}