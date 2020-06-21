using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;
using System.Dynamic;

namespace MKHOOK
{
    /// <summary>
    /// Clase que define la estructura que tendrá el fichero JSON que contiene toda la información acerca de los parámetros 
    /// que se van a medir, para poder detectar si se está produciendo una anomalía o no en el comportamiento del usuario.
    /// </summary>
    public class JsonClass
    {
        /// <value> Periodo de tiempo en el que se recoge la actividad del usuario. </value>
        public string TimeActivityPerSecond { get; set; }
        /// <value> Periodo de tiempo en el que se recoge cuál ha sido el uso del ratón. </value>
        public string MouseSamplePerSecond { get; set; }
        /// <value> Lista con las estadísticas de actividad del usuario.</value>
        public List<ActivityStats> Activity { get; set; }

    }
    /// <summary>
    /// Clase que contiene todos los parámetros que se van a medir sobre la actividad del usuario cada cierto tiempo.
    /// </summary>
    /// 
    public class ActivityStats
    {
        /// <value> Tiempo en el que se se ha medido la actividad del usuario </value>
        public TimeStats Time { get; set; }
        /// <value> Actividad del usuario con el uso del teclado  </value>
        public KeyboardStats Keyboard { get; set; }
        /// <value> Actividad del usuario con el uso del ratón  </value>
        public MouseStats Mouse { get; set; }
    }
    /// <summary>
    /// Clase que define cada cuanto se mide la actividad del usuario.
    /// </summary>
    /// 
    public class TimeStats
    {
        /// <value> Momento en el que se midió la actividad del usuario.</value>
        public string TimeElapsed { get; set; }

    }
    /// <summary>
    /// Clase que recoge la actividad que realiza un usuario utilizando el teclado.
    /// </summary>
    /// 
    public class KeyboardStats
    {
        /// <value> Número de teclas que ha pulsado un usuario cada cierto tiempo.</value>
        public int PressedKeys { get; set; }
        /// <value> Número de veces que un usuario ha pulsado la tecla "ESC".</value>
        public int ScapeKey { get; set; }
        /// <value> Número de veces que un usuario ha pulsado dos teclas a la vez. </value>
        public int TwoPressedKeys { get; set; }
    }
    /// <summary>
    /// Clase que recoge la actividad que realiza un usuario utilizando el ratón.
    /// </summary>
    /// 
    public class MouseStats
    {
        /// <value> Número de clicks que se ha realizado con el ratón en un periodo de tiempo. </value>
        public int MouseClicks { get; set; }
        /// <value> Distancia euclídea que ha recorrido el ratón un periodo de tiempo. </value>
        public double EuclideanDistance { get; set; }
        /// <value> Número de veces que se ha usado la rueda del ratón un periodo de tiempo. </value>
        public int MouseWheel { get; set; }

    }
}
