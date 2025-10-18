using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rincon.Models
{
    /// <summary>
    /// roles by users
    /// </summary>
    public enum Role
    {
        Admin,
        Operator
    }

    /// <summary>
    /// Product type
    /// </summary>
    public enum ProductType
    {
        Tirante,
        Polin,
        MedioPolin,
        Tabla
    }

    /// <summary>
    /// Wood state
    /// </summary>
    public enum WoodState
    {
        Fresco,
        Rustico,
        RusticoTratado,
        Cepillado,
        CepilladoTratado,
        Tratado,
        SinTratar
    }

    /// <summary>
    /// Machimbre
    /// </summary>
    public enum Machimbre
    {
        FrenteIngles,
        Entrepiso,
        Piso
    }

    /// <summary>
    /// Movement type
    /// </summary>
    public enum MovementType
    {
        Venta,
        Perdida,
        Procesado,
        Producción
    }
}
