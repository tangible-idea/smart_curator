﻿using Parse;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NFCProject {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    public App()
    {
      this.Startup += this.Application_Startup;

      // Initialize the Parse client with your Application ID and Windows Key found on
      // your Parse dashboard
      ParseClient.Initialize("vh60sQDbtfnIlFxn5HrK6oBj5SN1rqYeqtixIngY", "nhCNL1dnGWyWeZhnYx1m8HQ02pEU0B1xmIrNDu0Q");
    }

    private  void Application_Startup(object sender, StartupEventArgs args)
    {
      //ParseAnalytics.TrackAppOpened();
      
        
    }
  }
}
