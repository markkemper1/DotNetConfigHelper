using System;
using DotNetConfigHelper;

[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof($rootnamespace$.App_Start.DotNetConfigHelperStartup), "PreStart")]

namespace $rootnamespace$.App_Start {
    public static class DotNetConfigHelperStartup {
        public static void PreStart()
        {
            AppSettingsReplacer.Install(DotNetConfigHelper.ConfigProvider.CreateAndSetDefault());
        }
    }
}