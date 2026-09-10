namespace OM.Notifications.Processors.BizTalkProxy.Entities
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skandia.com.co/Notificaciones")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.skandia.com.co/Notificaciones", IsNullable = false)]

    public partial class NotificacionesRequest
    {
        private bool _isMasiveField;

        private string _idTablaMasivo;

        private int _operacionField;

        private string _tipoIdClienteField;

        private string _numIdClienteField;

        private string _contratoField;

        private string _contactoField;

        private string _imageField;

        private string _enviaAField;

        private string _tipomsjField;

        private string _firstNameField;

        private string _lastNameField;

        private string _nroConfirmacionField;

        private string _ipField;

        private string _descripcionField;

        private string _productoField;

        private NotificacionesRequestParameters[] _parametersField;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Tipomsj
        {
            get
            {
                return this._tipomsjField;
            }

            set
            {
                this._tipomsjField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string EnviaA
        {
            get
            {
                return this._enviaAField;
            }

            set
            {
                this._enviaAField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool IsMasive
        {
            get
            {
                return this._isMasiveField;
            }

            set
            {
                this._isMasiveField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int Operacion
        {
            get
            {
                return this._operacionField;
            }

            set
            {
                this._operacionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IdTablaMasivo
        {
            get
            {
                return this._idTablaMasivo;
            }

            set
            {
                this._idTablaMasivo = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TipoIdCliente
        {
            get
            {
                return this._tipoIdClienteField;
            }

            set
            {
                this._tipoIdClienteField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NumIdCliente
        {
            get
            {
                return this._numIdClienteField;
            }

            set
            {
                this._numIdClienteField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Contrato
        {
            get
            {
                return this._contratoField;
            }

            set
            {
                this._contratoField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Image
        {
            get
            {
                return this._imageField;
            }

            set
            {
                this._imageField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Contacto
        {
            get
            {
                return this._contactoField;
            }

            set
            {
                this._contactoField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FirstName
        {
            get
            {
                return this._firstNameField;
            }

            set
            {
                this._firstNameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LastName
        {
            get
            {
                return this._lastNameField;
            }

            set
            {
                this._lastNameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NroConfirmacion
        {
            get
            {
                return this._nroConfirmacionField;
            }

            set
            {
                this._nroConfirmacionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Ip
        {
            get
            {
                return this._ipField;
            }

            set
            {
                this._ipField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Descripcion
        {
            get
            {
                return this._descripcionField;
            }

            set
            {
                this._descripcionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Producto
        {
            get
            {
                return this._productoField;
            }

            set
            {
                this._productoField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Parameters", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NotificacionesRequestParameters[] Parameters
        {
            get
            {
                return this._parametersField;
            }

            set
            {
                this._parametersField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skandia.com.co/Notificaciones")]
    public partial class NotificacionesRequestParameters
    {
        private string _nameField;
        private string _valueField;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name
        {
            get
            {
                return this._nameField;
            }

            set
            {
                this._nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Value
        {
            get
            {
                return this._valueField;
            }

            set
            {
                this._valueField = value;
            }
        }
    }
}
