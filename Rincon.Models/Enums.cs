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

        Cepillado,
        Curado,
        CepilladoCurado
    }

    /// <summary>
    /// Machimbre
    /// </summary>
    public enum Machimbre
    {
        machimbre1,
        machimbre2,
        machimbre3,
        machimbre4
    }
}
