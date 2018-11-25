using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasticFourKinect.Forms
{
    // Interfaz para la carga de datos persistentes
    public interface LoadableForm
    {
        // ESTADOS
        void loadStates(string path);
        
        // GESTOS
        void loadGestures(string path);
        
        // ANIMACIONES
        void loadAnimation(string path);
    }
}
