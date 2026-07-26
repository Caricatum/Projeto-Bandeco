using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoBandejao.Services
{
    public static class UsuarioSession
    {
        public static Usuario UsuarioLogado
        {
            get;
            set;
        }
    }
}
