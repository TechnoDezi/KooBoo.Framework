using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KooBoo.Framework.BaseClasses
{
    public class KBModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            catch (HttpRequestValidationException)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("The characters in the field {0} were not allowed and removed.", bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelName));
            }
            catch(FormatException)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("The format of the value in the field {0} is not allowed", bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelName));
            }

            //Cast the value provider to an IUnvalidatedValueProvider, which allows to skip validation
            IUnvalidatedValueProvider provider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
            if (provider == null) return null;

            //Get the attempted value, skiping the validation
            var result = provider.GetValue(bindingContext.ModelName, skipValidation: true);

            return result.AttemptedValue;
        }
    }
}
