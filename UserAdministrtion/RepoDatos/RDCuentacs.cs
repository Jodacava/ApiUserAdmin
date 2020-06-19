using System;
using System.Collections.Generic;
using UserAdministration.Models;

namespace UserAdministration.RepoDatos
{
    public class RDCuentacs
    {
        public static List<Cuenta> _listaCuentas = new List<Cuenta>();

        public static string createCuenta(Cuenta newCuenta)
        {
            string respuesta = "ok";
            try
            {
                _listaCuentas.Add(newCuenta);
                return respuesta;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static Cuenta updateBalance(string cuentaNum,double valor)
        {
            Cuenta cuentaActualizada = new Cuenta();
            int idx = _listaCuentas.FindIndex(x => x.Numero == cuentaNum);
            if(idx == -1)
            {
                cuentaActualizada.Numero = cuentaNum;
                cuentaActualizada.Balance = valor;
                if (createCuenta(cuentaActualizada) == "ok") { return cuentaActualizada; }
                else return null;
                
            }            
            double newBalance = _listaCuentas[idx].Balance + valor;
            _listaCuentas[idx].Balance = newBalance;
            cuentaActualizada = _listaCuentas[idx];
            return cuentaActualizada;
        }

        public static Cuenta getBalance(string cuentaNum)
        {
            Cuenta balance = _listaCuentas.Find(x=> x.Numero == cuentaNum);
            return balance;
        }
    }
}
