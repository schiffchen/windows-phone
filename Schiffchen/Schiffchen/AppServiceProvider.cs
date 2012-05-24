using System;
using System.Collections.Generic;

namespace Schiffchen
{
    /// <summary>
    /// Implementiert IServiceProvider f�r die Anwendung. Dieser Typ wird �ber die
    /// App.Services-Eigenschaft verf�gbar gemacht und kann f�r ContentManager oder andere Typen verwendet werden, die Zugriff auf einen IServiceProvider ben�tigen.
    /// </summary>
    public class AppServiceProvider : IServiceProvider
    {
        // Eine Zuordnung des Diensttyps zu den eigentlichen Diensten
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// F�gt dem Dienstanbieter einen neuen Dienst hinzu.
        /// </summary>
        /// <param name="serviceType">Der Typ des hinzuzuf�genden Diensts.</param>
        /// <param name="service">Das Dienstobjekt selbst.</param>
        public void AddService(Type serviceType, object service)
        {
            // Eingabe �berpr�fen
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            if (service == null)
                throw new ArgumentNullException("service");
            if (!serviceType.IsAssignableFrom(service.GetType()))
                throw new ArgumentException("service does not match the specified serviceType");

            // Dienst zum W�rterbuch hinzuf�gen
            services.Add(serviceType, service);
        }

        /// <summary>
        /// Ruft einen Dienst vom Dienstanbieter ab.
        /// </summary>
        /// <param name="serviceType">Der Typ des abzurufenden Diensts.</param>
        /// <returns>Das f�r den angegebenen Typ registrierte Dienstobjekt.</returns>
        public object GetService(Type serviceType)
        {
            // Eingabe �berpr�fen
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // Dienst aus dem W�rterbuch abrufen
            return services[serviceType];
        }

        /// <summary>
        /// Entfernt einen Dienst aus dem Dienstanbieter.
        /// </summary>
        /// <param name="serviceType">Der Typ des zu entfernenden Diensts.</param>
        public void RemoveService(Type serviceType)
        {
            // Eingabe �berpr�fen
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // Dienst aus dem W�rterbuch entfernen
            services.Remove(serviceType);
        }
    }
}
