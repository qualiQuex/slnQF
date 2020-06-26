using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slnQF.Models
{
    public class ResultadoEnsayoProductoModels
    {

        public int ID_RESULTADO_ENSAYO_PROD;
        public int ID_LOTE_INGRESO;
        public int ID_ENSAYO;
        public int ID_PRUEBA;
        public int ID_PRUEBA_ESPECIFICA;
        public string RESULTADO;
        public int ID_TECNICO_ANALISTA;
        public int ID_REVISADO_POR;
        public int ID_ESTADO;
        public DateTime FECHA_CREACION;
        public int ID_USUARIO;
    }
    public partial class SP_REPORTE_CERTIFICADOModels
    {

        private string _TECNICOS;

        private int _ID_LOTE_INGRESO;

        private string _NO_INGRESO;

        private DateTime _FECHA_ANALISIS;

        private string _FECHA_FIN_ANALISIS;

        private string _TOMO;

        private string _PAGINA;

        private string _NO_LOTE;

        private DateTime _FECHA_FABRICACION;

        private DateTime _FECHA_VENCIMIENTO;

        private string _OBSERVACIONES;

        private string _PRODUCTO_NOMBRE;

        private int _CANTIDAD_MUESTRAS;

        private int _TAMANO_LOTE;

        private string _REFERENCIA;

        private string _FORMA_FARMACEUTICA_NOMBRE;

        private string _CONCENTRACION;

        private string _FABRICANTE_NOMBRE;

        private string _PAIS_NOMBRE;

        private string _TECNICO_ANALISTA;

        private string _REVISADO_POR;

        private string _ASEGURAMIENTO_POR;

        private string _PUESTO_ANALISTA;

        private string _PUESTO_REVISADO_POR;

        private string _PUESTO_ASEGURAMIENTO;

        private string _APROBADO;

        private string _REPROBADO;

        private string _UOM;

        private string _UOMPROD;

        public string UOM
        {
            get { return _UOM; }
            set { _UOM = value; }
        }

        private int _CANTIDAD_ENVASES;

        private string _CONDICION_ALMACENA;

        private string _PROVEEDOR;

        private string _ENSAYO_NOMBRE;

        private string _PRUEBA_NOMBRE;

        private string _PRUEBA_ESPECIFICA_NOMBRE;

        private string _ESPECIFICACION;

        private string _RESULTADO;

        private int _ID_CERTIFICADO;

        private string _CERTIFICADO;
        private string _PRESENTACION;


        private string _NOMBRE_COMERCIAL;

        private string _CLIENTE;

        public string CLIENTE
        {
            get { return _CLIENTE; }
            set { _CLIENTE = value; }
        }

        public string PRESENTACION
        {
            get { return _PRESENTACION; }
            set { _PRESENTACION = value; }
        }

        public string NOMBRE_COMERCIAL
        {
            get { return _NOMBRE_COMERCIAL; }
            set { _NOMBRE_COMERCIAL = value; }
        }

        public int ID_LOTE_INGRESO
        {
            get
            {
                return this._ID_LOTE_INGRESO;
            }
            set
            {
                if ((this._ID_LOTE_INGRESO != value))
                {
                    this._ID_LOTE_INGRESO = value;
                }
            }
        }

        public string NO_INGRESO
        {
            get
            {
                return this._NO_INGRESO;
            }
            set
            {
                if ((this._NO_INGRESO != value))
                {
                    this._NO_INGRESO = value;
                }
            }
        }

        public DateTime FECHA_ANALISIS
        {
            get
            {
                return this._FECHA_ANALISIS;
            }
            set
            {
                if ((this._FECHA_ANALISIS != value))
                {
                    this._FECHA_ANALISIS = value;
                }
            }
        }


        public string TOMO
        {
            get
            {
                return this._TOMO;
            }
            set
            {
                if ((this._TOMO != value))
                {
                    this._TOMO = value;
                }
            }
        }


        public string PAGINA
        {
            get
            {
                return this._PAGINA;
            }
            set
            {
                if ((this._PAGINA != value))
                {
                    this._PAGINA = value;
                }
            }
        }


        public string NO_LOTE
        {
            get
            {
                return this._NO_LOTE;
            }
            set
            {
                if ((this._NO_LOTE != value))
                {
                    this._NO_LOTE = value;
                }
            }
        }


        public DateTime FECHA_FABRICACION
        {
            get
            {
                return this._FECHA_FABRICACION;
            }
            set
            {
                if ((this._FECHA_FABRICACION != value))
                {
                    this._FECHA_FABRICACION = value;
                }
            }
        }


        public DateTime FECHA_VENCIMIENTO
        {
            get
            {
                return this._FECHA_VENCIMIENTO;
            }
            set
            {
                if ((this._FECHA_VENCIMIENTO != value))
                {
                    this._FECHA_VENCIMIENTO = value;
                }
            }
        }
        public string OBSERVACIONES
        {
            get
            {
                return this._OBSERVACIONES;
            }
            set
            {
                if ((this._OBSERVACIONES != value))
                {
                    this._OBSERVACIONES = value;
                }
            }
        }

        public string PRODUCTO_NOMBRE
        {
            get
            {
                return this._PRODUCTO_NOMBRE;
            }
            set
            {
                if ((this._PRODUCTO_NOMBRE != value))
                {
                    this._PRODUCTO_NOMBRE = value;
                }
            }
        }

        public int CANTIDAD_MUESTRAS
        {
            get
            {
                return this._CANTIDAD_MUESTRAS;
            }
            set
            {
                if ((this._CANTIDAD_MUESTRAS != value))
                {
                    this._CANTIDAD_MUESTRAS = value;
                }
            }
        }

        public int TAMANO_LOTE
        {
            get
            {
                return this._TAMANO_LOTE;
            }
            set
            {
                if ((this._TAMANO_LOTE != value))
                {
                    this._TAMANO_LOTE = value;
                }
            }
        }


        public string REFERENCIA
        {
            get
            {
                return this._REFERENCIA;
            }
            set
            {
                if ((this._REFERENCIA != value))
                {
                    this._REFERENCIA = value;
                }
            }
        }

        public string FORMA_FARMACEUTICA_NOMBRE
        {
            get
            {
                return this._FORMA_FARMACEUTICA_NOMBRE;
            }
            set
            {
                if ((this._FORMA_FARMACEUTICA_NOMBRE != value))
                {
                    this._FORMA_FARMACEUTICA_NOMBRE = value;
                }
            }
        }


        public string CONCENTRACION
        {
            get
            {
                return this._CONCENTRACION;
            }
            set
            {
                if ((this._CONCENTRACION != value))
                {
                    this._CONCENTRACION = value;
                }
            }
        }


        public string FABRICANTE_NOMBRE
        {
            get
            {
                return this._FABRICANTE_NOMBRE;
            }
            set
            {
                if ((this._FABRICANTE_NOMBRE != value))
                {
                    this._FABRICANTE_NOMBRE = value;
                }
            }
        }

        public string PAIS_NOMBRE
        {
            get
            {
                return this._PAIS_NOMBRE;
            }
            set
            {
                if ((this._PAIS_NOMBRE != value))
                {
                    this._PAIS_NOMBRE = value;
                }
            }
        }


        public string ENSAYO_NOMBRE
        {
            get
            {
                return this._ENSAYO_NOMBRE;
            }
            set
            {
                if ((this._ENSAYO_NOMBRE != value))
                {
                    this._ENSAYO_NOMBRE = value;
                }
            }
        }


        public string PRUEBA_NOMBRE
        {
            get
            {
                return this._PRUEBA_NOMBRE;
            }
            set
            {
                if ((this._PRUEBA_NOMBRE != value))
                {
                    this._PRUEBA_NOMBRE = value;
                }
            }
        }

        public string PRUEBA_ESPECIFICA_NOMBRE
        {
            get
            {
                return this._PRUEBA_ESPECIFICA_NOMBRE;
            }
            set
            {
                if ((this._PRUEBA_ESPECIFICA_NOMBRE != value))
                {
                    this._PRUEBA_ESPECIFICA_NOMBRE = value;
                }
            }
        }

        public string ESPECIFICACION
        {
            get
            {
                return this._ESPECIFICACION;
            }
            set
            {
                if ((this._ESPECIFICACION != value))
                {
                    this._ESPECIFICACION = value;
                }
            }
        }

        public string RESULTADO
        {
            get
            {
                return this._RESULTADO;
            }
            set
            {

                this._RESULTADO = value;

            }
        }

        public string TECNICO_ANALISTA
        {
            get
            {
                return _TECNICO_ANALISTA;
            }

            set
            {
                _TECNICO_ANALISTA = value;
            }
        }

        public string REVISADO_POR
        {
            get
            {
                return _REVISADO_POR;
            }

            set
            {
                _REVISADO_POR = value;
            }
        }

        public string ASEGURAMIENTO_POR
        {
            get
            {
                return _ASEGURAMIENTO_POR;
            }

            set
            {
                _ASEGURAMIENTO_POR = value;
            }
        }

        public string APROBADO
        {
            get
            {
                return _APROBADO;
            }

            set
            {
                _APROBADO = value;
            }
        }

        public string REPROBADO
        {
            get
            {
                return _REPROBADO;
            }

            set
            {
                _REPROBADO = value;
            }
        }

        public string PUESTO_ANALISTA
        {
            get
            {
                return _PUESTO_ANALISTA;
            }

            set
            {
                _PUESTO_ANALISTA = value;
            }
        }

        public string PUESTO_REVISADO_POR
        {
            get
            {
                return _PUESTO_REVISADO_POR;
            }

            set
            {
                _PUESTO_REVISADO_POR = value;
            }
        }

        public string PUESTO_ASEGURAMIENTO
        {
            get
            {
                return _PUESTO_ASEGURAMIENTO;
            }

            set
            {
                _PUESTO_ASEGURAMIENTO = value;
            }
        }

        public int ID_CERTIFICADO
        {
            get
            {
                return _ID_CERTIFICADO;
            }

            set
            {
                _ID_CERTIFICADO = value;
            }
        }

        public string CERTIFICADO
        {
            get
            {
                return _CERTIFICADO;
            }

            set
            {
                _CERTIFICADO = value;
            }
        }

        public string FECHA_FIN_ANALISIS
        {
            get
            {
                return _FECHA_FIN_ANALISIS;
            }

            set
            {
                _FECHA_FIN_ANALISIS = value;
            }
        }

        public string TECNICOS
        {
            get
            {
                return _TECNICOS;
            }

            set
            {
                _TECNICOS = value;
            }
        }

        public string UOMPROD
        {
            get
            {
                return _UOMPROD;
            }

            set
            {
                _UOMPROD = value;
            }
        }

        public int CANTIDAD_ENVASES
        {
            get
            {
                return _CANTIDAD_ENVASES;
            }

            set
            {
                _CANTIDAD_ENVASES = value;
            }
        }

        public string CONDICION_ALMACENA
        {
            get
            {
                return _CONDICION_ALMACENA;
            }

            set
            {
                _CONDICION_ALMACENA = value;
            }
        }

        public string PROVEEDOR
        {
            get
            {
                return _PROVEEDOR;
            }

            set
            {
                _PROVEEDOR = value;
            }
        }
    }
}