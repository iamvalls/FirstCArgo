using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace FirstCargoApp.Helper
{
    public class PlaceHolderAttribute : Attribute, IMetadataAware
    {
        private readonly string _placeholder;
        public PlaceHolderAttribute(string placeholder)
        {
            _placeholder = placeholder;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["placeholder"] = _placeholder;
        }
    }
}