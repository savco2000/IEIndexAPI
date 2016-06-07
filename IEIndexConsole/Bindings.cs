using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer;
using Ninject;
using Ninject.Modules;

namespace IEIndexConsole
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            //var kernel = new StandardKernel();
           
            //var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            
            //var mapper = config.CreateMapper();
            //kernel.Bind<IMapper>().ToConstant(mapper);
        }
    }
}
