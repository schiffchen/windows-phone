using System;
using System.Collections.Generic;

namespace Schiffchen
{
    /// <summary>
    /// Implementiert IServiceProvider für die Anwendung. Dieser Typ wird über die
    /// App.Services-Eigenschaft verfügbar gemacht und kann für ContentManager oder andere Typen verwendet werden, die Zugriff auf einen IServiceProvider benötigen.
    /// </summary>
    public class AppServiceProvider : IServiceProvider
    {
        // Eine Zuordnung des Diensttyps zu den eigentlichen Diensten
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Fügt dem Dienstanbieter einen neuen Dienst hinzu.
        /// </summary>
        /// <param name="serviceType">Der Typ des hinzuzufügenden Diensts.</param>
        /// <param name="service">Das Dienstobjekt selbst.</param>
        public void AddService(Type serviceType, object service)
        {
            // Eingabe überprüfen
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            if (service == null)
                throw new ArgumentNullException("service");
            if (!serviceType.IsAssignableFrom(service.GetType()))
                throw new ArgumentException("service does not match the specified serviceType");

            // Dienst zum Wörterbuch hinzufügen
            services.Add(serviceType, service);
        }

        /// <summary>
        /// Ruft einen Dienst vom Dienstanbieter ab.
        /// </summary>
        /// <param name="serviceType">Der Typ des abzurufenden Diensts.</param>
        /// <returns>Das für den angegebenen Typ registrierte Dienstobjekt.</returns>
        public object GetService(Type serviceType)
        {
            // Eingabe überprüfen
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // Dienst aus dem Wörterbuch abrufen
            return services[serviceType];
        }

        /// <summary>
        /// Entfernt einen Dienst aus dem Dienstanbieter.
        /// </summary>
        /// <param name="serviceType">Der Typ des zu entfernenden Diensts.</param>
        public void RemoveService(Type serviceType)
        {
            // Eingabe überprüfen
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // Dienst aus dem Wörterbuch entfernen
            services.Remove(serviceType);
        }
    }
}
