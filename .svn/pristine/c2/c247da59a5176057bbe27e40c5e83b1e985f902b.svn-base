using System.Collections.Generic;
using System.Linq;
using AppApi.Common.Model;
using AppApi.Entities.Models.Base;
using Microsoft.Extensions.Options;
using static AppApi.Common.Helper.MyFileProvider;

namespace AppApi.Common.Helper
{
  public interface IVBFileProvider
    {
        IMyFileProvider GetInstant(FileAliAs alias);
    }

    public class VBFileProvider : IVBFileProvider
    {
        private readonly IMyFileProvider _ImgFileProvider;
        private readonly IMyFileProvider _DocFileProvider;
        public VBFileProvider(IOptions<List<VirtualPathConfig>> configuration)
        {

            var f = configuration.Value.FirstOrDefault(x => x.Alias == "images");
            if (f != null)
            {
                _ImgFileProvider = new MyFileProvider(f.RealPath, f.Alias);

            }
            if (f != null)
            {
                f = configuration.Value.FirstOrDefault(x => x.Alias == "document");
                _DocFileProvider = new MyFileProvider(f.RealPath, f.Alias);
            }
        }

        public IMyFileProvider GetInstant(FileAliAs alias)
        {
            if (alias == FileAliAs.document)
            {
                return _DocFileProvider;
            }

            if (alias == FileAliAs.images)
            {
                return _ImgFileProvider;
            }

            return null;
        }
    }
}