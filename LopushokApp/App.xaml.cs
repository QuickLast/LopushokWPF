using LopushokApp.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LopushokApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LopushokOsipovEntities1 db = new LopushokOsipovEntities1();
        public static MainWindow main;
    }
}
