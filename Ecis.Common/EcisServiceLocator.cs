﻿using Microsoft.Practices.ServiceLocation;

namespace ZMH.Common
{
    public static class EcisServiceLocator
    {
        private static ServiceLocatorProvider currentProvider;

        public static void SetLocatorProvider(ServiceLocatorProvider newProvider)
        {
            currentProvider = newProvider;
        }

        public static IServiceLocator Current
        {
            get
            {
                return currentProvider();
            }
        }
    }
}