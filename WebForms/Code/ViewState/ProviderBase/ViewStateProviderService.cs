using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace WebForms.Code
{
    
    // ViewStateProviderService makes the viewstate providers available
    // to the client. This includes loading the providers declared in the 
    // web.config file.
   
    // Enterprise Design Patterns: Lazy Load.

    public static class ViewStateProviderService
    {
        private static ViewStateProviderBase _provider = null;
        private static ViewStateProviderCollection _providers = null;
        private static readonly object Locker = new object();

        // retrieves the viewstate information from the appropriate viewstate provider. 
        // Implements Lazy Load Design Pattern.

        public static object LoadPageState(string name)
        {
            // ensure provider is loaded
            LoadProviders();

            // delegate to the provider
            return _provider.LoadPageState(name);
        }

        
        // Saves any view or control state information to the appropriate 
        // viewstate provider. 
        
        public static void SavePageState(string name, object viewState)
        {
            // ensure provider is loaded
            LoadProviders();

            // Delegate to the provider
            _provider.SavePageState(name, viewState);
        }

        
        // Instantiates and manages the viewstate providers according to the 
        // registered providers in the "viewStateServices" section in web.config.
        
        private static void LoadProviders()
        {
            // providers are loaded just once
            if (_provider == null)
            {
                // Synchronize the process of loading the providers
                lock (Locker)
                {
                    // Confirm that _provider is still null
                    if (_provider == null)
                    {
                        // Get a reference to the <viewstateService> section
                        var section = (ViewStateProviderServiceSection)
                            WebConfigurationManager.GetSection("myviewstateSection/viewstateService");

                        // Load all registered providers
                        _providers = new ViewStateProviderCollection();

                        ProvidersHelper.InstantiateProviders
                            (section.Providers, _providers,
                            typeof(ViewStateProviderBase));

                        // Set _provider to the default provider
                        _provider = _providers[section.DefaultProvider];
                    }
                }
            }
        }
    }
}
