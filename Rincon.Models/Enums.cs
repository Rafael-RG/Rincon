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
        Tabla,
        Poste
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
        Cepillado4Caras,
        Tratado
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
    /// Deck type
    /// </summary>
    public enum DeckType
    {
        Comun,
        Clear
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
