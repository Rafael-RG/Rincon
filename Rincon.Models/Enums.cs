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
        CepilladoTratado
    }

    /// <summary>
    /// Machimbre
    /// </summary>
    public enum Machimbre
    {
        Deck,
        FrenteIngles,
        Entrepiso,
        Piso
    }
}
