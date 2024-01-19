using System;
using System.Windows.Navigation;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;
using System.Windows;

namespace WpfApp1
{

    /// <summary>
    /// Interaction logic for Marque.xaml
    /// </summary>
    public partial class nxWebBrowser 
    {

        public bool Active;

        public event OnMouseDownEventHandler        OnMouseDown;        public delegate void OnMouseDownEventHandler();
        public event OnBrowserAvailableEventHandler OnBrowserAvailable; public delegate void OnBrowserAvailableEventHandler();
        public event OnProcessDataEventHandler      OnProcessData;      public delegate void OnProcessDataEventHandler(string Data);

        public nxWebBrowser()
        {
            InitializeComponent();

            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
            this.PreviewMouseLeftButtonUp    += _PreviewMouseLeftButtonUp;
            webView.PreviewMouseLeftButtonUp += _PreviewMouseLeftButtonUp;


            InitializeAsync();
        }

        async void InitializeAsync()
        {
            var env = await CoreWebView2Environment.CreateAsync();
            await webView.EnsureCoreWebView2Async(env);
        }

        private async void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            Active = true;
            webView.WebMessageReceived  += WebView_WebMessageReceived;
            webView.NavigationCompleted += WebView_NavigationCompleted;
            webView.ContextMenuOpening  += WebView_ContextMenuOpening;

            //string script = "document.addEventListener('click', function (event)\r\n{\r\n    let elem = event.target;\r\n    " +
            //    "let jsonObject = {'status': 'click','response': 'left'} \r\n    window.chrome.webview.postMessage(jsonObject);\r\n});";
            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(script);

            OnBrowserAvailable?.Invoke();
        
        }

        private void WebView_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        public void Navigate(string URL)
        {
            webView.CoreWebView2.Navigate(URL);
        }

        public void NavigateToString(string URL)
        {
            webView.CoreWebView2.NavigateToString(URL);
        }


        private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string data = e.WebMessageAsJson;
            OnProcessData?.Invoke(data);


            //JsonDocument jsonDocument = JsonDocument.Parse(json);

            //// Access JSON properties
            //JsonElement rootElement = jsonDocument.RootElement;
            //JsonElement Value1 = rootElement.GetProperty("param1");
            //JsonElement Value2 = rootElement.GetProperty("param2");

        }

        private void _PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnMouseDown?.Invoke();
        }

        #region Navigated

        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
           // SetSilent(webView, true); // make it silent
        }

        //private void _Navigated(object sender, NavigationEventArgs e)
        //{

        //    // Inject JavaScript code into the web page
        //    var script = "function sendDataToWPF(data) { window.external.SendDataFromWebPage(data); }";
        //    Browser.InvokeScript("execScript", new object[] { script, "JavaScript" });
        //}
        //public static void SetSilent(System.Windows.Controls.WebBrowser browser, bool silent)
        //{
        //    if (browser == null)
        //        throw new ArgumentNullException("browser");

        //    // get an IWebBrowser2 from the document
        //    IOleServiceProvider sp = browser.Document as IOleServiceProvider;
        //    if (sp != null)
        //    {
        //        Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
        //        Guid IID_IWebBrowser2   = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

        //        object webBrowser;
        //        sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
        //        if (webBrowser != null)
        //        {
        //            webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
        //        }
        //    }
        //}


        //[ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        //private interface IOleServiceProvider
        //{
        //    [PreserveSig]
        //    int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        //}

        //public void SendDataFromWebPage(string data)
        //{
        //    OnProcessData?.Invoke(data);  
        //}


        public void Resize(System.Windows.Size PanelSize)
        {
            this.UpdateLayout();
            this.Width  = PanelSize.Width;
            this.Height = PanelSize.Height;
        }
        #endregion




    }
}
